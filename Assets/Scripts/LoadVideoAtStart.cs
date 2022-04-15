using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadVideoAtStart : MonoBehaviour
{

    private void Awake()
    {
        this.GetComponent<VideoPlayer>().targetTexture.Create();
        if (this.GetComponent<VideoPlayer>().targetTexture == null)
        {
            //print("u");
        }
    }

    private void OnDestroy()
    {
        this.GetComponent<VideoPlayer>().targetTexture.Release();
    }
}
