using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UseButtons : MonoBehaviour
{
    //UI
    public Button cheatsPanel;
    public Button settingsPanel;
    public GameObject xButton; 
    public GameObject buttons;
    public Slider musicSlider;
    public Slider SFXSlider;

    //Changing volume stuff
    public UnityEngine.Audio.AudioMixer mixer;
    public AudioSource audio;
    public static float savedMusicVol;
    public static float savedSFXVol;
    public bool playSliderSFX = false;

    public static bool firstLoad = true;

    void Awake() //DOESNT WORK
    {
        // Automatically fetches the fresh AudioSource when the scene reloads
        audio = GetComponent<AudioSource>(); 
    }

    void Start()
    {
        //DontDestroyOnLoad(audio); //hopefully fixes it

        //hides the extra panels
        cheatsPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);

        //sets the default numbers
        if (firstLoad == true) 
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
        playSliderSFX=true; //sets this to true so the slider sfx can play later
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
            //exits play mode inside the editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //closes the application
            Application.Quit();
        #endif
    }
    public void ChangeSFXVol(float volume)
    {
        mixer.SetFloat("sfxVol", volume);
        savedSFXVol = volume;
        if (audio == null)
        {
            Debug.Log("missing reference; " + gameObject.name);
        }
        //TRY DOING JUST PLAY INSTEAD OF PLAYONESHOT IN THE AUDIO SCRIPT FOR THE SLIDER SOUND SO THEY CANT OVERLAP, THAT WAY I DONT NEED TO USE !AUDIO PLAYING

        //doesnt work after the first time due to the camera being destroyed 
        //and audio not beingg reassigned to it
        if (!audio.isPlaying && playSliderSFX == true) 
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sliderSound);
        }

        //both dont work
        //if (!audio.isPlaying && playSliderSFX == true) 
        //if (!audioScript.audioSource.isPlaying && playSliderSFX == true) 


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
        //doesnt work after the first time due to the camera being destroyed 
        //and audio not beingg reassigned to it
        if (!audio.isPlaying && playSliderSFX == true)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.sliderSound);
        }
        
    }
}
