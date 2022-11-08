

// PlayerMovement - Script:


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

    private CharacterController controller = null;

    private float horizontal = 0f;
    private float vertical = 0f;
    private float verticalVelocity = 0f;

    [Space(10f)]
    [Header("Player Movement Properties:")]
    [SerializeField] [Range(0f, 7f)] private float playerSpeed = 4f;
    [SerializeField] [Range(2f, 7f)] private float jumpIntensity = 5f;
    [SerializeField] [Range(0f, 20f)] private float gravity = 10f;

    // FOR TESTING SAKE, WILL BE DE-SERIALIZED LATER
    [Space(10f)]
    [Header("Player Speed Properties:")]
    [SerializeField] [Range(1f, 4f)] private float walkSpeed = 3f;
    [SerializeField] [Range(4f, 6f)] private float sprintSpeed = 5f;
    private float idleSpeed = 0f;

    private Vector3 moveDirection = Vector3.zero;

    private bool walking = false;
    private bool sprinting = false;
    
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isJumping = false;

    [Space(10f)]
    [Header("Weapon Camera Properties:")]
    [SerializeField] private Animator weaponCameraAnimator = null;


    // Public Members:
    public Animator animator { get; set; }
    public bool switchingWeapons { get;  set; }


    private void Awake() {
        GetAllComponents();
    }


    private void Update() {
        GetInputsFromPlayer();
        MovePlayer();
    }



    // Private Helper Functions:

    /// <summary>
    /// Gets all the required components for this script
    /// </summary>
    private void GetAllComponents() {
        controller = GetComponent<CharacterController>();
    }


    /// <summary>
    /// Get all Inputs from the Player
    /// </summary>
    private void GetInputsFromPlayer() {   
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        HandleAnimationsAndPlayerSpeeds();

    }


    /// <summary>
    /// Moves the player on X-Z Plane:
    /// </summary>
    private void MovePlayer() {

        // Normalization prevents abnormally high diagnol-movement speed
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        // Converts to Worls-Space
        moveDirection = transform.TransformDirection(moveDirection);
        // Applies the Player-Speed
        moveDirection *= playerSpeed * Time.deltaTime;

        ApplyGravity();

        controller.Move(moveDirection);

        animator.SetBool("Walk", walking);
        animator.SetBool("Sprint", sprinting);

    }


    /// <summary>
    /// Simulates Gravity on the Player
    /// </summary>    
    private void ApplyGravity() {
        Jump();
        verticalVelocity -= gravity * Time.deltaTime;
        moveDirection.y = verticalVelocity * Time.deltaTime;
    }


    /// <summary>
    /// Allows the Player to Jump
    /// </summary>
    private void Jump() {

        isGrounded = controller.isGrounded;

        if (controller.isGrounded == true) {

            if (Input.GetKeyDown(KeyCode.Space)) {
                // Jump
                verticalVelocity = jumpIntensity;
                weaponCameraAnimator.SetTrigger("Jump");

            } else {
                // Prevents the player from falling off of ledges too quickly
                verticalVelocity = 0f;
            }

        }
    }
    

    /// <summary>
    /// Switches to the appropriate animations and player-speeds
    /// </summary>
    private void HandleAnimationsAndPlayerSpeeds() {

        if ((horizontal >= 0.2f || horizontal <= -0.2f) || (vertical >= 0.2f || vertical <= -0.2f)) {

            if (Input.GetKey(KeyCode.LeftShift) && (vertical > 0.5f || vertical < -0.5f) && !switchingWeapons) {        
                walking = false;                                            // Sprinting
                sprinting = true;

                playerSpeed = sprintSpeed;

            } else {                                                        // Walking
                walking = true;
                sprinting = false;

                playerSpeed = walkSpeed;

            }

        } else {                                                            // Idling
            walking = false;
            sprinting = false;

            playerSpeed = idleSpeed;

        }

    }




}
