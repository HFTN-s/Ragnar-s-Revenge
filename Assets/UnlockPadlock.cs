using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UnlockPadlock : MonoBehaviour
{
    public GameObject correctKey;
    public GameObject keyLockPosition;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            Debug.Log("Key tag is detected");
            GameObject key = other.gameObject;
            if (key.transform.localScale.x > 0.95)
            {
                if (key == correctKey)
                {
                    Destroy(key);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Key is incorrect");
                }
            }
        }
    }

    void CheckKey()
    {

    }

    void Unlock()
    {

    }
}
