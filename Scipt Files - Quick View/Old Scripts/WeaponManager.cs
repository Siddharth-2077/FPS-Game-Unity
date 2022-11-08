

// WeaponManager - Script:


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour {
    
    [Space(10f)]
    [Header("Script References:")]
    [SerializeField] private PlayerMovement playerMovementScript = null;

    [Space(20f)]
    [SerializeField] private List<Gun> Guns = new List<Gun>();

    [Space(10f)]
    [SerializeField] private Gun currentWeapon = null;

    [Space(10f)]
    [SerializeField] private Gun primaryWeapon = null;
    [SerializeField] private Gun secondaryWeapon = null;


    private bool canSwitchWeapon = false;
    private bool isAiming = false;
    private bool isFiring = false;

    // Mouse-Scroll Data used to Switch Weapons
    private float scrollScale = 0.75f;
    private float scrollValue = 0f;

    private WeaponRecoil recoil = null;

    private float lastFired = 0f;
    private float fireRate = 5f;


    private void Awake() {
        
        if (Guns.Count != 0) {

            int i = 0;

            while (primaryWeapon == null || secondaryWeapon == null) {

                if (Guns[i].gunType == GunType.SingleFire && primaryWeapon == null) {

                    primaryWeapon = Guns[i];

                }

                if (Guns[i].gunType == GunType.MultipleFire && secondaryWeapon == null) {

                    secondaryWeapon = Guns[i];

                }

                Guns[i].gameObject.SetActive(false);
                ++i;

            }
        }  
        
        recoil = gameObject.GetComponent<WeaponRecoil>();

    }


    private void Start() {

        if (primaryWeapon != null) {

            currentWeapon = primaryWeapon;

            primaryWeapon.gameObject.SetActive(true);
            secondaryWeapon.gameObject.SetActive(false);

        } else if (secondaryWeapon != null) {

            currentWeapon = secondaryWeapon;

            primaryWeapon.gameObject.SetActive(false);
            secondaryWeapon.gameObject.SetActive(true);

        } else { 
            
            Debug.LogWarning("ERROR: Both Primary and Secondary weapons are Non-Existent!"); 
        
        }

        // Assign the recoil parameters of the default gun:
        recoil.SetRecoilParameters(currentWeapon.recoilParameters, currentWeapon.recoilSpeed);

        canSwitchWeapon = true;
        playerMovementScript.animator = currentWeapon.animator;

    }



    private void Update() {

        SwitchWeapons();

        ShootWeapon();

        //AimInAndOut();              // To be implemented...

    }


    // Member Functions:
    /// <summary>
    /// Reads Mouse-Scroll Data and allows Swapping between the Primary and Secondary Weapons
    /// </summary>
    private void SwitchWeapons() {

        scrollValue += Input.mouseScrollDelta.y * scrollScale;

        if (scrollValue > 1f || scrollValue < -1f) {

            StartCoroutine(SwapWeapon(scrollValue));

        }

    }


    /// <summary>
    /// Swap between Primary and Secondary Weapons (if they exist) based on the Mouse-Scroll Data
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerator SwapWeapon(float value) {

        canSwitchWeapon = false;

        // Primary Weapon -> Secondary Weapon
        if (value < -1f && playerMovementScript.switchingWeapons == false &&
            secondaryWeapon != null && currentWeapon != secondaryWeapon) {

            // Holster the Current-Weapon (Primary-Weapon)
            /*primaryWeapon.animator.SetTrigger("Holster");

            playerMovementScript.switchingWeapons = true;*/

            // Wait for the Animation-End
            yield return new WaitForSeconds(0f);

            // Swap Weapons
            primaryWeapon.gameObject.SetActive(false);
            secondaryWeapon.gameObject.SetActive(true);

            // Reset the Scroll-Value
            scrollValue = 0f;

            // Assign the new Weapon as the Current-Weapon
            currentWeapon = secondaryWeapon;

            // Setup recoil for the new weapon:
            recoil.SetRecoilParameters(currentWeapon.recoilParameters, currentWeapon.recoilSpeed);

            playerMovementScript.switchingWeapons = false;

            canSwitchWeapon = true;
            playerMovementScript.animator = currentWeapon.animator;


        // Secondary Weapon -> Primary Weapon
        } else if (value > 1f && playerMovementScript.switchingWeapons == false && 
                   primaryWeapon != null && currentWeapon != primaryWeapon) {                   

            // Holster the Current-Weapon (Primary-Weapon)
            /*secondaryWeapon.animator.SetTrigger("Holster");

            playerMovementScript.switchingWeapons = true;*/

            // Wait for the Animation-End
            yield return new WaitForSeconds(0f);

            secondaryWeapon.gameObject.SetActive(false);
            primaryWeapon.gameObject.SetActive(true);

            // Reset the Scroll-Value
            scrollValue = 0f;

            // Assign the new Weapon as the Current-Weapon
            currentWeapon = primaryWeapon;

            // Setup recoil for the new weapon:
            recoil.SetRecoilParameters(currentWeapon.recoilParameters, currentWeapon.recoilSpeed);

            playerMovementScript.switchingWeapons = false;

            canSwitchWeapon = true;
            playerMovementScript.animator = currentWeapon.animator;


        // Reset the Scroll-Value if Scroll-Data points to the Same Weapon
        } else { 
            
            scrollValue = 0f; 
        
        }

    }


    /// <summary>
    /// Implements Shooting Functionality of the Currently-Selected Weapon
    /// </summary>
    private void ShootWeapon() {

        if (playerMovementScript.switchingWeapons == false /* && inspecting == false*/) {

            // For Single-Fire Weapons
            if (currentWeapon.gunType == GunType.SingleFire) {

                if (Input.GetMouseButtonDown(0)) {

                    if (Time.time - lastFired > 1 / fireRate) {
                        //Debug.Log(Time.time);
                        //Debug.Log(" LastShot + FireRate = " + (lastShotTime + currentWeapon.fireRate));

                        //if (Time.time > nextFire) {
                        //lastShotTime = Time.time;
                        currentWeapon.StartFiring();
                        lastFired = Time.time;

                        //nextFire = Time.time + (1/currentWeapon.fireRate);
                        recoil.SimulateRecoil();
                        // Implement Audio
                        currentWeapon.audio.Play();                            // TEST ==========  
                        isFiring = true;
                    }

                }

                if (Input.GetMouseButtonUp(0)) {

                    currentWeapon.StopFiring();

                    isFiring = false;
                }


            // For Multiple-Fire Weapons
            } else if (currentWeapon.gunType == GunType.MultipleFire) {

                if (Input.GetMouseButton(0)) {

                    currentWeapon.StartFiring();
                    recoil.SimulateRecoil();
                    isFiring = true;
                }

                if (Input.GetMouseButtonUp(0)) {

                    currentWeapon.StopFiring();
                    isFiring = false;
                }            
            
            } 
            
        }

    }


    private void AimInAndOut() {

        if (Input.GetMouseButtonDown(1) && isFiring == false) {

            if (isAiming == false) {

                if (isFiring == false) {
                    
                    currentWeapon.animator.SetBool("Aim In", true);
                    currentWeapon.animator.SetBool("Aim Out", false);
                    isAiming = true;
                }

            } else {

                if (isFiring == false) {

                    currentWeapon.animator.SetBool("Aim In", false);
                    currentWeapon.animator.SetBool("Aim Out", true);
                    isAiming = false;

                }

            }

        }

    }


}