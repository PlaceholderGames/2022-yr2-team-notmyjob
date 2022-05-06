using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalTeleport : MonoBehaviour
{
    public Transform reciever;
    
    public UnityEvent OnTeleport;

    private void OnTriggerEnter(Collider other)
    {
        // Get other's offset from the portal
        Vector3 offset = other.transform.position - transform.position;
        
        // Move other to the reciever with the offset
        other.transform.position = reciever.position + offset;
        
        if(other.transform.CompareTag("Player")) OnTeleport.Invoke();

    }
}
