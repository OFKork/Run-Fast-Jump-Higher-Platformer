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


    [Header("For Dashing")] 
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingPower = 24;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

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

    /*[Header("For Animations")] 
    public Animator animator;
    */
    
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

        if (Input.GetKeyDown((KeyCode.LeftShift)) && canDash)
        {
            StartCoroutine(Dash());
        }
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

        // animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
    }
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        
        facingRight = !facingRight;
        
    }

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
