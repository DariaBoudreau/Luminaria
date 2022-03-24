using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    //public CharacterController2D Aspen;
    public PlayerController Aspen;
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
            checkpointLight.gameObject.SetActive(true);
            Aspen.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, Aspen.transform.position.z);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
                if (!checkpointLight.activeInHierarchy)
                {
                    Aspen.GetComponent<Animator>().SetBool("Burn", true);
                    checkpointLight.SetActive(true);
                    gameMaster.lastCheckpointPosition = transform.position;
                    soundClip.Play();
                }
        }
    }
}
