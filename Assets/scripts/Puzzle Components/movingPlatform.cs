using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{

    [SerializeField] GameObject Player_Pos;
    //when the player enters the collusion box it will have its position updated with the moving platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == Player_Pos.transform)
        {
            Player_Pos.transform.parent = gameObject.transform;
        }
    }

    //when the player exits(jumps off) there will be no other factor affecting the player position outside of the player input
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == Player_Pos.transform)
        {
            Player_Pos.transform.parent = null;
        }
    }
}
