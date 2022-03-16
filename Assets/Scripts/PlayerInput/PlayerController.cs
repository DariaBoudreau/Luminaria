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
    //public PlayerCharging playerCharging;

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

    [Header("Ground Types")]
    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;
    private GroundType groundType;
    private CapsuleCollider2D controllerCollider;

    [Header("Testing Indicators")]
    private SpriteRenderer sr;

    public bool isInAir = false;
    public bool isBurning;
    public bool hasTransitioned = false;
    bool isGrounded = false;
    bool canDoubleJump = true;
    bool isGliding = false;
    bool hasLanded = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        controllerCollider = GetComponent<CapsuleCollider2D>();
        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");
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

    public void OnShine(InputAction.CallbackContext value)
    {
        if (value.performed && !hasTransitioned)
        {
            playerAnimation.StartBurningAnimation();
            isBurning = true;
            hasTransitioned = true;
        } 

        if (value.canceled)
        {
            isBurning = false;
            hasTransitioned = false;
        }

        playerAnimation.UpdateBurningAnimation(isBurning);
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

            if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
            {
                hasLanded = true;
            }
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

    private void FixedUpdate()
    {
        UpdateGrounding();
    }

    void UpdateGrounding()
    {
        // Taken from previous character controller because it was already written in the best way imo
        // Use character collider to check if touching ground layers
        if (controllerCollider.IsTouchingLayers(softGroundMask))
            groundType = GroundType.Soft;
        else if (controllerCollider.IsTouchingLayers(hardGroundMask))
            groundType = GroundType.Hard;
        else
            groundType = GroundType.None;

        // Update animator
        playerAnimation.UpdateGroundingAnimation(groundType != GroundType.None, hasLanded, groundType);
        hasLanded = false;
    }
}
