using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recall : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] Candles[] candles;
    [SerializeField] FloatingPlatform2[] platforms;



    private bool triggerActive;
    void Start()
    {
    
    }
    void Update()
    {
        if(aspenObject.isBurning && triggerActive)
        {
            aspenObject.currentCharge = aspenObject.maxCharge;

            foreach(Candles c in candles)
            {
                c.isLit = false;
                c.light.intensity = 0f;
                c.transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = false;
            }
            foreach(FloatingPlatform2 p in platforms)
            {
                p.isLit = false;
                p.light.intensity = 0f;
                p.transform.position = Vector3.MoveTowards(p.pos1.position, p.pos2.position, (p.riseRate*Time.deltaTime));
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
