using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    private GameObject[] lightSources;
    public GameObject parentTorch;
    public AudioClip[] jarlAudioClips;
    public AudioClip[] musicClips;
    public AudioSource jarlAudioSource_Skelly;
    public AudioSource jarlAudioSource_Ring;
    public AudioSource musicAudioSource;
    public placeRing PlaceRing;

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
        ActivateLightSources();
        musicAudioSource.clip = musicClips[0];
        musicAudioSource.Play();
    }


    public void ActivateLightSources()
    {
        // set to active
        for (int i = 0; i < lightSources.Length; i++)
        {
            lightSources[i].SetActive(true);
        }
    }

    public IEnumerator WaitForJarlAudioClip(int index)
    {
        PlayJarlAudioClip(index);
        yield return new WaitForSeconds(jarlAudioClips[index].length);
        StopJarlAudioClip();
    }

    public void PlayJarlAudioClip(int index)
    {
        if(PlaceRing.getJarlVoiceLocation())
        {
            jarlAudioSource_Skelly.clip = jarlAudioClips[index];
            jarlAudioSource_Skelly.Play();
        }
        else
        {
            jarlAudioSource_Ring.clip = jarlAudioClips[index];
            jarlAudioSource_Ring.Play();
        }
    }

    public void StopJarlAudioClip()
    {
        if (PlaceRing.getJarlVoiceLocation())
        {
            jarlAudioSource_Skelly.Stop();
        }
        else
        {
            jarlAudioSource_Ring.Stop();
        }
        
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
