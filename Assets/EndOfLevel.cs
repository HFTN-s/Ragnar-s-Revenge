using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using TMPro;

public class EndOfLevel : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject endOfLevelText;

    public Timer timer;
    public bool roomComplete = false;
    public bool canLeaveLevel = false;

    public AudioClip jarlSpeech;
    public AudioClip[] jarlVoiceLines;
    public AudioSource jarlAudioSource;
    [SerializeField] private TextMeshPro text;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("End of Level collision detected");
            //display end of level text
            endOfLevelText.SetActive(true);
            //stop player movement
            playerMovement.canMove = false;
            //wait 2 seconds
            StartCoroutine(WaitTwoSeconds());
            //wait for player input
            StartCoroutine(WaitForPlayerInput());
        }
    }

    //wait 3 seconds
    private IEnumerator WaitTwoSeconds()
    {
        yield return new WaitForSeconds(2);
    }

    void RoomCompleted()
    {
        if (!canLeaveLevel)
        {
            int timeTaken = timer.GetSeconds();
            DataPersistenceManager.instance.SaveHighScore(timeTaken);

            text.text = $"{timeTaken / 60:D2}:{timeTaken % 60:D2}";
            IncrementProgress(2);
            playerMovement.canMove = false;
            endOfLevelText.SetActive(true);
            Debug.Log("Playing End Jarl Voice Line");
            if (jarlAudioSource.isPlaying)
            {
                StartCoroutine(WaitForJarlSpeech());
            }
            else
            {
                jarlAudioSource.clip = jarlVoiceLines[5];
                jarlAudioSource.Play();

            }
            Debug.Log("End Jarl Voice Line Played");
            StartCoroutine(WaitForPlayerInput());
        }
    }


    private IEnumerator WaitForPlayerInput()
    {
        // Wait until player presses primary or secondary button
        while (true)
        {
            bool primaryButtonPressed = false;
            bool secondaryButtonPressed = false;

            if (playerMovement.rightHandController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed)
            {
                // Load next scene in index using SceneManager if A pressed
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                yield break;
            }
            else if (playerMovement.rightHandController.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonPressed) && secondaryButtonPressed)
            {
                // Load previous scene in index using SceneManager if B pressed
                SceneManager.LoadScene(0);
                yield break;
            }
            yield return null;
        }
    }



    private void IncrementProgress(int puzzlesCompleted)
    {
        if (DataPersistenceManager.instance == null)
        {
            Debug.LogError("DataPersistenceManager instance is null!");
        }
        else if (DataPersistenceManager.instance.GameData == null)
        {
            Debug.LogError("GameData on DataPersistenceManager instance is null!");
        }
        else
        {
            if (DataPersistenceManager.instance.GameData.playerProgress < puzzlesCompleted)
            // if player has completed more puzzles(levels) than the current progress , save progress
            {
                DataPersistenceManager.instance.GameData.playerProgress++;
                DataPersistenceManager.instance.SaveGame();
                Debug.Log("Progress incremented to: " + DataPersistenceManager.instance.GameData.playerProgress);
            }
        }

    }

    private void PlayJarlSpeech()
    {
        // Assuming jarlAudioSource and jarlSpeech are already assigned
        jarlAudioSource.clip = jarlSpeech;
        jarlAudioSource.Play();
    }

    public void PlayJarlVoiceLine(int lineIndex)
    {
        if (jarlAudioSource.isPlaying)
        {
            StartCoroutine(WaitForJarlSpeech());
        }
        jarlAudioSource.clip = jarlVoiceLines[lineIndex];
        jarlAudioSource.Play();
    }

    private IEnumerator WaitForJarlSpeech()
    {
        //stall until jarl speech is done, once finished , wait 3 seconds
        while (jarlAudioSource.isPlaying)
        {
            yield return null;
        }
        Debug.Log("Jarl has finished speaking, waiting 3 seconds");
        yield return new WaitForSeconds(1);
    }
}
