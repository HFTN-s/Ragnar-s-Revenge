using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeFuel : MonoBehaviour
{
    public int fuelAmount = 0;
    private bool isBurning = false;
    private Vector3 startPosition;
    public GameObject resetPosition;
    public GameObject[] forgeLights;
    public AudioSource forgeAudio;
    public AudioClip forgeStart;
    public AudioClip forgeStop;
    public AudioClip forgeBurning;
    public AudioClip forgeMetalInput;
    // Start is called before the first frame update

    void Start()
    {
        startPosition = resetPosition.transform.position;
        // set lights intensity to 0
        foreach (GameObject light in forgeLights)
        {
            light.GetComponent<Light>().intensity = 0;
        }
    }

    void Update()
    {
        if (fuelAmount > 0 && !isBurning)
        {
            FadeInLight();
            isBurning = true;
        }
        else if (fuelAmount == 0 && isBurning)
        {
            FadeOutLight();
            isBurning = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Metal" && !isBurning)
        {
            forgeAudio.clip = forgeMetalInput;
            forgeAudio.Play();
            StartCoroutine(FuelWait());
            forgeAudio.clip = forgeStart;
            forgeAudio.Play();
            isBurning = true;
            Debug.Log("Fuel added to forge. Current fuel amount: " + fuelAmount);
            // wait .5 seconds
            StartCoroutine(FuelWait());
            fuelAmount++;
            other.transform.position = startPosition;
            isBurning = false;
            Debug.Log("Forge is now burning.");
            //check if forgeAudio is playing, if playing wait for it to finish
            if (forgeAudio.isPlaying)
            {
                StartCoroutine(WaitForForgeAudio());
            }
            else
            {
                forgeAudio.clip = forgeBurning;
                // set audio to loop
                forgeAudio.loop = true;
                forgeAudio.Play();
            }

        }
    }

    private IEnumerator WaitForForgeAudio()
    {
        yield return new WaitForSeconds(forgeAudio.clip.length);
        forgeAudio.clip = forgeBurning;
        forgeAudio.Play();
    }

    IEnumerator FuelWait()
    {
        yield return new WaitForSeconds(1f);
    }

    void FadeInLight()
    {
        foreach (GameObject light in forgeLights)
        {
            StartCoroutine(LightFade(light.GetComponent<Light>(), 1, 2));
        }
    }

    void FadeOutLight()
    {
        foreach (GameObject light in forgeLights)
        {
            StartCoroutine(LightFade(light.GetComponent<Light>(), 0, 2));
            forgeAudio.clip = forgeStop;
            forgeAudio.loop = false;
            forgeAudio.Play();
        }
    }

    private IEnumerator LightFade(Light light, float targetIntensity, float duration)
    {
        float startIntensity = light.intensity;
        float time = 0;

        while (time < duration)
        {
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        light.intensity = targetIntensity;
    }
    
}
