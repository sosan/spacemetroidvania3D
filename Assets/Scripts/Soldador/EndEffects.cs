using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEffects : MonoBehaviour {

    public Light pointLight;
    public float pointLightRange = 10f;
    public float pointLightIntensity = 1f;
    public Color finalColor;
    public AnimationCurve progressCurve;
    public AnimationCurve impactCurve;
    public BeamLine getEndPointPositionFrom;
    public ParticleSystem[] emittingParticleSystems;
    public GameObject[] scalingComponents;
    public bool emit = false;

    private bool currentEmit = false;
    private Vector3 endPointPosition;
    private float globalProgress;
    private float globalResultProgress;
    private float globalImpactProgress;
    private float globalImpactResultProgress;
    private float resultProgress;
    private Vector3[] initialLocalScale;

    private void Start () 
    {
        SetEmission();
        endPointPosition = getEndPointPositionFrom.GetEndPointPosition();

        initialLocalScale = new Vector3[scalingComponents.Length];
        for (int i = 0; i < scalingComponents.Length; i++)
        {
            initialLocalScale[i] = scalingComponents[i].transform.localScale;        }

    }


    private void Update () 
    {

        endPointPosition = getEndPointPositionFrom.GetEndPointPosition();
        //print("endposiciont=" + endPointPosition);
        this.gameObject.transform.position = endPointPosition;

        if (currentEmit != emit)
        {
            SetEmission();
        }

        currentEmit = emit;

        globalImpactResultProgress = impactCurve.Evaluate(globalImpactProgress);
        if (globalImpactResultProgress == 0f)
        {
            globalImpactResultProgress = 0.001f;
        }

        globalResultProgress = progressCurve.Evaluate(globalProgress);
        resultProgress = globalImpactResultProgress + globalResultProgress;

        for (int i = 0; i < scalingComponents.Length; i++)
        {
            scalingComponents[i].transform.localScale = initialLocalScale[i] * resultProgress;
            if (resultProgress < 0.01f)
            {
                scalingComponents[i].gameObject.SetActive(false);
            }
            else
            {
                scalingComponents[i].gameObject.SetActive(true);
            }
        }

        if (pointLight != null)
        {
            pointLight.color = finalColor;
            pointLight.range = transform.lossyScale.x * pointLightRange;
            pointLight.intensity = resultProgress * pointLightIntensity;
        }

    }


    public void SetFinalColor(Color col)
    {
        finalColor = col;
    }

    private void SetEmission()
    {

        for (ushort i = 0; i < emittingParticleSystems.Length; i++)
        { 
            ParticleSystem.EmissionModule em = emittingParticleSystems[i].emission;
            em.enabled = emit;
        
        }

    }

    public void SetGlobalProgress(float gp)
    {
        globalProgress = gp;
    }

    public void SetGlobalImpactProgress(float gp)
    {
        globalImpactProgress = gp;
    }

    
}
