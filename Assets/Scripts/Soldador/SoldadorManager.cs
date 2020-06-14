using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ControlesGlobal;
using System;
using UniRx.Async;

public class SoldadorManager : MonoBehaviour 
{

    private Controles inputActions;


    [SerializeField] private bool changeAllMaxLength = true;
    [SerializeField] private float maxLength = 32f;
    [SerializeField] private float globalProgressSpeed = 1f;
    [SerializeField] private float globalImpactProgressSpeed = 1f;
    [SerializeField] private bool always = true;
    [SerializeField] private bool colorizeAll = true;
    
    [SerializeField] private Color finalColor = Color.white;
    [Range(0.2f, 1.0f)][SerializeField] private float gammaLinear = 1f;
    [SerializeField] private Renderer meshRend;
    [SerializeField] private float meshRendPower = 3f;
    [SerializeField] private Light pointLight;
    [SerializeField] private StartEffects startPointEffect;
    [SerializeField] private EndEffects endPointEffect;

    [SerializeField] private GameObject soldadorFX = null;

    private float globalProgress;
    private float globalImpactProgress;
    [SerializeField] private BeamLine[] lls;
    [SerializeField] private LightBeam[] lils;
    [SerializeField] private Renderer[] renderers;
    private bool isPressed = false;
    private bool isFirstTimePressed = false;


    private void Awake()
    {

        inputActions = new Controles();
        
        inputActions.PlayerMovement.soldador.started += Soldador_started;
        inputActions.PlayerMovement.soldador.canceled += Soldador_canceled;

        inputActions.PlayerMovement.soldador.Enable();
    }

    private void Start()
    {
        globalProgress = 1f;
        globalImpactProgress = 1f;

        lls = soldadorFX.GetComponentsInChildren<BeamLine>(true);
        lils = soldadorFX.GetComponentsInChildren<LightBeam>(true);
        renderers = soldadorFX.GetComponentsInChildren<Renderer>(true);

        for (ushort i = 0; i < renderers.Length; i++)
        { 
            renderers[i].material.SetFloat("_GammaLinear", gammaLinear);
        
        }

        if (colorizeAll == true)
        {
            for (ushort i = 0; i < lils.Length; i++)
            { 
                lils[i].SetFinalColor(finalColor);
        
            }
                   
            startPointEffect.SetFinalColor(finalColor);
            endPointEffect.SetFinalColor(finalColor);

            for (ushort i = 0; i < renderers.Length; i++)
            { 
                renderers[i].material.SetColor("_FinalColor", finalColor);
        
            }

        }      

    }

    private void Update()
    {

        //print("ispressed=" + isPressed  + " isfirst=" + isFirstTimePressed);

        startPointEffect.SetGlobalProgress(globalProgress);
        startPointEffect.SetGlobalImpactProgress(globalImpactProgress);
        endPointEffect.SetGlobalProgress(globalProgress);
        endPointEffect.SetGlobalImpactProgress(globalImpactProgress);
       

       
        if (globalProgress < 1f)
        {
            globalProgress += Time.deltaTime * globalProgressSpeed;
        }

        if (globalImpactProgress < 1f)
        {
            globalImpactProgress += Time.deltaTime * globalImpactProgressSpeed;
        }

        if (isPressed == true)
        { 
        
            globalProgress = 0f;
            globalImpactProgress = 0f;
        
        }


        
        for (ushort i = 0; i < lls.Length; i++)
        { 
            lls[i].SetGlobalProgress(globalProgress);
            lls[i].SetGlobalImpactProgress(globalImpactProgress);
            if (changeAllMaxLength == true)
            {
                lls[i].maxLength = maxLength;
            }            
        
        }

        for (ushort i = 0; i < lils.Length; i++)
        {
            
            lils[i].SetGlobalProgress(globalProgress);
            lils[i].SetGlobalImpactProgress(globalImpactProgress);
            if (changeAllMaxLength == true)
            {
                lils[i].maxLength = maxLength;
            }

        }

    }


    private void Soldador_canceled(InputAction.CallbackContext obj)
    {
        isPressed = false;
        isFirstTimePressed = false;
        endPointEffect.emit = false;

    }

    private void Soldador_started(InputAction.CallbackContext obj)
    {
        //print("started");
        
        isPressed = true;
        //globalProgress = 0f;
        globalImpactProgress = 0f;
        endPointEffect.emit = true;


    }


    public void ChangeColor(Color color)
    {
        finalColor = color;
    }

    
}
