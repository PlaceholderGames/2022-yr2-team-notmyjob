using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activator : MonoBehaviour
{

    [Header("How many executions allowed")]
    public int executions = 1;
    public float executeAfterSeconds = 1.5f;

    [Header("Events to execute"), Tooltip("Add events here when object enters collider")]
    public UnityEvent actionEvents;

    private int executionCount = 0;

    IEnumerator Execute(float seconds, UnityEvent action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
        executionCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody grabbedRigidbody = GameManager.getPlayer().GetComponent<GrabItem>().grabbedItem;
        GameObject grabbedItem = grabbedRigidbody != null ? grabbedRigidbody.gameObject : null;

        if(other != null && other.gameObject.tag == "ScalableObject")
        {
            // Peform activation
            if(other.gameObject != grabbedItem)
            {
                if(executionCount < executions) StartCoroutine(Execute(executeAfterSeconds, actionEvents));
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.tag == "ScalableObject")
        {
            executionCount--;
        }
    }
}
