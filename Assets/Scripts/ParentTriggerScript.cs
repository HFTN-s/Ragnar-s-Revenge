using UnityEngine;

public class ParentColliderScript : MonoBehaviour
{
    public AudioSource ragnarAudioSource;
    public AudioClip[] ragnarSpeech;

    // This method is called by the child script
    public void ChildOnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Entered Ragnar Speech Trigger");

            // Respond to the trigger event as needed
            if (ragnarAudioSource != null && ragnarSpeech.Length > 0)
            {
                ragnarAudioSource.clip = ragnarSpeech[0];
                ragnarAudioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource or Clip not properly assigned.");
            }
        }
    }
}
