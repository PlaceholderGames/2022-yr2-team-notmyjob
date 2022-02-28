using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform reciever;

    private bool isPlayerOverlapping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerOverlapping)
        {
            Vector3 portalToPlayer = GameManager.getPlayer().transform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // Player has teleported if true
            if(dotProduct < 0)
            {
                // Teleport
                float rotationDifference = Quaternion.Angle(transform.rotation, reciever.rotation);
                rotationDifference += 180;
                GameManager.getPlayer().transform.Rotate(Vector3.up, rotationDifference);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
                GameManager.getPlayer().transform.position = reciever.position + positionOffset;

                isPlayerOverlapping = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isPlayerOverlapping = other.gameObject == GameManager.getPlayer().gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerOverlapping = !(other.gameObject == GameManager.getPlayer().gameObject);
    }
}
