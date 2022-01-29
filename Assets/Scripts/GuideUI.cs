using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private GameObject ThingToGuide;
    [SerializeField] bool destroyOnFadeOut;
    private Coroutine fading;
    private SpriteRenderer[] spriteRenderers;
    private TMP_Text text;

    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        text = GetComponentInChildren<TMP_Text>();

        foreach (SpriteRenderer sr in spriteRenderers) {
            Color c = sr.color;
            c.a = 0;
            sr.color = c;
        }

        Color c_text = text.color;
        c_text.a = 0;
        text.color = c_text;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Aspen")
        {
            fading = StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Aspen")
        {
            fading = StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        if (fading != null)     // currently fading
            StopCoroutine(fading);

        float start = text.color.a;

        for (float ft = start; ft < 1f; ft += 0.1f)
        {
            foreach (SpriteRenderer sr in spriteRenderers) {
                Color c = sr.color;
                c.a = ft;
                sr.color = c;
            }

            Color c_text = text.color;
            c_text.a = ft;
            text.color = c_text;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeOut()
    {
        if (fading != null)     // currently fading
            StopCoroutine(fading);

        float start = text.color.a;

        for (float ft = start; ft >= 0f; ft -= 0.1f)
        {
            foreach (SpriteRenderer sr in spriteRenderers) {
                Color c = sr.color;
                c.a = ft;
                sr.color = c;
            }

            Color c_text = text.color;
            c_text.a = ft;
            text.color = c_text;

            yield return new WaitForSeconds(0.02f);
        }

        if (destroyOnFadeOut)
            Destroy(gameObject);
    }
}
