using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SharpenAxe : MonoBehaviour
{
    public Level3Manager level3Manager;
    public AxeCheck axeCheck;


    // Function to call when the puzzle is finished
    public void PuzzleFinished()
    {
        Debug.Log("Axe Sharp");
        // Play Jarl Audio Clip
        StartCoroutine(level3Manager.WaitForJarlAudioClip(8));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "CorrectSunStone"
        if (other.gameObject.CompareTag("Axe") && other.gameObject.name == "CorrectAxe")
        {
            if (axeCheck.sharp > 2)
            {
                // Call the FreezeAxe function
                PuzzleFinished();
            }
            else
            {
                Debug.Log("Sharpening axe");
                axeCheck.sharp += 1;
                // Play Jarl Audio Clip
                StartCoroutine(level3Manager.WaitForJarlAudioClip(5));
            }

        }
        else
        {
            // Play Jarl Audio Clip
            //StartCoroutine(level3Manager.WaitForJarlAudioClip(1));
        }
    }
}
