using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playerProgress;
    public int level1Score1Minutes;
    public int level1Score2Minutes;
    public int level1Score3Minutes;
    public int level2Score1Minutes;
    public int level2Score2Minutes;
    public int level2Score3Minutes;
    public int level3Score1Minutes;
    public int level3Score2Minutes;
    public int level3Score3Minutes;
    public int level4Score1Minutes;
    public int level4Score2Minutes;
    public int level4Score3Minutes;
    public int level1Score1Seconds;
    public int level1Score2Seconds;
    public int level1Score3Seconds;
    public int level2Score1Seconds;
    public int level2Score2Seconds;
    public int level2Score3Seconds;
    public int level3Score1Seconds;
    public int level3Score2Seconds;
    public int level3Score3Seconds;
    public int level4Score1Seconds;
    public int level4Score2Seconds;
    public int level4Score3Seconds;
    public float masterVolume;
    public float sfxVolume;
    public float speechVolume;
    public float musicVolume;

    public GameData()
    {
        this.playerProgress = 0;
        this.level1Score1Minutes = 0;
        this.level1Score2Minutes = 0;
        this.level1Score3Minutes = 0;
        this.level2Score1Minutes = 0;
        this.level2Score2Minutes = 0;
        this.level2Score3Minutes = 0;
        this.level3Score1Minutes = 0;
        this.level3Score2Minutes = 0;
        this.level3Score3Minutes = 0;
        this.level4Score1Minutes = 0;
        this.level4Score2Minutes = 0;
        this.level4Score3Minutes = 0;
        this.level1Score1Seconds = 0;
        this.level1Score2Seconds = 0;
        this.level1Score3Seconds = 0;
        this.level2Score1Seconds = 0;
        this.level2Score2Seconds = 0;
        this.level2Score3Seconds = 0;
        this.level3Score1Seconds = 0;
        this.level3Score2Seconds = 0;
        this.level3Score3Seconds = 0;
        this.level4Score1Seconds = 0;
        this.level4Score2Seconds = 0;
        this.level4Score3Seconds = 0;
        this.masterVolume = 1;
        this.sfxVolume = 1;
        this.speechVolume = 1;
        this.musicVolume = 1;
        
    }
}
