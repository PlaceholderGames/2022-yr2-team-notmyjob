using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform reciever;

    private void OnTriggerEnter(Collider other)
    {
        // Get other's offset from the portal
        Vector3 offset = other.transform.position - transform.position;
        
        // Move other to the reciever with the offset
        other.transform.position = reciever.position + offset;
    }
}
