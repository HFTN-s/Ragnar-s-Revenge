using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Video;
using UnityEngine.SceneManagement;  
using System.Collections;

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
        videoPlayer.enabled = false;

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Video collider triggered.");
        // Check if the object entering the trigger has a specific tag
        // For example, if the object has the tag "Chest"
        if (other.gameObject.CompareTag("Chest") && !isPlaying)
        {
            videoPlayer.enabled = true;
            // Prevent multiple triggers while the video is already playing
            isPlaying = true;
            Debug.Log("Playing video.");
            playerMovement.canMove = false;

            // Start playing the video
            videoPlayer.Play();

            // once video has finished , load index 1 scene
            StartCoroutine(WaitForVideoEnd());
            
        }
    }

    private IEnumerator WaitForVideoEnd()
    {
        // Wait for the video to finish playing
        yield return new WaitForSeconds((float)videoPlayer.length);
        // Load the next scene
        Debug.Log("Video has finished playing.");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
