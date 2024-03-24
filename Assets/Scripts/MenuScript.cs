using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GameObject startButton;
    private GameObject settingsButton;
    private GameObject quitButton;
    private GameObject storyButton;
    private GameObject sandboxButton;
    private GameObject musicVolumeButton;
    private GameObject sfxVolumeButton;
    private GameObject backButton;
    private TextMeshProUGUI startText;
    private TextMeshProUGUI settingsText;
    private TextMeshProUGUI quitText;
    private TextMeshProUGUI storyText;
    private TextMeshProUGUI sandboxText;
    private TextMeshProUGUI musicVolumeText;
    private TextMeshProUGUI sfxVolumeText;
    private TextMeshProUGUI backText;
    private Color hoverColor = Color.green;
    // Start is called before the first frame update
    void Start()
    {
        startButton = GameObject.Find("StartButton");
        settingsButton = GameObject.Find("SettingsButton");
        quitButton = GameObject.Find("QuitButton");
        storyButton = GameObject.Find("StoryButton");
        sandboxButton = GameObject.Find("SandboxButton");
        musicVolumeButton = GameObject.Find("MusicButton");
        sfxVolumeButton = GameObject.Find("SFXButton");
        backButton = GameObject.Find("BackButton");
        startText = startButton.GetComponentInChildren<TextMeshProUGUI>();
        settingsText = settingsButton.GetComponentInChildren<TextMeshProUGUI>();
        quitText = quitButton.GetComponentInChildren<TextMeshProUGUI>();
        storyText = storyButton.GetComponentInChildren<TextMeshProUGUI>();
        sandboxText = sandboxButton.GetComponentInChildren<TextMeshProUGUI>();
        musicVolumeText = musicVolumeButton.GetComponentInChildren<TextMeshProUGUI>();
        sfxVolumeText = sfxVolumeButton.GetComponentInChildren<TextMeshProUGUI>();
        backText = backButton.GetComponentInChildren<TextMeshProUGUI>();
        storyButton.SetActive(false);
        sandboxButton.SetActive(false);
        musicVolumeButton.SetActive(false);
        sfxVolumeButton.SetActive(false);
        backButton.SetActive(false);

    }

public void OnHoverEntered(HoverEnterEventArgs args)
    {
        // Update from 'interactable' to 'interactableObject'
        TextMeshProUGUI text = args.interactableObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            ChangeTextColour(text, hoverColor); // Ensure this function is in the same script
        }
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        // Update from 'interactable' to 'interactableObject'
        TextMeshProUGUI text = args.interactableObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            ChangeTextColour(text, Color.white); // Reset to default color
        }
    }

    // Make sure this method is in the same script as the hover methods
    public void ChangeTextColour(TextMeshProUGUI text, Color colour)
    {
        text.color = colour;
        //ckise
    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.name == "StartButton")
        {
            Debug.Log("Start button selected");
            // Show Story/Sandbox/Back Buttons, Hide Remaining Buttons
            storyButton.SetActive(true);
            sandboxButton.SetActive(true);
            backButton.SetActive(true);
            startButton.SetActive(false);
            settingsButton.SetActive(false);
            quitButton.SetActive(false);
        }

        if (args.interactableObject.transform.gameObject.name == "SettingsButton")
        {
            Debug.Log("Settings button selected");
            // Show Music/SFX/Back Buttons, Hide Remaining Buttons
            musicVolumeButton.SetActive(true);
            sfxVolumeButton.SetActive(true);
            backButton.SetActive(true);
            startButton.SetActive(false);
            settingsButton.SetActive(false);
            quitButton.SetActive(false);
        }

        if (args.interactableObject.transform.gameObject.name == "QuitButton")
        {
            Debug.Log("Quit button selected");
            Application.Quit();
        }

        if (args.interactableObject.transform.gameObject.name == "StoryButton")
        {
            Debug.Log("Story button selected");
            // Load Story Scene
        }

        if (args.interactableObject.transform.gameObject.name == "SandboxButton")
        {
            Debug.Log("Sandbox button selected");
            // Load Sandbox Scene
            SceneManager.LoadScene("Sandbox");
        }

        if (args.interactableObject.transform.gameObject.name == "MusicButton")
        {
            Debug.Log("Music button selected");
            // Adjust Music Volume
        }

        if (args.interactableObject.transform.gameObject.name == "SFXButton")
        {
            Debug.Log("SFX button selected");
            // Adjust SFX Volume
        }

        if (args.interactableObject.transform.gameObject.name == "BackButton")
        {
            Debug.Log("Back button selected");
            // Show Start/Settings/Quit Buttons, Hide Remaining Buttons
            startButton.SetActive(true);
            settingsButton.SetActive(true);
            quitButton.SetActive(true);
            storyButton.SetActive(false);
            sandboxButton.SetActive(false);
            musicVolumeButton.SetActive(false);
            sfxVolumeButton.SetActive(false);
            backButton.SetActive(false);
        }


    }
}
