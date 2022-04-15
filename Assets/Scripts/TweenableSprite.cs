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
        //Debug.Log("Tween");
        //TweenSprite();
        //if (canTween)
        //{
        //    TweenSprite();
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTween)
        {
            TweenSprite();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canTween = true;
    }

    void TweenResetTimer()
    {
        float currentTime = 0;

        while (currentTime < timer)
        {
            //Debug.Log(currentTime);
            currentTime += Time.deltaTime;
        }

        canTween = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //TweenResetTimer();
    }

    void TweenSprite()
    {
        LeanTween.cancel(gameObject);
        gameObject.transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * spriteTweenScale, spriteTweenTime).setEasePunch();
        canTween = false;
    }
}
