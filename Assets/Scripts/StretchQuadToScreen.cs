using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchQuadToScreen : MonoBehaviour
{
    float width;
    float height;

    // Start is called before the first frame update
    void Start()
    {
        width = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        height = Camera.main.orthographicSize * 2f;
        this.transform.localScale = new Vector3(width, height, 1f);
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 1f));
    }
}
