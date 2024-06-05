using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.XR;
using TMPro;
using UnityEngine.SceneManagement;
public class TutorialLevelManager : MonoBehaviour
{
    public GameObject fire;
    public Light[] lightSources;
    public AudioClip[] ragnarSpeech;
    public AudioSource ragnarAudioSource;
    public GameObject ragnarSpeechObject;
    public bool puzzle1Check = false;
    public bool puzzle2Check = false;
    public bool puzzle3Check = false;
    public bool puzzle1Complete = false;
    public bool puzzle2Complete = false;
    public bool puzzle3Complete = false;
    public bool roomComplete = false;
    public bool canLeaveLevel = false;

    [SerializeField] private GameObject puzzle1Trigger;
    [SerializeField] private GameObject puzzle2Trigger;
    [SerializeField] private GameObject puzzle3Trigger;
    public AudioSource puzzleCompleteAudio;

    public Timer timer;

    public GameObject VikingStatue;
    public GameObject VikingGhostStatue;

    public float waitTime = 5.0f; // time to wait between events in seconds

    public AudioClip jarlSpeech;
    public AudioClip[] jarlVoiceLines;
    public AudioSource jarlAudioSource;
    public AudioSource musicAudioSource;
    public PlayerMovement playerMovement;
    public bool hasRing;
    public bool needsMetal;
    public bool usedWrongKey;
    public GameObject[] fireObjects;
    public Material torchMaterial;
    public bool defeatedSkeleton;
    public bool unlockedDoor;
    public bool openedDoor;
    [SerializeField] public InputDevice leftHandController;
    //TMP text component for displaying text
    [SerializeField] private TextMeshPro text;
    public GameObject endOfLevelText;

    private void Start()
    {
        // Subscribe to the events for each puzzle base item
        // When event is triggered i.e when the puzzle is completed
        // set the puzzle check to true (Done in puzzle scripts)

        //Add listener to speech trigger for when an object enters the trigger

        //set ghost to inactive
        VikingGhostStatue.SetActive(false);
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerMovement.canMove = true;

        //set light intensity to 0 for all light objects in array
        // Set light intensity to 0 for all light objects in array
        puzzle2Trigger.SetActive(false);

        fire.SetActive(true);
        endOfLevelText.SetActive(false);

    }

    void Update()
    {
        if (puzzle1Check && !puzzle1Complete)
        // if puzzle 1 is Solved , then run event , set puzzle complete to true
        // and set puzzle Check back to false so it doesn't run again
        {
            Debug.Log("Puzzle 1 Complete");
            puzzleCompleteAudio.Play();
            Puzzle1Completed();
        }

        if (puzzle2Check && !puzzle2Complete)
        // if puzzle 2 is Solved , then run event , set puzzle complete to true
        {
            puzzleCompleteAudio.Play();
            Puzzle2Completed();
        }

        if (puzzle3Check && !puzzle3Complete)
        // if puzzle 3 is Solved , then run event , set puzzle complete to true
        {
            puzzleCompleteAudio.Play();
            Puzzle3Completed();
        }

        if (roomComplete)
        {
            // play one shot audio for puzzle complete
            //puzzleCompleteAudio.Play();
            // if all puzzles are solved, then run event
            RoomCompleted();
        }

        if (hasRing)
        {
            puzzle2Trigger.SetActive(true);

        }
    }

    void Puzzle1Completed()
{
    Debug.Log("Puzzle 1 Complete, Attempting to play audio");
    musicAudioSource.Pause();
    if (jarlAudioSource.isPlaying)
    {
        Debug.Log("Jarl is already speaking, waiting for him to finish");
        jarlAudioSource.Stop();
    }

    jarlAudioSource.clip = jarlVoiceLines[9];
    jarlAudioSource.Play();
    StartCoroutine(ResumeMusicAfterVoiceLine(jarlAudioSource));

    puzzle1Complete = true;
    puzzle1Check = false;
}


