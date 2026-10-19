using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI WinLoseText;
    public Button restartButton;
    private float time;
    void Start()
    {
        //if you died changes the ui
        if (player_controller.died == true)
        {
            WinLoseText.color = new Color(144f/255f, 17f/255f, 17f/255f);
            restartButton.GetComponent<Image>().color = new Color(144f/255f, 17f/255f, 17f/255f);
            WinLoseText.text="You Lost!";
        }

        //converts the time to minutes and seconds
        string time = (player_controller.totalTime/60).ToString();
        string[] time2= time.Split(".");
        float floatseconds = float.Parse("0."+time2[1]) * 60;
        string minutes = AddZero(time2[0]);
        string seconds = AddZero(floatseconds.ToString("F0"));
        string nicetime = minutes + ":" + seconds;

        //updates the text
        statsText.text = "Time: " + nicetime + "\n\nFood Collected: "+ player_controller.foodEaten.ToString() + "/18\n\nDamage Taken: " + player_controller.damageTaken.ToString();
        
    }
    //tries to add a zero in front of the number so its formatted like "01:06"
    string AddZero(string MinSec)
    {
        if (MinSec.Length == 1)
        {
            return "0" + MinSec;
        }
        else
        {
            return MinSec;
        }
    }
    public void QuitGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
        #if UNITY_EDITOR
            //exits play mode in the editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //closes the game
            Application.Quit();
        #endif
    }

    public void Restart()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
        SceneManager.LoadScene("MainMenuScene"); 
    }
}
