using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle1 : MonoBehaviour
{
    //importing the animator that will be switched on or off
    public GameObject anim;

    //prviding the name that each altar will be requesting
    [SerializeField] string PuzzleCode;
    private void OnTriggerStay(Collider other)
    {   
        //if the p[u
        if (other.gameObject.tag == PuzzleCode)
        {
            anim.GetComponent<Animator>().enabled = true;
            other.transform.rotation = gameObject.transform.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        anim.GetComponent<Animator>().enabled = false;
    }
}
