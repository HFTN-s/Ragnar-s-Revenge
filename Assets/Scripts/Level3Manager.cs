using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    private GameObject[] lightSources;
    public GameObject parentTorch;
    public AudioClip[] jarlAudioClips;
    public AudioSource jarlAudioSource;
    public AudioSource musicAudioSource;

    void Start()
    {
        // Initialize the lightSources array with the correct size
        lightSources = new GameObject[parentTorch.transform.childCount];
        for (int i = 0; i < parentTorch.transform.childCount; i++)
        {
            lightSources[i] = parentTorch.transform.GetChild(i).gameObject;
            // Set to inactive
            lightSources[i].SetActive(false);
        }
    }


    public void ActivateLightSources()
    {
        // set to active
        for (int i = 0; i < lightSources.Length; i++)
        {
            lightSources[i].SetActive(true);
        }
    }



    //wait for jarl audio to finish
    public IEnumerator WaitForJarlAudioClip(int index)
    {
        PlayJarlAudioClip(index);
        yield return new WaitForSeconds(jarlAudioClips[index].length);
        StopJarlAudioClip();
    }

    public void PlayJarlAudioClip(int index)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = jarlAudioClips[index];
        audioSource.Play();
    }

    public void StopJarlAudioClip()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void Puzzle1CompletedEvent()
    {
        Debug.Log("Puzzle 1 completed");
        // Play Jarl Audio Clip
        //StartCoroutine(WaitForJarlAudioClip(3));
        ActivateLightSources();
    }

    //Functions to run when puzzles have been completed in level 3 , called from Unity Events of other scripts

}
