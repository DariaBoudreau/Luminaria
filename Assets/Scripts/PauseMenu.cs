using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static GameObject menuPanel;
    public static GameObject keyGuidePanel;
    // Start is called before the first frame update
    void Start()
    {
        menuPanel = GameObject.Find("PauseMenuPanel");
        keyGuidePanel = GameObject.Find("KeyPanel");
        menuPanel.SetActive(false);
        keyGuidePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuPanel.activeInHierarchy && !keyGuidePanel.activeInHierarchy)
            {
                menuPanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                menuPanel.SetActive(false);
            }
        }

    }
}
