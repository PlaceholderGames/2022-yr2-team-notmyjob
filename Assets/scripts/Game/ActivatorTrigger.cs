using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorTrigger : MonoBehaviour
{
    [Header("Properties"), SerializeField, Tooltip("Only activate when this tag enters trigger")] private bool _specifyTags = false;
    [SerializeField] private List<string> _tags = new List<string>();

    [Space, SerializeField] private bool specificScale = true;
    [SerializeField] private Vector3 scale = new Vector3(1, 1, 1);
    
    [Space, Header("Actions to execute"), SerializeField] private UnityEvent OnTriggerEntered;
    [SerializeField] private UnityEvent OnTriggerStayed;
    [SerializeField] private UnityEvent OnTriggerExited;


    private void OnTriggerEnter(Collider other)
    {
        if(_specifyTags && !_tags.Contains(other.tag)) return;
        if(specificScale && other.transform.localScale != scale) return;
        
        OnTriggerEntered.Invoke();
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(_specifyTags && !_tags.Contains(other.tag)) return;
        if(specificScale && other.transform.localScale != scale) return;
        
        OnTriggerStayed.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if(_specifyTags && !_tags.Contains(other.tag)) return;
        if(specificScale && other.transform.localScale != scale) return;
        
        OnTriggerExited.Invoke();
    }
}
