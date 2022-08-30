using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallNEnemyDetector : MonoBehaviour
{
    
    Vector3 respawnPoint; //Create respawnpoint 
    public GameObject fallDetector; // checking for fall collider   
    public GameObject Enemy; // checking for enemy collider
    
    void Start()
    {
        respawnPoint = transform.position;
    }

    void Update()
    {
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }

        if (col.tag == "Enemy")
        {
            transform.position = respawnPoint;
        }
    }
}
