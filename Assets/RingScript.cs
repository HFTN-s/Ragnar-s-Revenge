using UnityEngine;

public class RingScript : MonoBehaviour
{
    private Transform ringFinger;
    private Rigidbody ringRigidbody;
    private TutorialLevelManager tutorialLevelManager;
    public AudioClip jarlRingOn;
    public AudioSource ringAudioSource;
    public BoxCollider puzzle2Trigger;

    void Start()
    {
        tutorialLevelManager = GameObject.Find("TutorialLevelManager").GetComponent<TutorialLevelManager>();
        // Find the ring finger and store its Transform
        GameObject fingerObject = GameObject.Find("RingHandTransform");
        if (fingerObject != null)
        {
            ringFinger = fingerObject.transform;
            Debug.Log("Ring finger found and assigned.");
        }
        else
        {
            Debug.LogError("Error: RingHandTransform not found in the scene.");
        }

        // Get the Rigidbody component
        ringRigidbody = GetComponent<Rigidbody>();
        if (ringRigidbody != null)
        {
            Debug.Log("Rigidbody component found and assigned.");
        }
        else
        {
            Debug.LogError("Error: Rigidbody component missing from the ring object.");
        }
    }

   void OnTriggerEnter(Collider other)
{
    if (other.gameObject.tag == "PlayerHand")
    {
        Debug.Log("Player's hand has touched the ring.");

        // Set the ring's parent to the ring finger's transform
        if (ringFinger != null)
        {
            puzzle2Trigger.enabled = true;

            transform.SetParent(ringFinger);

            // Set the local position to zero as specified
            transform.localPosition = new Vector3(0f, 0f, 0f);

            // Set the local rotation to match the specified values
            transform.localEulerAngles = new Vector3(-30.516f, -178.083f, -38.22f);

            // Set the local scale to match the specified values
            transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);

            Debug.Log("Ring's transform has been set relative to its parent.");
            // play audio
            tutorialLevelManager.PlayJarlVoiceLine(18);
        }
        else
        {
            Debug.LogError("Error: Ring finger transform is missing.");
        }

        // Disable the Rigidbody physics
        if (ringRigidbody != null)
        {
            ringRigidbody.isKinematic = true;
            Debug.Log("Rigidbody is now kinematic.");
        }
        else
        {
            Debug.LogError("Error: Rigidbody component missing from the ring object.");
        }

        tutorialLevelManager.hasRing = true;
        // Disable all BoxCollider components
        BoxCollider[] boxColliders = GetComponents<BoxCollider>();
        foreach (BoxCollider boxCollider in boxColliders)
        {
            boxCollider.enabled = false;
        }
        Debug.Log("All BoxCollider components on the ring have been disabled.");
    }
}


}
