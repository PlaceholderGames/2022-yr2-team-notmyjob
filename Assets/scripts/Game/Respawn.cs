using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // This will hold the position of the Spawn Point for the player
    [SerializeField] Transform spawnPoint;

    //if the player collides with the Death Box the player position will be changed to the spawnpoint position
    private void OnTriggerEnter(Collider other)
    {   
   
        if (other.tag == "Player")
        {
            other.transform.position = spawnPoint.position;
        }

        if (other.GetComponent<RespawnPoint>())
        {
            other.transform.position = other.transform.GetComponent<RespawnPoint>().point.position;
        }
    }
}
