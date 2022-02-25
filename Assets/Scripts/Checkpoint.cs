using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;
    [SerializeField] public CharacterController2D Aspen;
    [SerializeField] private GameObject checkpointLight;
    private GameMaster gameMaster;
    private int xOffset = -2;
    AudioSource soundClip;

    public void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        soundClip = GetComponent<AudioSource>();
        if (gameMaster.lastCheckpointPosition == this.transform.position)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            Aspen.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, Aspen.transform.position.z);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        triggerActive = true;
        if (other.gameObject.CompareTag("Aspen") && triggerActive == true) 
        {
                if (!checkpointLight.activeInHierarchy)
                {
                    checkpointLight.SetActive(true);
                    gameMaster.lastCheckpointPosition = transform.position;
                    soundClip.Play();
                }
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

    }
}
