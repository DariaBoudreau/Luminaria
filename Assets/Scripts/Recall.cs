using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recall : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    public delegate void RecallAction();
    public static event RecallAction Recalled;
    private bool triggerActive;

    void Update()
    {
        if(aspenObject.isBurning && triggerActive)
        {
            aspenObject.currentCharge = aspenObject.maxCharge;
            if(Recalled != null)
            {
                Recalled.Invoke();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = false;
        }
    }
}
