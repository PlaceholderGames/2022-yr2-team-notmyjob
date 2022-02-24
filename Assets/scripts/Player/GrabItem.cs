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
    public Rigidbody grabbedItem;

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

            if(Input.GetKey(GameManager.objectRotateButton))
            {
                float rotatingX = Input.GetAxis("Mouse X") * rotatingSpeed * Mathf.Deg2Rad;
                float rotatingY = Input.GetAxis("Mouse Y") * rotatingSpeed * Mathf.Deg2Rad;

                grabbedItem.transform.Rotate(Vector3.up, -rotatingX);
                grabbedItem.transform.Rotate(Vector3.right, rotatingY);
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
}
