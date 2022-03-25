using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CandleDoor : MonoBehaviour
{
    [SerializeField] Candles[] candles;
    //[SerializeField] bool startsAwake;
/*     void Start()
    {
        if(startsAwake)
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    } */

    void Update()
    {   
        bool success = Check();
        if(success == false)
        {
            //this.SetActive(false);
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<EdgeCollider2D>().enabled = false;
        }
        else
        {
            //this.SetActive(true);
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.GetComponent<EdgeCollider2D>().enabled = true;
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
