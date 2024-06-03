using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public void OpenDoorReceiver()
    {
        Debug.Log("OpenDoorReceiver called.");
        //Lerp rotation on the y axis to 30.
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        float timeElapsed = 0;
        float duration = 3f;
        float startRotation = transform.rotation.eulerAngles.y;
        float endRotation = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float newRotation = Mathf.Lerp(startRotation, endRotation, timeElapsed / duration);
            transform.rotation = Quaternion.Euler(0, newRotation, 0);
            yield return null;
        }
    }

    //OnHitByLasterCorrect() 
    public void OnHitByLaserCorrect()
    {
        OpenDoorReceiver();

        // play door open sound
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
