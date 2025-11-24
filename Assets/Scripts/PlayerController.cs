using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gravity = -2 * apexHeight / (apexTime * apexTime);
        initialJumpVelocity = 2 * apexHeight / apexTime;

        jumpTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerInput = Vector2.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerInput = Vector2.left;
        }
        MovementUpdate(playerInput);

        if (IsGrounded())
        {
            t = coyoteTime;
        }
        else
        {
            t -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && t > 0)
        {
            jumpTrigger = true;
            t = 0;
        }
        //if (IsGrounded() == false)
        //{
        //    Debug.Log("not grounded");
        //}

        //Debug.Log(rb.linearVelocity.y);
    }
    private void FixedUpdate()
    {
        rb.linearVelocityY += gravity * Time.fixedDeltaTime;

        if (jumpTrigger)
        {
            rb.linearVelocityY = initialJumpVelocity;
            jumpTrigger = false;
        }

        if (rb.linearVelocity.y < - terminalSpeed)
        {
            rb.linearVelocityY = - terminalSpeed;
        }
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        rb.linearVelocityX = playerInput.x * speed;
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
}
