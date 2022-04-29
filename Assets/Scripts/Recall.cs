using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recall : MonoBehaviour
{
    [SerializeField]
    //CharacterController2D aspenObject;
    PlayerCharging aspenObject;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip usedStation;

    public delegate void RecallAction();
    public static event RecallAction Recalled;
    private bool triggerActive;


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(aspenObject.isBurning && triggerActive && !audioSource.isPlaying)
        {
            Debug.Log("bababooey");
            Debug.Log(aspenObject.maxCharge);

            aspenObject.currentCharge = aspenObject.maxCharge;
            if (usedStation != null) audioSource.PlayOneShot(usedStation);
            aspenObject.CheckForFullLights();
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
