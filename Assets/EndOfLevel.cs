using System.Collections;
<<<<<<< Updated upstream
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
=======
using UnityEngine;
using UnityEngine.SceneManagement; // Add this to use SceneManager

public class EndOfLevel : MonoBehaviour
{
    public Timer timer; // Reference to your Timer script
    public PlayerMovement playerMovement; // Reference to your PlayerMovement script
    public bool canLeaveLevel = true; // Ensure canLeaveLevel is properly initialized or set in the Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the end of the level.");
            // turn off collider
            GetComponent<BoxCollider>().enabled = false;
            RoomCompleted();
        }
    }

    void RoomCompleted()
    {
            int timeTaken = timer.GetSeconds();
            DataPersistenceManager.instance.SaveHighScore(timeTaken);
            IncrementProgress(2);
            playerMovement.canMove = true;
            // Load the next level
            LoadNextLevel();
    }

    private void IncrementProgress(int puzzlesCompleted)
    {
        if (DataPersistenceManager.instance == null)
        {
            Debug.LogError("DataPersistenceManager instance is null!");
        }
        else if (DataPersistenceManager.instance.GameData == null)
        {
            Debug.LogError("GameData on DataPersistenceManager instance is null!");
        }
        else
        {
            if (DataPersistenceManager.instance.GameData.playerProgress < puzzlesCompleted)
            {
                DataPersistenceManager.instance.GameData.playerProgress++;
                DataPersistenceManager.instance.SaveGame();
                Debug.Log("Progress incremented to: " + DataPersistenceManager.instance.GameData.playerProgress);
            }
        }
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes to load.");
        }
>>>>>>> Stashed changes
    }
}
