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
                Debug.Log("Key is correct scale");
                Debug.Log("Key Object: " + key.name);
                //lock key to padlock
                key.transform.position = keyLockPosition.transform.position;
                key.transform.rotation = keyLockPosition.transform.rotation;
                //Stop tracking position of XR Hands
                key.GetComponent<XRGrabInteractable>().trackPosition = false;
                
                //Set up ConfigurableJoint
                ConfigurableJoint cj = key.AddComponent<ConfigurableJoint>();
                cj.xMotion = ConfigurableJointMotion.Locked;
                cj.yMotion = ConfigurableJointMotion.Locked;
                cj.zMotion = ConfigurableJointMotion.Locked;
                cj.angularXMotion = ConfigurableJointMotion.Free;
                cj.angularYMotion = ConfigurableJointMotion.Locked;
                cj.angularZMotion = ConfigurableJointMotion.Locked;
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
