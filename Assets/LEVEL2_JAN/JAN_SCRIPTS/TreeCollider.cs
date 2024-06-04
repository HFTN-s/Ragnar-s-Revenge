using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TreeCollider : MonoBehaviour
{
    public Level3Manager level3Manager;
    public ActivateBifrost activateBifrost;
    public AxeCheck axeCheck;
    public AudioSource treeAudioSource;
    public AudioClip axeOnceClip;
    public AudioClip axeTwiceClip;
    public AudioClip axeDoneClip;
    public AudioClip axeFailClip;
    public AudioClip axeDullClip;


    // Function to call when the puzzle is finished
    public void PuzzleFinished()
    {
        Debug.Log("Tree chopped");
        activateBifrost.setTreeChopped();
        if (activateBifrost.getYarlRingPlaced() && activateBifrost.getStoneInReceptical())
        {

        }
        else
        {
            StartCoroutine(level3Manager.WaitForJarlAudioClip(6));
        }
    }

    public void FreezeAxe(Collider other)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "CorrectSunStone"
        if (other.gameObject.CompareTag("Axe") && other.gameObject.name == "CorrectAxe")
        {
            if (axeCheck.sharp > 2)
            {
                // Call the FreezeAxe function
                FreezeAxe(other);
            }
            else
            {
                Debug.Log("Axe dull...");
                // Play Jarl Audio Clip
                StartCoroutine(level3Manager.WaitForJarlAudioClip(1));
            }

        }
        else if(other.gameObject.CompareTag("Axe") && other.gameObject.name != "CorrectAxe")
        {
            Debug.Log("You need the right axe...");
            // Play Jarl Audio Clip
            StartCoroutine(level3Manager.WaitForJarlAudioClip(2));
        }
    }

    // play axe noise if one is playing then stop it and play the new one corresponding to the sharpness
    public void PlayAxeNoise()
    {
        if (treeAudioSource.isPlaying)
        {
            treeAudioSource.Stop();
        }
        if (axeCheck.sharp == 1)
        {
            treeAudioSource.PlayOneShot(axeOnceClip);
        }
        else if (axeCheck.sharp == 2)
        {
            treeAudioSource.PlayOneShot(axeTwiceClip);
        }
        else if (axeCheck.sharp == 3)
        {
            treeAudioSource.PlayOneShot(axeDoneClip);
        }
        else if (axeCheck.sharp == 0)
        {
            treeAudioSource.PlayOneShot(axeDullClip);
        }
        else
        {
            treeAudioSource.PlayOneShot(axeFailClip);
        }
    }
}
