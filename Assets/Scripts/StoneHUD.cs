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
    [SerializeField] PickUps stone1;
    [SerializeField] PickUps stone2;
    [SerializeField] PickUps stone3;
    [SerializeField] PickUps stone4;

    void Update()
    {
        stone1_image.enabled = stone1.isCollected;
        stone2_image.enabled = stone2.isCollected;
        stone3_image.enabled = stone3.isCollected;
        stone4_image.enabled = stone4.isCollected;
    }
}
