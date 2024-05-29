using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReceptacleCheck : MonoBehaviour
{
    public GameObject receptacle; // Assign this in the Inspector
    public Level3Manager level3Manager;

    // Function to call when the puzzle is finished
    public void PuzzleFinished()
    {
        Debug.Log("Puzzle Finished!");
        // Add any actions you want to perform here after the puzzle is completed.
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "CorrectSunStone"
        if (other.gameObject.CompareTag("Sunstone"))
        {
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

            // Call the PuzzleFinished function
            PuzzleFinished();

            // Optionally, remove the object from the scene or disable it after checking
            // Destroy(other.gameObject);

            // Scale down and align the Sunstone with the receptacle
            StartCoroutine(ScaleAndAlignSunstone(other.gameObject));
            // Play Jarl Audio Clip
            //StartCoroutine(level3Manager.WaitForJarlAudioClip(0));
        }
        else
        {
            // Play Jarl Audio Clip
            //StartCoroutine(level3Manager.WaitForJarlAudioClip(1));
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
