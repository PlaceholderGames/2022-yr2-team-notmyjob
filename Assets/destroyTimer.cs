using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyTimer : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float dTime = 3.0f;
    void Start()
    {
        Destroy(gameObject, dTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
