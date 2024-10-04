using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPointAndTime : MonoBehaviour
{
    public TextMeshProUGUI tmp_Point;
    public TextMeshProUGUI tmp_Time;
    private bool isRunning = false;
    public float timeLeft = 10;
    private static int point = 0;
    public int Point
    {
        get { return point; }
        set { point = value; }
    }
    public float TimeLeft
    {
        get { return timeLeft; }
        set { timeLeft = value; }
    }
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
        UpdatePoint(point);
    }
    public void addPoint ()
    {
        point+=10;
    }
    void UpdateDisplayTime(float currentTime)
    {
        int second = Mathf.FloorToInt(currentTime % 60);
        tmp_Time.text = string.Format("{0:00s}", second);
    }
    void UpdatePoint(int point)
    {
        tmp_Point.text = point.ToString();
    }
    public void StartTime()
    {
        isRunning = true;
    }
    public void StopTime()
    {
        isRunning = false;
    }
    public void ResetTime()
    {
        timeLeft = 10;
    }
}
