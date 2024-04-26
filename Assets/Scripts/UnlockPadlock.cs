using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class UnlockPadlock : MonoBehaviour
{
    public GameObject correctKey;
    public GameObject ring;
    public UnityEvent OnUnlock;
    private AudioSource audioSource;
    public AudioClip unlockSound;
    public AudioClip wrongKeySound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            Debug.Log("Key tag is detected");
            GameObject key = other.gameObject;
            if (key.transform.localScale.x > 0.95)
            {
                if (key == correctKey)
                {
                    //attach ring script to ring if not already added
                    if (ring.GetComponent<RingScript>() == null)
                    {
                        ring.AddComponent<RingScript>();
                    }
                    //play unlock sound
                    audioSource.PlayOneShot(unlockSound);
                    //wait for sound to finish then destroy key 
                    Destroy(key);
                    //destroy parent object
                    //run unlock event
                    OnUnlock.Invoke();
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Debug.Log("Key is incorrect");
                    if (audioSource.isPlaying) {return;}
                    audioSource.PlayOneShot(wrongKeySound);
                }
            }
        }
    }

    void CheckKey()
    {

    }

    void Unlock()
    {

    }
}
