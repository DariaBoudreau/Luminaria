using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FloatingPlatform2 : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost;
    [SerializeField] float riseModifier = 1.0f;
    [SerializeField] float riseRate = 1f;
    [SerializeField] private bool isFloating = false;
    [SerializeField] private bool isLit = false;
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
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = false;
            other.transform.parent = GameObject.Find("MOTH").transform;
        }
    }

    void Start()
    {
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


        baseHeight = transform.localPosition.y;
        baseHorizon = transform.localPosition.x;
    }

    void Update()
    {
        if (triggerActive && aspenObject.isBurning)
        {
            if (aspenObject.currentCharge >= chargeCost && !isFloating)
            {
                Float();
            }
            else if (isLit == true && !isFloating)
            {
                Float();
            }
        }
    }
    private void Float()
    {
        Vector3 currentPos = this.GetComponent<Transform>().position;
        isLit = !isLit;
        isFloating = true;

        if (isLit == true)
        {
            light.intensity = maxIntensity;
        }
        else
        {
            light.intensity = 0f;
        }
        if (isLit == true && !horizontalMove)
        {
            aspenObject.currentCharge -= chargeCost;
            this.transform.position = currentPos + (Vector3.up * riseModifier);
        }
        else if (isLit == false && !horizontalMove)
        {
            aspenObject.currentCharge += chargeCost;
            this.transform.position = currentPos - (Vector3.up * riseModifier);
        }
        else if (isLit == true && horizontalMove)
        {
            aspenObject.currentCharge -= chargeCost;
            this.transform.position = currentPos + (Vector3.left * riseModifier);
        }
        else if (isLit == false && horizontalMove)
        {
            aspenObject.currentCharge += chargeCost;
            this.transform.position = currentPos - (Vector3.left * riseModifier);
        }
        isFloating = false;
    }
}