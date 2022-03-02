using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    Rigidbody2D playerRigidbody;

    [Header("Movement Settings")]
    public float movementSpeed;
    public float maxSpeed;
    public float speedModifier;
    public float deccel = .1f;
    public float turnSpeed = 0.1f;
    public float horizontalInput;
    public float jumpPower = 100f;
    public bool facingRight = true;

    public float jumpGravity = .5f;
    public float glidingModifier = 3f; 

    private Vector3 movementDirection;

    private void Start()
    {
        // Get rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void UpdateMovementData(Vector3 newMovementDirection, float newHorizontalInput, float moveSpeedModifier)
    {
        // Every frame update these values for moving
        movementDirection = newMovementDirection;
        horizontalInput = newHorizontalInput;
        speedModifier = moveSpeedModifier;
    }

    public void UpdateJump()
    {
        // Making it jump
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpPower);
    }

    public void UpdateGravity(float gravitymod, bool glidingbool)
    {
        float gravityApplied = gravitymod;

        if (playerRigidbody.velocity.y > 0f && glidingbool)
        {
            // If the player is jumping up, make gravity less intense
            gravityApplied *= jumpGravity;
        }

        playerRigidbody.gravityScale = gravityApplied;
    }

    void FixedUpdate()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        //Vector2 currentVelocity = playerRigidbody.velocity;
        // Movement value = the direction times speed times the modifier times deltatime 
        float movement = movementDirection.x * movementSpeed * speedModifier * Time.fixedDeltaTime;

        // Apply new velocity
        playerRigidbody.velocity = new Vector2(movement, playerRigidbody.velocity.y);

        // Flip player when changing direction
        if (!facingRight && horizontalInput > 0f)
        {
            FlipPlayer();
        } else if (facingRight && horizontalInput < 0f)
        {
            FlipPlayer();
        }

        //playerRigidbody.MovePosition(transform.position + movement);
    }

    void FlipPlayer()
    {
        // Flip the player
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
