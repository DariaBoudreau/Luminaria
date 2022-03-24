using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ChargeOrbs : MonoBehaviour
{
    [SerializeField]
    //CharacterController2D Aspen;
    PlayerCharging Aspen;

    int chargeLevel = 0;
    int maxCharge;

    [Header("MUST be = to the # of children. No need to drag in children here.")]
    [SerializeField]
    GameObject[] children;

    private void Start()
    {
        transform.position = Aspen.transform.position;
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
            children[i].GetComponentInChildren<Light2D>().enabled = i < charge ? true : false;
        }
    }
}
