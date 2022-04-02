using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakingPlatform : MonoBehaviour
{
    [SerializeField] Candles[] candles;
    [SerializeField] GameObject platform;
    [SerializeField] bool startsAwake;
    private bool awake;
    void Start()
    {
        if(startsAwake)
        {
            awake = true;
            platform.SetActive(true);
        }
        else
        {
            awake = false;
            platform.SetActive(false);
        }
    }
    void Update()
    {
        if(!awake)
        {
            bool success = Check();
            if(success)
            {
                awake = true;
                platform.SetActive(true);
            }
            else
            {
                awake = false;
                platform.SetActive(false);
            }
        }
    }

    public bool Check()
    {
        bool success = true;
        foreach(Candles c in candles)
        {
            if(!c.isLit)
            {
                success = false;
            }
        }
        return success;
    }
}
