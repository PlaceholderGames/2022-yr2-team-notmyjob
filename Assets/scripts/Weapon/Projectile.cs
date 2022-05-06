using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    // Time at which the projectile spawned
    float spawnTime = 0;

    // public float maxScale = 2.0f;
    // public float minScale = 0.25f;

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
    Rigidbody rigidbody;

    void Start()
    {
        spawnTime = Time.time;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerController player = GameManager.getInstance().getPlayer();
        
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
        Rigidbody heldItem = GameManager.getInstance().getPlayer().GetComponent<GrabItem>().getGrabbedObject();
        bool isHolding = heldItem != null;

        // Checks if the current held item is the object hit.
        if(isHolding && heldItem.gameObject == other.gameObject)
        {
            Destroy(gameObject);
            return;
        }

        // Check if the projectile has intersected
        // a scalable object
        if (CompareTag("ScalableObject") || other.transform.GetComponent<ScalableObject>())
        {
            ScalableObject scalableObj = other.transform.GetComponent<ScalableObject>();
            if (!scalableObj.canScale()) return;

            // Change the scale of the object by how much
            // the object should change in size
            Vector3 scaledChange = new Vector3(scaleChange.x * scalableObj.getScaledAxis().x,
                scaleChange.y * scalableObj.getScaledAxis().y, scaleChange.z * scalableObj.getScaledAxis().z);
            other.transform.localScale += scaledChange;

            // Change the mass of the object by how much
            // the object should change in mass
            other.GetComponent<Rigidbody>().mass += massChange;
            
            // Clamp the scale of the object to the max and min
            other.transform.localScale = new Vector3(
                Mathf.Clamp(other.transform.localScale.x, scalableObj.getMinimumScale(), scalableObj.getMaximumScale()),
                Mathf.Clamp(other.transform.localScale.y, scalableObj.getMinimumScale(), scalableObj.getMaximumScale()),
                Mathf.Clamp(other.transform.localScale.z, scalableObj.getMinimumScale(), scalableObj.getMaximumScale()));
        }

        // Destroy gameObject when done.
        if(other.GetComponent<ScalableObject>()) Destroy(gameObject);
    }
}

public enum ScaleType
{
    INCREASE,
    DECREASE
}