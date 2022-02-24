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

    [SerializeField] Image stone2_circle;
    [SerializeField] Image stone3_circle;
    [SerializeField] Image stone4_circle;

    void Start()
    {
        stone1_image.enabled = false;
        stone2_image.enabled = false;
        stone3_image.enabled = false;
        stone4_image.enabled = false;
        stone2_circle.enabled = false;
        stone3_circle.enabled = false;
        stone4_circle.enabled = false;
    }

    public void TurnOnStoneOne()
    {
        stone1_image.enabled = true;
        stone2_circle.enabled = true;
        stone3_circle.enabled = true;
        stone4_circle.enabled = true;
    }

    public void TurnOnStoneTwo()
    {
        stone2_image.enabled = true;
        stone2_circle.enabled = false;
    }

    public void TurnOnStoneThree()
    {
        stone3_image.enabled = true;
        stone3_circle.enabled = false;
    }

    public void TurnOnStoneFour()
    {
        stone4_image.enabled = true;
        stone4_circle.enabled = false;
    }
}
