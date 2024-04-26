using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FinishMould : MonoBehaviour
{
    private GameObject key;
    private GameObject mould;
    private bool keySubmerged = false;
    private AudioSource jarlVoice;
    public AudioClip boxKeyAudio;
    private bool hasMadeBoxKey = false;
    public AudioClip doorKeyAudio;
    private bool hasMadeDoorKey = false;

    void Start()
    {
        jarlVoice = GameObject.Find("JarlVoice").GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mould"))
        {
            mould = other.gameObject;
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
                // add xr grab interactable
                XRGrabInteractable grabInteractable = key.AddComponent<XRGrabInteractable>();
                //set to active
                grabInteractable.enabled = true;
                if (grabInteractable != null && grabInteractable.colliders.Count > 0)
                {
                    grabInteractable.colliders[0] = bc;
                }
                // Set collider for XRGrabInteractable
                //set to active
                grabInteractable.enabled = true;
                if (grabInteractable != null && grabInteractable.colliders.Count > 0)
                {
                    grabInteractable.colliders[0] = bc;
                }

                // Optionally add small forces to the key
                //rb.AddForce(Vector3.up * 0.01f, ForceMode.Impulse);
                //rb.AddForce(Vector3.forward * 0.01f, ForceMode.Impulse);

                // Detach the key from its parent and then delete the parent

                Debug.Log("Key is dropped");
                Destroy(mould);
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

    void PlayJarlVoice()
    {
        // if key made is "Key1" and hasMadeBoxKey is false , play box key audio, if audio is already playing then wait then play door key audio
        if (key.name == "Key1" && !hasMadeBoxKey)
        {
            WaitForJarlVoice();
            jarlVoice.clip = boxKeyAudio;
            jarlVoice.Play();
            hasMadeBoxKey = true;
        }
        else if (key.name == "Key3" && !hasMadeDoorKey)
        {
            WaitForJarlVoice();
            jarlVoice.clip = doorKeyAudio;
            jarlVoice.Play();
            hasMadeDoorKey = true;
        }
    }
    IEnumerator WaitForJarlVoice()
    {
        while (jarlVoice.isPlaying)
        {
            yield return null;
        }
        Debug.Log("Jarl has finished speaking, waiting 1 seconds");
        yield return new WaitForSeconds(1);
    }
}
