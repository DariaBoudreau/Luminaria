using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Animation Components")]
    [SerializeField] public Animator animator = null;
    [SerializeField] CharacterAudio audioPlayer = null;

    [Header("Animator Values")]
    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;
    private int animatorBurnTrigger;
    private int animatorBurningBool;
    private int animatorGlidingBool;

    GroundType currentGroundType;
    bool isBurning;

    [Header("Animation Overrides")]
    public AnimatorOverrideController AspenLeft;
    public AnimatorOverrideController AspenRight;

    public float runAnimationSpeedModifier = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorBurnTrigger = Animator.StringToHash("Burn");
        animatorBurningBool = Animator.StringToHash("Burning");
        animatorGlidingBool = Animator.StringToHash("Gliding");
    }

    public void UpdateRunAnimation(Vector2 velocity, float runSpeed)
    {
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) * runSpeed;

        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);
        audioPlayer.PlaySteps(currentGroundType, horizontalSpeedNormalized);
    }

    public void JumpAnimation()
    {
        animator.SetTrigger(animatorJumpTrigger);
        audioPlayer.PlayJump();
    }

    public void GlidingAnimation(bool isGliding) {
        animator.SetBool(animatorGlidingBool, isGliding);
    }

    public void UpdateGroundingAnimation(bool isGrounded, bool needsSFX, GroundType groundType)
    {
        animator.SetBool(animatorGroundedBool, isGrounded);
        currentGroundType = groundType;

        if (needsSFX)
        {
            audioPlayer.PlayLanding(currentGroundType);
        }
    }

    public void StartBurningAnimation()
    {
        animator.SetTrigger(animatorBurnTrigger);
    }

    public void UpdateBurningAnimation(bool isBurning)
    {
        animator.SetBool(animatorBurningBool, isBurning);
    }

    private void FixedUpdate()
    {
        UpdateAnimationOverride();
    }

    void UpdateAnimationOverride()
    {

    }
}
