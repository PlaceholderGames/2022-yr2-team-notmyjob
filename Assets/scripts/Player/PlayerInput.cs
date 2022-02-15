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
                return GetMouseOrStickLookAxis("Mouse X", InvertXAxis);
            case PlayerDirection.VERTICAL:
                return GetMouseOrStickLookAxis("Mouse Y", InvertYAxis);
            default:
                throw new ArgumentException("Invalid parameter", nameof(direction));
        }
    }

    public Vector3 getMove()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f,
                Input.GetAxisRaw("Vertical"));

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public bool handleButtonInput(string axis, InputMode mode)
    {
        if (CanProcessInput())
        {
            switch (mode)
            {
                case InputMode.HOLD:
                    return Input.GetButton(axis);
                case InputMode.PRESS:
                    return Input.GetButtonDown(axis);
                default:
                    return Input.GetButtonDown(axis);
            }
        }

        return false;
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    float GetMouseOrStickLookAxis(string mouseInputName, bool invert)
    {
        if (CanProcessInput())
        {
            float i = Input.GetAxisRaw(mouseInputName);

            // handle inverting vertical input
            if (invert)
                i *= -1f;

            // apply sensitivity multiplier
            i *= LookSensitivity;

            // reduce mouse input amount to be equivalent to stick movement
            i *= 0.01f;
#if UNITY_WEBGL
            // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
            i *= WebglLookSensitivityMultiplier;
#endif

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

public enum InputMode
{
    PRESS,
    HOLD
}
