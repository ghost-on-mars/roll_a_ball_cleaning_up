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
    //[SerializeField] private player_controller playerScript; //the player controller script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player_controller.died == true)
        {
            Debug.Log("lost");
            WinLoseText.color = new Color(144f/255f, 17f/255f, 17f/255f);
            restartButton.GetComponent<Image>().color = new Color(144f/255f, 17f/255f, 17f/255f);
            WinLoseText.text="You Lost!";
        }

        //string[] time = ((player_controller.totalTime/60).ToString("F2")).Split(".");
        string time = (player_controller.totalTime/60).ToString();
        Debug.Log(player_controller.damageTaken + "danmaage taken. " + player_controller.totalTime);
        string[] time2= time.Split(".");
        float floatseconds = float.Parse("0."+time2[1]) * 60;
        string minutes = AddZero(time2[0]);
        string seconds = AddZero(floatseconds.ToString("F0"));
        
        string nicetime = minutes + ":" + seconds;
        //Debug.Log("seconds 2" + seconds2 +  "seconds: "+player_controller.totalTime + "total time: " + time + ". time2[0]: " + time2[0] + ". time2[1]: "+ time2[1] + " seconds: "+ seconds);
        //string minutes= time.ToString("F0");    
        Debug.Log("nice time: "+nicetime + " time: " + player_controller.totalTime.ToString("F0"));
        statsText.text = "Time: " + nicetime + "\n\nFood Collected: "+ player_controller.foodEaten.ToString() + "/18\n\nDamage Taken: " + player_controller.damageTaken.ToString();
        
    }

    string AddZero(string MinSec)
    {
        if (MinSec.Length == 1)
        {
            return "0" + MinSec; //adds a zdero in front
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
            // Exits Play Mode inside the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Closes the actual built application
            Application.Quit();
        #endif
    }

    public void Restart()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonSound);
        SceneManager.LoadScene("MainMenuScene"); 
    }
}
