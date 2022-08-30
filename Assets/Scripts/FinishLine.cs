using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class FinishLine : MonoBehaviour
{
    // for ending Level
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            SceneManager.LoadScene("ScoreScreen");
        }
    }
}