    void Puzzle2Completed()
{
    if (hasRing && puzzle2Check && !puzzle2Complete)
    {
        Debug.Log("Puzzle 2 Complete");
        StartCoroutine(HandlePuzzle2Sequence());
    }
}

IEnumerator HandlePuzzle2Sequence()
{
    Debug.Log("Starting Puzzle 2 Sequence");
    musicAudioSource.Pause();
    puzzle2Complete = true;
    puzzle2Check = false;

    Debug.Log("Playing voice line 13");
    yield return StartCoroutine(PlayVoiceLineAndWait(13));
    yield return new WaitForSeconds(3);
    GameObject.Find("SKELETON").SendMessage("ActivateSkeleton");
    Debug.Log("Playing voice line 15");
    yield return StartCoroutine(PlayVoiceLineAndWait(15));
    yield return new WaitForSeconds(3);
    Debug.Log("Playing voice line 8");
    yield return StartCoroutine(PlayVoiceLineAndWait(8));
    Debug.Log("About to play music");
    musicAudioSource.Play();
}


    IEnumerator PlayVoiceLineAndWait(int lineIndex)
    {
        // Wait if there's already a voice line playing
        while (jarlAudioSource.isPlaying)
        {
            yield return null; // Wait until the current audio is finished
        }

        // Set the clip and play it
        jarlAudioSource.clip = jarlVoiceLines[lineIndex];
        jarlAudioSource.Play();

        // Wait until this voice line is finished before proceeding
        while (jarlAudioSource.isPlaying)
        {
            yield return null;
        }
    }




    void Puzzle3Completed()
{
    puzzle3Complete = true;
    puzzle3Check = false;
    musicAudioSource.Pause();
    
    if (jarlAudioSource.isPlaying)
    {
        StartCoroutine(WaitForJarlSpeech());
    }
    else
    {
        jarlAudioSource.clip = jarlVoiceLines[1];
        jarlAudioSource.Play();
        StartCoroutine(ResumeMusicAfterVoiceLine(jarlAudioSource));
    }
}
private IEnumerator ResumeMusicAfterVoiceLine(AudioSource audioSource)
{
    while (audioSource.isPlaying)
    {
        yield return null;
    }
    musicAudioSource.Play();
}


