using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3 : MonoBehaviour
{
    // storing the position of where the basketball will be respawned
    [SerializeField] Transform SpawnPoint;
    [SerializeField] GameObject basketball;
    [SerializeField] GameObject Door;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] Transform displaypos;
    //teh counter will indicate how many baskets did the player throw sucseffuly
    [SerializeField] private int counter;
    [SerializeField] string PuzzleCode;

    [SerializeField] bool Opened;

    void Start()
    {
        counter = 0;
        Opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Door.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        string fornow = other.GetComponent<PuzzlePiece>().PuzzleCode;
        if (fornow == this.PuzzleCode)
        {
            counter++;
            basketball.transform.position = SpawnPoint.position;
            if (floatingTextPrefab)
            {
                DisplayScore();
            }
            if (Opened == false && counter == 5)
            {
                Door.transform.rotation = Quaternion.Euler(0, 90, 0);
                Opened = true;
            }

        }
    }
    void DisplayScore()
    {

        var go = Instantiate(floatingTextPrefab, displaypos.transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = counter.ToString();
    }
}
