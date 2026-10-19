using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public player_controller playerScript; //the player controller script
    
    public Transform player;
    private NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    IEnumerator killSelf(float delayInSeconds)
    {
        // This line pauses execution for the specified number of seconds
        yield return new WaitForSeconds(delayInSeconds);

        Destroy(gameObject); //destroys itself
    }
    void Update()
    {
        if (player != null && playerScript.health > 0) //if its active and not dead
        {    
            navMeshAgent.SetDestination(player.position);
        }

        //if the health is at 10 and its the second level
        if (playerScript.health == 10 && playerScript.currentLevel ==2 || playerScript.remainingFood == 0 && playerScript.currentLevel ==2)
        {
            
            navMeshAgent.ResetPath(); //stops it from following player
            StartCoroutine(killSelf(2.0f));
        }
    }
}
