

// Gun - Script:


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GunType {None, SingleFire, MultipleFire}


[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour {

    // Public Parameters:
    public GunType gunType = GunType.None;
    public Animator animator { get; private set; }

    // Non-Serialized Private Parameters:
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private bool canFireAgain = true;
    private bool justFired = false;
    private float timer = 0f;
    private float timeBetweenShots = 0f;


    // Serialized Private Parameters:
    [Header("Weapon Properties:")]
    [Space(10f)]
    [SerializeField] private bool isFiring = false;
    [SerializeField] [Range(10f, 50f)] private float weaponRange = 10f;
    [SerializeField] private Transform shootOrigin = null;

    [Header("Weapon Recoil Parameters:")]
    [Space(10f)]
    [SerializeField] public Vector3 recoilParameters;
    [Tooltip("X = Return-Speed \nY = Snap-Speed")]
    [SerializeField] public Vector2 recoilSpeed;

    [Header("Weapon Audio")]
    [Space(10f)]
    public WeaponAudio audio = null;

    //[SerializeField] private List<ParticleSystem> muzzleFlash = null;
    //[SerializeField] private Image crosshairImage = null;

    // Debug Parameters:
    [Space(10f)]
    [Header("Debug Test:")]
    public Transform testSphere = null;


    private void Awake() {

        mainCamera = Camera.main;
        animator = GetComponent<Animator>();

    }

    private void Update() {

        Physics.Raycast(shootOrigin.position, mainCamera.transform.forward, out hit, weaponRange);
        testSphere.position = hit.point;

    }

    // Unity functions called when Object is Disabled or Destroyed:
    private void OnDisable() {
        ResetParameters();
    }

    private void OnDestroy() {
        ResetParameters();
    }




    // Public Member Functions:
    /// <summary>
    /// Fire the Current-Weapon
    /// </summary>
    public void StartFiring() {

        switch (gunType) {

            case GunType.SingleFire:                // Single-Fire Weapon
                Shoot_SingleFireWeapon();
                break;

            case GunType.MultipleFire:              // Multiple-Fire Weapon

                isFiring = true;
                animator.Play("Fire");
                /*foreach (ParticleSystem particle in muzzleFlash) {
                    particle.Emit(1);
                }*/
                break;

            case GunType.None: break;               // Default Case
                
        }   
    }


    /// <summary>
    /// Stop firing the Current-Weapon
    /// </summary>
    public void StopFiring() {
        isFiring = false;
    }


    // Private Member Functions:
    /// <summary>
    /// Handles the Shoot Functionality of a Single-Fire-Weapon
    /// </summary>
    private void Shoot_SingleFireWeapon() {
        isFiring = true;
        animator.Play("Fire");

        /*foreach (ParticleSystem particle in muzzleFlash)
            particle.Emit(1);*/

        justFired = true;
        timer = 0f;
    }


    /// <summary>
    /// Resets all the Parameters of the Weapon (Typically called when object is Disabled/Destroyed)
    /// </summary>
    private void ResetParameters() {
        canFireAgain = true;
        justFired = false;
        timer = 0f;
        timeBetweenShots = 0f;
    }


}