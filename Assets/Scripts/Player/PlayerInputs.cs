using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ControlesGlobal;
using System;

public class PlayerInputs : MonoBehaviour
{


    [Header("Managers")]
    [SerializeField] public Controles inputActions = null;
    [SerializeField] private PlayerFX playerFX;
    [SerializeField] private GameLogic gameLogic = null;
    [SerializeField] private RopeManager ropeManager = null;
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private PlayerActionCogerObjecto playerCogerObjeto = null;
    [SerializeField] private CharlaManager charlaManager = null;
    [SerializeField] private PlayerActionAtaque playerActionAtaque = null;
    [SerializeField] private PlayerActionSaltar playerActionSaltar = null;



    public Vector2 inputMovement = Vector2.zero;

    //private void OnEnable()
    //{

    //    inputActions.PlayerMovement.mover.Enable();
    //    inputActions.PlayerMovement.saltar.Enable();
    //    inputActions.PlayerMovement.activar.Enable();
    //    inputActions.PlayerMovement.flamete.Enable();
    //    inputActions.PlayerMovement.ataque.Enable();
    //    inputActions.PlayerMovement.cogerObjeto.Enable();

    //}


    //private void OnDisable()
    //{

    //    inputActions.PlayerMovement.mover.Disable();
    //    inputActions.PlayerMovement.saltar.Disable();
    //    inputActions.PlayerMovement.activar.Disable();
    //    inputActions.PlayerMovement.flamete.Disable();
    //    inputActions.PlayerMovement.ataque.Disable();
    //    inputActions.PlayerMovement.cogerObjeto.Disable();

    //}


    private void Awake()
    {
        inputActions = new Controles();
        inputActions.PlayerMovement.mover.started += MoverPlayer;
        inputActions.PlayerMovement.mover.canceled += PararPlayer;
        
        inputActions.PlayerMovement.saltar.started += Saltar_started;
        inputActions.PlayerMovement.saltar.canceled += Saltar_canceled;
        
        inputActions.PlayerMovement.activar.performed += Presionado_E;
        
        inputActions.PlayerMovement.flamete.performed += Flamete_performed;
        inputActions.PlayerMovement.flamete.canceled += Flamete_canceled;
        
        inputActions.PlayerMovement.ataque.started += Ataque_started;
        inputActions.PlayerMovement.ataque.canceled += Ataque_canceled;

        inputActions.PlayerMovement.cogerObjeto.started += CogerObjeto_started;
        inputActions.PlayerMovement.cogerObjeto.canceled += CogerObjeto_canceled;


        inputActions.PlayerMovement.mover.Enable();
        inputActions.PlayerMovement.saltar.Enable();
        inputActions.PlayerMovement.activar.Enable();
        inputActions.PlayerMovement.flamete.Enable();
        inputActions.PlayerMovement.ataque.Enable();
        inputActions.PlayerMovement.cogerObjeto.Enable();


    }

   
    private void CogerObjeto_started(InputAction.CallbackContext obj)
    {
        playerCogerObjeto.CogerObjeto_started();
    }

    private void CogerObjeto_canceled(InputAction.CallbackContext obj)
    {
        playerCogerObjeto.CancelarObjetoCogido();
    }


   

    private void Ataque_started(InputAction.CallbackContext obj)
    {
        playerActionAtaque.Ataque_started();
    }

    private void Ataque_canceled(InputAction.CallbackContext obj)
    {
        playerActionAtaque.Ataque_canceled();
    }

  

    private void Flamete_performed(InputAction.CallbackContext obj)
    {
        playerFX.PlayFlamete();
    }

    private void Flamete_canceled(InputAction.CallbackContext obj)
    {
        playerFX.StopFlamete();
    }

    private void Presionado_E(InputAction.CallbackContext obj)
    {
#if UNITY_EDITOR
        print("presionado E");
#endif

        if (gameLogic.isInCharla == false)
        { 

            if (ropeManager.isPresaSelected == true) return;
            if (playerMovement.isTeleporting == true) return;
        
            if (playerMovement.isDoorTriggerEnter == true)
            { 

                if (playerMovement.lastDoor == null) return;
                playerMovement.lastDoor.OpenDoor();
        
        
            }

        
        }
        else
        { 
            charlaManager.Clicked_SiguienteCharla();
            
        
        }
        

        
    }
   

    private void Saltar_started(InputAction.CallbackContext obj)
    {
        playerActionSaltar.Saltar_Started();
    }

    private void Saltar_canceled(InputAction.CallbackContext obj)
    {
        playerActionSaltar.Saltar_Canceled();
    }

    private void MoverPlayer(InputAction.CallbackContext obj)
    {
        inputMovement = obj.ReadValue<Vector2>();
    }

    private void PararPlayer(InputAction.CallbackContext obj)
    {
        inputMovement.x = 0;
        playerMovement.StopPlayerMovement();
        
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

 



}
