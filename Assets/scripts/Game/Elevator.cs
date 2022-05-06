// Big thank you to Domiii for the script
//
// https://gist.github.com/Domiii/2e324291f250f5b878ceaf9b91969219


/**
 * A simple elevator script. Can be attached to any object to make it move in the given direction.
 * Requires a button that calls CallElevator().
 */

using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class Elevator : MonoBehaviour {
    /// <summary>
    /// The distance between two floors
    /// </summary>
    public Vector3 FloorDistance = Vector3.up;
    public float Speed = 1.0f;
    public int Floor = 0;
    public int MaxFloor = 1;
    public int MinFlooor = 0;
    public Transform moveTransform;
    
    [SerializeField] private bool needsPlayer = false;

    private float tTotal;
    private bool isMoving;
    private float moveDirection;
    private int nextFloor;


    // Use this for initialization
    void Start () {
        moveTransform = moveTransform ?? transform;
    }

    // Update is called once per frame
    void Update () {
        if (isMoving) {
            // elevator is moving
            MoveElevator();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (needsPlayer && other.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (needsPlayer && other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    void MoveElevator() {
        var v = moveDirection * FloorDistance.normalized * Speed;
        var t = Time.deltaTime;
        var tMax = FloorDistance.magnitude / Speed;
        t = Mathf.Min (t, tMax - tTotal);
        moveTransform.Translate(v * t);
        tTotal += t;

        if (tTotal >= tMax) {
            // we arrived on floor
            isMoving = false;
            tTotal = 0;
            Floor += (int)moveDirection;
        }
    }

    /// <summary>
    /// Start moving up one floor
    /// </summary>
    public void StartMoveUp() {
        if (Floor >= MaxFloor) return;
        
        if (isMoving)
            return;

        isMoving = true;
        moveDirection = 1;
    }

    /// <summary>
    /// Start moving down one floor
    /// </summary>
    public void StartMoveDown()
    {
        if (Floor <= MinFlooor) return;
        
        if(isMoving) return;

        isMoving = true;
        moveDirection = -1;
    }

    /// <summary>
    /// Tell the elevator to move up or down
    /// </summary>
    public void CallElevator() {
        if (isMoving)
            return;

        // start moving
        if (Floor < MaxFloor) {
            StartMoveUp ();
        }
        else {
            StartMoveDown ();
        }
    }
}

// End of Domiii's script
[CustomEditor(typeof(Elevator))]
public class ElevatorEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        var elevator = target as Elevator;
        
        if (GUILayout.Button("Call Elevator")) {
            elevator.CallElevator();
        }
        
        if (GUILayout.Button("Start Move Up")) {
            elevator.StartMoveUp();
        }
        
        
        if (GUILayout.Button("Start Move Down")) {
            elevator.StartMoveDown();
        }
        
    }
}