using UnityEngine;

public class rotater : MonoBehaviour
{
    void Update()
    {
        //rotates the cubes in place
        transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);

        //rotates the sky
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.9f);
    }
}   
