using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelScore : MonoBehaviour
{
    public TextMeshProUGUI yourScoreText;


    void Start()
    {
        //Tried socre timer to next level scene text
        /*
        TimerController.instance.DisableScoreTimer();
        TimerController.instance.timer = yourScoreText;
        */
    }


    public void NextLevel()
    {
        SceneManager.LoadScene("Level 1");
    }
}
