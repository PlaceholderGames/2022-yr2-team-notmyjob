using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Player : MonoBehaviour
{   
    //this will have the player controller to detect if the player is colliding with the deathBox
    [SerializeField] CharacterController Player;
    //this will hold the player current position
    [SerializeField] Transform Player_pos;

    // This will hold the position of the Spawn Point for the player
    [SerializeField] Transform SpawnPoint;

    //if the player collides with the Death Box the player position will be changed to the spawnpoint position
    private void OnTriggerEnter(Collider other)
    {   
   
        if (other == Player)
        {
            Player_pos.transform.position = SpawnPoint.transform.position;
        }
    }
}
