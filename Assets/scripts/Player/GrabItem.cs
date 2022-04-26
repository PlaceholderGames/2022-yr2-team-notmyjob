using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private Transform pickupPoint;
    [Space] [SerializeField] private float pickupRange = 20;

    [Space] [SerializeField] private float movingSpeed = 100.0f;
    [SerializeField] private float rotatingSpeed = 100.0f;
    [SerializeField] private float throwDistance = 10.0f;

    private Rigidbody currentObject;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {   
        // Pickup or drop object
        if (Input.GetMouseButtonDown(2))
        {
            // Check if there's already an object in hand
            if (currentObject)
            {
                // Drop object
                currentObject.useGravity = true;
                currentObject = null;
                return;
            }
            
            // Send a raycast from center point
            Ray cameraRay = playerCamera.ViewportPointToRay(Vector3.one * 0.5f);
            if (Physics.Raycast(cameraRay, out RaycastHit hit, pickupRange))
            {
                // Check if raycast hit an object with a rigidbody
                if (hit.rigidbody)
                {
                    // Set current object to hit object
                    currentObject = hit.rigidbody;
                    currentObject.useGravity = false;
                    currentObject.freezeRotation = false;
                }
            }
        }
        
        // Throw object
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Check if object is in hand
            if (currentObject)
            {
                
                // Throw object using force
                currentObject.useGravity = true;
                currentObject.freezeRotation = false;
                currentObject.AddForce(playerCamera.transform.forward * throwDistance, ForceMode.VelocityChange);
                currentObject = null;
            }
        }
        
        // Rotate Object
        if(Input.GetKey(GameManager.getInstance().objectRotateButton))
        {
            // Check if object is in hand
            if (currentObject)
            {
                // Rotate object
                currentObject.freezeRotation = true;
                
                float rotatingX = Input.GetAxis("Mouse X") * rotatingSpeed * Mathf.Deg2Rad;
                float rotatingY = Input.GetAxis("Mouse Y") * rotatingSpeed * Mathf.Deg2Rad;

                currentObject.transform.Rotate(Vector3.up, -rotatingX);
                currentObject.transform.Rotate(Vector3.right, rotatingY);
            }
        }

    }

    private void FixedUpdate()
    {
        // Check if object is in hand
        if (currentObject)
        {
            // Move object to pickup point
            Vector3 directionToPoint = pickupPoint.position - currentObject.position;
            float distanceToPoint = directionToPoint.magnitude;

            // If object is too far away, set the position to pickup point
            if (distanceToPoint > pickupRange)
            {
                currentObject.position = pickupPoint.position;
                currentObject.velocity = Vector3.zero;
                currentObject.angularVelocity = Vector3.zero;
            } else
            {
                // Otherwise, move the object towards pickup point
                currentObject.velocity = directionToPoint * movingSpeed * distanceToPoint;
            }
        }
    }

    // Return grabbed object in hand
    public Rigidbody getGrabbedObject()
    {
        return currentObject;
    }
}

