using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCompleted : MonoBehaviour
{
    public TutorialLevelManager tutorialManager;
    private AudioSource audioSource;
    private bool hasPlayedPuzzle1Sound = false;
    private bool hasPlayedPuzzle2Sound = false;
    private bool hasPlayedPuzzle3Sound = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this object.");
        }

        if (tutorialManager == null)
        {
            Debug.LogError("TutorialLevelManager not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the puzzle has been completed and play the sound
        if (tutorialManager.puzzle1Complete && !hasPlayedPuzzle1Sound)
        {
            audioSource.Play(); // Play the sound
            hasPlayedPuzzle1Sound = true; // Set the boolean to true so the sound doesn't play again
        }
        else if (tutorialManager.puzzle2Complete && !hasPlayedPuzzle2Sound)
        {
            audioSource.Play();
            hasPlayedPuzzle2Sound = true;
        }
        else if (tutorialManager.puzzle3Complete && !hasPlayedPuzzle3Sound)
        {
            audioSource.Play();
            hasPlayedPuzzle3Sound = true;
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
