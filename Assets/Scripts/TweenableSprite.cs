using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenableSprite : MonoBehaviour
{
    [SerializeField] float spriteTweenTime = 5f;
    [SerializeField] float spriteTweenScale;
    [SerializeField] float timer;
    bool canTween = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Tween");

        if (canTween)
        {
            TweenSprite();
        }
    }

    void TweenResetTimer()
    {
        float currentTime = 0;

        while (currentTime < timer)
        {
            currentTime += Time.deltaTime;
        }

        currentTime = 0;
        canTween = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TweenResetTimer();
    }

    void TweenSprite()
    {
        LeanTween.cancel(gameObject);
        gameObject.transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * spriteTweenScale, spriteTweenTime).setEasePunch();
        canTween = false;
    }
}
