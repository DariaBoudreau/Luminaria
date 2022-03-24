using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    [System.NonSerialized] public Rigidbody2D playerRigidbody;

    [Header("Movement Settings")]
    public float movementSpeed;
    //public float maxSpeed;
    public float speedModifier;
    public float deccel = .1f;
    public float turnSpeed = 0.1f;
    public float horizontalInput;
    public float jumpPower = 100f;
    public float wetJumpPower = 50f;
    public float glideVelocityNegation = .3f;
    public float jumpGravity = .5f;
    //public float glidingModifier = 3f;

    public bool isFalling = false;
    public bool facingRight = false;
    public bool shouldPersistVelocity;

    private Vector3 movementDirection;
    [System.NonSerialized] private Vector2 previousVelocity;

    private void Start()
    {
        // Get rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();
        FlipPlayer();
    }

    public void UpdateMovementData(Vector3 newMovementDirection, float newHorizontalInput, float moveSpeedModifier)
    {
        // Every frame update these values for moving
        movementDirection = newMovementDirection;
        horizontalInput = newHorizontalInput;
        speedModifier = moveSpeedModifier;
    }

    public void UpdateJump(bool isWet)
    {
        // Making it jump
        if(isWet)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, wetJumpPower);
        }
        else
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpPower);
        }

    }

    private void Update()
    {
        UpdateFalling();
    }

    public void UpdateFalling()
    {
        if (playerRigidbody.velocity.y < 0f)
        {
            isFalling = true;
        }
    }

    public void UpdateGravity(float gravitymod, bool glidingbool)
    {
        float gravityApplied = jumpGravity;

        if (glidingbool)
        {
            // Check if rb has built up negative y velocity. If so, scale velocity by glideVelocityNegation
            if (playerRigidbody.velocity.y <= 0f)
            {
                float newVelocityY = playerRigidbody.velocity.y * glideVelocityNegation;
                Vector2 newVelocity = new Vector2(playerRigidbody.velocity.x, newVelocityY);
                playerRigidbody.velocity = newVelocity;
            }
            // If the player starts gliding, make gravity less intense
            gravityApplied *= gravitymod;
        }

        playerRigidbody.gravityScale = gravityApplied;
    }

    void FixedUpdate()
    {
        MoveThePlayer();
        previousVelocity = playerRigidbody.velocity;
    }

    void MoveThePlayer()
    {
        // Vector2 currentVelocity = playerRigidbody.velocity;
        // Movement value = the direction times speed times the modifier times deltatime 
        float movement = movementDirection.x * movementSpeed * speedModifier * Time.fixedDeltaTime;

        // If there is no horizontal input slow aspen the hell down!!!!!!!!!!!!
        if (horizontalInput == 0)
        {
            movement *= deccel;
        }

        if (shouldPersistVelocity)
        {
            playerRigidbody.velocity = new Vector2(previousVelocity.x, playerRigidbody.velocity.y);
        }
        else
        {
            // Apply new velocity
            playerRigidbody.velocity = new Vector2(movement, playerRigidbody.velocity.y);
        }

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

    public void LandPlayer()
    {
        isFalling = false;
    }
}
