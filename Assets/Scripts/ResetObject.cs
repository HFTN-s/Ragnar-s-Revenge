using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
  // On Collision or Trigger, reset object to this position
    private Vector3 resetPosition;
    private Quaternion resetRotation;


    void Start()
    {
        resetPosition = transform.position;
        resetRotation = transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reset")
        {
            Debug.Log("Resetting object position");
            transform.position = resetPosition;
            transform.rotation = resetRotation;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Reset")
        {
            Debug.Log("Resetting object position");
            transform.position = resetPosition;
            transform.rotation = resetRotation;
        }
    }
}
