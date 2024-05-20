using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHit : MonoBehaviour
{
    private GameObject skeleton;
    void Start()
    {
        skeleton = GameObject.Find("SKELETON");
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer")
        {
            Debug.Log("Skeleton has been hit by player");
            // Set the isCollided parameter to true to trigger the crawling animation
            //un child object from the parent
            //disable collider
            GetComponent<BoxCollider>().enabled = false;
            //send message to game object "SKELETON" to run SetDead()
            skeleton.SendMessage("SetDead");
            //turn off skeleton mesh collider
            GetComponent<MeshCollider>().enabled = false;
            //turn off skeleton navmesh agent
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            transform.parent = null;
            //add mesh renderer to the object
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            //add rigidbody to the object
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            // add force to the side when hit
            rb.AddForce(Vector3.right * 10);
            //add force upwards when hit
            rb.AddForce(Vector3.up * 10);
        }
    }
}
