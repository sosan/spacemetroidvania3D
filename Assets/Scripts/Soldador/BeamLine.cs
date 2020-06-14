using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLine : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;

    [SerializeField] public float maxLength = 1.0f;

    [SerializeField] private AnimationCurve shaderProgressCurve;
    [SerializeField] private AnimationCurve shaderImpactCurve;
    [SerializeField] private float moveHitToSource = 0.5f;
    [SerializeField] private int particleMeshLength = 1;
    [SerializeField] private bool scalingWithSize = true;
    [SerializeField] private float finalSize = 1f;

    private float preSize;
    private float HitLength;
    private ParticleSystem particle;
    private ParticleSystemRenderer particleRenderer;
    private Vector3 positionForExplosion;
    private Vector3[] particleSpawnPositions;
    private Vector3 endPoint;
    private float globalProgress;
    private float globalimpactProgress;
    private ParticleSystem.Particle[] particles;
    private int positionArrayLength;
    private bool tempFix = false;

    private RaycastHit[] resultsHit = new RaycastHit[10];

    private void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
        particleRenderer = this.GetComponent<ParticleSystemRenderer>();
        HitLength = 0;
        LaserCastRay();
        LaserControl();
        UpdateLaserParts();
        tempFix = true;
    }

    private void Update()
    {
        if (scalingWithSize == true)
        {
            finalSize = gameObject.transform.lossyScale.x;
        }
        LaserCastRay();
        LaserControl();
        if (positionArrayLength != particles.Length || preSize != finalSize)
        {
            UpdateLaserParts();
        }
        preSize = finalSize;
    }

    void LaserControl() 
    {
        float progress = shaderProgressCurve.Evaluate(globalProgress);
        particleRenderer.material.SetFloat("_Progress", progress);
        float impactProgress = shaderImpactCurve.Evaluate(globalimpactProgress);
        particleRenderer.material.SetFloat("_ImpactProgress", impactProgress);
        particleRenderer.material.SetVector("_StartPosition", transform.position);
        particleRenderer.material.SetVector("_EndPosition", endPoint);
        particleRenderer.material.SetFloat("_Distance", HitLength);
        particleRenderer.material.SetFloat("_MaxDist", HitLength);
        particleRenderer.material.SetFloat("_FinalSize", finalSize);
    }

    void LaserCastRay()
    {

        if (1 <= Physics.RaycastNonAlloc(this.transform.position, this.transform.right, resultsHit, maxLength, layerMask))
        {

            for (ushort i = 0; i < resultsHit.Length; i++)
            { 

                if (resultsHit[i].transform != null)
                {
                     
                    HitLength = resultsHit[i].distance;
                    positionForExplosion = Vector3.MoveTowards(resultsHit[i].point, transform.position, moveHitToSource);
                    positionArrayLength = Mathf.RoundToInt(resultsHit[i].distance / (particleMeshLength * finalSize));
                    if (positionArrayLength < resultsHit[i].distance)
                    {
                        positionArrayLength += 1;
                    }
                    particleSpawnPositions = new Vector3[positionArrayLength];
                    endPoint = resultsHit[i].point;

                    break;
                }
            }

            
        }
        else
        {
            HitLength = maxLength;
            positionArrayLength = Mathf.RoundToInt(maxLength / (particleMeshLength * finalSize));
            if (positionArrayLength < maxLength)
            {
                positionArrayLength += 1;
            }
            particleSpawnPositions = new Vector3[positionArrayLength];
            endPoint = Vector3.MoveTowards(transform.position, transform.right * 1000f, maxLength);
            positionForExplosion = endPoint;
        }
    }

    

    private void OnEnable()
    {
        if (tempFix == true)
        {
            UpdateLaserParts();
        }        
    }

    private void UpdateLaserParts()
    {
        particles = new ParticleSystem.Particle[positionArrayLength];

        for (int i = 0; i < positionArrayLength; i++)
        {

            
            float x = i*particleMeshLength * finalSize;
            //print("x=" + x);
            particleSpawnPositions[i] = new Vector3(0f, 0f, 0f) + new Vector3(x, 0f, 0f);
            particles[i].position = particleSpawnPositions[i];
            particles[i].rotation = -90f;
            particles[i].startSize = finalSize;
            particles[i].startColor = new Color(1f, 1f, 1f);
        }

        particle.SetParticles(particles, particles.Length);

    }

    public void SetGlobalProgress(float gp)
    {
        globalProgress = gp;
    }

    public Vector3 GetEndPointPosition()
    {
        return positionForExplosion;
    }

    public void SetGlobalImpactProgress(float gp)
    {
        globalimpactProgress = gp;
    }

  
}
