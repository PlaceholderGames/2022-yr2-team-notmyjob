using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle4 : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<string> Puzzlecode;
    List<Collider> collidingObjects;
    [SerializeField] GameObject Bishop;
    [SerializeField] Transform spawnforBishop;
    [SerializeField] AudioSource src;

    private bool hasBeenPlayed;
    private int checker;
    void Start()
    {
        collidingObjects = new List<Collider>();
        hasBeenPlayed = false;
        checker = 0;
    }

    // Update is called once per frame
    void Update()
    {
        checker = 0;
        for (int i = 0; i < collidingObjects.Count; i++)
        {
            string puzzlecode = collidingObjects[i].GetComponent<PuzzlePiece>().PuzzleCode;
            float Cmass = collidingObjects[i].GetComponent<Rigidbody>().mass;

            bool masscheck = Mathf.Approximately(Cmass, collidingObjects[i].GetComponent<PuzzlePiece>().mass);
            bool puzzlechecker = puzzlecode.Contains(puzzlecode);
            print(masscheck);
            print(puzzlechecker);
            if (masscheck == puzzlechecker)
            {
                checker++;
                print(checker);

            }

        }
        if (checker == Puzzlecode.Count && !hasBeenPlayed)
        {
            print("puzzle done");
            src.Play();
            hasBeenPlayed = true;
           var whydoihavetodothis = Instantiate(Bishop, spawnforBishop.position, Quaternion.identity);
            whydoihavetodothis.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            whydoihavetodothis.AddComponent<Rigidbody>();
            whydoihavetodothis.AddComponent<ScalableObject>();
            whydoihavetodothis.GetComponent<MeshCollider>().convex = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        collidingObjects.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        collidingObjects.Remove(other);
    }

}
