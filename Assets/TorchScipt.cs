using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TorchScipt : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private XRGrabInteractable torchGrabInteractable;
    private AudioSource torchAmbientSound; // Audio source to play sounds
    [SerializeField] private AudioClip[] torchSounds;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        torchGrabInteractable.onSelectEntered.AddListener(HandleTorchGrabbed);
        torchAmbientSound = gameObject.AddComponent<AudioSource>(); // Initialize the AudioSource
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wood")
        {
            Debug.Log("Torch hit wood");
            Debug.Log("Other will catch fire in later update"); 
            Debug.Log("will likely lead to player death"); 
            Debug.Log("so should be avoided");
        }

        if (other.gameObject.tag == "TorchHolder")
        {
            //get child component of torch holder

            Debug.Log("Torch placed in holder");
            //get transform of other object
            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;
            TorchPlaced();
        }
    }

    private void HandleTorchGrabbed(XRBaseInteractor interactor)
    {
        Debug.Log("Torch Grabbed");
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        rb.constraints = RigidbodyConstraints.None;
    }

    private void TorchPlaced()
    {
        Debug.Log("Torch Placed");
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }
    }
