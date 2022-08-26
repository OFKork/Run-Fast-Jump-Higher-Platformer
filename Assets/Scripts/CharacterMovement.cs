using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpingForce = 1f;
    [SerializeField] float slideSpeed = 1f;
    float moveHorizontal;
    bool Jump = false;
    bool isSliding = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    { 
        Running();
        Jumping();
        Slide();
    }

    void Running()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        rb2d.AddForce(new Vector2(moveHorizontal, 0f), ForceMode2D.Impulse);
    }

    void Jumping()
    {

        if (Input.GetButtonDown("Jump"))
        {
             rb2d.AddForce(new Vector2(0f, jumpingForce), ForceMode2D.Impulse);
        }
    }

    void Slide()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            Jump= false;
            Debug.Log("on Ground");
            
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            Jump= true;
            Debug.Log("Jumping");
            
        }
    }
}
