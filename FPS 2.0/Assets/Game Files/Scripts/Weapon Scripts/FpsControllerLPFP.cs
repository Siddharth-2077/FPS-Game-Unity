
using System;
using UnityEngine;

// Namespace
namespace FPSControllerLPFP {

    /// Manages a first person character
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class FpsControllerLPFP : MonoBehaviour {

        // PLAYER MOVEMENT COMPONENTS:
        [Header("Player Movement Properties:")]
        [Space(10f)]
        [SerializeField] [Range(0f, 7f)] private float playerSpeed = 4f;
        [SerializeField] [Range(2f, 7f)] private float jumpIntensity = 5f;
        [SerializeField] [Range(0f, 20f)] private float gravity = 10f;

        // FOR TESTING SAKE, WILL BE DE-SERIALIZED LATER
        [Header("Player Speed Properties:")]
        [Space(10f)]
        [SerializeField] [Range(1f, 4f)] private float walkSpeed = 3f;
        [SerializeField] [Range(4f, 6f)] private float sprintSpeed = 5f;

        private CharacterController characterController;
        private bool isGrounded;

        private Vector3 moveDirection = Vector3.zero;

        private readonly float idleSpeed = 0f;
        private float horizontal;
        private float vertical;
        private float verticalVelocity;


        // FREE LOOK COMPONENTS:
        [Header("Mandatory Transform Components:")]
        [Space(10f)]
        [Tooltip("Assign the Transform of the FPS Player")]
        [SerializeField] Transform playerTransform;
        [Tooltip("Assign the Transform of the FPS Camera-Rig")]
        [SerializeField] Transform cameraRigTransform;

        [Header("First-Person Camera Properties:")]
        [Space(10f)]
        [Tooltip("Invert the Up-Down rotation of the Camera \n Ticked by Default")]
        [SerializeField] private bool invertX;
        [Tooltip("Invert the Left-Right rotation of the Camera \n Un-Ticked by Default")]
        [SerializeField] private bool invertY;
        [Tooltip("The Rotation-Sensitivity of the FPS-Camera")]
        [SerializeField] [Range(0.5f, 5f)] private float cameraSensitivity = 1f;
        [Tooltip("X-Rotation limit values for the FPS-Camera")]
        [SerializeField] private Vector2 cameraXLimits = new Vector2(-60f, 60f);
        
        [Header("Camera Damping Properties:")]
        [Space(10f)]
        [SerializeField] [Range(5f, 30f)] private float dampingX = 5f;
        [SerializeField] [Range(5f, 30f)] private float dampingY = 5f;        

        private Vector2 currentMouseLook = Vector2.zero;
        private Vector2 lookAngle = Vector2.zero;

        [Header("Settings Menu Component")]
        [Space(10f)]
        [SerializeField] private SettingsMenu settingsMenu;

        [Header("Arms Components")]
        [Space(10f)]
        [Tooltip("The transform component that holds the gun camera."), SerializeField]
        private Transform arms;
        [Tooltip("The position of the arms and gun camera relative to the fps controller GameObject."), SerializeField]
        private Vector3 armPosition;

		[Header("Audio Clips")]
        [Space(10f)]
        [Tooltip("The audio clip that is played while walking."), SerializeField]
        private AudioClip walkingSound;
        [Tooltip("The audio clip that is played while running."), SerializeField]
        private AudioClip runningSound;
        private AudioSource audioSource;
        

        // Initializes the FpsController on start.
        private void Start() {
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            characterController = GetComponent<CharacterController>();
            
            audioSource = GetComponent<AudioSource>();
			arms = AssignCharactersCamera();
            audioSource.clip = walkingSound;
            audioSource.loop = true;            
            
        }
			
        private Transform AssignCharactersCamera() {
            var t = transform;
			arms.SetPositionAndRotation(t.position, t.rotation);
			return arms;
        }

        // Moves the camera to the character, processes jumping and plays sounds every frame.
        private void Update() {

            if (settingsMenu.gamePaused == false) {
                
                GetMouseInputs();
                
                isGrounded = characterController.isGrounded;

                GetInputsFromPlayer();

                arms.position = transform.position + transform.TransformVector(armPosition);

                MoveCharacter();
                Jump();
                PlayFootstepSounds();
            }

        }
        

        private void GetInputsFromPlayer() {

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            HandleAnimationsAndPlayerSpeeds();

        }

        private void HandleAnimationsAndPlayerSpeeds() {

            if ((horizontal >= 0.2f || horizontal <= -0.2f) || (vertical >= 0.2f || vertical <= -0.2f)) {

                if (Input.GetKey(KeyCode.LeftShift) && (vertical > 0.5f || vertical < -0.5f)/* && !switchingWeapons*/) {
                    //walking = false;                                            // Sprinting
                    //sprinting = true;

                    playerSpeed = sprintSpeed;

                } else {                                                        // Walking
                    //walking = true;
                    //sprinting = false;

                    playerSpeed = walkSpeed;
                }

            } else {                                                            // Idling
                //walking = false;
                //sprinting = false;

                playerSpeed = idleSpeed;

            }
        }

        private void GetMouseInputs() {

            float mouseY = Input.GetAxis("MouseY");
            float mouseX = Input.GetAxis("MouseX");            
            
            currentMouseLook = new Vector3(mouseY, mouseX, 0);

            lookAngle.x += currentMouseLook.x * cameraSensitivity * (invertX ? -1f : 1f);
            lookAngle.y += currentMouseLook.y * cameraSensitivity * (invertY ? -1f : 1f);

            lookAngle.x = Mathf.Clamp(lookAngle.x, cameraXLimits.x, cameraXLimits.y);

            LookAround();

        }

        
        private void LookAround() {

            if (playerTransform && cameraRigTransform) {
                
                Quaternion rotationX = Quaternion.Euler(lookAngle.x, 0f, 0f);
                Quaternion rotationY = Quaternion.Euler(0f, lookAngle.y, 0f);

                cameraRigTransform.localRotation = Quaternion.Slerp(cameraRigTransform.localRotation, rotationX, dampingX * Time.deltaTime);
                playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, rotationY, dampingY * Time.deltaTime);
                
                // Up-Down rotation:
                //cameraRigTransform.localRotation = Quaternion.Euler(lookAngle.x, 0f, 0f);

                // Left-Right rotation:
                //playerTransform.localRotation = Quaternion.Euler(0f, lookAngle.y, 0f);

            }

        }
        

        
        // CUSTOM DEFINITION:
        private void MoveCharacter() {
            
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            // Converts to World-Space
            moveDirection = transform.TransformDirection(moveDirection);
            // Applies the Player-Speed
            moveDirection *= playerSpeed * Time.deltaTime;

            ApplyGravity();

            characterController.Move(moveDirection);
            
        }

        // CUSTOM DEFINITION:
        private void ApplyGravity() {
            
            Jump();
            verticalVelocity -= gravity * Time.deltaTime;
            moveDirection.y = verticalVelocity * Time.deltaTime;
            
        }

        private void Jump() {

            isGrounded = characterController.isGrounded;

            if (characterController.isGrounded) {
                
                verticalVelocity = Input.GetKeyDown(KeyCode.Space) ? jumpIntensity : 0f;
                
            }
            
        }

        

        private void PlayFootstepSounds() {
            
            if (isGrounded && characterController.velocity.sqrMagnitude > 0.1f) {
                
                audioSource.clip = Input.GetButton("Fire3") ? runningSound : walkingSound;
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
                
            } else {
                
                if (audioSource.isPlaying) {
                    audioSource.Pause();
                }
            }
            
        }
        

    } // END OF CLASS
    
    
}