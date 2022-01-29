using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioForPickup : MonoBehaviour
{
    [Header("Aspen")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip stonePickup;
    [SerializeField] AudioClip orbPickup;

    void Start()
    {
        audioSource.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            if (gameObject.CompareTag("KeyStone"))
            {
                audioSource.PlayOneShot(stonePickup);
                
            }
           if(gameObject.CompareTag("LightOrb"))
            {
                audioSource.PlayOneShot(orbPickup);
            }

        }

    }
    
}
