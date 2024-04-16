using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouldTrigger : MonoBehaviour
{
    public UnityEvent OnMouldPlaced;
    public UnityEvent OnMouldRemoved;
    public GameObject currentMould = null; // Track the current mould in the placement area.
    public GameObject mouldPlacementObject;
    public GameObject mouldResetObject;
    public bool isMouldPlaced = false;
    private Vector3 mouldPlacementPosition;
    private Quaternion mouldPlacementRotation;
    private Vector3 mouldResetPosition;

    void Start()
    {
        mouldPlacementPosition = mouldPlacementObject.transform.position;
        mouldPlacementRotation = mouldPlacementObject.transform.rotation;
        mouldResetPosition = mouldResetObject.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mould" && currentMould == null) // Check if the collider is a mould and no mould is currently placed.
        {
            currentMould = other.gameObject; // Set the current mould.
            Debug.Log("Mould placed in trigger area.");
            //Set to the position of the mould placement object local position
            currentMould.transform.position = mouldPlacementPosition;
            currentMould.transform.rotation = mouldPlacementRotation;
            Debug.Log("Mould placed at position: " + mouldPlacementPosition);
            Debug.Log("Current Mould is " + currentMould.name);
            isMouldPlaced = true;
            OnMouldPlaced.Invoke();
        }

        else if (other.gameObject.tag == "Mould" && currentMould != null)
        {
            Debug.Log("Mould already placed in trigger area.");
            other.gameObject.transform.position = mouldResetPosition;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentMould) // Check if the exiting object is the current mould.
        {
            currentMould = null; // Clear the current mould.
            isMouldPlaced = false;
            OnMouldRemoved.Invoke();
        }
    }
   
}
