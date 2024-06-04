using UnityEngine;

public class RingScript : MonoBehaviour
{
    private Transform ringFinger;
    private Rigidbody ringRigidbody;
    private TutorialLevelManager tutorialLevelManager;
    public AudioClip jarlRingOn;
    public AudioSource ringAudioSource;
    public BoxCollider puzzle2Trigger;
    public GameObject WithRing;
    public GameObject WithoutRing;

    void Start()
    {
        tutorialLevelManager = GameObject.Find("TutorialLevelManager").GetComponent<TutorialLevelManager>();
        // Find the ring finger and store its Transform
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerHand")
        {
            Debug.Log("Player's hand has touched the ring.");

            // set without ring to false
            WithoutRing.SetActive(false);
            // set with ring to true
            WithRing.SetActive(true);
            // play audio
            tutorialLevelManager.PlayJarlVoiceLine(18);

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