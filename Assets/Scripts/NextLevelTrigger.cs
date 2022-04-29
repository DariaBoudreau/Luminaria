using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [Tooltip("Does the player need to press shift to use?")]
    [SerializeField]
    private bool playerMustPressKey = false;

    [Tooltip("The exact name of the scene to go to.")]
    [SerializeField] private string nextScene;

    [Tooltip("The object that will load the next level.")]
    [SerializeField]
    private SceneTransitionLevelLoader levelLoader;

    [Tooltip("Optional audio source.")]
    [SerializeField] AudioSource audioSource;

    [Tooltip("Optional audio clip.")]
    [SerializeField] AudioClip sfxPortal;

    [Tooltip("The time to wait before going to the desired scene.")]
    [SerializeField]
    float waitTime = 2;

    bool sfxHasPlayed = false;




    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aspen"))
        {
            if (playerMustPressKey && Input.GetKeyDown(KeyCode.LeftShift) || !playerMustPressKey)
            {
                //StartCoroutine(DelayedExitScene());
                levelLoader.LoadNextLevel();
                if (!sfxHasPlayed && audioSource != null && sfxPortal != null)
                {
                    audioSource.PlayOneShot(sfxPortal);
                    sfxHasPlayed = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aspen"))
        {
            if (playerMustPressKey && Input.GetKeyDown(KeyCode.LeftShift))
            {
                //StartCoroutine(DelayedExitScene());
                levelLoader.LoadNextLevel();
                if (!sfxHasPlayed && audioSource != null && sfxPortal != null)
                {
                    audioSource.PlayOneShot(sfxPortal);
                    sfxHasPlayed = true;
                }
            }
        }
    }

    private IEnumerator DelayedExitScene()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(nextScene);
    }
}
