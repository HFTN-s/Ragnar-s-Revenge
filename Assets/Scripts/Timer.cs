using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool isRunning = false;
    private float time = 0.0f;

    private string lastFormattedTime = "";

    void Start()
    {
    }

    void Update()
    {
        if (isRunning)
        {
            time += Time.deltaTime;
            //Debug.Log("Time: " + time);
        }
    }

    public int GetSeconds()
    {
        //convert to int
        int seconds = Mathf.RoundToInt(time);
        Debug.Log("Seconds: " + seconds);
        return seconds;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        time = 0.0f;
    }

    public void StartTimer()
    {
        isRunning = true;
    }
}
