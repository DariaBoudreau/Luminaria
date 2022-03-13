using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimation : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    [Header("Animations")]
    private int animatorNodTrigger;
    private int animatorGesutreTrigger;
    private int animatorTalkBool;

    // Start is called before the first frame update
    void Start()
    {
        animatorNodTrigger = Animator.StringToHash("Nod");
        animatorGesutreTrigger = Animator.StringToHash("Gesture");
        animatorTalkBool = Animator.StringToHash("isTalking");
    }

    public void PlayTalk(bool isTalking)
    {
        animator.SetBool(animatorTalkBool, true);
    }

    public void PlayNod()
    {

    }

    public void PlayGesture()
    {

    }
}
