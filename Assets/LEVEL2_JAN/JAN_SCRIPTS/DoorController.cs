using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Public variables to adjust the rotation from the Unity Editor
    public float rotationX = 0f;
    public float rotationY = 0f;
    public float rotationZ = 0f;

    void Start()
    {
        // Optionally, store the initial rotation if needed later
        // initialRotationEuler = transform.eulerAngles;
    }

    public void OpenDoor()
    {
        // Directly set the new rotation for the door
        transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
    }
}
