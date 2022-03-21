using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTester : MonoBehaviour
{
    public PlayerCharging aspen;
    public int chargeCost;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        aspen.chargeChange = chargeCost;
    }
}
