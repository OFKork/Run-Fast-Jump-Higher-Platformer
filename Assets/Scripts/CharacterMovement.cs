using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Basic movement and checking facing right
    [Header("For Movement")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool facingRight = true; 
    float moveHorizontal;

    // 1 time Dash
    [Header("For Dashing")] 
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingPower = 24;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    // Using ground layer and checking to  Jump 
    [Header("For Jumping")]
    [SerializeField] float jumpingForce = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJumping;

    
    //Using wall Layer to slide
    [Header("For WallSliding")]
    [SerializeField] float wallSlideSpeed = 1f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] Vector2 wallCheckSize;
    [SerializeField] bool isTouchingWall;
    [SerializeField] bool isWallSliding;

    //Using wall Layer to jump
    [Header("For WallJumping")]
    [SerializeField] Vector2 wallJumpForce;
    [SerializeField] bool wallJumping;
    [SerializeField] float wallJumpDuration;

    // Tried Some Animations
    /*[Header("For Animations")] 
    public Animator animator;
    */
    
    // rigibody
    [Header("Other")]
    Rigidbody2D rb2d;


    // rigidbody 
    void Start()
    {
        
        rb2d = gameObject.GetComponent<Rigidbody2D>();

    }

    // Calls all movements
    void Update()
    {
        
        Running();
        Jumping();
        WallJump();
        WallSlide();
        CheckWorld();

        if (Input.GetKeyDown((KeyCode.LeftShift)) && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    
    // Running on horizontal code
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

        // animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
    }
    
    // For facing right way 
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        
        facingRight = !facingRight;
        
    }

    // 1 time Dash 
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravvity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb2d.gravityScale = originalGravvity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }



    // Checking ground and Jump
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

    // If layer is a wall can do wall jump
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
    // Stops Wall Jump
    void StopWallJump()
    {
        wallJumping = false;
    }

    
    // Checking wall layer and not grounded can do wall slide
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
        
        //Wall slide code

        if (isWallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Math.Clamp(rb2d.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }
    
    
    
    // Checking Layers
    void CheckWorld()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
    }


    // Wall and Ground Layer Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheckPoint.position,groundCheckSize);
        
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
    }
}
