using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

using UnityEngine.InputSystem;

public enum GroundType
{
    None,
    Soft,
    Hard
}

public class CharacterController2D : MonoBehaviour
{
    static readonly float charScale = 0.3f;
    readonly Vector3 flippedScale = new Vector3(-1 * charScale, charScale, charScale);
    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    [Header("Character")]
    [SerializeField] Animator animator = null;
    [SerializeField] new Transform transform;
    [SerializeField] CharacterAudio audioPlayer = null;
    [SerializeField] PlayerInput playerInput;

    [Header("Equipment")]
    [SerializeField] Transform handAnchor = null;
    [SerializeField] UnityEngine.U2D.Animation.SpriteLibrary spriteLibrary = null;

    [Header("Movement")]
    [SerializeField] float acceleration = 30.0f;
    [SerializeField] float maxSpeed = 8.0f;
    [SerializeField] float jumpForce = 35.0f;
    [SerializeField] float minFlipSpeed = 0.1f;
    [SerializeField] float jumpGravityScale = 2.0f;
    [SerializeField] float fallGravityScale = 5.0f;
    [SerializeField] float groundedGravityScale = 1.0f;
    [SerializeField] bool resetSpeedOnLand = false;
    // [SerializeField] UnityEvent test;

    private Rigidbody2D controllerRigidbody;
    private CapsuleCollider2D controllerCollider;
    private float colliderOffset = .4f;
    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;

    private Vector2 movementInput;
    private bool jumpInput;

    [Header("Animation Overrides")]
    public AnimatorOverrideController AspenLeft;
    public AnimatorOverrideController AspenRight;

    private Vector2 prevVelocity;
    private GroundType groundType;
    private bool isJumping;
    private bool isFalling;
    private bool isGliding;
    private bool isSliding;
    private bool doubleJump = false;
    private bool faceRight = false;
    private bool hasTransitioned;
    private bool canJump = true;
    public bool shouldNod = false;

    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;
    private int animatorBurnTrigger;
    private int animatorBurningBool;
    private int animatorGlidingBool;
    private int animatorNodTrigger;

    // Staff Charge Variables
    [Header("Charge")]
    public bool isBurning = false;
    public int currentCharge;
    public int maxCharge = 3;
    private Light2D lt;
    private ParticleSystem ps;
    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule em;
    private float prevCharge;
    private float increment;

    public bool CanMove { get; set; }

    public bool isWet;

    void Start()
    {
#if UNITY_EDITOR
        if (Keyboard.current == null)
        {
            var playerSettings = new UnityEditor.SerializedObject(Resources.FindObjectsOfTypeAll<UnityEditor.PlayerSettings>()[0]);
            var newInputSystemProperty = playerSettings.FindProperty("enableNativePlatformBackendsForNewInputSystem");
            bool newInputSystemEnabled = newInputSystemProperty != null ? newInputSystemProperty.boolValue : false;

            if (newInputSystemEnabled)
            {
                var msg = "New Input System backend is enabled but it requires you to restart Unity, otherwise the player controls won't work. Do you want to restart now?";
                if (UnityEditor.EditorUtility.DisplayDialog("Warning", msg, "Yes", "No"))
                {
                    UnityEditor.EditorApplication.ExitPlaymode();
                    var dataPath = Application.dataPath;
                    var projectPath = dataPath.Substring(0, dataPath.Length - 7);
                    UnityEditor.EditorApplication.OpenProject(projectPath);
                }
            }
        }
#endif

        controllerRigidbody = GetComponent<Rigidbody2D>();
        controllerCollider = GetComponent<CapsuleCollider2D>();
        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");

        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorBurnTrigger = Animator.StringToHash("Burn");
        animatorBurningBool = Animator.StringToHash("Burning");
        animatorGlidingBool = Animator.StringToHash("Gliding");
        animatorNodTrigger = Animator.StringToHash("Nod");

        CanMove = true;

        ps = GetComponentInChildren<ParticleSystem>(true);
        lt = ps.gameObject.transform.parent.GetComponentInChildren<Light2D>(true);
        main = ps.main;
        em = ps.emission;
        currentCharge = 0;
        prevCharge = -1;
    }

    void Update()
    {
        Move();   
    }

    void Move()
    {
        var keyboard = Keyboard.current;

        if (!CanMove || keyboard == null)
            return;

        GetHorizontalMovement(keyboard);
        GetVerticalMovement(keyboard);
        GetChargeInput(keyboard);
    }

    void GetHorizontalMovement(Keyboard keyboard)
    {

        // Horizontal movement
        float moveHorizontal = 0.0f;

        if (!keyboard.shiftKey.isPressed)
        {
            isBurning = false;
            hasTransitioned = false;

            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
                moveHorizontal = keyboard.zKey.isPressed ? -3.0f : -1.0f;
            else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
                moveHorizontal = keyboard.zKey.isPressed ? 3.0f : 1.0f;

        }
        else if (!isFalling)
        {
            // only allow burning while falling and not moving
            if (!hasTransitioned)
            {
                animator.SetTrigger(animatorBurnTrigger);
                hasTransitioned = true;
            }
            isBurning = true;
        }

        movementInput = new Vector2(moveHorizontal, 0);
    }

    void GetVerticalMovement(Keyboard keyboard)
    {
        // Jumping input
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (!isJumping && canJump)
                jumpInput = true;
            else if (!doubleJump && canJump)
            {
                jumpInput = true;
                doubleJump = true;
                canJump = false;
            }
        }

