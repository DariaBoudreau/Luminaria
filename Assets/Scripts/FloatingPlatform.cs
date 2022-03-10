using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FloatingPlatform : MonoBehaviour
{
    #region Orignal Floating Platform Script

    /* [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost;
    [SerializeField] float riseModifier = 1.0f;
    [SerializeField] float riseRate = 0.01f;
    [SerializeField] private bool isFloating = false;
    [SerializeField] private bool startsLit = false;
    [SerializeField] private bool triggerActive;
    [SerializeField] private bool horizontalMove = false;
    private float baseHeight;
    private float baseHorizon;
    private Coroutine floating;
    new private Light2D light;
    private float maxIntensity;


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
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = false;
            other.transform.parent = GameObject.Find("MOTH").transform;
        }
    }

    void Start()
    {
        light = GetComponentInChildren<Light2D>(true);
        maxIntensity = light.intensity;
        if(startsLit == false)
        {
            light.intensity = 0f;
        }
        else
        {
            light.intensity = 2.06f;
        }


        baseHeight = transform.localPosition.y;
        baseHorizon = transform.localPosition.x;
    }

    void Update()
    {
        if (triggerActive && aspenObject.isBurning)
        {
            if (aspenObject.currentCharge >= chargeCost)
            {
                if (!isFloating) 
                {
                    floating = StartCoroutine(Float());
                    if (startsLit == false)
                    {
                         aspenObject.currentCharge -= chargeCost;
                    }
                    else
                    {
                        aspenObject.currentCharge += -chargeCost;
                    }
 
                }
            } 
            else 
            {
                // NOT ENOUGH CHARGE
            }
        }
    }

    private IEnumerator Float()
    {
        isFloating = true;
        if (startsLit == false)
        {
            light.intensity = maxIntensity;
        }
        else
        {
            light.intensity = 0f;
        }
        if (startsLit && !horizontalMove)
        {
            for (float ft = 0; ft < 2 * Math.PI; ft += riseRate)
            {
                float y = (float)(1 - Math.Cos(ft));
                transform.localPosition = new Vector3(baseHorizon, baseHeight + (riseModifier * y), transform.localPosition.z);

                if (ft > Math.PI)   // we've reached the apex, it's all downhill from here
                    light.intensity = y * maxIntensity / 2;

                yield return null;
            }
        }
        else if (!horizontalMove)
        {
            for (float ft = 0; ft < 2 * Math.PI; ft += riseRate)
            {
                float y = (float)(1 - Math.Cos(ft));
                transform.localPosition = new Vector3(baseHorizon, baseHeight + (riseModifier * y), transform.localPosition.z);

                if (ft > Math.PI)   // we've reached the apex, it's all downhill from here
                    light.intensity = y * maxIntensity / 2;

                yield return null;
            }
        }
        else if (horizontalMove == true)
        {
            for (float ft = 0; ft < 2 * Math.PI; ft += riseRate)
            {
                float x = (float)(1 - Math.Cos(ft));
                transform.localPosition = new Vector3(baseHorizon + (riseModifier * x), baseHeight, transform.localPosition.z);

                if (ft > Math.PI)   // we've reached the apex, it's all downhill from here
                    light.intensity = x * maxIntensity / 2;

                yield return null;
            }
        }

        isFloating = false;
    } */
    #endregion

    #region  New Floating Platform Script

    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost;
    [SerializeField] public float riseRate = 1f;
    [SerializeField] public bool isLit;
    [SerializeField] private bool startsLit;
    [SerializeField] private bool triggerActive;
    [SerializeField] public Transform pos1;
    [SerializeField] public Transform pos2;
    
    private Coroutine floating;
    new public Light2D light;
    private float maxIntensity;
    private bool waitingDelay = false;
    private bool shouldMove;

    void Start()
    {
        light = GetComponentInChildren<Light2D>(true);
        maxIntensity = light.intensity;
        if (startsLit)
        {
            isLit = true;
            light.intensity = maxIntensity;
            transform.position = pos2.position;
        }
        else
        {
            isLit = false;
            light.intensity = 0f;
            transform.position = pos1.position;
        }

    }

    void Update()
    {
        if (waitingDelay)
            return;
        if (!triggerActive)
            return;
        if (!aspenObject.isBurning)
            return;
        //if (aspenObject.currentCharge < chargeCost)
            //return;
        if (isLit)
        {
            StartCoroutine(Delay(2));
            AddCharge();
            shouldMove = true;
            isLit = false;
            waitingDelay = true;
        }
        else if (!isLit)
        {
            StartCoroutine(Delay(2));
            RemoveCharge();
            shouldMove = true;
            isLit = true;
            waitingDelay = true;
        }
            
        
    }

    void FixedUpdate()
    {
        if (shouldMove)
        {
            Float();
        }
    }

    void OnEnable()
    {
        Recall.Recalled += Reset;
    }

    void OnDisable()
    {
        Recall.Recalled -= Reset;
    }

    void OnTriggerEnter2D(Collider2D other)
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

    private void Reset()
    {
        if(startsLit)
        {
            isLit = true;
            float step = riseRate * Time.deltaTime; 
            light.intensity = maxIntensity;
            transform.position = Vector3.MoveTowards(pos2.position, pos1.position, step);
        }
        else
        {
            isLit = false;
            float step = riseRate * Time.deltaTime; 
            light.intensity = 0f;
            transform.position = Vector3.MoveTowards(pos1.position, pos2.position, step);
        }
    }
    private void Float()
    {
        float step = riseRate * Time.deltaTime; 

        if (isLit)
        {
            if (Vector3.Distance(transform.position, pos2.position) < .05)
                return;
                        
            light.intensity = maxIntensity;
            transform.position = Vector3.MoveTowards(transform.position, pos2.position, step);
        }
        else if (!isLit)
        {
            if (Vector3.Distance(transform.position, pos1.position) < .05)
                return;

            light.intensity = 0f;
            transform.position = Vector3.MoveTowards(transform.position, pos1.position, step);
        }
    }

     IEnumerator Delay(float seconds)
     {
        yield return new WaitForSeconds(seconds);
        waitingDelay = false;
     }

    void AddCharge()
    {
        aspenObject.currentCharge += chargeCost;
    }

    void RemoveCharge()
    {
        aspenObject.currentCharge -= chargeCost;
    }
    #endregion
}