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
    public static bool changedMode;

    void Start()
    {
        //hides the extra panels
        cheatsPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);

        //sets the default numbers
        if (firstLoad == true) 
        {
            savedMusicVol=0;
            savedSFXVol=-3;
            changedMode = true; //CHANGE THIS LATERRRRRRRRRRRRR
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
    public void PressedToggle(float mode)
    {
        //Debug.Log(mode + "mode");
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
        if (mode==0)
        {
            changedMode=false;
        }
        else if (mode==1)
        {
            changedMode=true;
        }
    }
    public void ChangeSFXVol(float volume)
    {
        mixer.SetFloat("sfxVol", volume); 
        savedSFXVol = volume; //updates the saved volume
        
        //plays the sfx. uses AudioManager.PlayOneSFX because that uses Play 
        //instead of PlayOneShot, ehich stops the sounds from overlapping
        if (playSliderSFX == true)
        {
            AudioManager.instance.PlayOneSFX(AudioManager.instance.sliderSound);
        }

    }
    public void ChangeMusicVol(float volume)
    {
        mixer.SetFloat("musicVol", volume);
        savedMusicVol=volume;
        
        if (playSliderSFX == true)
        {
            AudioManager.instance.PlayOneSFX(AudioManager.instance.sliderSound);
        }
    }
}
