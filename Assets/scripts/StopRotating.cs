    using UnityEngine;

public class LockRotation : MonoBehaviour
{
    // Define the exact global rotation you want to keep
    private Quaternion lockedRotation;
    public Transform targetObject;
    public float yPos;

    void Start()
    {
        // Capture the initial rotation at launch
        //lockedRotation = transform.rotation;
        Debug.Log(transform.rotation);
    }

    void LateUpdate()
    {

        //Vector3 myPosition = transform.position;

        Vector3 targetPosition = targetObject.position;
        //Debug.Log("Target Position: " + targetPosition + myPosition);

        transform.localPosition = new Vector3(0f, yPos, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);  
        // Enforce the locked rotation every frame after regular movements
        //transform.rotation = lockedRotation;
    }
}