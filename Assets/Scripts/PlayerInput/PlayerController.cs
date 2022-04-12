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
    public PlayerCharging playerCharging;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    public float movementSmoothingSpeed = 1f;
    public float runSpeed = 3f;
    private float moveSpeedModifier = 1f;
    public float normalGravity = 1f;
    private float horizontalMovement;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    public Vector2 lastInput;
    public Vector2 inputMovement;

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
    public bool isWet = false;
    //public bool isInWater = false;

    //Whether Aspen should be able to burn at all in this scene
    //Should be public so it can be changed in the animator
    public bool ableToBurn = true;

    bool isGrounded = false;
    bool canDoubleJump = true;
    bool isGliding = false;
    bool hasLanded = false;
    public bool jumpInput = false;
    

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
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (playerMovement.jumpPower > 0)
        {
            if (value.performed && isGrounded)
            {
                jumpInput = true;
                isInAir = true;
                playerMovement.UpdateJump(isWet);
                isGrounded = false;
                playerAnimation.JumpAnimation();
            }
            else
            {
                if (value.performed && canDoubleJump && !isWet)
                {
                    jumpInput = true;
                    playerMovement.UpdateJump(isWet);
                    canDoubleJump = false;
                    playerAnimation.JumpAnimation();
                }
                else
                {
                    if (value.performed && !isWet)
                    {
                        jumpInput = true;
                        isGliding = true;
                        playerMovement.UpdateGravity(isGliding);
                    }
                }
            }

            if (value.canceled)
            {
                isGliding = false;
                jumpInput = false;
                playerMovement.UpdateGravity(isGliding);
                sr.color = new Color(1, 1, 1, 1);
            }

            playerAnimation.GlidingAnimation(isGliding);
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

    public void OnShine(InputAction.CallbackContext value)
    {
        if (ableToBurn)
        {
            if (value.performed && !hasTransitioned)
            {
                playerAnimation.StartBurningAnimation();
                playerCharging.StartBurning();
                hasTransitioned = true;
            }

            if (value.canceled)
            {
                playerCharging.StopBurning();
                hasTransitioned = false;
            }

            playerAnimation.UpdateBurningAnimation(isBurning);
        }
    }

    void Update()
    {
        // Smooth the movement
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
        ShouldPersistVelocity();
        UpdatePlayerGliding();
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

    void UpdatePlayerGliding()
    {
        bool isFalling = playerMovement.isFalling;

        if (jumpInput && isFalling && !isWet)
        {
            //Debug.Log("Character should glide");
            isGliding = true;
            playerMovement.UpdateGravity(isGliding);
            playerAnimation.GlidingAnimation(isGliding);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Grounding for jumping
        if (collision.gameObject.CompareTag("Floor"))
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

        playerMovement.LandPlayer();
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
