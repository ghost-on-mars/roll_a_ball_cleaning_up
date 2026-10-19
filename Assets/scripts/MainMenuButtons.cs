using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UseButtons : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer mixer;
    public Button cheatsPanel;
    public Button settingsPanel;
    public GameObject xButton; 
    public GameObject buttons;
    public Slider musicSlider;
    public Slider SFXSlider;

    public static float savedMusicVol;
    public static float savedSFXVol;
    public static bool firstLoad = true;

    public AudioSource audio;
    public bool playSlider = false;

    void Awake() //DOESNT WORK
    {
        // Automatically fetches the fresh AudioSource when the scene reloads
        audio = GetComponent<AudioSource>(); 
    }

    void Start()
    {
        //DontDestroyOnLoad(audio); //hopefully fixes it

        cheatsPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);

        if (firstLoad == true) //sets the default numbers
        {
            savedMusicVol=0;
            savedSFXVol=-3;
        }
        
        //sets the volume to the saved one
        mixer.SetFloat("musicVol", savedMusicVol);
        mixer.SetFloat("sfxVol", savedSFXVol);
        musicSlider.value = savedMusicVol;
        SFXSlider.value = savedSFXVol;

        firstLoad = false;
    
    }
    public void PlayGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
        SceneManager.LoadScene("MainGameScene"); 
    }
    public void OpenCheats()
    {
        cheatsPanel.gameObject.SetActive(true);
        buttons.gameObject.SetActive(false);

        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
    }
    public void CloseCheats()
    {
        cheatsPanel.gameObject.SetActive(false);
        buttons.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
    }
    public void OpenSettings()
    {
        playSlider=true;
        settingsPanel.gameObject.SetActive(true);
        buttons.gameObject.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
    }
    public void CloseSettings()
    {
        settingsPanel.gameObject.SetActive(false);
        buttons.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
    }
    public void QuitGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
        #if UNITY_EDITOR
            // Exits Play Mode inside the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //closes the application
            Application.Quit();
        #endif
    }

    //Is there a better way to do this? like if the sliders could both share this function and give a parameter of "sfxVol" 
    public void ChangeSFXVol(float volume)
    {
        mixer.SetFloat("sfxVol", volume);
        savedSFXVol = volume;
        if (audio == null)
        {
            Debug.Log("missing reference; " + gameObject.name);
        }
        //this stops working after the first time
        if (!audio.isPlaying && playSlider == true) 
        {
            
            AudioManager.instance.PlaySFX(AudioManager.instance.sliderSound);
        }
        //both dont work
        //if (!audio.isPlaying && playSlider == true) 
        //if (!audioScript.audioSource.isPlaying && playSlider == true) 


        //Debug.Log("in change volume" + volume);
        //AudioManager.instance.PlaySFX(AudioManager.instance.sliderSound);
        /*if (slider == false)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
        }
        slider = true;*/

        //PlayerPrefs.SetFloat("GameVolume", volume); // Saves settings between sessions
    }
    public void ChangeMusicVol(float volume)
    {
        mixer.SetFloat("musicVol", volume);
        savedMusicVol=volume;
        if (audio == null)
        {
            Debug.Log("missing reference; " + gameObject.name);
        }
        //doesnt work after the first time
        if (!audio.isPlaying && playSlider == true)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sliderSound);
        }
        
    }
}
