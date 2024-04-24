using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    public GameObject[] interactableObjects;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject item in interactableObjects)
        {
            if (collision.gameObject == item)
            {
                Debug.Log("Object has been detected from the list");
                Debug.Log("Adding" + item.name + " to the list of interactable objects");
                if (item.GetComponent<InteractableObject>() == null)
                {
                    Debug.Log("Adding InteractableObject script to " + item.name);
                    item.AddComponent<InteractableObject>();
                }
            }
        }
    }
}
