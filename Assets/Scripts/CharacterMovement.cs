using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("For Movement")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool facingRight = true;
    float moveHorizontal;
    
    
    [Header("For Jumping")]
    [SerializeField] float jumpingForce = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJumping;




    [Header("For WallSliding")]
    [SerializeField] float wallSlideSpeed = 1f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] Vector2 wallCheckSize;
    [SerializeField] bool isTouchingWall;
    [SerializeField] bool isWallSliding;

    [Header("For WallJumping")]
    [SerializeField] Vector2 wallJumpForce;
    [SerializeField] bool wallJumping;
    [SerializeField] float wallJumpDuration;
    
    [Header("Other")]
    Rigidbody2D rb2d;


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
        WallJump();
        WallSlide();
        CheckWorld();
    }

    void Running()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        rb2d.AddForce(new Vector2(moveHorizontal, 0f), ForceMode2D.Impulse);

        if (moveHorizontal > 0 && !facingRight)
        {
            Flip(); 
        }
        if (moveHorizontal < 0 && facingRight)
        {
            Flip();
        }

    }
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        
        facingRight = !facingRight;
        
    }
    void Jumping()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if (isGrounded )
            {
                rb2d.AddForce(new Vector2(0f, jumpingForce), ForceMode2D.Impulse);
                isJumping = true;
            }
            else if (isWallSliding)
            {
                wallJumping = true;
                Invoke("StopWallJump", wallJumpDuration);
            }

        }
    }

    void WallJump()
    {
        if (wallJumping)
        {
            rb2d.velocity = new Vector2(-moveHorizontal * wallJumpForce.x, wallJumpForce.y);
        }
        else
        {
            Running();
        }
    }

    void StopWallJump()
    {
        wallJumping = false;
    }

    void WallSlide()
    {
        if (isTouchingWall && !isGrounded && rb2d.velocity.y > -2 )
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        
        //Wall slide

        if (isWallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Math.Clamp(rb2d.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }
    
    
    
    
    void CheckWorld()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheckPoint.position,groundCheckSize);
        
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
    }
}
