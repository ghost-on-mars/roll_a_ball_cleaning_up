using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class rotater : MonoBehaviour
{
    public float pos = 2;
    public float currentTime;
    public float randomDelay;

    void Start()
    {
        //makes a random delay so they all go at different times
        randomDelay = Random.Range(0f, 6.5f); 
    }
    
    void Update()
    {
        if (UseButtons.changedMode == false)
        {
            //rotates the cubes in place
            transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
        }

        if (UseButtons.changedMode == true)
        {
            currentTime = Time.time;
            
            if (Mathf.Round(currentTime-randomDelay) % 5 == 0) //so every five seconds
            {
                transform.position += new Vector3(0, pos, 0) * Time.deltaTime;
                //randomDelay = Random.Range(1, 6); //makes a new delay
            }
        }
        //rotates the sky
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.9f);
    }
}   
