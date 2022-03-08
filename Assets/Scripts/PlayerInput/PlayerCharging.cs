using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerCharging : MonoBehaviour
{
    [Header("Values")]
    public int currentCharge;
    public int maxCharge = 3;
    private Light2D lt;
    private ParticleSystem ps;
    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule em;
    private float prevCharge;

    public bool isBurning;
    public bool hasTransitioned;

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>(true);
        lt = ps.gameObject.transform.parent.GetComponentInChildren<Light2D>(true);
        main = ps.main;
        em = ps.emission;
        currentCharge = 0;
        prevCharge = -1;
    }

    private void FixedUpdate()
    {
        
    }
}
