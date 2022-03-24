using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    //[SerializeField] PlayerController aspenObject;
    [SerializeField] float wetTime;
    private Collider2D col;
    private IEnumerator coroutine;
    void Start()
    {
        col = GetComponent<Collider2D>();
        coroutine = Waterlog(wetTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            aspenObject.isWet = true;
/*             if(aspenObject.isWet)
            {
                StopCoroutine(coroutine);
            } */
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            StartCoroutine(coroutine);
        }
    }
    IEnumerator Waterlog(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        aspenObject.isWet = false;
    }
}
