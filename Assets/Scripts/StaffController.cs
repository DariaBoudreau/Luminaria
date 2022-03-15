using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffController : MonoBehaviour
{
    [SerializeField]
    bool shouldStartWithStaff = true;

    Animator animator;
    int animatorNoStaffBool;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorNoStaffBool = Animator.StringToHash("HasNoStaff");
        if (!shouldStartWithStaff)
        {
            animator.SetBool(animatorNoStaffBool, true);
        }
    }
}
