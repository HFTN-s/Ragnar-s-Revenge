using UnityEngine;
using UnityEngine.Events;

public class MouldPlacement : MonoBehaviour
{
    public UnityEvent OnMouldPlaced;
    private GameObject currentMould = null; // Track the current mould in the placement area.

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mould" && currentMould == null) // Check if the collider is a mould and no mould is currently placed.
        {
            FillMould fillMouldScript = other.GetComponent<FillMould>(); // Get the FillMould script attached to the mould.
            if (fillMouldScript != null)
            {
                fillMouldScript.SetCurrentMould(other.gameObject); // Call SetCurrentMould on the mould's FillMould script.
                OnMouldPlaced.Invoke();
                currentMould = other.gameObject; // Set the current mould.
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentMould) // Check if the exiting object is the current mould.
        {
            FillMould fillMouldScript = other.GetComponent<FillMould>(); // Get the FillMould script attached to the mould.
            if (fillMouldScript != null)
            {
                fillMouldScript.RemoveCurrentMould(); // Call RemoveCurrentMould on the mould's FillMould script.
                currentMould = null; // Clear the current mould.
            }
        }
    }
}
