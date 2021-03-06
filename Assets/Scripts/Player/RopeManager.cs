﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using System;
using UnityEngine.InputSystem;

using ControlesGlobal;
using System.Linq;
using Cinemachine;

public class RopeManager : MonoBehaviour
{

     private Controles inputActions;

    [Header("Player")]
    [SerializeField] private GameObject player = null;
    [SerializeField] private GameObject garfio = null;

    
    [Header("Camara")]
    [SerializeField] private CinemachineVirtualCamera camaraVirtual = null;
    

    [Header("Gancho")]
    [SerializeField] public LineRenderer ropeRenderer = null;
    [SerializeField] public LayerMask ropeLayerMask;
    [SerializeField] public float climbSpeed = 3f;
    [SerializeField] public GameObject ganchoFin = null;
    //[SerializeField] public DistanceJoint2D ropeJoint;
    //[SerializeField] public ConfigurableJoint ropeJoint = null;
    //[SerializeField] public SpringJoint ropeJoint = null;

    [SerializeField] private float offset = 1.4f;
    [SerializeField] public PlayerMovement playerMovement = null;
    [SerializeField] public PlayerActionCogerObjecto playerActionCogerObjecto = null;
    [SerializeField] public Vector3 presaEscaladaPosition = Vector3.zero;
    [SerializeField] public bool isPresaSelected = false;
    [SerializeField] private Rigidbody rigid = null;
    //[SerializeField] private BoxCollider playerCollider = null;
    [SerializeField] private float ropeMaxCastDistance = 20f;
    [SerializeField] private Rigidbody ropeHingeAnchorRb = null;
    [SerializeField] private SpriteRenderer ropeHingeAnchorSprite = null;

    private HingeJoint ropeJoint = null;

    private bool ropeAttached;
    //private List<Vector2> ropePositions = new List<Vector2>();
    private Vector2 movimientoGancho = Vector2.zero;
    
    
    private bool distanceSet;
    private bool isColliding;
    
    //private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
    private bool isGanchoClicked = false;
    private RaycastHit[] hitsResultados = new RaycastHit[20];

    [SerializeField] private bool conLinea = false;
    [SerializeField] private bool conLimites = true;
    [SerializeField] private bool conPuntero = true;

    [Header("Puntero")]
    [SerializeField] private GameObject puntero = null;
    [SerializeField] private Animation punteroAnim = null;
    [SerializeField] private SpriteRenderer punteroRender = null; 

    [Header("LINEA")]
    [SerializeField] private LineRenderer linea = null;
    [SerializeField] private GameObject circuloLimite = null;
    [SerializeField] private SpriteRenderer rendererCirculo = null;
    [SerializeField] private Camera camara = null;
    [SerializeField] public float maxDistance = 10.12f;

    [SerializeField] public Vector3 lastMousePosition = Vector3.zero;
    [SerializeField] public Vector3 currentMousePos = Vector3.zero;

    [SerializeField] private bool canAim = true;

    [Header("Presa")]
    [SerializeField] private PresaEscaladaManager presaEscalada = null;


    [Header("Flechas")]
    [SerializeField] private ParticleSystem flechaIzq = null;
    [SerializeField] private ParticleSystem flechaDer = null;



    [SerializeField] private UIGameManager uIGameManager = null;


    private void Awake()
    {

        inputActions = new Controles();

        //inputActions.PlayerGanchoMovement.mover.performed += MoverGancho;
        //inputActions.PlayerGanchoMovement.mover.canceled += PararGancho;
        inputActions.PlayerMovement.activar.performed += Presionado_E;
        inputActions.PlayerMovement.desactivar.performed += Presionado_Q;
        inputActions.PlayerMovement.activar.Enable();
        inputActions.PlayerMovement.desactivar.Enable();

    }



    private void OnEnable()
    {

        inputActions.PlayerMovement.activar.Enable();
        inputActions.PlayerMovement.desactivar.Enable();


    }


