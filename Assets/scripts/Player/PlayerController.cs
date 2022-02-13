using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [Tooltip("Reference to the main camera used for the player")]
    public Camera playerCamera;

    [Tooltip("Audio source for footsteps, jump, etc...")]
    public AudioSource audioSource;

    [Header("General")]
    [Tooltip("Force applied downward when in the air")]
    public float gravity = 20f;

    [Tooltip("Physic layers checked to consider the player grounded")]
    public LayerMask groundCheckLayers = -1;

    [Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
    public float groundCheckDistance = 0.05f;

    [Header("Movement")]
    [Tooltip("Max movement speed when grounded (when not sprinting)")]
    public float maxSpeedOnGround = 10f;

    [Tooltip(
        "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    public float movementSharpnessOnGround = 15;

    [Tooltip("Max movement speed when crouching")]
    [Range(0, 1)]
    public float maxSpeedCrouchedRatio = 0.5f;

    [Tooltip("Max movement speed when not grounded")]
    public float maxSpeedInAir = 10f;

    [Tooltip("Acceleration speed when in the air")]
    public float accelerationSpeedInAir = 25f;

    [Tooltip("Multiplicator for the sprint speed (based on grounded speed)")]
    public float sprintSpeedModifier = 2f;

    [Tooltip("Height at which the player dies instantly when falling off the map")]
    public float killHeight = -50f;

    [Header("Rotation")]
    [Tooltip("Rotation speed for moving the camera")]
    public float rotationSpeed = 200f;

    [Range(0.1f, 1f)]
    [Tooltip("Rotation speed multiplier when aiming")]
    public float aimingRotationMultiplier = 0.4f;

    [Header("Jump")]
    [Tooltip("Force applied upward when jumping")]
    public float jumpForce = 9f;

    [Header("Stance")]
    [Tooltip("Ratio (0-1) of the character height where the camera will be at")]
    public float cameraHeightRatio = 0.9f;

    [Tooltip("Height of character when standing")]
    public float capsuleHeightStanding = 1.8f;

    [Tooltip("Height of character when crouching")]
    public float capsuleHeightCrouching = 0.9f;

    [Tooltip("Speed of crouching transitions")]
    public float crouchingSharpness = 10f;

    [Header("Audio")]
    [Tooltip("Amount of footstep sounds played when moving one meter")]
    public float footstepSfxFrequency = 1f;

    [Tooltip("Amount of footstep sounds played when moving one meter while sprinting")]
    public float footstepSfxFrequencyWhileSprinting = 1f;

    [Tooltip("Sound played for footsteps")]
    public AudioClip footstepSfx;

    [Tooltip("Sound played when jumping")] public AudioClip jumpSfx;
    [Tooltip("Sound played when landing")] public AudioClip landSfx;

    [Tooltip("Sound played when taking damage froma fall")]
    public AudioClip fallDamageSfx;

    [Header("Fall Damage")]
    [Tooltip("Whether the player will recieve damage when hitting the ground at high speed")]
    public bool recievesFallDamage;

    [Tooltip("Minimun fall speed for recieving fall damage")]
    public float minSpeedForFallDamage = 10f;

    [Tooltip("Fall speed for recieving th emaximum amount of fall damage")]
    public float maxSpeedForFallDamage = 30f;

    [Tooltip("Damage recieved when falling at the mimimum speed")]
    public float fallDamageAtMinSpeed = 10f;

    [Tooltip("Damage recieved when falling at the maximum speed")]
    public float fallDamageAtMaxSpeed = 50f;

    // public UnityAction<bool> OnStanceChanged;

    public Vector3 CharacterVelocity { get; set; }
    public bool IsGrounded { get; private set; }
    public bool HasJumpedThisFrame { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsCrouching { get; private set; }

    public float RotationMultiplier
    {
        get { return 1f; }
    }

    PlayerInput playerInput;
    CharacterController controller;
    Vector3 groundNormal;
    Vector3 characterVelocity;
    Vector3 latestImpactSpeed;
    float lastTimeJumped = 0f;
    float cameraVerticalAngle = 0f;
    float footstepDistanceCounter;
    float targetCharacterHeight;

    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.07f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;

        controller.enableOverlapRecovery = true;

        SetCrouchingState(false, true);
        UpdateCharacterHeight(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead && transform.position.y < killHeight)
        {
            //Kill();
        }

        HasJumpedThisFrame = false;
        bool wasGrounded = IsGrounded;

        GroundCheck();

        // Handle fall damage

        UpdateCharacterHeight(true);
        HandleCharacterMovement();
    }

    void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance = IsGrounded ? (controller.skinWidth + groundCheckDistance) : k_GroundCheckDistanceInAir;

        // reset values before the ground check
        IsGrounded = false;
        groundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= lastTimeJumped + k_JumpGroundingPreventionTime)
        {
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(controller.height),
                controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers,
                QueryTriggerInteraction.Ignore))
            {
                // storing the upward direction for the surface found
                groundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(groundNormal))
                {
                    IsGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > controller.skinWidth)
                    {
                        controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    void HandleCharacterMovement()
    {
        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(
                new Vector3(0f, (playerInput.getLook(PlayerDirection.HORIZONTAL) * rotationSpeed * RotationMultiplier),
                    0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            cameraVerticalAngle += playerInput.getLook(PlayerDirection.VERTICAL) * rotationSpeed * RotationMultiplier;

            // limit the camera's vertical angle to min/max
            cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
        }

        // character movement handling
        bool isSprinting = playerInput.handleButtonInput("Sprint");
        {
            if (isSprinting)
            {
                isSprinting = SetCrouchingState(false, false);
            }

            float speedModifier = isSprinting ? sprintSpeedModifier : 1f;

            // converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput = transform.TransformVector(playerInput.getMove());

            // handle grounded movement
            if (IsGrounded)
            {
                // calculate the desired velocity from inputs, max speed, and current slope
                Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;
                // reduce speed if crouching by crouch speed ratio
                if (IsCrouching) targetVelocity *= maxSpeedCrouchedRatio;
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, groundNormal) *
                                 targetVelocity.magnitude;

                // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                    movementSharpnessOnGround * Time.deltaTime);

                // jumping
                if (IsGrounded && playerInput.handleButtonInput("Jump"))
                {
                    // force the crouch state to false
                    if (SetCrouchingState(false, false))
                    {
                        // start by canceling out the vertical component of our velocity
                        CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);

                        // then, add the jumpSpeed value upwards
                        CharacterVelocity += Vector3.up * jumpForce;

                        // play sound
                        //AudioSource.PlayOneShot(jumpSfx);

                        // remember last time we jumped because we need to prevent snapping to ground for a short time
                        lastTimeJumped = Time.time;
                        HasJumpedThisFrame = true;

                        // Force grounding to false
                        IsGrounded = false;
                        groundNormal = Vector3.up;
                    }
                }

                // footsteps sound
                float chosenFootstepSfxFrequency =
                    (isSprinting ? footstepSfxFrequencyWhileSprinting : footstepSfxFrequency);
                if (footstepDistanceCounter >= 1f / chosenFootstepSfxFrequency)
                {
                    footstepDistanceCounter = 0f;
                    //AudioSource.PlayOneShot(footstepSfx);
                }

                // keep track of distance traveled for footsteps sound
                footstepDistanceCounter += CharacterVelocity.magnitude * Time.deltaTime;
            }
            // handle air movement
            else
            {
                // add air acceleration
                CharacterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;

                // limit air speed to a maximum, but only horizontally
                float verticalVelocity = CharacterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);
                CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // apply the gravity to the velocity
                CharacterVelocity += Vector3.down * gravity * Time.deltaTime;
            }
        }

        // apply the final calculated velocity value as a character movement
        Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
        Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(controller.height);
        controller.Move(CharacterVelocity * Time.deltaTime);

        // detect obstructions to adjust velocity accordingly
        latestImpactSpeed = Vector3.zero;
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius,
            CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1,
            QueryTriggerInteraction.Ignore))
        {
            // We remember the last impact speed because the fall damage logic might need it
            latestImpactSpeed = CharacterVelocity;

            CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
        }
    }

    void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            controller.height = targetCharacterHeight;
            controller.center = Vector3.up * controller.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.up * targetCharacterHeight * cameraHeightRatio;
            // m_Actor.AimPoint.transform.localPosition = controller.center;
        }

        // Update smooth height
        else if (controller.height != targetCharacterHeight)
        {
            // resize the capsule and adjust camera position
            controller.height = Mathf.Lerp(controller.height, targetCharacterHeight, crouchingSharpness * Time.deltaTime);
            controller.center = Vector3.up * controller.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, Vector3.up * targetCharacterHeight * cameraHeightRatio, crouchingSharpness * Time.deltaTime);
            //m_Actor.AimPoint.transform.localPosition = m_Controller.center;
        }
    }

    bool SetCrouchingState(bool crouched, bool ignoreObstructions)
    {
        // set appropriate heights
        if (crouched)
        {
            targetCharacterHeight = capsuleHeightCrouching;
        }
        else
        {
            // Detect obstructions
            if (!ignoreObstructions)
            {
                Collider[] standingOverlaps = Physics.OverlapCapsule(
                    GetCapsuleBottomHemisphere(),
                    GetCapsuleTopHemisphere(capsuleHeightStanding),
                    controller.radius,
                    -1,
                    QueryTriggerInteraction.Ignore);
                foreach (Collider c in standingOverlaps)
                {
                    if (c != controller)
                    {
                        return false;
                    }
                }
            }

            targetCharacterHeight = capsuleHeightStanding;
        }

        /*if (OnStanceChanged != null)
        {
            OnStanceChanged.Invoke(crouched);
        }*/

        IsCrouching = crouched;
        return true;
    }

    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * controller.radius);
    }

    // Gets the center point of the top hemisphere of the character controller capsule    
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - controller.radius));
    }

    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= controller.slopeLimit;
    }
}
