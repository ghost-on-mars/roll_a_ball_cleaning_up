using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GunchPosition : MonoBehaviour
{
    public player_controller playerScript;    
    public Transform player;

    void Update()
    {
        //if player is active and not dead
        if (player != null && playerScript.health > 0) 
        {
            Vector3 targetPosition = player.position;
            Vector3 lookAtPos = new Vector3 (targetPosition.x+playerScript.movementX,0, targetPosition.z+playerScript.movementY );
            
            //Debug.Log(playerScript.movementX + " x. "+ playerScript.movementY +" y.");
            
            //the problem is that the collider doesnt turn with gunch, it turns with the sphere
            if (playerScript.movementX !=0 || playerScript.movementY !=0) //only if a key is being pressed
            {
                
                //makes you face the right direction
                transform.LookAt(targetPosition);
                float yAngle = transform.eulerAngles.y; //gets the current angle
                    
                //keeps the x and z at 0 and only changes the y angle
                transform.localEulerAngles = new Vector3(0, yAngle, 0); 

            }
            transform.position = targetPosition;
        }
    }
    void LateUpdate()
    {
        if (playerScript.movementX ==0 & playerScript.movementY ==0) //only if a key is being pressed
            {
                

                // transform.LookAt(lookAtPos);
                
                
            }
    }
}
