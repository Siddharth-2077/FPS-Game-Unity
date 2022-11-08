

// FreeLook - Script:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FreeLook : MonoBehaviour {

    [Space(10f)] 
    [Header("Mandatory Transform Components:")]
    [Tooltip("Assign the Transform of the FPS Player")]
    [SerializeField] Transform playerTransform = null;
    [Tooltip("Assign the Transform of the FPS Camera-Rig")]
    [SerializeField] Transform cameraRigTransform = null; 

    [Space(10f)]
    [Header("First-Person Camera Properties:")]
    [Tooltip("Invert the Up-Down rotation of the Camera \n Ticked by Default")]
    [SerializeField] private bool invertX = true;
    [Tooltip("Invert the Left-Right rotation of the Camera \n Un-Ticked by Default")]
    [SerializeField] private bool invertY = false;
    [Tooltip("Is the Cursor visible in the Game-Window")]
    [SerializeField] bool cursorVisible = false;
    [Tooltip("The Rotation-Sensitivity of the FPS-Camera")]
    [SerializeField] [Range(0.5f, 5f)] private float cameraSensitivity = 1f;
    [Tooltip("X-Rotation limit values for the FPS-Camera")]
    [SerializeField] private Vector2 cameraXLimits = new Vector2(-60f, 60f);

    

    private Vector2 currentMouseLook = Vector2.zero;
    private Vector2 lookAngle = Vector2.zero;


    private void Start() {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 

    }


    private void Update() {

        LockAndUnlockCursor();

        cursorVisible = Cursor.visible;

        if (Cursor.visible == false)
            GetMouseInputs();

    }


    private void LateUpdate() {

        if (Cursor.visible == false)
            LookAround();

    }

    
    private void GetMouseInputs() {

        currentMouseLook = new Vector2(Input.GetAxis("MouseY"), Input.GetAxis("MouseX"));

        lookAngle.x += currentMouseLook.x * cameraSensitivity * (invertX ? -1f : 1f);
        lookAngle.y += currentMouseLook.y * cameraSensitivity * (invertY ? -1f : 1f);

        lookAngle.x = Mathf.Clamp(lookAngle.x, cameraXLimits.x, cameraXLimits.y);

    }

    private void LookAround() {

        if (playerTransform != null && cameraRigTransform != null) {

            // Up-Down rotation:
            cameraRigTransform.localRotation = Quaternion.Euler(lookAngle.x, 0f, 0f);

            // Left-Right rotation:
            playerTransform.localRotation = Quaternion.Euler(0f, lookAngle.y, 0f);

        }

    }


    private void LockAndUnlockCursor() {

        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (Cursor.visible == true) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                

            } else {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                

            }

        }

    }


}