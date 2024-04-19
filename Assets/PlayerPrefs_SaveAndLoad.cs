using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefs_SaveAndLoad : MonoBehaviour
{
    public void CompleteLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("Level" + levelIndex + "Completed", 1);
        Debug.Log("Level" + levelIndex + " Completed");
        PlayerPrefs.Save();

        foreach (var key in PlayerPrefs.GetString())
        {
            Debug.Log(key + " : " + PlayerPrefs.GetString(key));
        }
    }

    // Returns true if the level is unlocked
public bool IsLevelUnlocked(int levelIndex)
{
    return PlayerPrefs.GetInt("Level" + levelIndex + "Completed", 0) == 1;
}

}
