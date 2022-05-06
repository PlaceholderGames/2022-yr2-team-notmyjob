using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Transform point;

    private void Update()
    {
        if (Vector3.Distance(transform.position, point.position) > 10000f)
        {
            transform.position = point.position;
        }
    }
}
