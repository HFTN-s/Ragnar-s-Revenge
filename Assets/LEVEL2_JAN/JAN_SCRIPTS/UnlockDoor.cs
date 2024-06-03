using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject door; // Assign this in the Inspector

    public void disableDoorCollider()
    {
        door.GetComponent<Collider>().enabled = false;
    }
}
