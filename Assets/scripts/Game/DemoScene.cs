using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScene : MonoBehaviour
{

    public GameObject insertHere;

    public void SetLightColour(Light light)
    {
        light.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    void Update()
    {
        insertHere.SetActive(GameManager.getPlayer().GetComponent<GrabItem>().getGrabbedObject() != null);
        if(insertHere.activeSelf) insertHere.transform.LookAt(Camera.main.transform);
    }
}
