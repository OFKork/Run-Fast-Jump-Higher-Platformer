using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallNEnemyDetector : MonoBehaviour
{
    Vector3 respawnPoint;
    public GameObject fallDetector;
    public GameObject Enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position;
    }

    // Update is called once per frame
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
