using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform reciever;

    private bool isPlayerOverlapping = false;

    // Update is called once per frame
    // void Update()
    // {
    //     if(isPlayerOverlapping)
    //     {
    //         Vector3 portalToPlayer = GameManager.getInstance().getPlayer().transform.position - transform.position;
    //         float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
    //         Debug.Log(dotProduct);
    //
    //         // Player has teleported if true
    //         if(dotProduct < 0f)
    //         {
    //             // Teleport
    //             float rotationDifference = Quaternion.Angle(transform.rotation, reciever.rotation);
    //             //rotationDifference += 180;
    //             GameManager.getInstance().getPlayer().transform.Rotate(Vector3.up, rotationDifference);
    //
    //             Vector3 positionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
    //             GameManager.getInstance().getPlayer().transform.position = reciever.position + positionOffset;
    //
    //             isPlayerOverlapping = false;
    //         }
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = other.transform;
        bool canTeleport = otherTransform.tag == "Player" || otherTransform.tag == "ScalableObject";

        if(canTeleport)
        {
            //isPlayerOverlapping = true;
            if (other.transform.GetComponent<Rigidbody>())
            {
                Rigidbody otherBody = other.transform.GetComponent<Rigidbody>();
                otherBody.velocity = Vector3.zero;
            }
            other.transform.position = reciever.transform.position;
        }
    }
}
