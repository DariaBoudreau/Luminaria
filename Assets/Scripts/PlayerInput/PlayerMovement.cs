using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    [System.NonSerialized] public Rigidbody2D playerRigidbody;

    [Header("Movement Settings")]
    public float movementSpeed = 500;
    //public float maxSpeed;
    public float speedModifier = 1;
    public float deccel = 0.01f;
    public float turnSpeed = 3f;
    public float horizontalInput;
    public float jumpPower = 60f;
    public float wetJumpPower = 35f;

    [Tooltip("Velocity while gliding is scaled by this value")]
    public float glideVelocityNegation = 0.97f;
    public float jumpGravity = 10f;
    public float maxJumpVelo = 1500f;
    //public float glidingModifier = 3f;

    public bool isFalling = false;
    public bool facingRight = false;
    public bool shouldFlipAtStart = true;
    public bool shouldPersistVelocity;

    public bool CanMove { get; set; }
    
    private float movementDirection;
    [System.NonSerialized] private Vector2 previousVelocity;

    private void Start()
    {
        // Get rigidbody
        playerRigidbody = GetComponent<Rigidbody2D>();
        if (shouldFlipAtStart)
        {
            FlipPlayer();
        }
        CanMove = true;
    }

    private float NormalizeMovement(float newMovementDirectionAxis)
    {
        if (newMovementDirectionAxis > 0.1)
        {
            return 1;
        }
        else if (newMovementDirectionAxis < -0.1)
        {
            return -1;
        }
        else if (newMovementDirectionAxis == 0)
        {
            return 0;
        }
        else { return 0;  }
    }

    public void UpdateMovementData(Vector3 newMovementDirection, float newHorizontalInput, float moveSpeedModifier)
    {
        // Every frame update these values for moving
        movementDirection = NormalizeMovement(newHorizontalInput);
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
            playerRigidbody.AddForce(Vector2.up * jumpPower);
        }

    }

    /// <summary>
    /// Clamping the max y velocity 
    /// </summary>
    private void ClampYVelocity()
    {
        var clampedVelocity = Mathf.Clamp(playerRigidbody.velocity.y, -maxJumpVelo, maxJumpVelo);
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, clampedVelocity);
    }

    public void UpdateFalling()
    {
        if (playerRigidbody.velocity.y < 0f)
        {
            isFalling = true;
            UpdateGravity(false);
        }
    }

    public void UpdateGravity(bool glidingbool)
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
                if (Mathf.Abs(previousVelocity.x) > .9f)
                {
                    previousVelocity.x *= .9f;
                }
            }
        }

        playerRigidbody.gravityScale = gravityApplied;
    }

    void FixedUpdate()
    {
        MoveThePlayer();
        UpdateFalling();
        ClampYVelocity();
        previousVelocity = playerRigidbody.velocity;
    }

    void MoveThePlayer()
    {
        if (!CanMove) return;
        // Vector2 currentVelocity = playerRigidbody.velocity;
        // Movement value = the direction times speed times the modifier times deltatime 
        float movement = movementDirection * movementSpeed * speedModifier * Time.fixedDeltaTime;

        // If there is no horizontal input slow aspen the hell down!!!!!!!!!!!!
        
        // Flip player when changing direction
        if (!facingRight && horizontalInput > 0f)
        {
            FlipPlayer();
        } else if (facingRight && horizontalInput < 0f)
        {
            FlipPlayer();
        }

        if (shouldPersistVelocity)
        {
            playerRigidbody.velocity = new Vector2(previousVelocity.x, playerRigidbody.velocity.y);
        }
        else
        {
            if (horizontalInput == 0)
            {
                movement *= deccel;
            }
            
            // Apply new velocity
            playerRigidbody.velocity = new Vector2(movement, playerRigidbody.velocity.y);
        }


        //playerRigidbody.MovePosition(transform.position + movement);
    }

    //public so it can be flipped with a signal in the inspector
    public void FlipPlayer()
    {
        // Flip the player
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        //playerRigidbody.velocity *= -0.1f;
    }

    public void LandPlayer()
    {
        isFalling = false;
    }
}
