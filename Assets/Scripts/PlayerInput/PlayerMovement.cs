using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    private Rigidbody2D playerRigidbody;

    [Header("Movement Values")]
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float acceleration = 30.0f;
    [SerializeField] float maxSpeed = 8.0f;
    [SerializeField] float jumpForce = 35.0f;
    [SerializeField] float minFlipSpeed = 0.1f;
    [SerializeField] float jumpGravityScale = 2.0f;
    [SerializeField] float fallGravityScale = 5.0f;
    [SerializeField] float groundedGravityScale = 1.0f;
    [SerializeField] bool resetSpeedOnLand = false;
    public bool jumpInput = false;
    

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
    }

    void MovePlayer()
    {
        Vector3 movement = movementDirection * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void DetermineJumpState()
    {
        // Jumping input
        if (jumpInput)
        {
            if (!isJumping && canJump)
                willJump = true;
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
        DetermineJumpState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }


}
