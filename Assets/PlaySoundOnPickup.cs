using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script is attached to any object that can be picked up by the player
// This script will play a sound when the object is picked up
// This script is attached to the object that is being picked up
// The object that is being picked up has an AudioSource component attached to it
// The AudioSource component has a clip set
public class PlaySoundOnPickup : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        audioSource.loop = false; // Set the audio to not loop
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
