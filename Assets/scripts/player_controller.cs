//using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.Rendering;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;

public class player_controller : MonoBehaviour
{
    //controls
    private CommandActions keyboardActions;

    public Transform gunchTransform;
    public GameObject gunchBody;


    //UI
    public TextMeshProUGUI healthText;
    public GameObject ouchText;
    public RectTransform ouchTransform;
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
    //private Collider collider2;
    public BoxCollider headCollider;
    public BoxCollider bodyCollider;    
    public float movementX;
    public float movementY;
    private float startingX;
    private float startingY;
    public Vector3 lockAngle;
    public bool thing = false;
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
        speed=10;
        currentLevel = 1;
        died = false;
        ouchTransform.anchoredPosition = new Vector2(0, 50f);//sets pos of ouch
        //starts the timer
        startTime = Time.time;

        //rigid body
        rb = GetComponent<Rigidbody>(); 
        rb.linearDamping = 0f; 
        rb.angularDamping = 0.05f; 
        rb.rotation = Quaternion.Euler(0, 0, 0);
        meshRenderer = GetComponent<MeshRenderer>();
        //BoxCollider collider2 = GetComponent<BoxCollider>();
        //CapsuleCollider bodyCollider = GetComponent<CapsuleCollider>();
        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.radius=0.5f;
        

        LayerMask includedLayers = LayerMask.GetMask("Default", "navmesh");
        sphere.includeLayers = includedLayers;

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
        meshRenderer.enabled = true;
        gunchBody.SetActive(false);
        headCollider.enabled=false;  
        bodyCollider.enabled=false; 
        //collider2.enabled=false;
        //bodyCollider.enabled=flse;
        
        //if the bonus game mode was turned on
        if (UseButtons.changedMode == true)
        {
            ouchTransform.anchoredPosition = new Vector2(0, 80f); //moves ouch text higher

            // Hide the mesh
            meshRenderer.enabled = false;
            gunchBody.SetActive(true);
            sphere.enabled=true;
            includedLayers = LayerMask.GetMask( "navmesh"); //only includes the floor
            sphere.includeLayers = includedLayers;
            
            if (sphere != null)
            {
                //Debug.Log("shrunk it");
                //sphere.radius = 0.15f; //shrinks it so you can fit in gaps better
                //sphere.center = new Vector3(0f, -0.33f, 0.07f);
            }
            //collider2.enabled=true; //enables the second collider 
            headCollider.enabled=true;  
            bodyCollider.enabled=true;  
            evilPickup1.SetActive(false);
            evilPickup2.SetActive(true);
            //this is so you slide less if youre not the ball
            speed=40;
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
        winTextObject.gameObject.SetActive(false);
        endScreenButton.gameObject.SetActive(false);
        keepPlayingButton.gameObject.SetActive(false);

        AudioManager.instance.PlaySFX(AudioManager.instance.dieSound);
    
        GetComponent<MeshRenderer>().enabled = false; //just hides it
        gunchBody.SetActive(false);
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
        startingX = movementX;
        startingY = movementY;
        Debug.Log("yoooooooooo");
        
        if (thing ==true) //if touching a wall  
        
        {
            Debug.Log("yo");
            if (movementX==1)//facing rigth
            {
                lockAngle = new Vector3(0, 90, 0); 
            }
            if (movementX==-1)//facing left
            {
                lockAngle = new Vector3(0, 270, 0); 
            }
            if (movementY==1)//facing left
            {
                lockAngle = new Vector3(0, 0, 0); 
            }
            if (movementY==-1 )//facing right
            { 
                lockAngle = new Vector3(0, 180, 0); 
            }
            //lockAngle = rb.rotation.eulerAngles; 
            
            
            //bing=true
            //IN OTHER SCRIPT DO IF BING = true INSTEAD OF THIGN  
        }
       

    }
    void SetHealthText() 
    {
        healthText.text =  "Health: " + health.ToString() + "/10";
        if (health == 10 || remainingFood == 0) //if you ahve full health or got all the food
        {
            winTextObject.SetActive(true);
            loseTextObject.SetActive(false);

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
        bodyCollider.transform.rotation = gunchTransform.transform.rotation;
        headCollider.transform.rotation = gunchTransform.transform.rotation;

        //Debug.Log(bodyCollider.transform.rotation + " body angle. "+ gunchTransform.transform.rotation);
        //moves you
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);

        //fully gets rid of sliding
        /*if (movementX ==0 & movementY==0)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }*/

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
        if (currentY <= -19.5f && currentLevel ==1 ) 
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
        //Debug.Log("hit a wall");
        //if (collision.gameObject.CompareTag("evilpickup2")||collision.gameObject.CompareTag("obstacles") )
        //{
        

        /*if (movementX == startingX || movementY == startingY)
        {*/
        if (collision.gameObject.tag != "floor")
        {
            
            rb.constraints = RigidbodyConstraints.FreezeRotation; // Freezes ALL rotation axes
            //Debug.Log(movementX + " " +startingX + " " + movementY +" "+startingY); 
            Debug.Log(movementX + " x. "+ movementY + " y. " + rb.rotation.eulerAngles + " angel?");
            
            //Debug.Log("hit a wall "+ thing + ". tag was "+ collision.gameObject.tag); 
            if (movementX==1)//facing rigth
            {
                lockAngle = new Vector3(0, 90, 0); 
            }
            if (movementX==-1)//facing left
            {
                lockAngle = new Vector3(0, 270, 0); 
            }
            if (movementY==1)//facing left
            {
                lockAngle = new Vector3(0, 0, 0); 
            }
            if (movementY==-1)//facing right
            { 
                lockAngle = new Vector3(0, 180, 0); 
            }
            //rb.rotation = Quaternion.Euler(0, 45, 0);
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            /*if (thing==false)
            {
                lockAngle = new Vector3(0, rb.rotation.eulerAngles.y, 0); 
            }*/
            thing=true;
            //OnMove(movementVector);
            
        }
        else
        {
            thing=false;
        }
            //rb.rotation = Quaternion.Euler(lockAngle); //sets your rotation the hwere you werer when you presed a key
        //}
        
        /*else
        {
            //THSI IS ALWAYS TRUE CHANEG TAHT BRUH
            Debug.Log("thing is false");
            thing=false;
        }*/

        //rb.linearVelocity = Vector3.zero;

        if (collision.gameObject.CompareTag("enemy") && died==false)
        {
            damageTaken = damageTaken + 10; 
            Die();            
        }
    
    }
    private void OnCollisionExit(Collision collision)
    {
        thing=false;    
    }
}
