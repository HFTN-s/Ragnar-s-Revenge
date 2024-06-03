using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class placeRing : MonoBehaviour
{
    public GameObject collider_ring; // Assign this in the Inspector
    public GameObject yarl_ring; // Assign this in the Inspector
    public GameObject hand_WR; // Assign this in the Inspector
    public GameObject hand_NR; // Assign this in the Inspector
    public Boolean yarlInSkelly;  
    public Level3Manager level3Manager;
    public ActivateBifrost activateBifrost;

    private void Start()
    {
        yarlInSkelly = false;
    }

    public Boolean getJarlVoiceLocation()
    {
        return yarlInSkelly;
    }

    // Function to call when the puzzle is finished
    public void PuzzleFinished()
    {
        Debug.Log("Removed Ring!");
        activateBifrost.setYarlRingPlaced();
        // Play Jarl Audio Clip
        if (activateBifrost.getTreeChopped() && activateBifrost.getStoneInReceptical())
        {

        }
        else
        {
            StartCoroutine(level3Manager.WaitForJarlAudioClip(7));
        }
    }

    public void RemoveRing()
    {
        if (yarl_ring == null)
        {
            Debug.LogError("Please assign a yarl_ring GameObject in the inspector.");
            return;
        }
        else
        {
            if (hand_WR == null)
            {
                Debug.LogError("Please assign a hand_WR GameObject in the inspector.");
                return;
            }
            else
            {
                // Enable the GameObject
                hand_WR.SetActive(false);
            }
            if (hand_NR == null)
            {
                Debug.LogError("Please assign a hand_NR GameObject in the inspector.");
                return;
            }
            else
            {
                // Enable the GameObject
                hand_NR.SetActive(true);
                yarl_ring.SetActive(true);
                yarlInSkelly = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "PlayerHand"
        if (other.gameObject.CompareTag("PlayerHand") && other.gameObject.name == "LeftHand Controller WR")
        {
            // Call the RemoveRing function
            RemoveRing();

            // Call the PuzzleFinished function
            PuzzleFinished();
        }
    }
}

