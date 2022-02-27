using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activator : MonoBehaviour
{

    public float executeAfterSeconds = 1.5f;

    [Header("Events to execute"), Tooltip("Add events here when object enters collider")]
    public UnityEvent actionEvents;

    IEnumerator Execute(float seconds, UnityEvent action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody grabbedRigidbody = GameManager.getPlayer().GetComponent<GrabItem>().grabbedItem;
        GameObject grabbedItem = grabbedRigidbody != null ? grabbedRigidbody.gameObject : null;

        if(other != null && other.gameObject.tag == "ScalableObject")
        {
            // Peform activation
            if(other.gameObject != grabbedItem)
            {
                StartCoroutine(Execute(executeAfterSeconds, actionEvents));
            }

        }
    }
}
