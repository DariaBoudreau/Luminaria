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

    [Header("Typewriter Settings")]
    [Tooltip("Use this to set the dialogue IF it is NOT already set in child")]
    [SerializeField]
    private string[] lines;

    [Tooltip("If true, dialogue will change when left mouse/shift is pressed, if false will be time based")]
    [SerializeField]
    private bool shouldChangeOnButtonPress = true;

    [Tooltip("Only affects if it should change on button press, if true, text loops if player keeps clicking")]
    [SerializeField]
    private bool shouldLoop = false;

    [Tooltip("Set speed of multiline text")]
    [SerializeField]
    private float textSpeed;

    // In a time based dialogue box, the amount of time after the last character has printed before switching to the next line
    private float waitSpeed = 1f;

    private int index;
    private bool aspenIsNear = false;
    private bool textNullOrEmptyAtStart = false;

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

        textNullOrEmptyAtStart = string.IsNullOrEmpty(text.text);
    }

    // Update is called once per frame
    private void Update()
    {
        if (aspenIsNear && textNullOrEmptyAtStart)
        {
                if (shouldChangeOnButtonPress)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        if (text.text == lines[index])
                        {
                            NextLine();
                        }
                        else
                        {
                            StopAllCoroutines();
                            text.text = lines[index];
                        }
                    }
                }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aspen"))
        {
            fading = StartCoroutine(FadeIn());
            aspenIsNear = true;
            //If the text to display is not preset in the TMP text then we will set it from the lines field in the inspector
            if (textNullOrEmptyAtStart)
            {
                text.text = string.Empty;
                text.alignment = TextAlignmentOptions.TopLeft;
                StartDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aspen"))
        {
            fading = StartCoroutine(FadeOut());
            aspenIsNear = false;
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

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        if (!shouldChangeOnButtonPress)
        {
            StartCoroutine(TriggerNextLineBasedOnTime());
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (shouldLoop && shouldChangeOnButtonPress || !shouldChangeOnButtonPress)
            {
                index = 0;
            }
            if (shouldLoop && shouldChangeOnButtonPress)
            {
                text.text = string.Empty;
                StartCoroutine(TypeLine());
            }
        }
    }

    IEnumerator TriggerNextLineBasedOnTime()
    {
        yield return new WaitForSeconds(waitSpeed);
        NextLine();
    }
}
