using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject PuzzlePiece;

    [SerializeField] string PuzzleCode;

    private void OnTriggerStay(Collider other)
    {
        string fornow = other.GetComponent<PuzzlePiece>().PuzzleCode;
        if (fornow == this.PuzzleCode)
        {
            print("Working");
        }
    }
}
