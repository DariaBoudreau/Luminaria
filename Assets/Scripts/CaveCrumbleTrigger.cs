using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCrumbleTrigger : MonoBehaviour
{
    [SerializeField] CharacterController2D Aspen;
    [SerializeField] bool triggerActive = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip rockCrumbles;


    private void Start()
    {
        audioSource.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = true;
            audioSource.PlayOneShot(rockCrumbles);

        }
    }
}