        // Gliding
        if (keyboard.spaceKey.isPressed && isFalling)
        {
            isGliding = true;
        }
        else
        {
            isGliding = false;
        }
    }
    
    void GetChargeInput(Keyboard keyboard)
    {
        // Debug Charging
        if (keyboard.cKey.wasPressedThisFrame)
            currentCharge++;
        if (keyboard.xKey.wasPressedThisFrame)
            currentCharge--;
    }

    private float bounds(float n, int lower, int upper)
    {
        if (n < lower) return lower;
        if (n > upper) return upper;
        return n;
    }

    void FixedUpdate()
    {
        UpdateGrounding();
        UpdateVelocity();
        UpdateAnimation();
        UpdateJump();
        UpdateGravityScale();

        if (prevCharge != currentCharge)
            UpdateCharge();

        prevVelocity = controllerRigidbody.velocity;
    }

    public void Nod()
    {
        animator.SetTrigger(animatorNodTrigger);
        shouldNod = false;
    }

    public void UpdateCharge()
    {
        currentCharge = (int) bounds(currentCharge, 0, maxCharge);

        main.maxParticles = 7 * currentCharge;
        em.rateOverTime = 3 * currentCharge;

        if (prevCharge < currentCharge) {
            StartCoroutine(FadeUp());
        } else if (prevCharge > currentCharge) {
            StartCoroutine(FadeDown());
        }

        prevCharge = (float) currentCharge;
    }

    private IEnumerator FadeUp()
    {
        for (float ft = prevCharge; ft < currentCharge; ft += 0.1f) {
            main.simulationSpeed = ft + 1;
            lt.intensity = ft;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator FadeDown()
    {
        for (float ft = prevCharge; ft >= currentCharge; ft -= 0.1f) {
            main.simulationSpeed = ft + 1;
            lt.intensity = ft;
            yield return new WaitForSeconds(0.05f);
        }
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
        animator.SetBool(animatorGroundedBool, groundType != GroundType.None);
    }
    
    private void UpdateVelocity()
    {
        Vector2 velocity = controllerRigidbody.velocity;

        // Apply acceleration directly as we'll want to clamp
        // prior to assigning back to the body.
        velocity += movementInput * acceleration * Time.fixedDeltaTime;

        // Clamp horizontal speed.
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        // Check to see if the player is in the air and not sending input
        if (isJumping && movementInput.x == 0)
        {
            // persist previous x velocity if no input is being sent
            controllerRigidbody.velocity = new Vector2(prevVelocity.x, velocity.y);
        }
        else
        {
            // Assign velocity back to the body.
            controllerRigidbody.velocity = velocity;
        }

        // We've consumed the movement, reset it.
        movementInput = Vector2.zero;

        // Update animator running speed
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / maxSpeed;
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);

        // Play audio
        audioPlayer.PlaySteps(groundType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && controllerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (jumpInput)
        {
            // Set animator
            animator.SetTrigger(animatorJumpTrigger);

            // Jump using impulse force
            // controllerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            controllerRigidbody.AddForce(transform.up * 2200f);

            // We've consumed the jump, reset it.
            jumpInput = false;

            // Set jumping flag
            isJumping = true;

            // Play audio
            audioPlayer.PlayJump();
        }

        // Landed
        else if (isJumping && isFalling && groundType != GroundType.None)
        {
            // Since collision with ground stops rigidbody, reset velocity
            if (resetSpeedOnLand)
            {
                prevVelocity.y = controllerRigidbody.velocity.y;
                controllerRigidbody.velocity = prevVelocity;
            }

            // Reset jumping flags
            isJumping = false;
            isFalling = false;
            doubleJump = false;

            // Play audio
            audioPlayer.PlayLanding(groundType);
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool(animatorBurningBool, isBurning);
        animator.SetBool(animatorGlidingBool, isGliding);

        animator.runtimeAnimatorController = faceRight? AspenRight : AspenLeft;

        // Use animator overrides to flip character depending on direction
        if (controllerRigidbody.velocity.x > minFlipSpeed)
        {
            faceRight = true;
            controllerCollider.GetComponent<CapsuleCollider2D>().offset = new Vector2(colliderOffset, controllerCollider.offset.y);
        }
        else if (controllerRigidbody.velocity.x < -minFlipSpeed)
        {
            faceRight = false;
            controllerCollider.GetComponent<CapsuleCollider2D>().offset = new Vector2(-colliderOffset, controllerCollider.offset.y);
        }

        if (shouldNod)
        {
            Nod();
        }
    }


    private void UpdateGravityScale()
    {
        // Use grounded gravity scale by default.
        var gravityScale = groundedGravityScale;

        if (groundType == GroundType.None)
        {
            // If not grounded then set the gravity scale according to upwards (jump) or downwards (falling) motion.
            gravityScale = controllerRigidbody.velocity.y > 0.0f ? jumpGravityScale : fallGravityScale;           
        }

        controllerRigidbody.gravityScale = isGliding ? gravityScale / 3 : gravityScale;
    }

    public void GrabItem(Transform item)
    {
        // Attach item to hand
        item.SetParent(handAnchor, false);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
    }

    public void SwapSprites(UnityEngine.U2D.Animation.SpriteLibraryAsset spriteLibraryAsset)
    {
        spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //Debug.Log(controllerRigidbody.velocity);
            canJump = true;
            //Debug.Log(controllerRigidbody.velocity);
        }
    }
}
