using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rb_AddForce : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //add force in forward direction
        rb.AddForce(transform.forward * -1000);
        
    }
}
