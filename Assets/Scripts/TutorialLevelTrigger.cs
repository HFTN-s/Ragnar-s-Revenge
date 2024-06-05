using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TutorialLevelTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] GameObject fadeToBlackobject;
    public AudioSource mainMenuMusic;
    public PlayerMovement playerMovement;
    private VideoPlayer videoPlayer;
    public VideoClip endClip;

    private void Start()
    {
        playerMovement = GameObject.Find("MainMenuPlayer").GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        videoPlayer = fadeToBlackobject.GetComponentInChildren<VideoPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Player"))
        {
            fadeToBlackobject.SetActive(true);  
            mainMenuMusic.Stop();
            playerMovement.canMove = false;

            // when video is done playing
            StartCoroutine(PlayVideo());
        }
    }

    private IEnumerator PlayVideo()
    {
        videoPlayer.clip = endClip;
        videoPlayer.Play();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        yield return new WaitForSeconds((float)videoPlayer.length);
        videoPlayer.Stop();
        DelayedAction();
    }

    private void DelayedAction()
    {
        audioSource.Stop();
        SceneManager.LoadScene("TutorialLevel", LoadSceneMode.Single);
    }
}
