using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool isRunning = false;
    private float time = 0.0f;
    private int minutes = 0;
    private float seconds = 0;
    private string lastFormattedTime = "";

    void Start()
    {
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            time += Time.deltaTime;
            minutes = (int)time / 60;
            seconds = time % 60;

            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (formattedTime != lastFormattedTime)
            {
                Debug.Log(formattedTime);
                lastFormattedTime = formattedTime;
            }
        }
    }
}
