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

    //TRY DOING JUST PLAY FOR THE SLIDER SOUND SO THEY CANT OVERLAP, THAT WAY I DONT NEED TO USE !AUDIO PLAYING
    //plays the sound
    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}