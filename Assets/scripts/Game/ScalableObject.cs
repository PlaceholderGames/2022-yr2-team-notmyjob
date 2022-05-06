using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{

    [SerializeField]
    private bool isScalable = true;
    
    // [HideInInspector] public bool overrideLimits = false;
    // [HideInInspector] public bool overrideScaleAxis = false;
    
    [SerializeField] private float minimumScale = 0.25f;
    [SerializeField] private float maximumScale = 2.0f;

    [SerializeField] private Vector3 scaleFactor = new Vector3(1, 1, 1);

    public bool canScale()
    {
        return isScalable;
    }

    public Vector3 getScaledAxis()
    {
        return scaleFactor;
    }

    public float getMinimumScale()
    {
        return minimumScale;
    }
    
    public float getMaximumScale()
    {
        return maximumScale;
    }
}