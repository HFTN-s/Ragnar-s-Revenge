using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ForgeMould : MonoBehaviour
{
    private bool forgeRunning = false;
    private GameObject currentMould;
    private GameObject mouldObject;
    public ForgeFuel forgeFuelScript;
    public ParticleSystem moltenMetal;
    public MouldTrigger mouldTrigger;
    private float scale = 0.01f;
    public GameObject[] keyObjects;
    public Texture moltenMetalMaterial;
    private AudioSource jarlAudioSource;
    private TutorialLevelManager levelManager;
    private AudioClip outOfMetalAudio;
    private bool outOfMetalPlayed = false;
    private AudioClip noMouldAudio;
    private bool noMouldPlayed = false;


    void Start()
    {
        levelManager = GameObject.Find("TutorialLevelManager").GetComponent<TutorialLevelManager>();
        outOfMetalAudio = levelManager.jarlVoiceLines[6];
        noMouldAudio = levelManager.jarlVoiceLines[12];
        forgeFuelScript = GetComponent<ForgeFuel>();
        moltenMetal.Stop();
        Debug.Log("Forge script initialized, fuel amount: " + forgeFuelScript.fuelAmount);
        jarlAudioSource = GameObject.Find("JarlVoice").GetComponent<AudioSource>();
    }

    public void StartForge()
    {
        Debug.Log("Attempting to start forge...");
        if (forgeFuelScript.fuelAmount > 0)
        {
            if (!forgeRunning)
            {
                Debug.Log("Forge is starting...");
                currentMould = mouldTrigger.currentMould;

                if (currentMould != null)
                {
                    moltenMetal.Play();
                    forgeFuelScript.fuelAmount -= 1;
                    Debug.Log("Forge started. Fuel reduced, new fuel amount: " + forgeFuelScript.fuelAmount);
                    mouldObject = currentMould.transform.GetChild(0).gameObject;
                    Debug.Log("Found mould: " + currentMould.name + ", starting IncreaseScale coroutine with object: " + mouldObject.name);
                    StartCoroutine(IncreaseScale());
                }
                else
                {
                    JarlSaysNoMould();
                    Debug.Log("StartForge called but no mould found.");
                }
            }
            else
            {
                Debug.Log("Forge already running.");
            }
        }
        else
        {
            JarlSaysOutOfMetal();
            Debug.Log("Not enough fuel to start the forge.");
        }
    }

    public void StopForge()
    {
        Debug.Log("Stopping forge...");
        if (forgeRunning)
        {
            moltenMetal.Stop();
            StopCoroutine(IncreaseScale());
            forgeRunning = false;
            Debug.Log("Forge stopped.");
        }
        else
        {
            Debug.Log("StopForge called but forge was not running.");
        }
    }

    IEnumerator IncreaseScale()
    {
        forgeRunning = true;
        Debug.Log("IncreaseScale coroutine started with object: " + mouldObject.name);

        float targetScale = GetTargetScale(mouldObject.name);
        float scaleIncrement = GetScaleIncrement(mouldObject.name);

        mouldObject.GetComponent<Renderer>().material.mainTexture = moltenMetalMaterial;

        while (scale < targetScale)
        {
            scale += scaleIncrement;
            mouldObject.transform.localScale = new Vector3(scale, scale, scale);
            Debug.Log("Increasing scale of " + mouldObject.name + " to: " + scale);
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Scale increase completed for " + mouldObject.name);

        // Stop the particle effect after scaling is complete
        moltenMetal.Stop();

        // Decrement the fuel
        forgeFuelScript.fuelAmount -= 1;
        Debug.Log("Forge fuel decremented. New fuel amount: " + forgeFuelScript.fuelAmount);

        // Reset scale for the next use
        scale = 0.01f;
        forgeRunning = false;
    }


    // Example method to get the target scale based on the mould name.
    float GetTargetScale(string mouldName)
    {
        switch (mouldName)
        {
            case "KeyMould1": return 1.05f;
            case "KeyMould2": return 2.05f;
            case "KeyMould3": return 3.05f;
            default: return 1f; // default scale if no match is found.
        }
    }

    // Example method to get the scale increment based on the mould name.
    float GetScaleIncrement(string mouldName)
    {
        switch (mouldName)
        {
            case "KeyMould1": return 0.05f;
            case "KeyMould2": return 0.1f;
            case "KeyMould3": return 0.15f;
            default: return 0.05f; // default increment if no match is found.
        }
    }

    void JarlSaysOutOfMetal()
    {
        //if jarl audio source is not playing, then play out of metal audio, else wait until jarl audio source is done playing then play out of metal audio
        if (!jarlAudioSource.isPlaying && !outOfMetalPlayed)
        {
            jarlAudioSource.clip = outOfMetalAudio;
            jarlAudioSource.Play();
            outOfMetalPlayed = true;
        }
        else
        {
            StartCoroutine(WaitForJarlSpeech());
        }
    }

    void JarlSaysNoMould()
    {
        //if jarl audio source is not playing, then play no mould audio, else wait until jarl audio source is done playing then play no mould audio
        if (!jarlAudioSource.isPlaying && !noMouldPlayed)
        {
            jarlAudioSource.clip = noMouldAudio;
            jarlAudioSource.Play();
            noMouldPlayed = true;
        }
        else
        {
            StartCoroutine(WaitForJarlSpeech());
        }
    }

    IEnumerator WaitForJarlSpeech()
    {
        //wait for jarl to finish speaking
        yield return new WaitWhile(() => jarlAudioSource.isPlaying);
        //play out of metal audio
    }
}
