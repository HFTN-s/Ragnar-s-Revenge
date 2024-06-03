using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Video;

public class StartVideoOnTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public GameObject TreasureChest;
    private bool isPlaying = false; // Flag to prevent multiple triggers
    public PlayerMovement playerMovement;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not assigned.");
            return;
        }

        // Enable the VideoPlayer component
        videoPlayer.enabled = true;

        videoPlayer.playOnAwake = false; // Change this to true if you want the video to play automatically on scene load
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Video collider triggered.");
        // Check if the object entering the trigger has a specific tag
        // For example, if the object has the tag "Chest"
        if (other.gameObject.CompareTag("Chest") && !isPlaying)
        {
            // Prevent multiple triggers while the video is already playing
            isPlaying = true;
            Debug.Log("Playing video.");
            playerMovement.canMove = false;

            // Start playing the video
            videoPlayer.Play();
        }
    }
}
