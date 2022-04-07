using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;

public class CutsceneSkip : MonoBehaviour
{
    [SerializeField]
    VideoPlayer video;

    [SerializeField]
    PlayableDirector playableDirector;

    bool keyPressed = false;
    // should set it to the 3949th frame of the timeline, which is around the 65.5 second mark
    double timeToSet = 65.5;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !keyPressed)
        {
            keyPressed = true;
            SkipCutscene();
        }
    }

    void SkipCutscene()
    {
        playableDirector.time = timeToSet;
        video.Stop();
    }
}
