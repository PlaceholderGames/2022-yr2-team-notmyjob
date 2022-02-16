using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    // Time at which the projectile spawned
    float spawnTime = 0;

    public float maxScale = 2.0f;
    public float minScale = 0.25f;

    public float massChange = 0.1f;
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
            other.GetComponent<Rigidbody>().mass += massChange;

            if(other.transform.localScale.x <= minScale)
            {
                other.transform.localScale = Vector3.one * minScale;

            }
            if (other.transform.localScale.x >= maxScale)
            {
                other.transform.localScale = Vector3.one;
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