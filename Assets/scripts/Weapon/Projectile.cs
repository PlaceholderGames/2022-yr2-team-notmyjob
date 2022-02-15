using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    // Time at which the projectile spawned
    float spawnTime = 0;

    // How much force should be applied;
    float force = 10f;

    // Scaling (Negative numbers will decrease in scale, positive will increase)
    public Vector3 scaleChange;

    // How long the projectile should live for
    public float timeToLive = 10f;

    // Direction of the projectile;
    public Vector3 direction;
    new Rigidbody rigidbody;

    void Start()
    {
        spawnTime = Time.time;
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.useGravity = true;
    }

    void Update()
    {
        rigidbody.AddForce(direction * force);

        if (Time.time >= spawnTime + timeToLive)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "ScalableObject")
        {
            other.transform.localScale += scaleChange;
            if(other.transform.localScale.x <= 0)
            {
                other.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
            Destroy(gameObject);
        }
    }
}

public enum ScaleType
{
    INCREASE,
    DECREASE
}