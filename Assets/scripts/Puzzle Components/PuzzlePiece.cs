using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    // Start is called before the first frame update
    public string PuzzleCode;
    [SerializeField] Transform spawnpoint;
    [SerializeField] GameObject altar;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(altar.transform.position, transform.position);

        if (distance >= 200)
        {
            Transform temp = transform.parent;
            Destroy(gameObject);
            GameObject newObject = Instantiate(this.gameObject, spawnpoint.position, Quaternion.identity);
            newObject.transform.parent = temp;
            newObject.GetComponent<Rigidbody>().useGravity = true;
            //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            //transform.position = spawnpoint.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

}
