using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SunstoneChcker : MonoBehaviour
{
    public UnityEvent puzzle2CompletedEvent;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sunstone"))
        {
            Debug.Log("Sunstone collision detected");

            //freeze sunstone
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            puzzle2CompletedEvent.Invoke();
        }
    }
}
