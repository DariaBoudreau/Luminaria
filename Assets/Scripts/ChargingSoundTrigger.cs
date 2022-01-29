using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingSoundTrigger : MonoBehaviour
{
    [SerializeField] CharacterController2D Aspen;
    [SerializeField] bool triggerActive = false;
    [SerializeField] AudioSource audioSource;
    

    private void Start()
    {
        audioSource.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            triggerActive = true;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
