using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMaterialProperty : MonoBehaviour
{
    [Header("Bounciness"), Tooltip("The bounciness of the object.")]
    [SerializeField, Range(0,1)] private float bounciness;
    [SerializeField] private PhysicMaterialCombine bouncinessCombine = PhysicMaterialCombine.Average;

    [Header("Friction"), Tooltip("The friction of the object.")]
    [SerializeField, Range(0,1)] private float friction = 0.5f;
    [SerializeField, Range(0,1)] private float movingFriction = 0.5f;
    [SerializeField] private PhysicMaterialCombine frictionCombine = PhysicMaterialCombine.Average;

    private PhysicMaterial physicMaterial;
    
    void Awake()
    {
        physicMaterial = new PhysicMaterial(transform.name + "_PhysicMaterial");
    }

    private PhysicMaterial UpdateMaterial(PhysicMaterial material)
    {
        material.bounciness = bounciness;
        material.bounceCombine = bouncinessCombine;
        material.dynamicFriction = movingFriction;
        material.frictionCombine = frictionCombine;
        material.staticFriction = friction;

        return material;
    }
    
    private void OnValidate()
    {
        physicMaterial = UpdateMaterial(physicMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        physicMaterial = UpdateMaterial(physicMaterial);
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider _collider in colliders)
        {
            // Set the collider's material to the physic material
            _collider.material = physicMaterial;
        }
    }
}
