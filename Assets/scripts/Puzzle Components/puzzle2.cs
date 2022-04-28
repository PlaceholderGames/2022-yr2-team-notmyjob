using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject PuzzlePiece;
    [SerializeField] GameObject myPrefab;
    [SerializeField] Transform spawnPoint_forp;
    [SerializeField] GameObject teleporterBox;
    //check for player using the right puzzle piece
    [SerializeField] string PuzzleCode;


    //private boolin to determine if the object has been spwaned before
    private bool hasSpawned;

    private void Start()
    {
        hasSpawned = false;
        teleporterBox.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        string fornow = other.GetComponent<PuzzlePiece>().PuzzleCode;
        if (fornow == this.PuzzleCode && hasSpawned == false)
        {
            teleporterBox.SetActive(true);
            Instantiate(myPrefab, spawnPoint_forp.transform.position, Quaternion.identity);
            hasSpawned = true;
        }
    }
}
