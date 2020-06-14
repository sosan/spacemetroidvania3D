using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEffects : MonoBehaviour
{

    public Light pointLight;
    public float pointLightRange = 10f;
    public float pointLightIntensity = 1f;
    public ParticleSystem[] scalingParticleSystems;
    public ParticleSystem[] emittingParticleSystems;
    public bool emit = false;
    public AnimationCurve progressCurve;
    public AnimationCurve impactCurve;
    public Color finalColor;

    private float globalProgress;
    private float globalResultProgress;
    private float globalImpactProgress;
    private float globalImpactResultProgress;
    private float resultProgress;
    private Vector3[] initialLocalScale;

    private void Start()
    {
        initialLocalScale = new Vector3[scalingParticleSystems.Length];
        for (int i = 0; i < scalingParticleSystems.Length; i++)
        {
            initialLocalScale[i] = scalingParticleSystems[i].transform.localScale;
        }
    }

    private void Update()
    {

        if (resultProgress > 0.9f)
        {
            emit = true;
        }
        else
        {
            emit = false;
        }

        globalImpactResultProgress = impactCurve.Evaluate(globalImpactProgress);
        if (globalImpactResultProgress == 0f)
        {
            globalImpactResultProgress = 0.001f;
        }

        globalResultProgress = progressCurve.Evaluate(globalProgress);
        resultProgress = globalImpactResultProgress + globalResultProgress;


        for (int i = 0; i < scalingParticleSystems.Length; i++)
        {
            scalingParticleSystems[i].transform.localScale = initialLocalScale[i] * resultProgress;

            if (resultProgress < 0.01f)
            {
                scalingParticleSystems[i].gameObject.SetActive(false);
            }
            else
            {
                scalingParticleSystems[i].gameObject.SetActive(true);
            }
        }

        if (emit == true)
        {

            for(ushort i = 0; i < emittingParticleSystems.Length; i++)
            { 
                var em = emittingParticleSystems[i].emission;
                em.enabled = true; 
            
            }

        }
        else
        {
            for(ushort i = 0; i < emittingParticleSystems.Length; i++)
            { 
                var em = emittingParticleSystems[i].emission;
                em.enabled = false; 
            
            }
            
        }

        pointLight.color = finalColor;
        pointLight.range = transform.lossyScale.x * pointLightRange;
        pointLight.intensity = resultProgress * pointLightIntensity;

    }

    public void SetGlobalProgress(float gp)
    {
        globalProgress = gp;
    }

    public void SetFinalColor(Color col)
    {
        finalColor = col;
    }

    public void SetGlobalImpactProgress(float gp)
    {
        globalImpactProgress = gp;
    }

    
}
