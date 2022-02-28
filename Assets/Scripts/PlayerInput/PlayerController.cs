using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviours")]
    public PlayerMovement playerMovement;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    public float runSpeed = 3f;
    private float moveSpeedModifier = 1f;
    private float normalGravity = 1f;
    public float glidingModifier = 3f;
    private float horizontalMovement;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    bool isGrounded = false;
    bool canDoubleJump = true;


    public void OnMovement(InputAction.CallbackContext value)
    {
        // The input movement saved to a vector
        Vector2 inputMovement = value.ReadValue<Vector2>();

        // Facing right or left for flipplayer
        horizontalMovement = value.ReadValue<Vector2>().x;

        // Putting the movement and saving it to rawmovement
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);

        Debug.Log(rawInputMovement);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        // Normal jump
        // TODO add double jump
        if (value.performed && isGrounded)
        {
            playerMovement.UpdateJump();
            isGrounded = false;
        } else
        {
            if (value.performed && canDoubleJump)
            {
                playerMovement.UpdateJump();
                canDoubleJump = false;
            } else
            {
                if (value.performed)
                {
                    playerMovement.UpdateGravity(glidingModifier);
                }
            }
        }

        // TODO add gravity changes with gliding
        
        if (value.canceled)
        {
            playerMovement.UpdateGravity(normalGravity);
        }
    }

    public void OnRun(InputAction.CallbackContext value)
    {
        // If hitting run button, speed modifier go up
        if (value.performed)
        {
            moveSpeedModifier = runSpeed;
        }

        // If not hitting run button speed modifier go down
        if (value.canceled)
        {
            moveSpeedModifier = 1f;
        }
    }

    void Update()
    {
        // Smooth the movement
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    void UpdatePlayerMovement()
    {
        // Updating the movement data for movement class
        playerMovement.UpdateMovementData(smoothInputMovement, horizontalMovement, moveSpeedModifier);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Grounding for jumping
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
            canDoubleJump = true;
        }
    }
}
