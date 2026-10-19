using UnityEngine;

public class rotater : MonoBehaviour
{
    void Update()
    {
        //rotates them in place
        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
    }
}   
