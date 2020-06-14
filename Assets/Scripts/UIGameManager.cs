using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;
using ControlesGlobal;
using ModuloEscritura.EscrituraTextMeshPro;
using UnityEngine.InputSystem;


public class UIGameManager : MonoBehaviour
{

    private Controles inputActions;

    [Header("Managers")]
    [SerializeField] private SceneLoaderCommander sceneLoader = null;
    [SerializeField] private PlayerMovement playerMovement = null;


    [Header("Paneles")]
    [SerializeField] private GameObject panelGameOver = null;
    [SerializeField] private GameObject panelEscape = null;


    public bool isPaused = false;

    



    private void Awake()
    {
        panelGameOver.SetActive(false);
        panelEscape.SetActive(false);
        
        inputActions = new Controles();
        inputActions.PlayerMovement.escape.started += ShowEscapePanel;

        inputActions.PlayerMovement.escape.Enable();
        
    }

    private void ShowEscapePanel(InputAction.CallbackContext obj)
    {

        Volver();
    }

    public void Volver()
    { 
        if (panelEscape.activeSelf == true)
        { 
            isPaused = false;
            Time.timeScale = 1;
            panelEscape.SetActive(false);
            
        
        }
        else
        { 
            isPaused = true;
            Time.timeScale = 0;
            panelEscape.SetActive(true);
        }
       
    
    }

    private void Start()
    {

    }


    public void ShowGameOver()
    { 
        
    
        panelGameOver.SetActive(true);



    }

    public void ClickedRetryYes()
    { 
        
        sceneLoader.GoToSigleSceneByName("Testing_jl_3D");

    
    }
    public void ClickedRetryNo()
    { 
        
        
        ClickedMenuPrincipal();
    
    }


    public void ClickedMenuPrincipal()
    { 
    
        sceneLoader.GoToSigleSceneByName("MainMenu");
    }

    public void ClickedSalirJuego()
    {

        sceneLoader.QuitGame();
    
    }

    







}
