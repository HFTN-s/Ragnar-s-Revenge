using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHolderScript : MonoBehaviour
{
    private bool torchPlaced = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Torch Holder hit");
        if (other.gameObject.tag == "Torch" && torchPlaced == false)
        {
            Debug.Log("Torch placed in holder");
            torchPlaced = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("Torch Holder hit");
        if (other.gameObject.tag == "Torch" && torchPlaced == true)
        {
            Debug.Log("Torch removed from holder");
            torchPlaced = false;
        }
    }
}