    void RoomCompleted()
{
    if (!canLeaveLevel)
    {
        int timeTaken = timer.GetSeconds();
        DataPersistenceManager.instance.SaveHighScore(timeTaken);
        text.text = $"{timeTaken / 60:D2}:{timeTaken % 60:D2}";
        IncrementProgress(1);
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
        canLeaveLevel = true;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            yield break;
        }
        else if (playerMovement.rightHandController.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonPressed) && secondaryButtonPressed)
        {
            // Load previous scene in index using SceneManager if B pressed
            SceneManager.LoadScene(1, LoadSceneMode.Single);
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

    // Handles trigger events from child colliders
    public void HandleTriggerEnter(int colliderID)
    {
        switch (colliderID)
        {
            case 1: // ID for puzzle 1 collider
                Debug.Log("Ragnars Speech Triggered");

                if (ragnarAudioSource != null)
                {
                    StartCoroutine(PlayRagnarsSpeechAndHandleStatue());

                }
                else
                {
                    Debug.LogWarning("AudioSource or Clip not properly assigned.");
                }
                // Handle Puzzle 1 trigger
                break;
            case 2: // ID for puzzle 2 collider
                Debug.Log("Puzzle 2 Triggered");
                // Handle Puzzle 2 trigger
                break;
            // Add more cases as needed
            default:
                Debug.LogWarning("Unknown collider triggered");
                break;
        }
    }

    private IEnumerator PlayRagnarsSpeechAndHandleStatue()
    {
        // pause music 
        musicAudioSource.Pause();
        playerMovement.canMove = false; // Stop player movement
        VikingGhostStatue.SetActive(true);
        fire.SetActive(true);
        Destroy(ragnarSpeechObject); // Destroy the speech trigger object
        ragnarAudioSource.clip = ragnarSpeech[0]; // Assign the first clip in the array
        ragnarAudioSource.Play(); // Play the audio

        // Fade out lights
        foreach (Light lightSource in lightSources)
        {
            StartCoroutine(LightFade(lightSource, 0, 2)); // Fade to 0 intensity over 2 seconds
            SetFireObjectsInactive();
        }

        // Wait while the audio is playing
        while (ragnarAudioSource.isPlaying)
        {
            yield return null; // Wait until next frame
        }

        // Fade lights back in
        foreach (Light lightSource in lightSources)
        {
            StartCoroutine(LightFade(lightSource, 1, 2)); // Fade back to full intensity over 2 seconds
            SetFireObjectsActive();
        }
        VikingGhostStatue.SetActive(false);
        playerMovement.canMove = true; // Allow player movement again
        yield return new WaitForSeconds(3);
        // pause music
        PlayJarlSpeech(); // Start playing Jarl's speech
    }

    private void PlayJarlSpeech()
    {
        musicAudioSource.Pause();
        // Assuming jarlAudioSource and jarlSpeech are already assigned
        jarlAudioSource.clip = jarlSpeech;
        jarlAudioSource.Play();
        // Wait for Jarl's speech to finish then resume music
        StartCoroutine(WaitForJarlSpeech());
    }

    public void PlayJarlVoiceLine(int lineIndex)
    {
        musicAudioSource.Pause();
        if (jarlAudioSource.isPlaying)
        {
            StartCoroutine(WaitForJarlSpeech());
        }
        else
        {
            jarlAudioSource.clip = jarlVoiceLines[lineIndex];
            jarlAudioSource.Play();
            StartCoroutine(WaitForJarlSpeech()); // Ensure the coroutine is started here too
        }
    }


    private IEnumerator LightFade(Light light, float targetIntensity, float duration)
    {
        float startIntensity = light.intensity;
        float time = 0;

        while (time < duration)
        {
            Debug.Log("Fading light");
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        light.intensity = targetIntensity;
    }

    private IEnumerator WaitForJarlSpeech()
    {
        // Stall until Jarl's speech is done, once finished, wait 3 seconds
        while (jarlAudioSource.isPlaying)
        {
            musicAudioSource.Pause();
            yield return null;
        }
        Debug.Log("Jarl has finished speaking, waiting 3 seconds");
        yield return new WaitForSeconds(3);
        musicAudioSource.Play(); // Resume the music
    }

    void PlayEndJarlVoiceLine(int clipIndex)
{
    jarlAudioSource.clip = jarlVoiceLines[clipIndex];
    jarlAudioSource.Play();
}

    //set fire objects to active
    public void SetFireObjectsActive()
    {
        foreach (GameObject fireObject in fireObjects)
        {
            GameObject parent = fireObject.transform.parent.gameObject;
            Debug.Log("Parent: " + parent.name);
            if (parent.GetComponent<twinkle>() != null)
            {
                parent.GetComponent<twinkle>().enabled = true;
            }
            foreach (Transform child in parent.transform)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Debug.Log("Child: " + child.name);
                    //set emission to 0
                    child.GetComponent<Renderer>().material.SetColor("_EmissionColor", torchMaterial.GetColor("_EmissionColor"));
                }
                fireObject.SetActive(true);
            }
        }
    }
    //set fire objects to inactive
    public void SetFireObjectsInactive()
    {
        foreach (GameObject fireObject in fireObjects)
        {
            GameObject parent = fireObject.transform.parent.gameObject;
            Debug.Log("Parent: " + parent.name);
            if (parent.GetComponent<twinkle>() != null)
            {
                parent.GetComponent<twinkle>().enabled = false;
            }
            foreach (Transform child in parent.transform)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Debug.Log("Child: " + child.name);
                    //set emission to 0
                    child.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                }
                fireObject.SetActive(false);
            }
        }

    }
}