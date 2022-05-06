using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectExtended : MonoBehaviour
{
    public static void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        if(obj.transform.childCount > 0)
        {
            foreach (Transform child in obj.transform)
            { 
                GameObjectExtended.SetLayer(child.gameObject, layer);
            }
        }
    }

    public static void SetLayer(GameObject obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        obj.layer = layer;
    }
    
    public void SetLayer(string layerName)
    {
        GameObjectExtended.SetLayer(this.gameObject, layerName);
    }

    public void SetLayerRecursively(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        GameObjectExtended.SetLayer(gameObject, layer);
    }

    public void SetGrabbedItemLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        SetLayer(GameManager.getInstance().getPlayer().GetComponent<GrabItem>().getGrabbedObject().gameObject, layer);
    }
    
    public void SetLastGrabbedItemLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        SetLayer(GameManager.getInstance().getPlayer().GetComponent<GrabItem>().getLastGrabbedObject().gameObject, layer);
    }
}