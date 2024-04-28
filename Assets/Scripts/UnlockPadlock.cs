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
    public AudioSource jarlVoicePadlock;
    public TutorialLevelManager levelManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.transform.parent.gameObject.name == "Padlock1")
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
                        //play voice line 14 from jarlVoiceLines in tutorial level manager
                        levelManager.PlayJarlVoiceLine(14);
                        Destroy(transform.parent.gameObject);
                    }
                    else
                    {
                        Debug.Log("Key is incorrect");
                        if (audioSource.isPlaying) { return; }
                        audioSource.PlayOneShot(wrongKeySound);
                    }
                }
            }
        }

        if (this.transform.parent.gameObject.name == "Padlock1 (1)")
        {
            if (other.gameObject.tag == "Key")
            {
                Debug.Log("Key tag is detected");
                GameObject key = other.gameObject;
                if (key.transform.localScale.x > 2.5)
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

                        levelManager.PlayJarlVoiceLine(4);
                        Destroy(transform.parent.gameObject);
                    }
                    else
                    {
                        Debug.Log("Key is incorrect");
                        if (audioSource.isPlaying) { return; }
                        audioSource.PlayOneShot(wrongKeySound);
                    }
                }
            }
        }
    }
}
