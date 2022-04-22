using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneHUD : MonoBehaviour
{
    [SerializeField] Image stone1_image;
    [SerializeField] Image stone2_image;
    [SerializeField] Image stone3_image;
    [SerializeField] Image stone4_image;

    [SerializeField] Image stone1_circle;
    [SerializeField] Image stone2_circle;
    [SerializeField] Image stone3_circle;
    [SerializeField] Image stone4_circle;

    [SerializeField] float stoneTweenTime = 5f;
    [SerializeField] float stoneTweenScale;

    // Booleans for showing the stones
    bool stoneOneCollected, stoneTwoCollected, stoneThreeCollected, stoneFourCollected;

    // Booleans for showing the frames
    bool showFrameOne, showFrameTwo, showFrameThree, showFrameFour;

    // Booleans for tweening
    bool tweenStoneOne, tweenStoneTwo, tweenStoneThree, tweenStoneFour;


    void Start()
    {
        HideAllImages();
    }

    void HideAllImages()
    {
        stone1_image.enabled = false;
        stone2_image.enabled = false;
        stone3_image.enabled = false;
        stone4_image.enabled = false;
        stone1_circle.enabled = false;
        stone2_circle.enabled = false;
        stone3_circle.enabled = false;
        stone4_circle.enabled = false;
        stoneOneCollected = false;
        stoneTwoCollected = false;
        stoneThreeCollected = false;
        stoneFourCollected = false;
        showFrameOne = false;
        showFrameTwo = false;
        showFrameThree = false;
        showFrameFour = false;
        tweenStoneOne = true;
        tweenStoneTwo = true;
        tweenStoneThree = true;
        tweenStoneFour = true;
    }

    void UpdateStones()
    {
        StoneCheck();
        // Starts out as false (so image is off and turns on when is collected)
        stone1_image.enabled = stoneOneCollected;
        if (stoneOneCollected && tweenStoneOne)
        {
            TweenStone(stone1_image);
            tweenStoneOne = false;
        }

        stone2_image.enabled = stoneTwoCollected;
        if (stoneOneCollected && tweenStoneTwo)
        {
            TweenStone(stone2_image);
            tweenStoneTwo = false;
        }

        stone3_image.enabled = stoneThreeCollected;
        if (stoneOneCollected && tweenStoneThree)
        {
            TweenStone(stone3_image);
            tweenStoneThree = false;
        }

        stone4_image.enabled = stoneFourCollected;
        if (stoneOneCollected && tweenStoneFour)
        {
            TweenStone(stone4_image);
            tweenStoneFour = false;
        }

        // Starts out as false and becomes true when first stone is collected
        stone1_circle.enabled = showFrameOne;
        stone2_circle.enabled = showFrameTwo;
        stone3_circle.enabled = showFrameThree;
        stone4_circle.enabled = showFrameFour;
    }

    public void TurnOnStoneOne()
    {
        stoneOneCollected = true;
        TurnOnFrames();
        UpdateStones();
    }

    public void TurnOnStoneTwo()
    {
        stoneTwoCollected = true;
        TurnOnFrames();
        UpdateStones();
    }

    public void TurnOnStoneThree()
    {
        stoneThreeCollected = true;
        TurnOnFrames();
        UpdateStones();
    }

    public void TurnOnStoneFour()
    {
        stoneFourCollected = true;
        TurnOnFrames();
        UpdateStones();
    }

    void TurnOnFrames()
    {
        showFrameOne = true;
        showFrameTwo = true;
        showFrameThree = true;
        showFrameFour = true;
    }
    
    void StoneCheck()
    {
        // If both collected and frame are true, it means the stone is collected but the frame is also visible
        // So then make it invisible with false

        if (stoneOneCollected && showFrameOne)
        {
            showFrameOne = false;
        }

        if (stoneTwoCollected && showFrameTwo)
        {
            showFrameTwo = false;
        }

        if (stoneThreeCollected && showFrameThree)
        {
            showFrameThree = false;
        }

        if (stoneFourCollected && showFrameFour)
        {
            showFrameFour = false;
        }
    }

    void TweenStone(Image stone)
    {
        GameObject stoneobject = stone.gameObject;
        LeanTween.cancel(stoneobject);
        stone.transform.localScale = Vector3.one;

        LeanTween.scale(stoneobject, Vector3.one * stoneTweenScale, stoneTweenTime).setEasePunch();
    }
}