    private void OnDisable()
    {

        inputActions.PlayerMovement.activar.Disable();
        inputActions.PlayerMovement.desactivar.Disable();
        

    }

    

    private void Start()
    {
        
    }


    private void Update()
    {

        if (uIGameManager.isPaused == true) return;

        UpdateRopePositions();
        //HandleRopeLength();
        UpdateMouse();
        UpdateBounds();
       

    }

    private void Presionado_Q(InputAction.CallbackContext obj)
    {

        QuitarGarfio();
    }

    public void QuitarGarfio()
    { 
    
        if (isPresaSelected == true && ropeAttached == true)
        {

            if (ropeJoint == null) return;

            flechaDer.Stop();
            flechaIzq.Stop();

            camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.82f;
            ropeJoint.connectedBody = null;
            playerMovement.isSwinging = false;
            
            
            ropeJoint = null;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;


            if (playerMovement.isFacingRight == true)
            { 
                player.transform.rotation = Quaternion.identity;
                rigid.AddForce(new Vector3(2f, -4f, 0), ForceMode.Impulse);
            }
            else
            { 
                player.transform.rotation = Quaternion.Euler(0, 180, 0);

                rigid.AddForce(new Vector3(-2f, -4f, 0), ForceMode.Impulse);
            
            }

            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            
            playerMovement.giroPlayer.rotation = Quaternion.Euler(
                    0,
                    playerMovement.giroPlayer.localEulerAngles.y,
                    playerMovement.giroPlayer.localEulerAngles.z
            );

            



            presaEscalada = null;
            isPresaSelected = false;
            isColliding = false;
            ropeAttached = false;
            ropeRenderer.enabled = false;

        }

    
    }

    public void QuitarGarfioColisionadoHueco()
    { 

        //if (isPresaSelected == true && ropeAttached == true)
        {

            if (ropeJoint == null) return;

            camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.82f;
            ropeJoint.connectedBody = null;

            flechaDer.Stop();
            flechaIzq.Stop();
            
            ropeJoint = null;

            //playerMovement.giroPlayer.rotation = Quaternion.Euler(
            //        0,
            //        playerMovement.giroPlayer.localEulerAngles.y,
            //        playerMovement.giroPlayer.localEulerAngles.z
            //);
            
            rigid.constraints = RigidbodyConstraints.FreezeRotation;

            presaEscalada = null;
            isPresaSelected = false;
            isColliding = false;
            ropeAttached = false;
            ropeRenderer.enabled = false;
            playerMovement.isSwinging = false;

        }
    
    }


