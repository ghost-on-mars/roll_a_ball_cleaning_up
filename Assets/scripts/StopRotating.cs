using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Quaternion lockedRotation;
    public Transform targetObject;
    public float yPos;

    void LateUpdate()
    {
        //stops the text above the enemy's head from rotating with it
        Vector3 targetPosition = targetObject.position;
        transform.localPosition = new Vector3(0f, yPos, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);  
    }
}