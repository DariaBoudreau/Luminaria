using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    //[SerializeField] CharacterController2D aspenObject;
    [SerializeField] PlayerController aspenObject;
    [SerializeField] PlayerCharging aspenCharging;
    [SerializeField] float wetTime;
    private Collider2D col;
    void Start()
    {
        col = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            aspenCharging.isInWater = true;
            aspenObject.isWet = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            aspenCharging.isInWater = false;
            StartCoroutine(Waterlog(wetTime));
        }
    }
    IEnumerator Waterlog(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(!aspenCharging.isInWater)
        {
            aspenObject.isWet = false;
        }
    }
}
