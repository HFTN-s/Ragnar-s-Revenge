using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.Events;
public class TutorialLevelTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] GameObject fadeToBlackobject;
    public AudioSource mainMenuMusic;

    private void Start()
    {
        fadeToBlackobject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
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
            Invoke("DelayedAction", 5.0f);
            // fade to black
        }
    }

    void DelayedAction()
    {
        audioSource.Stop();
        SceneManager.LoadScene("TutorialLevel");
    }
    

}
