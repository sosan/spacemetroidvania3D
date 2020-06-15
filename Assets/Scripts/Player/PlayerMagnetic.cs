using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ControlesGlobal;
using System;

public class PlayerMagnetic : MonoBehaviour
{

    private Controles inputActions;

    public PlayerMovement playerMovement = null;

    [SerializeField] private Rigidbody rigid = null;
    [SerializeField] private Transform bisagra = null;
    [SerializeField] public bool isCollideWithMagnetic = false;
    [SerializeField] public TipoEsquina tipoEsquina = TipoEsquina.ninguna;

    private bool isIzquierdaAbajo = false;
    private Quaternion rotDestination = Quaternion.identity;
    private bool doRotation = false;


    private void OnEnable()
    {
        inputActions.PlayerMovement.magnetico.Enable();

    }


    private void OnDisable()
    {
        inputActions.PlayerMovement.magnetico.Disable();
        
    }


    private void Awake()
    {
        
        inputActions = new Controles();

        inputActions.PlayerMovement.magnetico.started += AnclarPlayer;
        inputActions.PlayerMovement.magnetico.canceled += DesAnclarPlayer;

        inputActions.PlayerMovement.magnetico.Enable();

    }

    private void DesAnclarPlayer(InputAction.CallbackContext obj)
    {
        playerMovement.gravedadFX.Stop();
        tipoEsquina = TipoEsquina.ninguna;

        if (playerMovement.isMagnetic == true)
        { 
            playerMovement.isMagnetic = false;
        
            Quaternion rotTemp = Quaternion.AngleAxis(0, Vector3.forward);
            rigid.rotation = rotTemp;
            rigid.useGravity = true;


            

        
        }

        
    }

    private void AnclarPlayer(InputAction.CallbackContext obj)
    {

# if UNITY_EDITOR
        print("PULSADO G. COLLIDEWITHMAGNETIC=" + isCollideWithMagnetic + " tipoEsquina=" + tipoEsquina);
# endif
        playerMovement.gravedadFX.Play();

        if (isCollideWithMagnetic == true)
        { 
            playerMovement.isMagnetic = true;

            switch (tipoEsquina)
            {
                case TipoEsquina.izquierdaAbajo: IzquierdaAbajo(); break;
                case TipoEsquina.izquierdaArriba: IzquierdaArriba(); break;
                case TipoEsquina.derechaArriba: DerechaArriba(); break;
                case TipoEsquina.derechaAbajo: DerechaAbajo(); break;

            }
            
            

        }
       

        
    }

    void Start()
    {
        
    }
    

    private void FixedUpdate()
    {

        if (playerMovement.isMagnetic == false) return;

        if (doRotation == true)
        { 
        
            if (Quaternion.Angle(rigid.rotation, rotDestination) <= 0.01f)
            { 

                doRotation = false;
                rigid.useGravity = false;
                playerMovement.isCompletedRotation = true;
                
            }
            else
            { 
                playerMovement.isCompletedRotation = false;
                rigid.rotation = Quaternion.Slerp(rigid.rotation, rotDestination, 10 * Time.fixedDeltaTime);
            }

        }

       




    }

    public void IzquierdaAbajo()
    { 

        if (playerMovement.isMagnetic == false) return;
        Quaternion rotTemp = Quaternion.AngleAxis(23f, Vector3.forward);

        if (Quaternion.Angle(rigid.rotation, rotTemp) <= 0.01f) return;

        rotDestination = rotTemp;
        doRotation = true;


    }

    public void IzquierdaArriba()
    { 

        if (playerMovement.isMagnetic == false) return;
        Quaternion rotTemp = Quaternion.AngleAxis(0f, Vector3.forward);

        if (Quaternion.Angle(rigid.rotation, rotTemp) <= 0.01f) return;

        rotDestination = rotTemp;
        doRotation = true;
    
    
    }

    public void DerechaArriba()
    { 
        if (playerMovement.isMagnetic == false) return;

        Quaternion rotTemp = Quaternion.AngleAxis(-23f, Vector3.forward);

        if (Quaternion.Angle(rigid.rotation, rotTemp) <= 0.01f) return;

        rotDestination = rotTemp;
        doRotation = true;
    
    }

    public void DerechaAbajo()
    { 
    
        if (playerMovement.isMagnetic == false) return;
        Quaternion rotTemp = Quaternion.AngleAxis(0, Vector3.forward);

        if (Quaternion.Angle(rigid.rotation, rotTemp) <= 0.01f) return;

        rotDestination = rotTemp;
        doRotation = true;
    
    }

}
