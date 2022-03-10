using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Animation Components")]
    [SerializeField] public Animator animator = null;

    [Header("Animator Values")]
    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;
    private int animatorBurnTrigger;
    private int animatorBurningBool;
    private int animatorGlidingBool;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRunAnimation(Vector2 velocity, float runSpeed)
    {
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / runSpeed;
        horizontalSpeedNormalized *= runAnimationSpeedModifier;
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);
    }

    public void JumpAnimation()
    {
        animator.SetTrigger(animatorJumpTrigger);
    }

    public void GlidingAnimation(bool isGliding) {
        animator.SetBool(animatorGlidingBool, isGliding);
    }

    public void UpdateGroundingAnimation(bool isGrounded)
    {
        animator.SetBool(animatorGroundedBool, isGrounded);
    }

    public void UpdateBurningAnimation()
    {
        animator.SetTrigger(animatorBurnTrigger);
    }

    private void FixedUpdate()
    {
        UpdateAnimationOverride();
    }

    void UpdateAnimationOverride()
    {

    }
}
