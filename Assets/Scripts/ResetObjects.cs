using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    public GameObject resetTransformObject;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        other.gameObject.transform.position = resetTransformObject.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = resetTransformObject.transform.position;
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Object has exited the trigger");
        Debug.Log("Object has been reset to" + resetTransformObject.transform.position);
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("Object has exited the collision");
        Debug.Log("Object has been reset to" + resetTransformObject.transform.position);
    }
}
