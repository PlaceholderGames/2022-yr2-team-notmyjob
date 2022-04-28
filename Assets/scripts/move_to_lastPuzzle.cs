using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_to_lastPuzzle : MonoBehaviour
{
    // this variable contains the player current position
    [SerializeField] Transform Player_pos;
    // this variable contains the position of the spawn point where the player will be moved.
    [SerializeField] Transform SP_lastPuzzle;

    //This gameobject will be used only once. after it has been used it will be destroyed.
    //this bolian will indicate has it been triggered or not
    private bool hasTriggered;
    void Start()
    {
        hasTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTriggered)
        {
            gameObject.transform.parent = null;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.tag == "Player")
        {
            Player_pos.position = SP_lastPuzzle.position;
            hasTriggered = true;
        }
    }
}
