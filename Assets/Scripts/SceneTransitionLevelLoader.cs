using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionLevelLoader : MonoBehaviour
{
    [SerializeField]
    Animator transition;

    [SerializeField]
    float transitionTime = 1f;

    [SerializeField]
    bool shouldFadeIn = true;

    [SerializeField]
    string sceneToLoad;

    [SerializeField]
    Canvas transitionObject;

    private void Start()
    {
        if (!shouldFadeIn)
        {
            transitionObject.enabled = false;
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transitionObject.enabled = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
