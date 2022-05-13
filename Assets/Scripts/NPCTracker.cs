using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTracker : MonoBehaviour
{
    [SerializeField] int NPCsNeeded;
    public int NPCsRescued;
    private Collider2D barrier;
    void Start()
    {
        barrier = GetComponent<Collider2D>();
    }
    void Update()
    {
        if(NPCsNeeded == NPCsRescued)
        {
            Destroy(barrier.gameObject);
        }
        else
        {
            //Display some text
        }
    }
}