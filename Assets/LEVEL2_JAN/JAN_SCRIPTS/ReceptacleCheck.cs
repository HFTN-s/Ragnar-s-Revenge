using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReceptacleCheck : MonoBehaviour
{
    public GameObject receptacle; // Assign this in the Inspector
    public GameObject Tricircle_Glow;
    public Level3Manager level3Manager;
    public ActivateBifrost activateBifrost;
    public AudioSource receptacleAudioSource;
    public AudioClip receptacleAudioClip;

    private int tries = 0;

    // Function to call when the puzzle is finished
    public void PuzzleFinished()
    {
        Debug.Log("Stone In Receptical!");
        activateBifrost.setStoneInReceptical();
        StartCoroutine(level3Manager.WaitForJarlAudioClip(6));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "CorrectSunStone"
        if (other.gameObject.CompareTag("Sunstone") && other.gameObject.name == "CORRECT_STONE")
        {
            Tricircle_Glow.SetActive(true);

            // Disable gravity for the Sunstone
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
            }

            // Set as kinematic if it exists
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }

            // Set as kinematic if it exists
            XRGrabInteractable xRGrabInteractable = other.GetComponent<XRGrabInteractable>();
            if (xRGrabInteractable != null)
            {
                xRGrabInteractable.enabled = false;
            }

            // Play the audio clip
            receptacleAudioSource.PlayOneShot(receptacleAudioClip);

            // Call the PuzzleFinished function
            PuzzleFinished();

            // Scale down and align the Sunstone with the receptacle
            StartCoroutine(ScaleAndAlignSunstone(other.gameObject));
            // Play Jarl Audio Clip
            //StartCoroutine(level3Manager.WaitForJarlAudioClip(0));
        }
        else if (other.gameObject.CompareTag("Sunstone") && other.gameObject.name != "CORRECT_STONE")
        {
            if (other.gameObject.name == "INCORRECT_STONE_1" || other.gameObject.name == "INCORRECT_STONE_2" || other.gameObject.name == "INCORRECT_STONE_3" || other.gameObject.name == "INCORRECT_STONE_4")
            {
                if (tries > 2)
                {
                    StartCoroutine(level3Manager.WaitForJarlAudioClip(4));
                    Debug.Log("You had 3 tries...");
                    tries = 0;
                }
                else
                {
                    // Play Jarl Audio Clip
                    StartCoroutine(level3Manager.WaitForJarlAudioClip(3));
                    Debug.Log("That is not the right stone...");
                    tries++;
                }
            }
        }
    }


    IEnumerator ScaleAndAlignSunstone(GameObject sunstone)
    {
        // Directly set the Sunstone's position to the target position without moving it over time
        sunstone.transform.position = receptacle.transform.position; // Position the Sunstone exactly where the receptacle is

        // Instantly rotate the Sunstone to match the receptacle's rotation
        sunstone.transform.rotation = receptacle.transform.rotation;

        // Scale down the Sunstone to 80% of its original size
        sunstone.transform.localScale *= 0.8f; // Reduce the scale by 20%

        // No need for a loop or waiting for frames since the movement is instantaneous
        yield return null; // This line is kept to satisfy the IEnumerator contract, even though it's unnecessary for this operation
    }

}
