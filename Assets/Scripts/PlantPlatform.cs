using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlantPlatform : MonoBehaviour
{
       [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost;
    [SerializeField] public float riseRate = 1f;
    [SerializeField] public bool isLit;
    [SerializeField] private bool startsLit;
    [SerializeField] private bool triggerActive;
    [SerializeField] public Transform pos1;
    [SerializeField] public Transform pos2;
    
    private Coroutine floating;
    private float maxIntensity;
    private bool waitingDelay = false;
    private bool shouldMove;

    void Start()
    {
        if (startsLit)
        {
            isLit = true;
            transform.position = pos2.position;
        }
        else
        {
            isLit = false;
            transform.position = pos1.position;
        }

    }

    void Update()
    {
        if (waitingDelay)
            return;
        if (!triggerActive)
            return;
        if (!aspenObject.isBurning)
            return;
        if(!aspenObject.isWet)
            return;
        if (aspenObject.currentCharge < chargeCost)
            return;
        if (isLit)
        {
            StartCoroutine(Delay(2));
            AddCharge();
            shouldMove = true;
            isLit = false;
            waitingDelay = true;
        }
        else if (!isLit)
        {
            StartCoroutine(Delay(2));
            RemoveCharge();
            shouldMove = true;
            isLit = true;
            waitingDelay = true;
        }
            
        
    }

    void FixedUpdate()
    {
        if (shouldMove)
        {
            Move();
        }
    }

    void OnEnable()
    {
        Recall.Recalled += Reset;
    }

    void OnDisable()
    {
        Recall.Recalled -= Reset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = true;
            other.transform.parent = this.gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = false;
            other.transform.parent = GameObject.Find("MOTH").transform;
        }
    }

    private void Reset()
    {
        if(startsLit)
        {
            isLit = true;
            float step = riseRate * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(pos2.position, pos1.position, step);
        }
        else
        {
            isLit = false;
            float step = riseRate * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(pos1.position, pos2.position, step);
        }
    }
    private void Move()
    {
        float step = riseRate * Time.deltaTime; 

        if (isLit)
        {
            if (Vector3.Distance(transform.position, pos2.position) < .05)
                return;
                        
            transform.position = Vector3.MoveTowards(transform.position, pos2.position, step);
        }
        else if (!isLit)
        {
            if (Vector3.Distance(transform.position, pos1.position) < .05)
                return;
            transform.position = Vector3.MoveTowards(transform.position, pos1.position, step);
        }
    }

     IEnumerator Delay(float seconds)
     {
        yield return new WaitForSeconds(seconds);
        waitingDelay = false;
     }

    void AddCharge()
    {
        aspenObject.currentCharge += chargeCost;
    }

    void RemoveCharge()
    {
        aspenObject.currentCharge -= chargeCost;
    }
}
