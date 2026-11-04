using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class rotater : MonoBehaviour
{
    public float pos = 2;
    public float currentTime;

    void Start()
    {
        //StartCoroutine(Jump(2f));
    }
    IEnumerator GoDown(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds); //waits for however amny seconds
        pos = 0;
        StartCoroutine(Jump(4f));
        //transform.Translate(Vector3.down * 20 * Time.deltaTime);
    }
    IEnumerator Jump(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        //transform.position = new Vector3(0, 2, 0) * Time.deltaTime;
        pos=2;
        transform.position += new Vector3(0, pos, 0) * Time.deltaTime;
        StartCoroutine(GoDown(0.6f));
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
            
            //transform.Translate(Vector3.up * 20 * Time.deltaTime);
            
            
            //StartCoroutine(Jump(2f));
            currentTime = Time.time;
            
            if (Mathf.Round(currentTime) % 4 == 0) //so every four seconds
            {
                transform.position += new Vector3(0, pos, 0) * Time.deltaTime;
            }
            
            
            //rotates the treat in place    
            //transform.Rotate (Vector3.up * 20 * Time.deltaTime);
        }
        //rotates the sky
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.9f);
    }
}   
