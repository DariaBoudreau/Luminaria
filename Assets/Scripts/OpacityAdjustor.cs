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
    private float elapsedTime;

    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }

    // MoveTowards called in update, lerp called as coroutine
    void Update()
    {
        // Uncomment for MoveTowards option
        // FadeAlpha();
    }

    // If using MoveTowards option, remove elapsed time reset and coroutine start
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            shouldFade = true;
            elapsedTime = 0;
            StartCoroutine(FadeToAlpha());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Aspen"))
        {
            shouldFade = false;
            elapsedTime = 0;
            StartCoroutine(FadeToAlpha());
        }
    }

    // MoveTowards option
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

    // Traditional Lerp option
    IEnumerator FadeToAlpha()
    {
        Color tempColor = sprite.color;
        float targetAlpha;

        if (shouldFade)
            targetAlpha = fadedAlpha;
        else
            targetAlpha = opaqueAlpha;

        while (sprite.color.a != targetAlpha)
        {
            elapsedTime += fadeSpeed * Time.deltaTime;

            tempColor.a = Mathf.Lerp(tempColor.a, targetAlpha, elapsedTime);
            sprite.color = tempColor;

            yield return null;
        }

        yield return null;
    }
}
