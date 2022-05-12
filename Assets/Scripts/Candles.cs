using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Candles : MonoBehaviour
{
    //[SerializeField] CharacterController2D aspenObject;
    [SerializeField] PlayerController aspenObject;
    [SerializeField] PlayerCharging aspenCharging;
    [SerializeField] int chargeCost;
    [SerializeField] public bool isLit;
    [SerializeField] public bool startsLit;
    [SerializeField] private bool triggerActive;
    [SerializeField] private AudioClip lightCandle;
    [SerializeField] public AudioSource audioSource;
    new public Light2D light;
    private float maxIntensity;
    private bool waitingDelay = true;
    void Start()
    {
        audioSource.GetComponent<AudioSource>();
        light = GetComponentInChildren<Light2D>(true);
        maxIntensity = light.intensity;
        if (startsLit)
        {
            light.intensity = maxIntensity;
            transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            light.intensity = 0f;
            transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    void Update()
    {
        if (triggerActive && aspenCharging.isBurning && waitingDelay)
        {
            if(!aspenCharging.isInWater && !aspenObject.isWet)
            {
                if (isLit)
                {
                    ExtinguishCandle();
                }
                else if (aspenCharging.currentCharge >= chargeCost && !isLit)
                {
                    // I had to put the charge subtraction outside the method for something -Ricky
                    aspenCharging.currentCharge -= chargeCost;
                    LightCandle();
                }  
            }
        }
    }
    
    void OnEnable()
    {
        Recall.Recalled += Reset;
        SpiderSac.Lighted += LightCandle;
    }

    void OnDisable()
    {
        Recall.Recalled -= Reset;
        SpiderSac.Lighted -= LightCandle;
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

    public void LightCandle()
    {
        waitingDelay = false;
        isLit = true;
        //aspenObject.isWet = false;
        light.intensity = maxIntensity;
        transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(Delay(1));
        audioSource.PlayOneShot(lightCandle);
    }

    private void ExtinguishCandle()
    {
        waitingDelay = false;
        isLit = false;
        aspenCharging.currentCharge += chargeCost;
        light.intensity = 0f;
        transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(Delay(1));
    }

    private void Reset()
    {
        if(startsLit)
        {
            isLit = true;
            light.intensity = maxIntensity;
            transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            isLit = false;
            light.intensity = 0f;
            transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        waitingDelay = true;
    }
}