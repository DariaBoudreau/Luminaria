using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour
{
    public void OpenSurvey()
    {
        Application.OpenURL("https://forms.gle/azmkaVzwbUDrXhiw7");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Application.Quit();
    }
}
