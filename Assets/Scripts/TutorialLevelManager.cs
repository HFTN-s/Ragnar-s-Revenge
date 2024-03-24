using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] tutorialClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool puzzle1Complete = false;
    [SerializeField] private bool puzzle2Complete = false;
    [SerializeField] private bool puzzle3Complete = false;
    [SerializeField] private bool levelComplete = false;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject[] eyelids;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BlinkingCameraEffect()
    {
        // Blinking camera effect

    }
}
