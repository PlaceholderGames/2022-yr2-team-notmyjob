using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    // Start is called before the first frame update
    public string PuzzleCode;
    public float mass;
    [SerializeField] float howFar;
    [SerializeField] Transform spawnpoint;
    [SerializeField] GameObject altar;
    [SerializeField] AudioSource src;
    void Start()
    {
        src = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(altar.transform.position, transform.position);

        if (distance >= howFar)
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
        //gameObject.GetComponent<AudioSource>().Play();
        src.Play();
    }

}
