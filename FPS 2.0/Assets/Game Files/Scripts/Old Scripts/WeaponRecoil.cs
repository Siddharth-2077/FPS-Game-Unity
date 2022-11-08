

// Weapon-Recoil Script:

using UnityEngine;


public class WeaponRecoil : MonoBehaviour {

    [Space(10f)]
    [Header("Required Components:")]
    [SerializeField] private Transform fpsRigTransform = null;

    [Space(10f)]
    [Header("Recoil Parameters:")]
    [Tooltip(" X = RecoilX\n Y = RecoilY\n Z = RecoilZ")]
    [SerializeField] private Vector3 Recoil;
    [Tooltip(" X = Return-Speed\n Y = Snap-Speed")]
    [SerializeField] private Vector2 Speeds;

    private Vector3 currentrotation = Vector3.zero;
    private Vector3 targetRotation = Vector3.zero;


    public void Update() {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * Speeds.x);
        currentrotation = Vector3.Slerp(currentrotation, targetRotation, Time.deltaTime * Speeds.y);
        fpsRigTransform.localRotation = Quaternion.Euler(currentrotation);
    }


    // Public Member-Functions:

    public void SimulateRecoil() {
        targetRotation += new Vector3(-Recoil.x, Random.Range(-Recoil.y, Recoil.y), Random.Range(-Recoil.z, Recoil.z));
    }

    /// <summary>
    /// recoilXYZ contains recoilX, recoilY and recoilZ in order;
    /// speeds contains returnSpeed(X) and snapSpeed(Y)
    /// </summary>
    /// <param name="recoilXYZ"></param>
    /// <param name="speeds"></param>
    public void SetRecoilParameters(Vector3 recoilXYZ, Vector2 speeds) {
        Recoil.x = recoilXYZ.x;
        Recoil.y = recoilXYZ.y;
        Recoil.z = recoilXYZ.z;
        Speeds.x = speeds.x;
        Speeds.y = speeds.y;
    }

}