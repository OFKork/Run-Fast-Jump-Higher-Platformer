using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TimerController : MonoBehaviour
{
    /// <summary>
    ///  This code simply using for time counting on screens left corner and measuring for score system
    /// </summary>
    public static TimerController instance; 
        
    public TextMeshProUGUI timer;
    public float time;
    float msec;
    float sec;
    float min;
    bool countTime = true;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (countTime)
        {
            StartCoroutine(ScoreTimer());

        }
        
    }

    public void DisableScoreTimer()
    {
        countTime = false;
    }

    IEnumerator ScoreTimer()
    {
        while (true)
        {
            time += Time.deltaTime;
            msec = (int)((time -(int)time)*100);
            sec = (int)((time % 60));
            min = (int)(time / 60 % 60);

            timer.text = string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec);

            yield return null;
        }
    }



}