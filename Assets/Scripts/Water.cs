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
    private SpriteRenderer sprite;
    private Color trueColor;
    [Header("The specific color of blue Aspen turns")]
    [SerializeField] Color blue = new Color(61f,128f,200f,255f);
    void Start()
    {
        col = GetComponent<Collider2D>();
        sprite = aspenObject.GetComponent<SpriteRenderer>();
        trueColor = sprite.color;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            sprite.color = blue;
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
            sprite.color = trueColor;
        }
    }
}
