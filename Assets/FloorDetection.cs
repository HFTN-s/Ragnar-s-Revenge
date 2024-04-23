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
                item.AddComponent<InteractableObject>();
            }
        }
    }
}
