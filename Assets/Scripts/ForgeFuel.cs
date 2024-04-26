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
            isBurning = true;
            Debug.Log("Fuel added to forge. Current fuel amount: " + fuelAmount);
            // wait .5 seconds
            StartCoroutine(FuelWait());
            fuelAmount++;
            other.transform.position = startPosition;
            isBurning = false;
            Debug.Log("Forge is now burning.");

        }
    }

    IEnumerator FuelWait()
    {
        yield return new WaitForSeconds(2f);
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
