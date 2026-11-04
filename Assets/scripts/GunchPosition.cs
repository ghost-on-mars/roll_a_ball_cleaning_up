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
            transform.position = new Vector3(targetPosition.x-0.1f,targetPosition.x,targetPosition.z-0.1f);
            Vector3 currentAngles = player.localEulerAngles;

            // Force the Z axis to stay at 0 
            transform.localEulerAngles = new Vector3(0, currentAngles.y,0);
        }
    }
}
