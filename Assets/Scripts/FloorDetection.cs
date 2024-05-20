using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    public GameObject[] interactableObjects;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //mute audio source
        audioSource.mute = true;
        //wait 3 seconds before allowing audio to play
        StartCoroutine(EnableAudio());
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject item in interactableObjects)
        {
            if (collision.gameObject == item)
            {
                audioSource.Play();
                //Debug.Log("Object has been detected from the list");
                //Debug.Log("Adding" + item.name + " to the list of interactable objects");
                if (item.GetComponent<InteractableObject>() == null)
                {
                    //Debug.Log("Adding InteractableObject script to " + item.name);
                    item.AddComponent<InteractableObject>();
                }
            }
        }
    }

    private IEnumerator EnableAudio()
    {
        yield return new WaitForSeconds(3f);
        audioSource.mute = false;
        Debug.Log("Audio source is now enabled.");
    }
}
