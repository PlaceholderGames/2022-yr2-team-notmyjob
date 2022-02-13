using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    CharacterController controller;
    PlayerInput playerInput;


    public Camera playerCamera;

    [Header("Forces")]
    public float playerSpeed = 10f;
    public float jumpHeight = 2f;

    [Header("Rotation")]
    [Tooltip("Player rotation (Camera rotation) speed")]
    public float cameraRotationSpeed = 200f;

    [Header("Ground")]
    public Transform groundCheck;
    public float groundDistance = 0.05f;
    public LayerMask groundLayer;

    bool isGrounded;
    bool wasGrounded;

    float cameraVerticalAngle = 0f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;

        wasGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        updateCharacterMovement();
    }

    void updateCharacterMovement()
    {
        // Camera movement
        Vector3 facing = new Vector3(0f, (playerInput.getLook(PlayerDirection.HORIZONTAL) * cameraRotationSpeed), 0f);
        transform.Rotate(facing);

        cameraVerticalAngle += playerInput.getLook(PlayerDirection.VERTICAL) * cameraRotationSpeed;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -45f, 45);
        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);

        // Player Movement
    }
}
