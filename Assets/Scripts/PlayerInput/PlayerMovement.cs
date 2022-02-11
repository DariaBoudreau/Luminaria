using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    private Rigidbody2D playerRigidbody;
    private CapsuleCollider2D controllerCollider;
    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;

    [Header("Movement Values")]
    [SerializeField] float moveSpeed = 100.0f;
    [SerializeField] float acceleration = 30.0f;
    [SerializeField] float maxSpeed = 8.0f;
    [SerializeField] float jumpForce = 2220;
    [SerializeField] float minFlipSpeed = 0.1f;
    [SerializeField] float jumpGravityScale = 2.0f;
    [SerializeField] float fallGravityScale = 5.0f;
    [SerializeField] float groundedGravityScale = 1.0f;
    [SerializeField] bool resetSpeedOnLand = false;
    public bool jumpInput;
    

    // Stored Fields
    private Vector3 movementDirection;
    private Vector2 prevVelocity;
    private GroundType groundType;
    private bool isJumping;
    private bool isFalling;
    private bool isGliding;
    private bool isSliding;
    private bool willJump;
    private bool doubleJump = false;
    private bool faceRight = false;
    private bool hasTransitioned;
    private bool canJump = true;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        controllerCollider = GetComponent<CapsuleCollider2D>();
        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");
    }

    void MovePlayer()
    {
        Vector3 movement = movementDirection * moveSpeed * Time.fixedDeltaTime;
        //playerRigidbody.MovePosition(transform.position + movement);
    }

    void DetermineJumpState()
    {
        // Jumping input
        if (jumpInput)
        {
            if (!isJumping && canJump)
            {
                Debug.Log("You will jump");
                willJump = true;
            }
            else if (!doubleJump && canJump)
            {
                willJump = true;
                doubleJump = true;
                canJump = false;
            }
        }

        // Gliding
        if (jumpInput && isFalling)
        {
            isGliding = true;
        }
        else
        {
            isGliding = false;
        }
    }

    public void UpdateDirection(Vector3 newMovementDirection)
    {
        movementDirection = newMovementDirection;
    }

    void FixedUpdate()
    {
        MovePlayer();
        UpdateGrounding();
        UpdateVelocity();
        DetermineJumpState();
        UpdateJump();
        UpdateGravityScale();

        prevVelocity = playerRigidbody.velocity;
    }

    private void UpdateGrounding()
    {
        // Use character collider to check if touching ground layers
        if (controllerCollider.IsTouchingLayers(softGroundMask))
            groundType = GroundType.Soft;
        else if (controllerCollider.IsTouchingLayers(hardGroundMask))
            groundType = GroundType.Hard;
        else
            groundType = GroundType.None;

        // Update animator
        //animator.SetBool(animatorGroundedBool, groundType != GroundType.None);
    }

    private void UpdateVelocity()
    {
        Vector3 velocity = playerRigidbody.velocity;

        // Apply acceleration directly as we'll want to clamp
        // prior to assigning back to the body.
        velocity += movementDirection * acceleration * Time.fixedDeltaTime;

        // We've consumed the movement, reset it.
        movementDirection = Vector2.zero;

        // Clamp horizontal speed.
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        // Assign back to the body.
        playerRigidbody.velocity = velocity;

        // Update animator running speed
        //var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / maxSpeed;
        //animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);

        // Play audio
        //audioPlayer.PlaySteps(groundType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && playerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (willJump)
        {
            // Set animator
            //animator.SetTrigger(animatorJumpTrigger);

            // Jump using impulse force
            // controllerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerRigidbody.AddForce(transform.up * jumpForce);

            // We've consumed the jump, reset it.
            willJump = false;

            // Set jumping flag
            isJumping = true;

            jumpInput = false;

            // Play audio
            //audioPlayer.PlayJump();
        }

        // Landed
        else if (isJumping && isFalling && groundType != GroundType.None)
        {
            // Since collision with ground stops rigidbody, reset velocity
            if (resetSpeedOnLand)
            {
                prevVelocity.y = playerRigidbody.velocity.y;
                playerRigidbody.velocity = prevVelocity;
            }

            // Reset jumping flags
            isJumping = false;
            isFalling = false;
            doubleJump = false;

            // Play audio
            //audioPlayer.PlayLanding(groundType);
        }
    }

    private void UpdateGravityScale()
    {
        // Use grounded gravity scale by default.
        var gravityScale = groundedGravityScale;

        if (groundType == GroundType.None)
        {
            // If not grounded then set the gravity scale according to upwards (jump) or downwards (falling) motion.
            gravityScale = playerRigidbody.velocity.y > 0.0f ? jumpGravityScale : fallGravityScale;
        }

        playerRigidbody.gravityScale = isGliding ? gravityScale / 3 : gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }
}
