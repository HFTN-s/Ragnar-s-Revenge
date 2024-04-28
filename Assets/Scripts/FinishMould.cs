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
    public AudioClip mediumKeyAudio;
    private bool hasMadeMediumKey = false;
    public AudioClip doorKeyAudio;
    private bool hasMadeDoorKey = false;
    public AudioClip sizzleAudio;
    public AudioSource splashSource;
    private TutorialLevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.Find("TutorialLevelManager").GetComponent<TutorialLevelManager>();
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
                splashSource.Play();
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

    void PlayJarlVoice()
    {
        // if key made is "Key1" and hasMadeBoxKey is false , play box key audio, if audio is already playing then wait then play door key audio
        if (key.name == "KeyMould1" && !hasMadeBoxKey)
        {
            levelManager.PlayJarlVoiceLine(0);
            hasMadeBoxKey = true;
        }
        else if (key.name == "KeyMould2" && !hasMadeDoorKey && !hasMadeMediumKey)
        {
            levelManager.PlayJarlVoiceLine(11);
            hasMadeMediumKey = true;
        }
        else if (key.name == "KeyMould3" && !hasMadeDoorKey)
        {
            levelManager.PlayJarlVoiceLine(3);
            hasMadeDoorKey = true;
        }
    }
}

