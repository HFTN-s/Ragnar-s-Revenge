using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorLevel3 : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void Puzzle3CompletedEvent()
    {
        //wait 15 seconds
        StartCoroutine(WaitForSeconds());
        Destroy(gameObject);
    } 

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(15);
        particleSystem.Stop();
    }
}
