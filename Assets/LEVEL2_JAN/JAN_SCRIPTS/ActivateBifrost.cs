using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBifrost : MonoBehaviour
{
    public Boolean YarlRingPlaced;
    public Boolean StoneInReceptical;
    public Boolean TreeChopped;

    public GameObject Bifrost; // Assign this in the Inspector
    public GameObject Aperture_Open; // Assign this in the Inspector
    public GameObject Aperture_Closed; // Assign this in the Inspector

    public DoorController doorController;
    public AudioSource bifrostAudioSource;
    [SerializeField] private AudioSource puzzleCompletedAudioSource;
    public GameObject endOfLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Boolean getYarlRingPlaced()
    {
        return YarlRingPlaced;
    }
    public Boolean getStoneInReceptical()
    {
        return StoneInReceptical;
    }
    public Boolean getTreeChopped()
    {
        return TreeChopped;
    }

    public void setYarlRingPlaced()
    { 
        YarlRingPlaced = true;
        Debug.Log("YarlRingPlaced set to true.");
        puzzleCompletedAudioSource.Play();
        activate_Bifrost();
    }
    public void setStoneInReceptical()
    { 
        StoneInReceptical = true;
        puzzleCompletedAudioSource.Play();
        Debug.Log("StoneInReceptical set to true.");
        activate_Bifrost();
    }
    public void setTreeChopped()
    { 
        TreeChopped = true;
        puzzleCompletedAudioSource.Play();
        Debug.Log("TreeChopped set to true.");
        activate_Bifrost();
    }

    public void activate_Bifrost()
    {
        if (YarlRingPlaced && StoneInReceptical && TreeChopped)
        {
            Debug.Log("All puzzles completed!");
            endOfLevel.SetActive(true);
            bifrostAudioSource.Play();
            // Add any actions you want to perform here after the puzzle is completed.
            // Check if the target GameObject has been assigned in the Inspector
            if (Bifrost == null)
            {
                Debug.LogError("Please assign a Bifrost GameObject in the inspector.");
                return;
            }
            // Enable the GameObject
            Bifrost.SetActive(true);


            // Add any actions you want to perform here after the puzzle is completed.
            // Check if the target GameObject has been assigned in the Inspector
            if (Aperture_Open == null)
            {
                Debug.LogError("Please assign a Aperture_Open GameObject in the inspector.");
                return;
            }

            // Enable the GameObject
            Aperture_Open.SetActive(true);

            // Check if the target GameObject has been assigned in the Inspector
            if (Aperture_Closed == null)
            {
                Debug.LogError("Please assign a Aperture_Closed GameObject in the inspector.");
                return;
            }

            // Enable the GameObject
            Aperture_Closed.SetActive(false);

            doorController.OpenDoor();

        }
        else
        {
            if (YarlRingPlaced)
            {
                Debug.Log("Yarl ring puzzle done!");
            }
            else if(TreeChopped)
            {
                Debug.Log("Tree puzzle done!");
            }
            else if(StoneInReceptical)
            {
                Debug.Log("Stone puzzle done!");
            }
            else
            {
                Debug.Log("Complete All three puzzles.");
            }
        }
    }
}
