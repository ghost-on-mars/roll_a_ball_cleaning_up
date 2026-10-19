using UnityEngine;
using UnityEngine.AI;

public class ObstacleScript : MonoBehaviour

    //private bool land = false;
{
    [SerializeField] private LayerMask navMeshLayer; // Select your NavMesh layer in the Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //helllo
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("raisedcube"))
        {
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //makes it make the sound evrey time the player touches it
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
        }

        // Check if the collided object's layer is included in the navMeshLayer mask
        if ( gameObject.CompareTag("raisedcube") && ((1 << collision.gameObject.layer) & navMeshLayer) != 0)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
        }
    }
}