    private void Presionado_E(InputAction.CallbackContext obj)
    {
    
        if (isPresaSelected == true && ropeAttached == false && playerActionCogerObjecto.isBoxTaken == false && 
            playerMovement.isPlayerFalling == false)
        {
#if UNITY_EDITOR        
            print("pulsado E desde Ropemanager");
#endif            
            Vector3 facingDirection = Vector3.zero;
            facingDirection = presaEscaladaPosition - player.transform.position;

            Vector3 aimDirection = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) +
                facingDirection.normalized * ropeMaxCastDistance;

            GanchoShow(aimDirection);
        }

    }

    private void UpdateMouse()
    {


        if (isPresaSelected) return;

        if (Mouse.current.leftButton.isPressed == true)
        {
            

            if (isGanchoClicked == true) return;
            if (playerMovement.isSwinging == true) return;
            if (ropeAttached == true) return;
            
            isGanchoClicked = true;
            var direccion = currentMousePos - player.transform.position;
            var angulo = Mathf.Atan2(direccion.y, direccion.x);
            if (angulo < 0f)
            {
                angulo = Mathf.PI * 2 + angulo;
            }

            var destinoFinal = Quaternion.Euler(0, 0, angulo * Mathf.Rad2Deg) * Vector2.right;

            GanchoShow(destinoFinal);
            
            
        }

        if (Mouse.current.rightButton.isPressed == true)
        { 
            
            if (ropeAttached == true)
            { 

                isColliding = false;
                presaEscaladaPosition = Vector3.zero;
                isPresaSelected = false;
                
                ResetRope();
                
            }
        
        }

    }


   

    private async void GanchoShow(Vector3 destinoPos)
    {
        

        
        bool encontrado = false;

        //Debug.DrawLine(player.transform.position, destinoPos, Color.red, 10000);

        if (isPresaSelected == true)
        { 

            //print("destinopos=" + destinoPos + " presapos=" + presaEscaladaPosition + "playerPos=" + player.transform.position);
            RaycastHit hit = default;
            if (Physics.Linecast(player.transform.position, presaEscaladaPosition, out hit, ropeLayerMask) == true )
            { 
                
                //print("coliision" + hit.transform.name);
                ropeJoint = hit.transform.GetComponent<HingeJoint>();

                if (ropeJoint != null)
                { 
                    ropeRenderer.enabled = true;
                    
                    camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.66f;

                    Vector3 direction = Vector3.zero;

                    if (playerMovement.isFacingRight == true)
                    {
                        direction = new Vector3(10f, 15f, 0);
                    }
                    else
                    {
                        direction = new Vector3(-10f, 15f, 0);
                    }

                    playerMovement.isUsedGancho = true;
                    playerMovement.propulsion.Play();
                    
                    rigid.AddForce(direction, ForceMode.Impulse);
                    await UniTask.Delay(TimeSpan.FromMilliseconds(300));

                    playerMovement.isSwinging = true;

                    //playerMovement.giroPlayer.rotation = Quaternion.Euler(
                    //    30,
                    //    playerMovement.giroPlayer.localEulerAngles.y,
                    //    playerMovement.giroPlayer.localEulerAngles.z
                    //);


                    flechaDer.Play();
                    flechaIzq.Play();
                    rigid.constraints = RigidbodyConstraints.None;
                    ropeJoint.connectedBody = rigid;
                    ropeJoint.autoConfigureConnectedAnchor = true;
                    playerMovement.propulsion.Stop();
                    Vector3 coordHit = hit.point;
                    coordHit.x += 1;

                    isColliding = false;
                    ropeAttached = true;
                    encontrado = true;

                    ropeRenderer.SetPosition(0, player.transform.position);
                    ropeRenderer.SetPosition(1, coordHit);
                    
                    playerMovement.ropeHook = coordHit;
                    
            
                }
            }

           

        
        
        }

        isGanchoClicked = false;

        if (encontrado == false)
        { 
            
            ropeRenderer.enabled = false;
            ropeAttached = false;
            
        }

        
    }

    private void UpdateBounds()
    { 

        currentMousePos = GetMousePosition();
        var diferencia = currentMousePos - player.transform.position;
        
        if (conLimites == true)
        { 
            var aimAngle = Mathf.Atan2(diferencia.y, diferencia.x);
            if (aimAngle < 0f)
            {
                aimAngle = Mathf.PI * 2 + aimAngle;
            }


            float distanciaSqr = SquareDistance(player.transform.position, currentMousePos);
            float alphaCirculo = Mathf.Lerp(rendererCirculo.color.a, canAim ? Mathf.Clamp01(1f * Square(distanciaSqr) / (Square(maxDistance) * Square(maxDistance))) : 0f, Time.unscaledDeltaTime * 6f);
            rendererCirculo.color = new Color(
                rendererCirculo.color.r, 
                rendererCirculo.color.g, 
                rendererCirculo.color.b,
                alphaCirculo
            );

            circuloLimite.transform.rotation = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg);

        
        }
        else
        { 
        
            rendererCirculo.color = new Color(
                rendererCirculo.color.r, 
                rendererCirculo.color.g, 
                rendererCirculo.color.b,
                0
            );
        
        }

        if (conLinea == true)
        { 
            linea.SetPosition(0, player.transform.position);
        
            diferencia = Vector2.ClampMagnitude(diferencia, maxDistance);
            Vector2 destinoPos = player.transform.position + diferencia;
            linea.SetPosition(1, destinoPos);
        
            //jl. por si necesitamos collider en la linea
            //Mesh mesh = new Mesh();
            //linea.BakeMesh(mesh, true);
            //meshCollider.sharedMesh = mesh;

        
        }

        if (conPuntero == true)
        { 

            if (ropeAttached == false)
            { 

                if (playerMovement.isWalking == true)
                { 
                    punteroRender.color = new Color(punteroRender.color.r, punteroRender.color.g, punteroRender.color.b, 0);
                
                }
                else
                { 
                    punteroRender.color = new Color(punteroRender.color.r, punteroRender.color.g, punteroRender.color.b, 1);
                    
                }

                
                
            }
            else
            { 
            
                punteroRender.color = new Color(punteroRender.color.r, punteroRender.color.g, punteroRender.color.b, 0);
            
            }

            if (conLimites == true)
            { 
            
                diferencia = Vector2.ClampMagnitude(diferencia, maxDistance);
                Vector2 destinoPos = player.transform.position + diferencia;

                puntero.transform.position = destinoPos;

               
        
            }
            else
            { 

                //currentMousePos.z = 0;
                puntero.transform.position = currentMousePos;
            
            }
            
        
        
        }
        
        

    }

    private void UpdateRopePositions()
    {

        if (ropeAttached)
        {
            ropeRenderer.SetPosition(0, garfio.transform.position);
            ropeRenderer.SetPosition(1, playerMovement.ropeHook);
        }
    }

    private void ResetRope()
    {

        playerMovement.isSwinging = false;
        ropeAttached = false;
        
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, garfio.transform.position);
        ropeRenderer.SetPosition(1, garfio.transform.position);
        //ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
    }

   

    private void MoverGancho(InputAction.CallbackContext obj)
    {

        if (ropeAttached == false) return;

        movimientoGancho = obj.ReadValue<Vector2>();

       
       
    }

    private void PararGancho(InputAction.CallbackContext obj)
    {

        if (ropeAttached == false) return;

        movimientoGancho = Vector2.zero;

       
    }


    //private void HandleRopeLength()
    //{
        
    //    if (ropeAttached == false) return;

    //    //jl. deberia subir directo y no usar las teclas arriba / abajo. o si?
    //    if (movimientoGancho.y >= 0.1f && isColliding == false)
    //    {
    //        ropeJoint.projectionDistance -= Time.deltaTime * climbSpeed;
    //    }
    //    else if (movimientoGancho.y < 0f)
    //    {
    //        ropeJoint.projectionDistance += Time.deltaTime * climbSpeed;
    //    }
       

    //}


    private Vector3 GetMousePosition()
    { 
    
        Vector3 vectorMouse = Mouse.current.position.ReadValue();
        vectorMouse.z = 10;

        Vector3 currentMousePos = camara.ScreenToWorldPoint(vectorMouse);
        
        return currentMousePos;
    
    
    }
   

    private float Square(float f)
	{
		return f * f;
	}

    private float Distance(Vector2 v1, Vector2 v2)
	{
		return Mathf.Sqrt(SquareDistance(v1, v2));
	}

    private float SquareDistance(Vector2 v1, Vector2 v2)
	{
		return (v2.x - v1.x) * (v2.x - v1.x) + (v2.y - v1.y) * (v2.y - v1.y);
	}


    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PresaEscalada"))
        { 
            presaEscalada = collision.GetComponent<PresaEscaladaManager>();
            presaEscalada.ActivarSelectedRender();
            presaEscaladaPosition = collision.gameObject.transform.position;
            //presaEscaladaPosition.z = 0;
            //isColliding = true;
            isPresaSelected = true;
        }

        
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("PresaEscalada"))
        { 
        
            if (ropeAttached == true ) return;

            collision.GetComponent<PresaEscaladaManager>().DesActivarSelectedRender();
            presaEscalada = null;
            isColliding = false;
            presaEscaladaPosition = Vector3.zero;
            isPresaSelected = false;
            ResetRope();
        }


        
    }



}
