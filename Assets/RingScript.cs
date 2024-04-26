using UnityEngine;

public class RingScript : MonoBehaviour
{
    private Transform ringFinger;
    
    void Start()
    {
        // Find the ring finger and store its Transform
        GameObject fingerObject = GameObject.Find("RingHandTransform");
        ringFinger = fingerObject.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerHand")
        {
            // Set the ring's parent to the ring finger's transform
            transform.parent = ringFinger;
            // Optionally, you can also immediately set the ring's position and rotation to match the finger
            transform.position = ringFinger.position;
            transform.rotation = ringFinger.rotation;
        }
    }
}
