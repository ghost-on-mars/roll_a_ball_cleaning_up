//using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.Rendering;

public class player_controller : MonoBehaviour
{
    public float speed = 10; 
    public TextMeshProUGUI healthText;
    //public TextMeshProUGUI winText;
    public GameObject ouchText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject trapdoor;
    public GameObject enemy;
    public GameObject enemyText;
    public GameObject skullEmoji;
    //public GameObject buttons;
    public Button keepPlayingButton;
    public Button endScreenButton;
    [SerializeField] private TextMeshPro enemytextbox; 
    private Rigidbody rb; //rb is th name of the variabel
    private float movementX;
    private float movementY;
    public float remainingFood;
    public int health;
    public int currentLevel;
    public static int foodEaten = 0;
    public static int damageTaken = 0;
    

    public Slider healthSlider;
    public float maxHealth = 10;
    //private float currentHealth;

    bool fallen = false;
    bool inVoid = false;
    public static bool died = false;
    
    private CommandActions keyboardActions;
    float startTime;
    float endTime;
    public static float totalTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foodEaten= 0;
        damageTaken=0;
        died = false;
        Debug.Log(died);
        startTime = Time.time;
        rb = GetComponent<Rigidbody>(); //gets the data form rigid body
        health = 5; 
        remainingFood = 8;
        currentLevel = 1;
        SetHealthText();
        ouchText.SetActive(false);
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        enemyText.SetActive(false);
        skullEmoji.SetActive(false);
        endScreenButton.gameObject.SetActive(false);
        keepPlayingButton.gameObject.SetActive(false);
        

        keyboardActions = new CommandActions();
        keyboardActions.Enable();
        keyboardActions.commands.beatLevel.performed += onControl;
    }

    
    private void onControl(InputAction.CallbackContext context)
    {
        Debug.Log("auto win");
        health=10;
        SetHealthText();
       
    }

    public void EndScreenPress()
    {
        endTime=Time.time;
        totalTime=endTime-startTime;
        SceneManager.LoadScene("EndScreenScene"); 
    }
    public void KeepPlaying()
    {
        keepPlayingButton.gameObject.SetActive(false);
        //winTextObject.SetActive(false);
    }
    void Die()
    {
        died = true;

        endTime=Time.time;
        totalTime=endTime-startTime;

        health = 0;
        SetHealthText();
        healthSlider.value = health;
        loseTextObject.gameObject.SetActive(true);

        AudioManager.instance.PlaySFX(AudioManager.instance.dieSound);
    
        GetComponent<MeshRenderer>().enabled = false; //just hides it
        rb.isKinematic = true; //stops it from moving
        
        //waits 2 seconds and then goes to the end screen
        StartCoroutine(EndScreen(2.0f)); 
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
    IEnumerator EndScreen(float delayInSeconds)
    {
        //Time.timeScale = 1;
        // This line pauses execution for the specified number of seconds
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("EndScreenScene"); 
    }

    //makes the ball teleport to a new position after a delay
    IEnumerator ChangeLevel(float delayInSeconds, GameObject texttohide)
    {
        // This line pauses execution for the specified number of seconds
        yield return new WaitForSeconds(delayInSeconds);

   
        //JUST NAKE A FUNCTION FOR NEXT LEVEL NOT INDIVIDUAL THINGS
        //rb.position = new Vector3(0f, 5f, newPos); //moves it to the new position
        trapdoor.SetActive(false);
        HideText(texttohide); //hides the text saying level complete
    }
    void OnMove(InputValue movementValue) //movement value is a variabel
    {
        //funstion body
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        //Debug.Log(movementVector.x);

        
        
        //Debug.Log(movementY);

    }
    void SetHealthText() 
    {
        healthText.text =  "Health: " + health.ToString() + "/10";
        if (health == 10 || remainingFood == 0) //full health or no food left
        {
            winTextObject.SetActive(true);
            AudioManager.instance.PlaySFX(AudioManager.instance.winSound);
            //StartCoroutine(ChangeLevel(2.0f, winTextObject)); 

            if (currentLevel == 1) //if you're on the first level
            {
                Debug.Log("shouldnt be in here");
                StartCoroutine(ChangeLevel(2.0f, winTextObject)); 
            }

            if (currentLevel == 2) //if you're on the second level
            {
                //first in the enemy script it will stop moving
                //StartCoroutine(ChangeLevel(2.0f, winTextObject)); 
                skullEmoji.SetActive(true);
                endScreenButton.gameObject.SetActive(true);
                keepPlayingButton.gameObject.SetActive(true);    
                Debug.Log(foodEaten + "food" + damageTaken +"damaage" + totalTime + "time");
                
                //StartCoroutine(EndScreen(2.0f)); 
            }
            
        }
        if (health == 0 && died == false) 
        {
            Die();
        }    
    }
    void FixedUpdate()
    {   
        
        //ADD healthER SHOWING HOW MANY FOODYOU GOT. SO EX 12/18
        
        
        //changung movementX just changes where its moving, not the position
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
        
        
        Vector3 posVector = transform.position;
        float currentY = transform.position.y;
        

        healthSlider.value = health;//updates the slider to match the health

        if (currentY <= 0.3f && currentY >= -0.3f && !fallen)//when you first fall
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.fallSound);
            fallen = true;
        }
        if (currentY == -19.5f && currentLevel ==1 ) //once you land
        {
            currentLevel = 2;
            remainingFood = 10; //resets remaining food
            if (health >= 5)
            {
                health = health - 5; //sets your health to half
            }
            else if (health > 1) //if its less than five it goes down by 1
            {
                health = health - 1;
            }
            SetHealthText();

            AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
            ouchText.SetActive(true);
            enemyText.SetActive(true);
            StartCoroutine(ChangeLevel(1.5f, enemyText));
            StartCoroutine(ChangeLevel(1.0f, ouchText));  //to hide the ouch after 2 sec
            //Destroy(gameObject); 
            
        }
        if (currentY <= -28f && !inVoid)//if you fall off the map
        {
            inVoid=true;
            AudioManager.instance.PlaySFX(AudioManager.instance.dieSound);
            Die();
        }
    }
    void HideText(GameObject hiddentext)
    {
        hiddentext.SetActive(false);
        
    }
    void OnTriggerEnter(Collider other) 
    {
        if (health > 0) //so only if you haevnt lost yet
        {
            if (other.gameObject.CompareTag("pickup")) 
            {
                remainingFood = remainingFood -1; //updates the remaining food
                foodEaten = foodEaten +1;
                Debug.Log(foodEaten + "food eaten");
                AudioManager.instance.PlaySFX(AudioManager.instance.pickupSound);
                other.gameObject.SetActive(false);
                if (health < 10) //so it doesnt go over ten
                {
                    health = health + 1;
                    SetHealthText();
                }
                
            }
            if (other.gameObject.CompareTag("evilpickup")) 
            {
                //other.gameObject.SetActive(false);
                //doesnt dissapear
                AudioManager.instance.PlaySFX(AudioManager.instance.hurtSound);
                health = health -1; //updates the score
                damageTaken = damageTaken +1;
                SetHealthText(); //updates text
            }
        }    
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(died);
        if (collision.gameObject.CompareTag("enemy") && died==false)
        {
            Debug.Log("should be dead");
            damageTaken = damageTaken + 10;//health; //could do damageTaken +10
            Die();
            
            //winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}
