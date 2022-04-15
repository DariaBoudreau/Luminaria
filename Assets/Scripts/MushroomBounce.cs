using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBounce : MonoBehaviour
{
    private SpriteRenderer sr;
    bool isDown;
    [SerializeField] Vector3 downScaleVector;
    [SerializeField] Vector3 bounceScaleVector;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        isDown = false;
    }

    // when player lands on the mushroom, it squishes and goes down
    // when player jumps off the mushroom, it goes back up a little above normal height then back down to normal (more dynamic bounce)

    // methods to write :
    // - making mushroom go down
    // - making mushroom go back up (with little bounce)
    // - on collider enter? on collider exit? 

    // possible issues
    // has to change both sprite scale and position at once maybe?
    // edge collider is a child of the main object, unsure how to access that in code

    private void GoDown()
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, Vector3.one, 2).setEasePunch();
        //use transform local scale
        //Debug.Log("Landed");
        //this.transform.localScale = downScaleVector;
        isDown = true;
    }

    private void BounceUp()
    {
        //Debug.Log("Bouncing");
        this.transform.localScale = bounceScaleVector;
        isDown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // not even sure if this if statement is needed, putting it here just in case tho
        if (collision.gameObject.tag == "Aspen" && !isDown)
        {
            GoDown();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BounceUp();
    }
}
