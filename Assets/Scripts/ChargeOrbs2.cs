using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ChargeOrbs2 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 50;
    [SerializeField] float xOffset = 3;

    PlayerMovement aspenMovement;
    PlayerCharging aspenCharging;

    int lastCharge;
    int maxCharge;

    GameObject[] children;

    private void OnEnable()
    {
        aspenMovement = GameObject.Find("Aspen").GetComponent<PlayerMovement>();
        aspenCharging = GameObject.Find("Aspen").GetComponent<PlayerCharging>();
    }

    private void Start()
    {
        int childCount = transform.childCount;
        children = new GameObject[childCount];

        transform.position = aspenMovement.transform.position;
        maxCharge = aspenCharging.maxCharge;

        int index = 0;
        foreach (Transform child in transform)
        {
            children[index] = child.gameObject;
            index++;
        }

        lastCharge = aspenCharging.currentCharge;

        UpdateFollowerOrbs(aspenCharging.currentCharge);
    }

    private void Update()
    {
        if (lastCharge != aspenCharging.currentCharge)
        {
            UpdateFollowerOrbs(aspenCharging.currentCharge);
        }
        lastCharge = aspenCharging.currentCharge;
    }



    // Movement in FixedUpdate because it is following an object moving via rigidbody forces
    private void FixedUpdate()
    {
        UpdateOrbPosition();
    }

    void UpdateOrbPosition()
    {
        int direction = aspenMovement.facingRight ? 1 : -1;

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] == null)
                continue;
            Vector3 desiredPosition = aspenMovement.transform.position - new Vector3((xOffset + i) * direction, 0, 0);
            Vector3 moveTowards = desiredPosition - children[i].transform.position;
            moveTowards *= moveSpeed / (i + 1);
            children[i].transform.position += moveTowards * Time.deltaTime;
        }
    }

    // Called once per shine instead of continuously in update
    public void UpdateFollowerOrbs(int charge)
    {
        for (int i = 0; i < maxCharge; i++)
        {
            children[i].GetComponent<SpriteRenderer>().enabled = i < charge ? true : false;
            children[i].GetComponentInChildren<Light2D>().enabled = i < charge ? true : false;
        }
    }
}
