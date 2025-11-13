using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;

    public float rayCastOffset;
    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        if (Input.GetKey(KeyCode.RightArrow) && IsGrounded())
        {
            playerInput = Vector2.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && IsGrounded())
        {
            playerInput = Vector2.left;
        }
        MovementUpdate(playerInput);

        //if (IsGrounded() == false)
        //{
        //    Debug.Log("not grounded");
        //}
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
        return Physics2D.Raycast(transform.position, Vector2.down, rayCastOffset);
    }

    public FacingDirection GetFacingDirection()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            return FacingDirection.right;
        }
        else
        {
            return FacingDirection.left;
        }
    }
}
