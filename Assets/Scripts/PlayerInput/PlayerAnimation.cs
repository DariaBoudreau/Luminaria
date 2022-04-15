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
    private int animatorNodTrigger;

    GroundType currentGroundType;

    [Header("Animation Overrides")]
    public AnimatorOverrideController AspenLeft;
    public AnimatorOverrideController AspenRight;

    public float runAnimationSpeedModifier = 0.5f;

    private bool isRunning = false;
    private float speedNormalized;

    // Start is called before the first frame update
    void Start()
    {
        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorBurnTrigger = Animator.StringToHash("Burn");
        animatorBurningBool = Animator.StringToHash("Burning");
        animatorGlidingBool = Animator.StringToHash("Gliding");
        animatorNodTrigger = Animator.StringToHash("Nod");
    }
    
    public void UpdateRunAnimation(Vector2 velocity, float runSpeed)
    {
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) * runSpeed;

        //Debug.Log(currentGroundType);
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);
        if (velocity.x > 0.1 || velocity.x < 0.1)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        speedNormalized = horizontalSpeedNormalized;
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
        currentGroundType = groundType;
        animator.SetBool(animatorGroundedBool, isGrounded);
        //Debug.Log(currentGroundType);
        //Debug.Log(isGrounded);

        if (needsSFX)
        {
            //audioPlayer.PlayLanding(currentGroundType);
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

    public void PlayNod()
    {
        //Debug.Log("Aspen will nod");
        animator.SetTrigger(animatorNodTrigger);
    }

    private void FixedUpdate()
    {
        if (isRunning)
        {
            audioPlayer.PlaySteps(currentGroundType, speedNormalized);
        }
        UpdateAnimationOverride();
    }

    void UpdateAnimationOverride()
    {

    }
}
