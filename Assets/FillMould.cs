using System.Collections;
using UnityEngine;

public class FillMould : MonoBehaviour
{
    public float scaleIncrease = 0.01f;
    public bool isFilling = false;
    public GameObject MouldPlacement;
    public bool isMouldPlaced = false;
    public GameObject mould;

    // Method to start filling a given mould with a given mouldFill object.
    // This method is now public and expects specific mould and mouldFill objects as arguments.

  // Assuming SetCurrentMould and RemoveCurrentMould manage state rather than set a mould object.
public void SetCurrentMould(GameObject mould)
{
    if (isMouldPlaced) return;
    isMouldPlaced = true;
    // Additional logic to prepare the mould for filling (if necessary).
}

public void RemoveCurrentMould()
{
    if (!isMouldPlaced) return;
    isMouldPlaced = false;
    // Additional logic to reset or clean up the mould (if necessary).
    PauseScaleMould(); // Consider pausing or stopping the filling process here.
}


    public void DoFillMould(GameObject mould)
    {
        if (isFilling) return; // Prevents starting a new fill process if one is already ongoing.
        StartCoroutine(ScaleMould(mould)); // Starts the coroutine to fill the mould.
    }

    // Coroutine to scale the mouldFill object.
    // It only requires the mouldFill GameObject to function.
    IEnumerator ScaleMould(GameObject mould)
    {
        isFilling = true;
        Debug.Log("Filling mould");
        GameObject mouldFill = mould.transform.GetChild(0).gameObject; // Gets the mouldFill object from the mould.
        Vector3 scaleChange = new Vector3(scaleIncrease, scaleIncrease, scaleIncrease) * Time.deltaTime;
        while (mouldFill.transform.localScale.x < 1)
        {
            mouldFill.transform.localScale = Vector3.MoveTowards(mouldFill.transform.localScale, Vector3.one, scaleChange.magnitude);
            yield return new WaitForSeconds(0.05f); // Waits for a short duration before continuing the loop.
        }
        mouldFill.transform.localScale = Vector3.one; // Ensures the scale is set to exactly 1.

        isFilling = false; // Marks the end of the filling process.
    }

    // Method to pause the filling process of a mouldFill object.
    // It should ideally reference the specific coroutine of the mouldFill being scaled,
    // but since Unity doesn't allow stopping coroutines by passing a parameter directly to StopCoroutine,
    // this method simply stops all coroutines, which is a limitation in this approach.
    public void PauseScaleMould()
    {
        StopAllCoroutines(); // Stops all coroutines to pause the scaling.
        isFilling = false;
    }
}
