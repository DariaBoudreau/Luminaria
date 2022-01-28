using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;

public class Burnables : MonoBehaviour
{
    [SerializeField] CharacterController2D Aspen;
    [SerializeField] bool triggerActive = false;
    [SerializeField] int chargeCost = 1;
    [SerializeField] int psRenderLayer = 26;
    [Header("Hidden by it")]
    [SerializeField] GameObject thingToHide;
    [SerializeField] string nameOfIt;
    private bool hasBurned = false;
    private ParticleSystem ps;
    private Light2D lt;
    private Collider2D barrier;
    private AudioSource soundClip;
    private GameObject fire;

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>(true);
        lt = GetComponentInChildren<Light2D>(true);             // ambient light
        fire = ps.gameObject.transform.parent.gameObject;       // fire element
        soundClip = GetComponent<AudioSource>();
        if (!gameObject.CompareTag("HidingSomething"))
        {
            barrier = GetComponentInChildren<Collider2D>();
        }
        else
        {
            thingToHide = GameObject.Find(nameOfIt);
            thingToHide.GetComponent<Collider2D>().enabled = false;
        }
        // lt.gameObject.SetActive(false);                         // make sure is off before burning
        fire.SetActive(false);
        ps.GetComponent<Renderer>().sortingLayerName = "Foreground";
        ps.GetComponent<Renderer>().sortingOrder = psRenderLayer;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = false;
        }
    }

    void Update()
    {
        if (triggerActive && Aspen.isBurning) {
            if (Aspen.currentCharge != 0) {
                if (!hasBurned) {
                    hasBurned = true;
                    soundClip.Play();
                    StartCoroutine(IsBurning());

                    fire.SetActive(true);
                    ps.Play();
                    lt.gameObject.SetActive(true);
                    Aspen.currentCharge -= chargeCost;
                }
            } else {
                // NOT ENOUGH CHARGE
            }
        }
    }

    private IEnumerator IsBurning()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a = ft;
            GetComponent<SpriteRenderer>().color = c;

            lt.intensity = 3*ft;

            yield return new WaitForSeconds(0.1f);
        }

        ps.Stop();

        Color end = GetComponent<SpriteRenderer>().color;
        end.a = 0f;
        GetComponent<SpriteRenderer>().color = end;
        Destroy(lt.gameObject);
        if (!gameObject.CompareTag("HidingSomething"))
        {
            Destroy(barrier.gameObject);
        }
        else
        {
            thingToHide.GetComponent<Collider2D>().enabled = true;
        }
    }
}
