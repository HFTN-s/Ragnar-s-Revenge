using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCheck : MonoBehaviour
{
    public int sharp = 0;
    public Level3Manager level3Manager;

    public void IncreaseSharpness()
    {
        sharp++;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Whetstone"))
        {
            Debug.Log("Whetstone collision detected");
            IncreaseSharpness();
        }

        //Jarl Voice Lines

        if (sharp == 1)
        {
            // Play Jarl Audio Clip
            //StartCoroutine(level3Manager.WaitForJarlAudioClip(0));
        }
        else if (sharp == 2)
        {
            // Play Jarl Audio Clip
            //StartCoroutine(level3Manager.WaitForJarlAudioClip(1));
        }
        else if (sharp == 3)
        {
            // Play Jarl Audio Clip
            //StartCoroutine(level3Manager.WaitForJarlAudioClip(2));
        }
    }

    
}
