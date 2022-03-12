using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimation : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    [Header("Animations")]
    private int animatorNodTrigger;
    private int animatorGesutreTrigger;
    private int animatorTalkTrigger;

    // Start is called before the first frame update
    void Start()
    {
        animatorNodTrigger = Animator.StringToHash("Nod");
        animatorGesutreTrigger = Animator.StringToHash("Gesture");
        animatorTalkTrigger = Animator.StringToHash("Talk");
    }

    public void PlayTalk()
    {

    }

    public void PlayNod()
    {

    }

    public void PlayGesture()
    {

    }
}
