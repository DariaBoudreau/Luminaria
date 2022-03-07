using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviours")]
    public PlayerMovement playerMovement;
    public PlayerAnimation playerAnimation;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    public float runSpeed = 3f;
    private float moveSpeedModifier = 1f;
    public float normalGravity = 1f;
    public float glidingModifier = 3f;
    private float horizontalMovement;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    public Vector2 lastInput;
    public Vector2 inputMovement;
    //public Vector2 last

    [Header("Testing Indicators")]
    private SpriteRenderer sr;

    public bool isInAir = false;
    bool isGrounded = false;
    bool canDoubleJump = true;
    bool isGliding = false;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
        horizontalMovement = value.ReadValue<Vector2>().x;

        // Facing right or left for flipplayer
        horizontalMovement = value.ReadValue<Vector2>().x;

        // Putting the movement and saving it to rawmovement
        rawInputMovement = new Vector3(inputMovement.x, inputMovement.y, 0);

        playerAnimation.UpdateRunAnimation(inputMovement, runSpeed);

        Debug.Log(rawInputMovement);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        // Normal jump
        // TODO add double jump
        if (value.performed && isGrounded)
        {
            isInAir = true;
            playerMovement.UpdateJump();
            isGrounded = false;
            playerAnimation.JumpAnimation();
        } else
        {
            if (value.performed && canDoubleJump)
            {
                playerMovement.UpdateJump();
                canDoubleJump = false;
                playerAnimation.JumpAnimation();
            } else
            {
                if (value.performed && !isGliding)
                {
                    isGliding = true;
                    playerMovement.UpdateGravity(glidingModifier, isGliding);
                    sr.color = new Color(0, 1, 0, 1);
                } else
                {
                    if (value.performed)
                    {
                        //isGliding = true;
                        playerMovement.UpdateGravity(glidingModifier, isGliding);
                    }
                }
            }
        }

        // TODO add gravity changes with gliding
        if (value.canceled)
        {
            isGliding = false;
            playerMovement.UpdateGravity(normalGravity, isGliding);
            sr.color = new Color(1, 1, 1, 1);
        }

        playerAnimation.GlidingAnimation(isGliding);
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
        ShouldPersistVelocity();
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
            isInAir = false;
            isGrounded = true;
            canDoubleJump = true;
            isGliding = false;
        }
    }

    void ShouldPersistVelocity()
    {
        if (isInAir && inputMovement.x == 0)
        {
            playerMovement.shouldPersistVelocity = true;
        }
        else
        {
            playerMovement.shouldPersistVelocity = false;
        }
    }
}
