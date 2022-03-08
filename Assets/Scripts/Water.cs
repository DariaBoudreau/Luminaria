using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    private Collider2D col;
    void Start()
    {
        col = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            aspenObject.isWet = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            StartCoroutine(Waterlog(10));
        }
    }

    IEnumerator Waterlog(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        aspenObject.isWet = false;
    }
}
