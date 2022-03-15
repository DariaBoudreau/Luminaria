using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBridge : MonoBehaviour
{
    [SerializeField] PlayerController aspenObject;
    [SerializeField] float timeToWait;
    private Collider2D col;
    void Start()
    {
        col = GetComponent<Collider2D>();
    }   

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            StartCoroutine(DelayDestroy(timeToWait));
        }
    }

    IEnumerator DelayDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
        
    }
}
