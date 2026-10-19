using UnityEngine;
using UnityEngine.AI;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private LayerMask navMeshLayer; 

    private void OnCollisionEnter(Collision collision)
    {
        //makes it make the sound evrey time the player touches it
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
        }

        //plays the sound whenever they hit the ground
        if ( gameObject.CompareTag("raisedcube") && ((1 << collision.gameObject.layer) & navMeshLayer) != 0)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
        }
    }
}
