using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabItem : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private Transform pickupPoint;
    [SerializeField] private float pickupRange = 20;
    [SerializeField] private GameObject pickupUI;
    [SerializeField] private GameObject throwUI;
    [SerializeField] private GameObject rotateUI;

    [Space] [SerializeField] private float movingSpeed = 100.0f;
    [SerializeField] private float rotatingSpeed = 100.0f;
    [SerializeField] private float throwDistance = 10.0f;
    //Aucio clips and the sounds sopurce for the player
    [SerializeField] AudioClip _pickup, _putdown, _throw;
    [SerializeField] AudioSource src;

    private Rigidbody lastObject;
    private Rigidbody currentObject;

    private string getSpritePath(string name)
    {
        return $"ControllerPrompts/KeyboardMouse/{name.Split('_')[name.Split('_').Length - 1]}/{name}";
    }
    
    private void Start()
    {
        playerCamera = Camera.main;
        AudioSource src = gameObject.GetComponent<AudioSource>();
        
        string pickupImage = GameManager.GetControllerName(GameManager.GetKeyCodesFromAxis("Hold")[0]);
        pickupUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(getSpritePath(pickupImage));
        
        string throwImage = GameManager.GetControllerName(GameManager.GetKeyCodesFromAxis("Throw")[0]);
        throwUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(getSpritePath(throwImage));

        string rotateImage = GameManager.GetControllerName(GameManager.GetKeyCodesFromAxis("Rotate")[0]);
        rotateUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(getSpritePath(rotateImage));
            
        pickupUI.SetActive(false);
        throwUI.SetActive(false);
        rotateUI.SetActive(false);
    }

    private void Update()
    {
        Ray cameraRay = playerCamera.ViewportPointToRay(Vector3.one * 0.5f);
        
        // If the player is not holding an object, show the pickup UI
        // If the player is holding an object, show the throw UI and show the pickup UI but change the text to "Drop"
        
        // Player is not holding an object
        if (currentObject == null)
        {
            bool canPickup = Physics.Raycast(cameraRay, out RaycastHit hitInfo, pickupRange);
            pickupUI.GetComponentInChildren<TMPro.TMP_Text>().text = "Pickup";
            pickupUI.SetActive(canPickup && hitInfo.rigidbody != null);
            throwUI.SetActive(false);
            rotateUI.SetActive(false);
        }
        else
        {
            pickupUI.transform.position = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
            pickupUI.SetActive(true);
            throwUI.SetActive(true);
            rotateUI.SetActive(true);
            pickupUI.GetComponentInChildren<TMPro.TMP_Text>().text = "Drop";
        }

        // Pickup or drop object
        if (Input.GetKeyDown(GameManager.GetKeyCodesFromAxis("Hold")[0]))
        {
            // Check if there's already an object in hand
            if (currentObject)
            {
                // Drop object
                currentObject.useGravity = true;
                src.clip = _putdown;
                src.PlayOneShot(_putdown);
                lastObject = currentObject;
                currentObject = null;
                return;
            }
            
            // Send a raycast from center point
            if (Physics.Raycast(cameraRay, out RaycastHit hit, pickupRange))
            {
                // Check if raycast hit an object with a rigidbody
                if (hit.rigidbody)
                {
                    // Set current object to hit object
                    currentObject = hit.rigidbody;
                    currentObject.useGravity = false;
                    currentObject.freezeRotation = false;
                    src.clip = _pickup;
                    src.PlayOneShot(_pickup);
                }
            }
        }
        
        // Throw object
        if (Input.GetKeyDown(GameManager.GetKeyCodesFromAxis("Throw")[0]))
        {
            // Check if object is in hand
            if (currentObject)
            {
                // Throw object using force
                currentObject.useGravity = true;
                currentObject.freezeRotation = false;
                src.clip = _throw;
                src.PlayOneShot(_throw);
                currentObject.AddForce(playerCamera.transform.forward * throwDistance, ForceMode.VelocityChange);
                lastObject = currentObject;
                currentObject = null;
            }
        }
        
        // Rotate Object
        if(Input.GetKey(GameManager.GetKeyCodesFromAxis("Rotate")[0]) && currentObject) {
            // Rotate object
            currentObject.freezeRotation = true;
                
            float rotatingX = Input.GetAxis("Mouse X") * rotatingSpeed * Mathf.Deg2Rad;
            float rotatingY = Input.GetAxis("Mouse Y") * rotatingSpeed * Mathf.Deg2Rad;

            currentObject.transform.Rotate(Vector3.up, -rotatingX);
            currentObject.transform.Rotate(Vector3.right, rotatingY);
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
    
    public Rigidbody getLastGrabbedObject()
    {
        return lastObject;
    }
}

