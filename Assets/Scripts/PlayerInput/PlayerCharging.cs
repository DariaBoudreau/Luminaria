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

    // Pretty much everything here is the same as Miles' original code

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
        if (prevCharge != currentCharge)
            UpdateCharge();
    }

    private float bounds(float n, int lower, int upper)
    {
        if (n < lower) return lower;
        if (n > upper) return upper;
        return n;
    }

    public void UpdateCharge()
    {
        currentCharge = (int)bounds(currentCharge, 0, maxCharge);

        main.maxParticles = 7 * currentCharge;
        em.rateOverTime = 3 * currentCharge;

        if (prevCharge < currentCharge)
        {
            StartCoroutine(FadeUp());
        }
        else if (prevCharge > currentCharge)
        {
            StartCoroutine(FadeDown());
        }

        prevCharge = (float)currentCharge;
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
