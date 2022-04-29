using UnityEngine;

public class Charging : MonoBehaviour
{
    [SerializeField] PlayerCharging Aspen;
    [SerializeField] bool triggerActive = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip usedStation;
    private void Start()
    {
        audioSource.GetComponent<AudioSource>();
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
        if (triggerActive && Aspen.isBurning && audioSource.isPlaying == false)
        {
            Aspen.currentCharge = Aspen.maxCharge;
            audioSource.PlayOneShot(usedStation);
        }
    }
}
