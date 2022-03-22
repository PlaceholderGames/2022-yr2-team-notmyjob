using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSetup : MonoBehaviour
{
    public new Camera camera;
    public Material mat;

    private void Start()
    {
        if(camera.targetTexture != null)
        {
            camera.targetTexture.Release();
        }
        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        mat.mainTexture = camera.targetTexture;
    }
}
