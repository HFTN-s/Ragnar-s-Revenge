using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Rb_AddForce : MonoBehaviour
{
    private Rigidbody rb;
    public int force = 1000;
    public int pieceID;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //add random force in forward direction
        //wait a second then add force forward and up
        if (rb.isKinematic)
        {
            //set xr grab interactable active to false
            GetComponent<XRGrabInteractable>().enabled = false;
        }
        StartCoroutine(AddForce());


        
    }

    IEnumerator AddForce()
    {
        // wait random time
        // set kinematic to false
        rb.AddForce(transform.forward * Random.Range(-force, -force));
        rb.AddForce(transform.up * Random.Range(-force, force));
        rb.AddForce(transform.right * Random.Range(-force, force));
        yield return new WaitForSeconds(Random.Range(1, 3));
        rb.AddForce(transform.forward * Random.Range(force, -force));
        yield return new WaitForSeconds(Random.Range(1, 3));
        rb.AddForce(transform.up * Random.Range(-force, force));
        yield return new WaitForSeconds(Random.Range(1, 3));
        rb.AddForce(transform.right * Random.Range(force, -force));
    }

    void OnCollisionEnter(Collision collision)
    {
        //if object hits boundary , then add force the opposite direction
        if (collision.gameObject.tag == "Boundary")
        {
            rb.AddForce(-collision.contacts[0].normal * force);
        }
    }
}
