using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityAdjustor : MonoBehaviour
{
    private SpriteRenderer sprite;
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            Color temp = sprite.color;
            temp.a = 0f;
            sprite.color = temp;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            Color temp = sprite.color;
            temp.a = 1f;
            sprite.color = temp;
        }
    }
}
