using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CandleDoor : MonoBehaviour
{
    [SerializeField] Candles[] candles;
    void Start()
    {

    }

    void Update()
    {   
        bool success = Check();
        if(success == true)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public bool Check()
    {
        bool success = true;
        foreach(Candles c in candles)
        {
            if(c.isLit == false)
            {
                success = false;
            }
        }
 
        return success;
        
    }
}
