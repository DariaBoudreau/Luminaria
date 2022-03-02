using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Candles : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost;
    [SerializeField] public bool isLit = false;
    [SerializeField] private bool triggerActive;
    new public Light2D light;
    private float maxIntensity;
    private bool waitingDelay = true;
    void Start()
    {
        isLit = false;
        triggerActive = false;
        light = GetComponentInChildren<Light2D>(true);
        transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = false;
        maxIntensity = light.intensity;
        if (isLit)
        {
            light.intensity = maxIntensity;
        }
        else
        {
            light.intensity = 0f;
        }
    }
    void Update()
    {
            if (triggerActive && aspenObject.isBurning && waitingDelay)
            {
                if (isLit)
                {

                    ExtinguishCandle();
                }
                else if (aspenObject.currentCharge >= chargeCost && !isLit)
                {

                    LightCandle();
                }

            }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
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

    private void LightCandle()
    {
        waitingDelay = false;
        isLit = true;
        aspenObject.currentCharge -= chargeCost;
        light.intensity = maxIntensity;
        transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(Delay(1));
    }

    private void ExtinguishCandle()
    {
        waitingDelay = false;
        isLit = false;
        aspenObject.currentCharge += chargeCost;
        light.intensity = 0f;
        transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(Delay(1));
    }

    IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        waitingDelay = true;
    }
}