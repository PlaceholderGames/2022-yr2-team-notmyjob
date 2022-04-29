using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3 : MonoBehaviour
{
    // storing the position of where the basketball will be respawned
    [SerializeField] Transform SpawnPoint;
    [SerializeField] GameObject basketball;
    //teh counter will indicate how many baskets did the player throw sucseffuly
    [SerializeField] private int counter;
    [SerializeField] string PuzzleCode;


    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        string fornow = other.GetComponent<PuzzlePiece>().PuzzleCode;
        if (fornow == this.PuzzleCode)
        {
            counter++;
            basketball.transform.position = SpawnPoint.position;
        }
    }
}
