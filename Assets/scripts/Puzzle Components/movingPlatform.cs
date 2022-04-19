using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    //this should be the player, using it so the player doesnt' fall off the platform
    public GameObject Player;
    
    //when the player enters the collusion box it will have its position updated with the moving platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }

    //when the player exits(jumps off) there will be no other factor affecting the player position outside of the player input
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}
