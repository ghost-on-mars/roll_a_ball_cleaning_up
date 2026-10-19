using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public player_controller playerScript;    
    public Transform player;
    private NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    IEnumerator killSelf(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds); //waits for however amny seconds
        Destroy(gameObject); //destroys itself
    }
    void Update()
    {
        //if player is active and not dead it chases you
        if (player != null && playerScript.health > 0) 
        {
            navMeshAgent.SetDestination(player.position); 
        }

        //once the player beats the second level
        if (playerScript.health == 10 && playerScript.currentLevel ==2 || playerScript.remainingFood == 0 && playerScript.currentLevel ==2)
        {
            navMeshAgent.ResetPath(); //stops it from following player
            StartCoroutine(killSelf(2.0f));
        }
    }
}
