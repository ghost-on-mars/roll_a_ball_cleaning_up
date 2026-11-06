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
    //controls
    private CommandActions keyboardActions;

    public Transform gunchBody;


    //UI
    public TextMeshProUGUI healthText;
    public GameObject ouchText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    [SerializeField] private TextMeshPro enemytextbox; 
    public GameObject enemyText;
    public GameObject skullEmoji;
    public Button keepPlayingButton;
    public Button endScreenButton;
    public Slider healthSlider;
    public float maxHealth = 10;

    //player setup
    private MeshRenderer meshRenderer;
    public float speed = 10; 
    private Rigidbody rb; 
    private Collider collider2;
    public BoxCollider headCollider;
    public BoxCollider bodyCollider;    
    public float movementX;
    public float movementY;
    public float remainingFood = 8;
    public int health;
    public static int foodEaten = 0;
    public static int damageTaken = 0;  
    bool fallen = false;
    bool inVoid = false;
    public static bool died = false;
    public int currentLevel; 

    //other game objects
    public GameObject trapdoor;
    public GameObject enemy; 
    public GameObject evilPickup1;
    public GameObject evilPickup2;
    
    //timer setup
    float startTime;
    float endTime;
    public static float totalTime;

    void Start()
    {
        //resets some variables (can i move these to when theyre defined???)
        foodEaten= 0;
        damageTaken=0;
        health = 5; 
        remainingFood = 8;
        currentLevel = 1;
        died = false;

        //starts the timer
        startTime = Time.time;

        //rigid body
        rb = GetComponent<Rigidbody>(); 
        rb.linearDamping = 0f; 
        rb.angularDamping = 0.05f; 
        //BoxCollider collider2 = GetComponent<BoxCollider>();
        //CapsuleCollider bodyCollider = GetComponent<CapsuleCollider>();
        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.radius=0.5f;

        //hiding objects
        SetHealthText();
        ouchText.SetActive(false);
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        enemyText.SetActive(false);
        skullEmoji.SetActive(false);
        evilPickup1.SetActive(true);
        evilPickup2.SetActive(false);
        endScreenButton.gameObject.SetActive(false);
        keepPlayingButton.gameObject.SetActive(false);
        //collider2.enabled=false;
        //bodyCollider.enabled=false;
        
        //if the bonus game mode was turned on
        if (UseButtons.changedMode == true)
        {
            Debug.Log("hi");
            meshRenderer = GetComponent<MeshRenderer>();
            // Hide the mesh
            meshRenderer.enabled = false;
            sphere.enabled=true;
            
            if (sphere != null)
            {
                //Debug.Log("shrunk it");
                //sphere.radius = 0.15f; //shrinks it so you can fit in gaps better
                //sphere.center = new Vector3(0f, -0.33f, 0.07f);
            }
            collider2.enabled=true; //enables the second collider 
            bodyCollider.enabled=true;  
            evilPickup1.SetActive(false);
            evilPickup2.SetActive(true);
            //this is so you slide less if youre not the ball
            speed=30;
            rb.linearDamping = 3f; 
            rb.angularDamping = 3f;     

        }

        //commands
        keyboardActions = new CommandActions();
        keyboardActions.Enable();
        keyboardActions.commands.beatLevel.performed += onControl;
    }

    private void onControl(InputAction.CallbackContext context)
    {
        //automatically beats the level
        health=10;
        SetHealthText();
        enemyText.SetActive(false); //hides the exclamation mark so it doesnt overlap with the skull
        ouchText.SetActive(false);
    }

    public void EndScreenPress()
    {
        //gets the final time and moves to the end scene
        endTime=Time.time;
        totalTime=endTime-startTime;
        SceneManager.LoadScene("EndScreenScene"); 
    }
    public void KeepPlaying()
    {
        keepPlayingButton.gameObject.SetActive(false);
    }
    void Die()
    {
        died = true;

        //gets the final time
        endTime=Time.time;
        totalTime=endTime-startTime;

        //displays the text
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
        //waits then takes you to the end screen
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("EndScreenScene"); 
    }
    IEnumerator ChangeLevel(float delayInSeconds, GameObject texttohide)
    {
        // This line pauses execution for the specified number of seconds
        yield return new WaitForSeconds(delayInSeconds);
        
        trapdoor.SetActive(false); //hides the trapdoor so you cna go to next level
        HideText(texttohide); //hides the text saying level complete
    }
    void HideText(GameObject hiddentext)
    {
        hiddentext.SetActive(false);
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    void SetHealthText() 
    {
        healthText.text =  "Health: " + health.ToString() + "/10";
        if (health == 10 || remainingFood == 0) //if you ahve full health or got all the food
        {
            winTextObject.SetActive(true);
            AudioManager.instance.PlaySFX(AudioManager.instance.winSound);

            if (currentLevel == 1) //if you're on the first level
            {
                StartCoroutine(ChangeLevel(2.0f, winTextObject)); 
            }

            if (currentLevel == 2) //if you're on the second level
            {
                skullEmoji.SetActive(true);

                //displays the options for whether to end or continue
                endScreenButton.gameObject.SetActive(true);
                keepPlayingButton.gameObject.SetActive(true);    
            }
        }

        if (health == 0 && died == false) // uses died == false so you cant die twice
        {
            Die();
        }    
    }
    void FixedUpdate()
    {   
        bodyCollider.transform.rotation = gunchBody.transform.rotation;
        headCollider.transform.rotation = gunchBody.transform.rotation;
        Debug.Log(bodyCollider.transform.rotation);
        //moves you
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);

        //collider2.      

        /*if (noInputActive) 
        {
            // Instantly drops horizontal sliding to zero, keeping gravity intact
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); 
        }*/
        
        //keeps track of your y position
        Vector3 posVector = transform.position;
        float currentY = transform.position.y;
        
        healthSlider.value = health;//updates the slider to match the health

        //when you first fall plays the sound
        if (currentY <= 0.3f && currentY >= -0.3f && !fallen)
        {
            //makes you fall better if youre not the ball
            if (UseButtons.changedMode == true)
            { 
                rb.linearDamping = 0f; 
                rb.angularDamping = 0.05f; 
            }

            AudioManager.instance.PlaySFX(AudioManager.instance.fallSound);
            fallen = true;
        }
        //once you land
        if (currentY == -19.5f && currentLevel ==1 ) 
        {
            if (UseButtons.changedMode == true)
            { 
                //sets the damping back once you land
                rb.linearDamping = 3f; 
                rb.angularDamping = 3f; 
            }
            currentLevel = 2;
            remainingFood = 10; //resets remaining food
            if (health >= 5)
            {
                health = health - 5; //sets your health to -5
            }
            else if (health > 1) //if 1<health<5 it just goes down by 1
            {
                health = health - 1;
            }
            SetHealthText();//updates it

            AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
            ouchText.SetActive(true);
            enemyText.SetActive(true);
            
            //to hide the texts after a few seconds sec
            StartCoroutine(ChangeLevel(1.5f, enemyText));
            StartCoroutine(ChangeLevel(1.0f, ouchText));  
        }
        //if you fall off the map
        if (currentY <= -28f && !inVoid)
        {
            inVoid=true;
            AudioManager.instance.PlaySFX(AudioManager.instance.dieSound);
            Die();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (health > 0) //so only if you haevnt lost yet
        {
            //when you collect food
            if (other.gameObject.CompareTag("pickup")) 
            {
                remainingFood = remainingFood -1; //updates the remaining food
                foodEaten = foodEaten +1;
                AudioManager.instance.PlaySFX(AudioManager.instance.pickupSound);
                other.gameObject.SetActive(false); //deletes that food
                //so it doesnt go over ten
                if (health < 10) 
                {
                    health = health + 1;
                    SetHealthText();
                }
            }
            //when you hit an obstacle
            if (other.gameObject.CompareTag("evilpickup")) 
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.hurtSound);
                health = health -1; //updates the score
                damageTaken = damageTaken +1;
                SetHealthText(); //updates text
            }
        }    
    }
    //when you hit the enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy") && died==false)
        {
            damageTaken = damageTaken + 10; 
            Die();            
        }
    }
}
