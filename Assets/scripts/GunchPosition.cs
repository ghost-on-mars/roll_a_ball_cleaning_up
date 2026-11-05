using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Gunch : MonoBehaviour
{
    public player_controller playerScript;    
    public Transform player;

        void Update()
    {
        //if player is active and not dead
        if (player != null && playerScript.health > 0) 
        {
            Vector3 targetPosition = player.position;
            Vector3 lookatPos = new Vector3(targetPosition.x+playerScript.movementX, targetPosition.y, targetPosition.z+playerScript.movementY);
            //lookAtPos = new Vector3 (targetPosition.x-0.1f,targetPosition.x,targetPosition.z-0.1f);
            //transform.position = new Vector3(targetPosition.x-0.1f,targetPosition.y,targetPosition.z-0.1f);
            
            //z is nack and forth x is left and right
            transform.position = targetPosition;

            Debug.Log((targetPosition.x+playerScript.movementX) + " x. " + playerScript.movementY + " y");
            transform.LookAt(lookatPos);//playerScript.movementX,0, playerScript.movementY );
            Vector3 currentAngles = player.localEulerAngles;
            currentAngles.x=0f;
            currentAngles.z=0f;

            // Force the Z axis to stay at 0 
            //transform.localEulerAngles = new Vector3(0, currentAngles.y,0);
        }
    }
}
