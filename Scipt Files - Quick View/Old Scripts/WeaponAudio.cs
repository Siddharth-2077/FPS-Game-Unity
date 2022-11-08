

// WeaponAudio - Script:

using UnityEngine;


public class WeaponAudio : MonoBehaviour {
    
    [Header("Audio Components:")]
    [Space(10f)]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip audioClip = null;
    [SerializeField] [Range(0f, 1f)] private float volume = 1.0f;

    public void PlayOneShot() {
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void Play() {
        audioSource.Play();
    }

}
