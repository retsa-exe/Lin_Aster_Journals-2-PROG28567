using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public LayerMask groundTiles;
    public float rayCastOffset;

    public float apexHeight, apexTime, terminalSpeed;
    float gravity, initialJumpVelocity;

    bool jumpTrigger = false;

    public float coyoteTime;
    float t;

    public int health;
    bool hasDied;

    public float accelerationTime, decelerationTime;
    float acceleration, deceleration;

    Vector3 currentVelocity;

    //dash variables
    public float dashdistance, dashDuration;
    bool isDashing;
    float dashTimer;
    float dashSpeed;
    float dashDirection;

    //double jump variables
    bool doubleJumpAvailable;
    bool isDoubleJump;
    public enum FacingDirection
    {
        left, right
    }

    public enum CharacterState
    {
        idle, walk, jump, death
    }

    public CharacterState currentState = CharacterState.idle;
    public CharacterState previousState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gravity = -2 * apexHeight / (apexTime * apexTime);
        initialJumpVelocity = 2 * apexHeight / apexTime;

        //initialize variables
        jumpTrigger = false;
        hasDied = false;
        isDashing = false;
        dashTimer = 0;
        isDoubleJump = false;

        //calculate acceleration and deceleration
        acceleration = speed / accelerationTime;
        deceleration = speed / decelerationTime;

        //calculate the dash speed
        dashSpeed = dashdistance / dashDuration;
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerInput.x += 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerInput.x -= 1;
        }
        MovementUpdate(playerInput);
        StateUpdate();

        if (IsGrounded())
        {
            t = coyoteTime;

            //reset double jump when grounded
            isDoubleJump = false;
            doubleJumpAvailable = true;
        }
        else
        {
            t -= Time.deltaTime;
        }

        //jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (t > 0)
            {
                jumpTrigger = true;
                t = 0;
            }
            else if (doubleJumpAvailable)
            {
                isDoubleJump = true;
                doubleJumpAvailable = false;
            }
        }
        //if (IsGrounded() == false)
        //{
        //    Debug.Log("not grounded");
        //}

        //Debug.Log(rb.linearVelocity.y);

        //perform dash when press z
        if (Input.GetKeyDown(KeyCode.Z) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;

            //determine dash direction
            if (GetFacingDirection() == FacingDirection.right)
            {
                dashDirection = 1;
            }
            else
            {
                dashDirection = -1;
            }
        }

    }
    private void FixedUpdate()
    {
        rb.linearVelocityY += gravity * Time.fixedDeltaTime;

        if (jumpTrigger && !isDoubleJump)
        {
            rb.linearVelocityY = initialJumpVelocity;
            jumpTrigger = false;
        }

        if (isDoubleJump)
        {
            rb.linearVelocityY = initialJumpVelocity;
            isDoubleJump = false;
        }

        if (rb.linearVelocity.y < - terminalSpeed)
        {
            rb.linearVelocityY = - terminalSpeed;
        }

        //dashing logic
        if (isDashing)
        {
            rb.linearVelocityX = dashDirection * dashSpeed;
            //decrease the dash timer
            dashTimer -= Time.fixedDeltaTime;
            //stop dashing if timer is up
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        //block horizontal movement when dashing
        if (isDashing)
        {
            return;
        }

        //rb.linearVelocityX = playerInput.x * speed;
        if (playerInput.x != 0)
        {
            currentVelocity += playerInput.x * acceleration * Vector3.right * Time.deltaTime;
            if (Mathf.Abs(currentVelocity.x) > speed) 
            { 
                currentVelocity = new Vector3(Mathf.Sign(currentVelocity.x) * speed, currentVelocity.y); 
            }
        }
        else
        {
            Vector3 amountChanged = deceleration * currentVelocity.normalized * Time.deltaTime;
            if (amountChanged.magnitude > Mathf.Abs(currentVelocity.x)) 
            { 
                currentVelocity.x = 0; 
            }
            else 
            { 
                currentVelocity -= amountChanged; 
            }
        }
        rb.linearVelocityX = currentVelocity.x;
    }

    public bool IsWalking()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }
        return false;
    }
    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, rayCastOffset, groundTiles);
    }

    public FacingDirection GetFacingDirection()
    {
        if (rb.linearVelocityX > 0)
        {
            return FacingDirection.right;
        }
        else
        {
            return FacingDirection.left;
        }
    }

    public bool HasDied()
    {
        bool isDead = health <= 0;
        if (isDead && hasDied == false)
        {
            hasDied = true;
            return true;
        }
        return false;
    }

    void StateUpdate()
    {
        previousState = currentState;

        if (IsWalking() && IsGrounded())
        {
            currentState = CharacterState.walk;
        }
        else if (!IsGrounded())
        {
            currentState = CharacterState.jump;
        }
        else
        {
            currentState = CharacterState.idle;
        }

        if (health <= 0)
        {
            currentState = CharacterState.death;
        }
    }
}
