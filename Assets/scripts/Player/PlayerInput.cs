using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Tooltip("Sensitivity multiplier for moving the camera around")]
    public float LookSensitivity = 1f;

    [Tooltip("Additional sensitivity multiplier for WebGL")]
    public float WebglLookSensitivityMultiplier = 0.25f;

    [Tooltip("Limit to consider an input when using a trigger on a controller")]
    public float TriggerAxisThreshold = 0.4f;

    [Tooltip("Used to flip the vertical input axis")]
    public bool InvertYAxis = false;

    [Tooltip("Used to flip the horizontal input axis")]
    public bool InvertXAxis = false;

    PlayerController m_PlayerCharacterController;
    bool m_FireInputWasHeld;

    public float getLook(PlayerDirection direction)
    {
        switch(direction)
        {
            case PlayerDirection.HORIZONTAL:
                return GetMouseOrStickLookAxis("Mouse X", "");
            case PlayerDirection.VERTICAL:
                return GetMouseOrStickLookAxis("Mouse Y", "");
            default:
                throw new ArgumentException("Invalid parameter", nameof(direction));
        }
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
    {
        if (CanProcessInput())
        {
            // Check if this look input is coming from the mouse
            bool isGamepad = false; //Input.GetAxis(stickInputName) != 0f;
            float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

            // handle inverting vertical input
            if (InvertYAxis)
                i *= -1f;

            // apply sensitivity multiplier
            i *= LookSensitivity;

            if (isGamepad)
            {
                // since mouse input is already deltaTime-dependant, only scale input with frame time if it's coming from sticks
                i *= Time.deltaTime;
            }
            else
            {
                // reduce mouse input amount to be equivalent to stick movement
                i *= 0.01f;
#if UNITY_WEBGL
                    // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                    i *= WebglLookSensitivityMultiplier;
#endif
            }

            return i;
        }

        return 0f;
    }
}

public enum PlayerDirection
{
    HORIZONTAL,
    VERTICAL
}
