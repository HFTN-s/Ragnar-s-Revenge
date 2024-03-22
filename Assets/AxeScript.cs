using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AxeScript : MonoBehaviour
{
    [SerializeField] private GameObject bladeObject;
    private Rigidbody rb;
    private Collider bladeCollider;
    [SerializeField] private int minimumForce = 1;
    [SerializeField] private XRGrabInteractable axeGrabInteractable;
    private AudioSource audioSource; // Audio source to play sounds
    [SerializeField] private AudioClip[] axeSounds;

    void Start()
    {
        bladeCollider = bladeObject.GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        axeGrabInteractable.onSelectEntered.AddListener(HandleAxeGrabbed);
        audioSource = gameObject.AddComponent<AudioSource>(); // Initialize the AudioSource
    }

    void OnCollisionEnter(Collision other)
    {
        if (bladeCollider.bounds.Intersects(other.collider.bounds))
        {
            Debug.Log("Blade hit something!");
            foreach (ContactPoint contact in other.contacts)
            {
                if (other.gameObject.tag == "Wood" && rb.velocity.magnitude > minimumForce)
                {
                    Debug.Log("Hit Wood");
                    FreezeRigidbody();
                    PlayAxeSound(); // Play the sound effect
                    break;
                }
            }
        }
    }

    private void FreezeRigidbody()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    private void PlayAxeSound()
    {
        if (audioSource != null && axeSounds.Length > 0)
        {
            // Play a random sound from the axeSounds array
            audioSource.clip = axeSounds[Random.Range(0, axeSounds.Length)];
            audioSource.Play();
        }
    }

    void HandleAxeGrabbed(XRBaseInteractor interactor)
    {
        Debug.Log("Axe Grabbed");
        rb.constraints = RigidbodyConstraints.None;
    }
}
