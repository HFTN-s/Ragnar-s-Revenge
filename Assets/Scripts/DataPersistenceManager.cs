using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class DataPersistenceManager : MonoBehaviour
{
    public GameData gameData;
    private string dataFilePath;

    public static DataPersistenceManager instance { get; private set; }

    // Public property to access game data
    public GameData GameData
    {
        get { return gameData; }
        set { gameData = value; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Found more than one Data Persistence Manager instance");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        dataFilePath = Application.persistentDataPath + "/gameData.json";

        // Avoid overwriting existing GameData when recompiling scripts in the editor
        if (gameData == null)
        {
            gameData = new GameData(); // This line could reset your GameData in play mode
        }
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        GameData = new GameData();
        SaveGame(); // Save initial game data when starting a new game
    }

    // Public method to load game data
    public void LoadGame()
    {
        if (File.Exists(dataFilePath))
        {
            try
            {
                string dataAsJson = File.ReadAllText(dataFilePath);
                Debug.Log("Loaded game data: " + dataAsJson);
                GameData = JsonUtility.FromJson<GameData>(dataAsJson);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load game data: " + ex.Message);
                NewGame(); // If failed to load, start a new game
            }
        }
        else
        {
            Debug.Log("No data was found. Creating new game data");
            NewGame();
        }
    }

    // Public method to save game data
    public void SaveGame()
    {
        try
        {
            string dataAsJson = JsonUtility.ToJson(GameData);
            File.WriteAllText(dataFilePath, dataAsJson);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save game data: " + ex.Message);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveHighScore(int level, int seconds)
    {
        // Convert seconds to minutes and seconds
        int newScoreMinutes = seconds / 60;
        int newScoreSeconds = seconds % 60;

        // Check and update high scores based on the current level
        switch (level)
        {
            case 1:
                UpdateHighScores(ref GameData.level1Score1Minutes, ref GameData.level1Score1Seconds,
                                 ref GameData.level1Score2Minutes, ref GameData.level1Score2Seconds,
                                 ref GameData.level1Score3Minutes, ref GameData.level1Score3Seconds,
                                 newScoreMinutes, newScoreSeconds);
                break;

            case 2:
                UpdateHighScores(ref GameData.level2Score1Minutes, ref GameData.level2Score1Seconds,
                                 ref GameData.level2Score2Minutes, ref GameData.level2Score2Seconds,
                                 ref GameData.level2Score3Minutes, ref GameData.level2Score3Seconds,
                                 newScoreMinutes, newScoreSeconds);
                break;

            case 3:
                UpdateHighScores(ref GameData.level3Score1Minutes, ref GameData.level3Score1Seconds,
                                 ref GameData.level3Score2Minutes, ref GameData.level3Score2Seconds,
                                 ref GameData.level3Score3Minutes, ref GameData.level3Score3Seconds,
                                 newScoreMinutes, newScoreSeconds);
                break;

            case 4:
                UpdateHighScores(ref GameData.level4Score1Minutes, ref GameData.level4Score1Seconds,
                                 ref GameData.level4Score2Minutes, ref GameData.level4Score2Seconds,
                                 ref GameData.level4Score3Minutes, ref GameData.level4Score3Seconds,
                                 newScoreMinutes, newScoreSeconds);
                break;

            default:
                Debug.LogError("Level not supported for high scores");
                break;
        }

        // Log and save
        Debug.Log("High score saved");
        Debug.Log($"High score for level {level}: {newScoreMinutes} minutes and {newScoreSeconds} seconds");
        SaveGame();
    }

    // Refactor the repeated logic into this method
    private void UpdateHighScores(ref int score1Minutes, ref int score1Seconds,
                                  ref int score2Minutes, ref int score2Seconds,
                                  ref int score3Minutes, ref int score3Seconds,
                                  int newScoreMinutes, int newScoreSeconds)
    {
        // Convert all times to seconds for comparison
        int newScoreTotalSeconds = newScoreMinutes * 60 + newScoreSeconds;
        int score1TotalSeconds = score1Minutes * 60 + score1Seconds;
        int score2TotalSeconds = score2Minutes * 60 + score2Seconds;
        int score3TotalSeconds = score3Minutes * 60 + score3Seconds;

        // Automatically replace scores of 0
        if (score1TotalSeconds == 0)
        {
            score1Minutes = newScoreMinutes;
            score1Seconds = newScoreSeconds;
        }
        else if (newScoreTotalSeconds < score1TotalSeconds)
        {
            // Shift down previous high scores
            score3Minutes = score2Minutes;
            score3Seconds = score2Seconds;
            score2Minutes = score1Minutes;
            score2Seconds = score1Seconds;
            // Insert new high score
            score1Minutes = newScoreMinutes;
            score1Seconds = newScoreSeconds;
        }
        else if (score2TotalSeconds == 0)
        {
            score2Minutes = newScoreMinutes;
            score2Seconds = newScoreSeconds;
        }
        else if (newScoreTotalSeconds < score2TotalSeconds)
        {
            // Shift down previous high scores
            score3Minutes = score2Minutes;
            score3Seconds = score2Seconds;
            // Insert new high score
            score2Minutes = newScoreMinutes;
            score2Seconds = newScoreSeconds;
        }
        else if (score3TotalSeconds == 0)
        {
            score3Minutes = newScoreMinutes;
            score3Seconds = newScoreSeconds;
        }
        else if (newScoreTotalSeconds < score3TotalSeconds)
        {
            // Update the third high score
            score3Minutes = newScoreMinutes;
            score3Seconds = newScoreSeconds;
        }
    }

    public void SaveAudioSettings(Slider masterSlider, Slider sfxSlider, Slider speechSlider, Slider musicSlider)
    {
        // Save the current audio settings to the game data
        GameData.masterVolume = masterSlider.value;
        GameData.sfxVolume = sfxSlider.value;
        GameData.speechVolume = speechSlider.value;
        GameData.musicVolume = musicSlider.value;
        SaveGame();
    }

    public void LoadAudioSettings(Slider masterSlider, Slider sfxSlider, Slider speechSlider, Slider musicSlider)
    {
        // Load the audio settings from the game data
        masterSlider.value = GameData.masterVolume;
        sfxSlider.value = GameData.sfxVolume;
        speechSlider.value = GameData.speechVolume;
        musicSlider.value = GameData.musicVolume;
        Debug.Log("Loaded audio settings");
        Debug.Log("Master Volume: " + masterSlider.value);
        Debug.Log("SFX Volume: " + sfxSlider.value);
        Debug.Log("Speech Volume: " + speechSlider.value);
        Debug.Log("Music Volume: " + musicSlider.value);
    }
}
