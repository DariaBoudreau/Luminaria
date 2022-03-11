using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndManger : MonoBehaviour
{
    [SerializeField] private GameObject ActivePortal;
    [SerializeField] private GameObject[] KeyStone;
    [SerializeField] Animator animator;
    public static int totalNumofStone;
    [SerializeField] private bool conditionClear = false;
    [SerializeField] private bool portalActivatedOnce = false;
    [SerializeField] private bool portalActiveDone = false;
    [SerializeField] private string nextScene;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sfxPortal;
    bool sfxHasPlayed = false;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.SetBool("PortalActivating", false);
        ActivePortal.SetActive(false);
        for (int i = 0; i < KeyStone.Length; i++)
        {
            totalNumofStone++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (totalNumofStone <= 0)
        {
            conditionClear = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aspen"))
        {
            if (conditionClear)
            {
                StartCoroutine(SetPortal());
                if (!sfxHasPlayed)
                {
                    audioSource.PlayOneShot(sfxPortal);
                    sfxHasPlayed = true;
                }
            }
        }
    }

    private IEnumerator SetPortal()
    {
        if (conditionClear && !portalActivatedOnce)
        {
            portalActivatedOnce = true;
            animator.SetBool("PortalActivating", true);
            yield return new WaitForSeconds(8);
            ActivePortal.SetActive(true);
            portalActiveDone = true;
            StartCoroutine(DelayedExitScene());
            
        }
    }

    private IEnumerator DelayedExitScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(nextScene);
    }
}
