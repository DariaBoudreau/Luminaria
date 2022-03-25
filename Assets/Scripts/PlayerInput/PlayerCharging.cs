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

    public int chargeChange;

    public bool isBurning;
    public bool hasTransitioned;
    public bool isInWater;

    // Pretty much everything here is the same as Miles' original code

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>(true);
        lt = ps.gameObject.transform.parent.GetComponentInChildren<Light2D>(true);
        main = ps.main;
        em = ps.emission;
        //currentCharge = maxCharge;
        prevCharge = -1;
    }

    private void FixedUpdate()
    {
        
    }

    private float bounds(float n, int lower, int upper)
    {
        if (n < lower) return lower;
        if (n > upper) return upper;
        return n;
    }

    public void StartBurning()
    {
        isBurning = true;
    }

    public void StopBurning()
    {
        isBurning = false;
    }

    public void SpendCharge()
    {
        if (chargeChange >= 1)
        {
            StartCoroutine(FadeUp());
        }

        if (chargeChange <= -1)
        {
            StartCoroutine(FadeDown());
        }

        currentCharge -= chargeChange;
        UpdateParticles();
        Debug.Log(currentCharge);
    }

    public void UpdateParticles()
    {
        main.maxParticles = 7 * currentCharge;
        em.rateOverTime = 3 * currentCharge;
    }

    private IEnumerator FadeUp()
    {
        for (float ft = prevCharge; ft < currentCharge; ft += 0.1f)
        {
            main.simulationSpeed = ft + 1;
            lt.intensity = ft;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator FadeDown()
    {
        for (float ft = prevCharge; ft >= currentCharge; ft -= 0.1f)
        {
            main.simulationSpeed = ft + 1;
            lt.intensity = ft;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
