using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Video;
public class TutorialLevelTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] GameObject fadeToBlackobject;
    public AudioSource mainMenuMusic;
    public PlayerMovement playerMovement;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        playerMovement = GameObject.Find("MainMenuPlayer").GetComponent<PlayerMovement>();
        //fadeToBlackobject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        // video player is a child of fadeToBlackobject
        videoPlayer = fadeToBlackobject.GetComponentInChildren<VideoPlayer>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        //disable trigger collider
        if (other.gameObject.tag == "Player")
        {
            mainMenuMusic.Stop();
            audioSource.Play();
            fadeToBlackobject.SetActive(true);
            playerMovement.canMove = false;

            // when video is done playing
            StartCoroutine(PlayVideo());
            // fade to black
        }
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds((float)videoPlayer.length);

    }

    void DelayedAction()
    {
        audioSource.Stop();
        SceneManager.LoadScene("TutorialLevel");
    }
}
