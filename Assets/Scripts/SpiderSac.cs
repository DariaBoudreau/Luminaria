using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSac : MonoBehaviour
{
    [SerializeField] PlayerCharging aspenObject;
    [SerializeField] int chargeCost;
    public delegate void CandleLight();
    public static event CandleLight Lighted;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 startPos;
    public bool triggerActive;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        col = this.GetComponent<Collider2D>();
        startPos = transform.position;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = true;
            if (aspenObject.isBurning && triggerActive)
            {
                rb.gravityScale = 1;
            }
        }
        else if(other.gameObject.CompareTag("Candle") || other.gameObject.CompareTag("burnable"))
        {
            if(Lighted != null)
            {
                Lighted();
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

    void OnEnable()
    {
        Recall.Recalled += ResetSac;
    }

    void OnDisable()
    {
        Recall.Recalled -= ResetSac;
    }

    private void ResetSac()
    {
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        transform.position = startPos;
    }


}
