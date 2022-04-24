using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSetup : MonoBehaviour
{
    [SerializeField] private String name;
    
    public new Camera portalCameraSource;
    public new Camera portalCameraDestination;
    
    public Material matSource;
    public Material matDestination;

    private void Start()
    {
        if(portalCameraSource.targetTexture != null)
        {
            portalCameraSource.targetTexture.Release();
        }
        
        if(portalCameraDestination.targetTexture != null)
        {
            portalCameraDestination.targetTexture.Release();
        }
        
        
        portalCameraSource.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        portalCameraDestination.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        
        matSource.mainTexture = portalCameraSource.targetTexture;
        matDestination.mainTexture = portalCameraDestination.targetTexture;
    }
}
