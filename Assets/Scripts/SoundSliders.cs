using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSliders : MonoBehaviour
{
    public AudioMixer audioMixer; // Assign this in the inspector

    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider speechSlider;
    public Slider musicSlider;

    void Start()
    {
        DataPersistenceManager.instance.LoadAudioSettings(masterSlider, sfxSlider, speechSlider, musicSlider);
        SetVolume("Master", masterSlider.value);
        SetVolume("SFX", sfxSlider.value);
        SetVolume("Speech", speechSlider.value);
        SetVolume("Music", musicSlider.value);
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        masterSlider.onValueChanged.AddListener(value => {
            SetVolume("Master", value);
            SaveCurrentAudioSettings();
        });
        sfxSlider.onValueChanged.AddListener(value => {
            SetVolume("SFX", value);
            SaveCurrentAudioSettings();
        });
        speechSlider.onValueChanged.AddListener(value => {
            SetVolume("Speech", value);
            SaveCurrentAudioSettings();
        });
        musicSlider.onValueChanged.AddListener(value => {
            SetVolume("Music", value);
            SaveCurrentAudioSettings();
        });
    }

    private void SetVolume(string parameterName, float volume)
    {
        float volumeDb = volume <= 0 ? -80.0f : Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(parameterName, volumeDb);
    }

    private void SaveCurrentAudioSettings()
    {
        DataPersistenceManager.instance.SaveAudioSettings(masterSlider, sfxSlider, speechSlider, musicSlider);
    }
}
