using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    //this will hold the player came that will give tha position for the item
    [SerializeField] Camera cam;

    [Tooltip("This value will determen the max distance of the player can grab item")]
    [SerializeField] float maxGrabingDist = 20.0f;
    [Tooltip("This value will determen the max distance of the player can throw item")]
    [SerializeField] float throwDist = 10.0f;
    [Tooltip("This value will determen how fast can the item move while the player is holding it")]
    [SerializeField] float movingSpeed = 10.0f;
    [Tooltip("This will set at what speed the item will rotate ")]
    [SerializeField] float rotatingSpeed = 20.0f;

    //this will be the position where the item is being held infront of the player
    [Tooltip("This should be an empty gameobject in fornt of the player that will determine the position of the item that is being held")]
    [SerializeField] Transform holdingPoint;
    //this will check if the item has a rigidbody
    Rigidbody grabbedItem;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is holding an item
        if (grabbedItem)
        {   
            //this will allow to held item to move with the player camera with a set speed
            grabbedItem.MovePosition(Vector3.Lerp(grabbedItem.position, holdingPoint.transform.position, Time.deltaTime * movingSpeed));

            //if the player press the t key while holding an item ->
            //it will be thrown away with a set force
            if (Input.GetKeyDown(KeyCode.T))
            {
                grabbedItem.isKinematic = false;
                grabbedItem.AddForce(cam.transform.forward * throwDist, ForceMode.VelocityChange);
                grabbedItem = null;

            }
            //this will rotate the object while the player is holding it ->
            //sadly for now I was able to set it up with key inputs
            if (Input.GetKeyDown(KeyCode.L))
            {
                grabbedItem.transform.Rotate(Vector3.up, -rotatingSpeed);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                grabbedItem.transform.Rotate(Vector3.up, +rotatingSpeed);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                grabbedItem.transform.Rotate(Vector3.right, -rotatingSpeed);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                grabbedItem.transform.Rotate(Vector3.right, +rotatingSpeed);
            }

        }
        //Check for player pressing middle mouse button 
        if (Input.GetMouseButtonDown(2))
        {
            if (grabbedItem)
            {
                grabbedItem.isKinematic = false;
                grabbedItem = null;
            }
            else
            {   
                //creating a ray that will check for items to grab in the ->
                //direction the player camrea is facing
                RaycastHit hit;
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                if (Physics.Raycast(ray, out hit, maxGrabingDist))
                {
                    grabbedItem = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (grabbedItem)    //checks if the grabed item does have a rigidbody
                    {
                        grabbedItem.isKinematic = true;
                    }
                }
            }
        }
    }

    void OnMouseDrag()
    {   
        //This will get the mouse input on the X axis
        float rotatingX = Input.GetAxis("Mouse X") * rotatingSpeed * Mathf.Deg2Rad;
        //This will get the mouse input on the Y axis
        float rotatingY = Input.GetAxis("Mouse Y") * rotatingSpeed * Mathf.Deg2Rad;

        grabbedItem.transform.RotateAround(Vector3.up, -rotatingX);
        grabbedItem.transform.RotateAround(Vector3.right, rotatingY);
    }
}
