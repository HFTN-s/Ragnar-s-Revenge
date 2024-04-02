using UnityEngine;

public class ChildColliderTrigger : MonoBehaviour
{
    public TutorialLevelManager parentScript; // Assign this via the Inspector.
    public int colliderID; // An example identifier. You could also use a string or enum for more descriptive IDs.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentScript.HandleTriggerEnter(colliderID);
        }
    }
}
