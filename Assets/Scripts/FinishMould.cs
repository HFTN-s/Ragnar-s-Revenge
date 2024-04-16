using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FinishMould : MonoBehaviour
{
    private GameObject key;
    private bool keySubmerged = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mould"))
        {
            Debug.Log("Mould tag is detected");
            key = other.transform.GetChild(0).gameObject;
            if (key == null)
            {
                Debug.Log("Key is null");
                return;
            }
            if (key.transform.localScale.x > 0.95)
            {
                Debug.Log("Key is correct scale");
                Debug.Log("Key Object: " + key.name);
                keySubmerged = true;
                StartCoroutine(KeyCooldownTimer(keySubmerged));
                key.transform.SetParent(null);

                // Enable the Rigidbody and add a MeshCollider
                Rigidbody rb = key.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = key.AddComponent<Rigidbody>();
                }
                rb.isKinematic = false;
                rb.useGravity = true;

                BoxCollider bc = key.GetComponent<BoxCollider>();
                if (bc == null)
                {
                    bc = key.AddComponent<BoxCollider>();
                }

                // Set collider for XRGrabInteractable
                XRGrabInteractable grabInteractable = key.GetComponent<XRGrabInteractable>();
                if (grabInteractable != null && grabInteractable.colliders.Count > 0)
                {
                    grabInteractable.colliders[0] = bc;
                }

                // Optionally add small forces to the key
                rb.AddForce(Vector3.up * 0.01f, ForceMode.Impulse);
                rb.AddForce(Vector3.forward * 0.01f, ForceMode.Impulse);

                // Detach the key from its parent

                Debug.Log("Key is dropped");
            }
        }
    }

    // Corrected coroutine

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mould"))
        {
            keySubmerged = false;
            key = null;
        }
    }
    IEnumerator KeyCooldownTimer(bool keySubmerged)
    {
        while (keySubmerged)
        {
            Debug.Log("Starting cooldown timer");
            yield return new WaitForSeconds(5);
            Debug.Log("Cooldown timer finished");
        }
    }
}
