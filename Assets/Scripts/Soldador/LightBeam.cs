using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeam : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;

    public float maxLength = 32.0f;

    public AnimationCurve shaderProgressCurve;
    public AnimationCurve shaderImpactCurve;
    public float moveHitToSource = 0.5f;
    public int distanceBetweenLights = 1;
    public bool scalingWithSize = true;
    public float finalSize = 1f;
    public Light lightPrefab;
    public float lightRange = 5f;
    public float lightIntensity = 1f;
    public Color finalColor;
    public bool scaleDensityWithSize = false;



    private Color currentColor;
    private Vector3[] pointLightSpawnPositions;
    private float globalProgress;
    private float globalimpactProgress;
    private int positionArrayLenght;
    private Light[] lights;
    private int roundedMaxLength;

    private float progress;
    private float impactProgress;
    private float resultProgress;
    
    private RaycastHit[] resultsHit = new RaycastHit[25];

    private void Start()
    {
        roundedMaxLength = Mathf.RoundToInt(maxLength);
        CreateLights();
        LaserCastRay();
        ActivateLights();
        LaserControl();
        UpdateLaserParts();
    }


    private void Update()
    {
        if (scalingWithSize == true)
        {
            finalSize = gameObject.transform.lossyScale.x;
        }
        LaserCastRay();
        LaserControl();

        if (positionArrayLenght != lights.Length || currentColor != finalColor)
        {
            ActivateLights();
        }

        UpdateLaserParts();
        currentColor = finalColor;
    }

    private void LaserControl()
    {
        progress = shaderProgressCurve.Evaluate(globalProgress);
        impactProgress = shaderImpactCurve.Evaluate(globalimpactProgress);
        resultProgress = progress + impactProgress;
    }

    public void SetFinalColor(Color col)
    {
        finalColor = col;
    }

    private void LaserCastRay()
    {
        if (1 <= Physics.RaycastNonAlloc(this.transform.position, this.transform.right, resultsHit, maxLength, layerMask))
        {

            for (ushort i = 0; i < resultsHit.Length; i++)
            { 
                if (resultsHit[i].transform != null)
                { 
                    positionArrayLenght = Mathf.RoundToInt(resultsHit[i].distance / distanceBetweenLights);
                    pointLightSpawnPositions = new Vector3[positionArrayLenght];

                    break;
            
                }
                
            }

            
        }
        else
        {
            positionArrayLenght = Mathf.RoundToInt(maxLength / distanceBetweenLights);
            pointLightSpawnPositions = new Vector3[positionArrayLenght];
        }
    }

    private void CreateLights()
    {
        lights = new Light[roundedMaxLength];

        for (int i = 0; i < roundedMaxLength; i++)
        {
            lights[i] = (Light)Instantiate(lightPrefab);
            lights[i].transform.parent = transform;
            lights[i].gameObject.SetActive(false);
            lights[i].color = finalColor;
        }
    }

    private void ActivateLights()
    {
        for (int i = 0; i < roundedMaxLength; i++)
        {
            lights[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < positionArrayLenght; i++)
        {
            lights[i].color = finalColor;
            lights[i].gameObject.SetActive(true);
        }
    }

    

    void UpdateLaserParts()
    {
        for (int i = 0; i < positionArrayLenght; i++)
        {
            float x = i * distanceBetweenLights * (1 / finalSize);

            pointLightSpawnPositions[i] = new Vector3(0f, 0f, 0f) + new Vector3(x, 0f, 0f);
            lights[i].transform.localPosition = pointLightSpawnPositions[i];
            lights[i].intensity = resultProgress * lightIntensity;
            lights[i].range = lightRange * finalSize;
        }
    }

    public void SetGlobalProgress(float gp)
    {
        globalProgress = gp;
    }

    public void SetGlobalImpactProgress(float gp)
    {
        globalimpactProgress = gp;
    }

    
}
