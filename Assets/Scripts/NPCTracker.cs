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
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            if(NPCsNeeded == NPCsRescued)
            {
                Destroy(barrier);
            }
            else
            {
                //Display some text
            }
        }
    }
}
