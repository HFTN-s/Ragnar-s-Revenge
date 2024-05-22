using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RingChecker : MonoBehaviour
{
    public GameObject ring;
    public GameObject jarlFinger;
    public Level3Manager level3Manager;
    public UnityEvent puzzle3CompletedEvent;
    public string ringName = "Ring";
    void OnTriggerEnter(Collider other)
    {
        ring = GameObject.Find(ringName);
        if (other.gameObject.CompareTag("PlayerHands"))
        {
            //change ring transform to jarls finger transform
            ring.transform.SetParent(jarlFinger.transform);
            ring.transform.position = jarlFinger.transform.position;
            ring.transform.rotation = jarlFinger.transform.rotation;

            //Level 3 Puzzle 2 Completed
            puzzle3CompletedEvent.Invoke();
        }
    }
}
