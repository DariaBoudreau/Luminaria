using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit the Game");
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        PauseMenu.menuPanel.SetActive(false);
    }

    public void OpenKeys()
    {
        if (PauseMenu.menuPanel.activeInHierarchy)
        {
            PauseMenu.menuPanel.SetActive(false);
            PauseMenu.keyGuidePanel.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        if (PauseMenu.keyGuidePanel.activeInHierarchy)
        {
            PauseMenu.keyGuidePanel.SetActive(false);
        }

        PauseMenu.menuPanel.SetActive(true);
    }
}
