using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSac : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost;
    public delegate void CandleLight();
    public static event CandleLight Lighted;

    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        col = this.GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            if (aspenObject.isBurning)
            {
                rb.gravityScale = 1;
            }
        }
        else if(other.gameObject.CompareTag("Candle"))
        {
            if(Lighted != null)
            {
                Lighted();
            }
        }
    }


}
