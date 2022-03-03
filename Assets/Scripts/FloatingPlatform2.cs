using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FloatingPlatform2 : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost;
    [SerializeField] public float riseRate = 1f;
    [SerializeField] public bool isLit = false;
    [SerializeField] private bool triggerActive;
    [SerializeField] public Transform pos1;
    [SerializeField] public Transform pos2;
    
    private Coroutine floating;
    new public Light2D light;
    private float maxIntensity;
    private bool waitingDelay = false;

    void Start()
    {
        //transform.position = pos1.position;
        light = GetComponentInChildren<Light2D>(true);
        maxIntensity = light.intensity;
        if (isLit == false)
        {
            light.intensity = 0f;
        }
        else
        {
            light.intensity = 2.06f;
        }

    }

    void Update()
    {
        if (triggerActive && aspenObject.isBurning)
        {
            if (aspenObject.currentCharge >= chargeCost && !waitingDelay)
            {
                Float();
            }
            else if (isLit == true && !waitingDelay)
            {
                Float();
            }
        }
    }
    void OnEnable()
    {
        Recall.Recalled += Float;
    }

    void OnDisable()
    {
        Recall.Recalled -= Float;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = true;
            other.transform.parent = this.gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = false;
            other.transform.parent = GameObject.Find("MOTH").transform;
        }
    }
    private void Float()
    {
        isLit = !isLit;
        waitingDelay = true;
        float step = riseRate * Time.deltaTime; 

        if (isLit)
        {
            light.intensity = maxIntensity;
            aspenObject.currentCharge -= chargeCost;
            this.transform.position = Vector3.MoveTowards(pos2.position, pos1.position, step);
            StartCoroutine(Delay(0.5f));
        }
        else if (isLit == false)
        {
            light.intensity = 0f;
            aspenObject.currentCharge += chargeCost;
            this.transform.position = Vector3.MoveTowards(pos1.position, pos2.position, step);
            StartCoroutine(Delay(0.5f));
        }
    }

     IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        waitingDelay = false;
    }
}