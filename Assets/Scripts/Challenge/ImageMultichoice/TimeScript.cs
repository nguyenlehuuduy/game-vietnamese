using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeScript : MonoBehaviour
{
    public Text tmp_Time;
    private bool isRunning = false; 
    private float timeLeft =80;
    // Start is called before the first frame update
    void Start()
    {
        StartTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateDisplayTime(timeLeft);
            }
            else
            {
                timeLeft = 0;
                StopTime();
            }
            
        }
        
    }
    void UpdateDisplayTime(float currentTime)
    {

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int second = Mathf.FloorToInt(currentTime % 60);
        tmp_Time.text= string.Format("{0:00}:{1:00}",minutes,second);
    }
    public void StartTime()
    {
        isRunning = true;
    }
    public void StopTime()
    {
        isRunning=false;
    }
}
