using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class StretchQuadToScreen : MonoBehaviour
{
    float width;
    float height;

    [SerializeField] VideoPlayer video;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material cutsceneMaterial;

    // Start is called before the first frame update
    void Start()
    {
        width = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        height = Camera.main.orthographicSize * 2f;
        this.transform.localScale = new Vector3(width, height, 1f);
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 1f));
        GetComponent<Renderer>().material = whiteMaterial;
        video.Prepare();
    }

    private void OnEnable()
    {
        video.prepareCompleted += StartCutscene;
    }

    private void OnDisable()
    {
        video.prepareCompleted -= StartCutscene;
    }

    void StartCutscene(VideoPlayer video)
    {
        if (video.targetTexture.IsCreated() == true)
        {
            video.Play();
            StartCoroutine(MaterialChangeCo());
        }
    }

    private IEnumerator MaterialChangeCo()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<Renderer>().material = cutsceneMaterial;
    }
}
