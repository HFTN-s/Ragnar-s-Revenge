using System.Collections;
using UnityEngine;

public class ForgeMould : MonoBehaviour
{
    private bool forgeRunning = false;
    private GameObject currentMould;
    private GameObject mouldObject;
    public ForgeFuel forgeFuelScript;
    public ParticleSystem moltenMetal;
    public MouldTrigger mouldTrigger;
    private float scale = 0.01f;

    void Start()
    {
        forgeFuelScript = GetComponent<ForgeFuel>();
        moltenMetal.Stop();
        Debug.Log("Forge script initialized, fuel amount: " + forgeFuelScript.fuelAmount);
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
        Debug.Log("IncreaseScale coroutine started.");
        while (scale <= 1.05f)
        {
            mouldObject.transform.localScale = new Vector3(scale, scale, scale);
            scale += 0.05f;
            Debug.Log("Increasing scale of " + mouldObject.name + " to: " + scale);
            yield return new WaitForSeconds(0.1f); // You might want to slow down the scaling over time
        }
        moltenMetal.Stop();
        forgeRunning = false;
        scale = 0.01f;
        Debug.Log("Scale increase completed, stopping moltenMetal particle system and resetting scale.");
    }
}
