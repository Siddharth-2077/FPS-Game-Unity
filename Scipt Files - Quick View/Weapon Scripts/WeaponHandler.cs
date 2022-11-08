
// WeaponHandler - Script:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    private Weapon[] weaponArr;

    [Header("Debug Variables:"), Space(10f)]
    [SerializeField] private float scrollScale = 0.3f;
    [SerializeField] [Range(-1.5f, 1.5f)] private float scrollValue = 0f;

    [Header("Weapon Lists:"), Space(10f)]
    [SerializeField] private List<Weapon> primaryWeapons;
    [SerializeField] private List<Weapon> secondaryWeapons;

    [Header("Weapons:"), Space(10f)]
    [SerializeField] private Weapon currentWeapon = null;
    [SerializeField] private WeaponType currentType = WeaponType.None;
    [Space(10f)]
    [SerializeField] private Weapon primary = null;
    [SerializeField] private Weapon secondary = null;


    private void Start() {
        weaponArr = GetComponentsInChildren<Weapon>();
        TransferToLists();
        GetPrimaryAndSecondaryWeapons();
        AssignInitialWeapon();
    }


    private void LateUpdate() {
        GetMouseScrollData();
        SwitchWeapons();
    }


    private void TransferToLists() {
        for (int i = 0; i < weaponArr.Length; i++) {
            if (weaponArr[i].type == WeaponType.Primary)
                primaryWeapons.Add(weaponArr[i]);
            else if (weaponArr[i].type == WeaponType.Secondary)
                secondaryWeapons.Add(weaponArr[i]);
        }
        weaponArr = null;
    }


    /// <summary>
    /// Assigns the Primary and Secondary weapon slots (if they exist)
    /// </summary>
    private void GetPrimaryAndSecondaryWeapons() {
        // Get Primary-Weapon
        if (primaryWeapons.Count >= 1) {
            primary = primaryWeapons[0];
        } else {
            Debug.LogWarning("WARNING: No Primary Weapon exists/found !");
        }

        // Get Secondary-Weapon
        if (secondaryWeapons.Count >= 1) {
            secondary = secondaryWeapons[0];
        } else {
            Debug.LogWarning("WARNING: No Secondary Weapon exists/found !");
        }
    }


    /// <summary>
    /// Called initially, in the Start() function
    /// </summary>
    private void AssignInitialWeapon() {

        if (primary != null) {
            currentWeapon = primary;
            currentType = currentWeapon.type;

            currentWeapon.gameObject.SetActive(true);
            DisableOtherWeapons();

        } else if (secondary != null) {
            currentWeapon = secondary;
            currentType = currentWeapon.type;

            currentWeapon.gameObject.SetActive(true);
            DisableOtherWeapons();

        } else {
            currentWeapon = null;
            Debug.LogWarning("WARNING: No Weapons (Primary & Secondary) found/exist !");
        }

    }


    /// <summary>
    /// Disables all other weapon instances, except the currently active weapon
    /// </summary>
    private void DisableOtherWeapons() {
        for (int i = 0; i < primaryWeapons.Count; i++) {
            if (primaryWeapons[i] != currentWeapon)
                primaryWeapons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < secondaryWeapons.Count; i++) {
            if (secondaryWeapons[i] != currentWeapon)
                secondaryWeapons[i].gameObject.SetActive(false);
        }
    }


    private void GetMouseScrollData() {
        scrollValue += Input.mouseScrollDelta.y * scrollScale;
        if (currentType == WeaponType.Primary && scrollValue > 0f) { scrollValue = 0f; }
        if (currentType == WeaponType.Secondary && scrollValue < 0f) { scrollValue = 0f; }
    }

    private void ResetScrollData() { scrollValue = 0f; }

    private void SwitchWeapons() {

        if (scrollValue > 1f) {             // Assign Primary (Up-Scroll)

            if (currentType == WeaponType.Primary) {
                // Do nothing
                ResetScrollData();

            } else if (currentType == WeaponType.Secondary) {
                // Assign the primary weapon
                if (primary != null) {
                    currentWeapon.gameObject.SetActive(false);
                    currentWeapon = primary;
                    currentType = currentWeapon.type;
                    currentWeapon.gameObject.SetActive(true);
                    //DisableOtherWeapons();
                }
                ResetScrollData();
            }

        } else if (scrollValue < -1f) {     // Assign Secondary (Down-Scroll)

            if (currentType == WeaponType.Secondary) {
                // Do nothing
                ResetScrollData();

            } else if (currentType == WeaponType.Primary) {
                // Assign the primary weapon
                if (secondary != null) {
                    currentWeapon.gameObject.SetActive(false);
                    currentWeapon = secondary;
                    currentType = currentWeapon.type;
                    currentWeapon.gameObject.SetActive(true);
                    //DisableOtherWeapons();
                }
                ResetScrollData();
            }
        }
    }

}