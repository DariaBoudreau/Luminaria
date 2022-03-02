using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeOrbs : MonoBehaviour
{
    [SerializeField]
    CharacterController2D Aspen;

    int chargeLevel = 0;
    int maxCharge;

    GameObject[] children = new GameObject[3];

    private void Start()
    {
        maxCharge = Aspen.maxCharge;
        int index = 0;
        foreach (Transform child in transform)
        {
            children[index] = child.gameObject;
            index++;
        }
    }

    private void Update()
    {
        chargeLevel = Aspen.currentCharge;
        UpdateFollowerOrbs(chargeLevel);
    }

    void UpdateFollowerOrbs(int charge)
    {
        for (int i = 0; i < maxCharge; i++)
        {
            children[i].GetComponent<SpriteRenderer>().enabled = i < charge ? true : false;
        }
    }
}
