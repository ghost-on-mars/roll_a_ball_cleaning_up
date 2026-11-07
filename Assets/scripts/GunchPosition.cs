using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GunchPosition : MonoBehaviour
{
    public player_controller playerScript;    
    public Transform player;
    private float startingX;
    private float startingY;
    private Vector3 targetPosition;

    void Update()
    {
        //if player is active and not dead
        if (player != null && playerScript.health > 0) 
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y-0.5f, player.position.z);
            //Vector3 lookAtPos = new Vector3 (targetPosition.x+playerScript.movementX,0, targetPosition.z+playerScript.movementY );
            
            startingX=playerScript.movementX;
            startingY=playerScript.movementY;

            if (playerScript.movementX !=0 || playerScript.movementY !=0) //only if a key is being pressed
            {
                
                //makes you face the right direction
                transform.LookAt(targetPosition);
                float yAngle = transform.eulerAngles.y; //gets the current angle
                    
                //keeps the x and z at 0 and only changes the y angle
                transform.localEulerAngles = new Vector3(0, yAngle, 0); 

            }
            //Debug.Log(playerScript.movementX + " x. "+ playerScript.movementY +" y.");
            
            //the problem is that the collider doesnt turn with gunch, it turns with the sphere
            
            transform.position = targetPosition;
            //targetPosition.position.y=0;
        }
    }
    void LateUpdate()
    {   
        //if touching an obstacle keep the position the same

        //only if theres been in change is what you pressed. so if 
        if (playerScript.movementX != startingX || playerScript.movementY != startingY) //only if a key is being pressed
            {
                //makes you face the right direction
                //transform.LookAt(targetPosition);
                //float yAngle = transform.eulerAngles.y; //gets the current angle
                    
                //keeps the x and z at 0 and only changes the y angle
                //transform.localEulerAngles = new Vector3(0, yAngle, 0); 
                /*if (playerScript.movementX !=0 || playerScript.movementY !=0) //only if a key is being pressed
                {
                    
                    //makes you face the right direction
                    transform.LookAt(targetPosition);
                    float yAngle = transform.eulerAngles.y; //gets the current angle
                        
                    //keeps the x and z at 0 and only changes the y angle
                    transform.localEulerAngles = new Vector3(0, yAngle, 0); 

                }*/
                    // transform.LookAt(lookAtPos);
                
                
            }
    }
}
