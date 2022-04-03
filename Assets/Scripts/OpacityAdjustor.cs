using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityAdjustor : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadedAlpha;
    [SerializeField] float opaqueAlpha;

    private SpriteRenderer sprite;
    private bool shouldFade;

    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FadeAlpha();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            shouldFade = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            shouldFade = false;
        }
    }

    void FadeAlpha()
    {
        float step = fadeSpeed * Time.deltaTime;
        Color tempColor = sprite.color;

        if(shouldFade)
            tempColor.a = Mathf.MoveTowards(tempColor.a, fadedAlpha, step);
        else
            tempColor.a = Mathf.MoveTowards(tempColor.a, opaqueAlpha, step);

        sprite.color = tempColor;
    }
}
