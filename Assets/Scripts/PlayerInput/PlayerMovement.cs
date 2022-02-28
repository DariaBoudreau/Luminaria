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
    public float deccel = .1f;
    public float turnSpeed = 0.1f;
    public float horizontalInput;
    public float jumpPower = 100f;
    public bool facingRight = true;

    private Vector3 movementDirection;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void UpdateMovementData(Vector3 newMovementDirection, float newHorizontalInput)
    {
        movementDirection = newMovementDirection;
        horizontalInput = newHorizontalInput;
    }

    public void UpdateJump()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpPower);
    }

    void FixedUpdate()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        Vector2 currentVelocity = playerRigidbody.velocity;
        float movementdir = movementDirection.x * movementSpeed * Time.fixedDeltaTime;

        playerRigidbody.velocity = new Vector2(movementdir, playerRigidbody.velocity.y);

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
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
