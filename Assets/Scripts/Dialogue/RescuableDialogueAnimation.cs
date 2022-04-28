using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescuableDialogueAnimation : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    [Header("Animations")]
    private int animatorTalkBool;

    // Start is called before the first frame update
    void Start()
    {
        animatorTalkBool = Animator.StringToHash("isTalking");
    }

    public void PlayTalk(bool isTalking)
    {
        animator.SetBool(animatorTalkBool, true);
    }
}
