using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Endpoint : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;
    [SerializeField] public CharacterController2D Aspen;
    [SerializeField] private string nextScene;
    private bool hasPlayedClip = false;
    private bool leave = false;
    AudioSource soundClip;

    public void Start()
    {
        soundClip = GetComponent<AudioSource>();
        //StartCoroutine(DelayedExitScene());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = false;
        }
    }

    private void Update()
    {
        if (triggerActive && Aspen.isBurning) {
            if (!hasPlayedClip) {
                soundClip.Play();
                transform.GetChild(2).gameObject.SetActive(true);
                hasPlayedClip = true;
                leave = true;
            }
        }
    }

    private IEnumerator DelayedExitScene()
    {
        while(true) {
            yield return new WaitForSeconds(2);
            if (leave) {
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
