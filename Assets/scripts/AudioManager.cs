using System.Security.AccessControl;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Components")]
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip pickupSound;
    public AudioClip dieSound;
    public AudioClip hurtSound;
    public AudioClip fallSound;
    public AudioClip landSound;
    public AudioClip winSound;
    public AudioClip buttonSound;
    public AudioClip sliderSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //keeps audio playing across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //plays the sfx
    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    //plays the sfx wihtout overlapping. used for the slider sound
    public void PlayOneSFX(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}