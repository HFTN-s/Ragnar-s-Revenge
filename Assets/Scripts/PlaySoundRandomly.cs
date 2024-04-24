using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySoundRandomly : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] birdSound;

    // To prevent multiple Invoke calls
    private bool isWaitingToPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the audio source is not playing and we're not already waiting to play a sound
        if (!audioSource.isPlaying && !isWaitingToPlay)
        {
            isWaitingToPlay = true;
            Invoke("PlaySound", Random.Range(5f, 15f));
        }
    }

    void PlaySound()
    {
        Debug.Log("Playing sound");
        //play a random sound from the array
        audioSource.clip = birdSound[Random.Range(0, birdSound.Length)];
        audioSource.Play();
        isWaitingToPlay = false;
    }
}
