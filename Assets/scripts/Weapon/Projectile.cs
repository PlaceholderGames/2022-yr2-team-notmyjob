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
    public float force = 10f;

    // Scaling (Negative numbers will decrease in scale, positive will increase)
    public Vector3 scaleChange;

    // How long the projectile should live for
    public float timeToLive = 10f;
    private float aliveFor = 0;

    // Direction of the projectile;
    public Vector3 direction;
    new Rigidbody rigidbody;

    void Start()
    {
        spawnTime = Time.time;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Add force to projectile;
        rigidbody.AddRelativeForce(direction * force);

        // Set scale to how long projectile has been alive for
        transform.localScale = Vector3.one * aliveFor;

        // Destroy the projectile once it reaches its death time
        if (Time.time >= spawnTime + timeToLive)
        {
            Destroy(gameObject);
        }

        // Keep track of how long projectile
        // has been alive for;
        aliveFor += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player is currently holding an object
        Rigidbody heldItem = GameManager.getPlayer().GetComponent<GrabItem>().getGrabbedObject();
        bool isHolding = heldItem != null;

        // Checks if the current held item is the object hit.
        if(isHolding && heldItem.gameObject == other.gameObject)
        {
            Destroy(gameObject);
            return;
        }

        // Check if the projectile has intersected
        // a scalable object
        if (other.transform.tag == "ScalableObject")
        {
            // Change the scale of the object by how much
            // the object should change in size
            other.transform.localScale += scaleChange;

            // Change the mass of the object by how much
            // the object should change in mass
            other.GetComponent<Rigidbody>().mass += massChange;

            // Check for scale limits on object
            if(other.transform.localScale.x <= minScale) other.transform.localScale = Vector3.one * minScale;
            else if (other.transform.localScale.x >= maxScale) other.transform.localScale = Vector3.one;
            
            // Destroy object when done.
            Destroy(gameObject);
        }
    }
}

public enum ScaleType
{
    INCREASE,
    DECREASE
}