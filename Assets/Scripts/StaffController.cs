using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StaffController : MonoBehaviour
{
    [SerializeField]
    bool shouldStartWithStaff = true;

    Animator animator;
    int animatorNoStaffBool;

    [SerializeField]
    CharacterDialogue characterDialogue;

    [SerializeField]
    PlayableDirector playableDirector;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorNoStaffBool = Animator.StringToHash("HasNoStaff");
        if (!shouldStartWithStaff)
        {
            animator.SetBool(animatorNoStaffBool, true);
        }
    }

    private void OnEnable()
    {
        characterDialogue.finishedTalking += PlayTimeline;
    }

    private void OnDisable()
    {
        characterDialogue.finishedTalking -= PlayTimeline;
    }

    void PlayTimeline()
    {
        playableDirector.Play(playableDirector.playableAsset);
        animator.SetBool(animatorNoStaffBool, false);
    }
}
