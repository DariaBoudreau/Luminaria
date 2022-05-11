using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SpiderSac : MonoBehaviour
{
    [SerializeField] PlayerCharging aspenObject;
    [SerializeField] int chargeCost;
    [SerializeField] Candles candle;
    [SerializeField] Burnables web;
    public delegate void CandleLight();
    public static event CandleLight Lighted;
    private bool hasFallen = false;
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 startPos;
    public bool triggerActive;
    private ParticleSystem ps;
    private Light2D lt;
    private Collider2D barrier;
    private AudioSource soundClip;
    private GameObject fire;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        col = this.GetComponent<Collider2D>();
        startPos = transform.position;
        ps = GetComponentInChildren<ParticleSystem>();
        //ps.GetComponent<Renderer>().sortingOrder = 26;
        lt = GetComponentInChildren<Light2D>();
        //fire = ps.gameObject.transform.parent.gameObject;
        soundClip = GetComponent<AudioSource>();
        //fire.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(candle != null)
        {
            if(other.gameObject == candle.gameObject)
            {
                candle.LightCandle();
            }
        }
        if(web != null)
        {
            if(other.gameObject == web.gameObject)
            {
                web.Burn();
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = true;
            if (aspenObject.isBurning && triggerActive && !hasFallen)
            {
                hasFallen = true;
                rb.gravityScale = 1;
                aspenObject.currentCharge -= chargeCost;

                soundClip.Play();
                //fire.SetActive(true);
                ps.Play();
                lt.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = false;
        }
    }
    private void ResetSac()
    {
        hasFallen = false;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        transform.position = startPos;
    }
}
