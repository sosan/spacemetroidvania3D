using System.Collections;
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

        if (isPresaSelected == true && ropeAttached == true)
        {

            if (ropeJoint == null) return;

            camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.82f;
            ropeJoint.connectedBody = null;

            
            
            ropeJoint = null;
            if (playerMovement.isFacingRight == true)
            { 
                player.transform.rotation = Quaternion.identity;

                rigid.AddForce(new Vector3(5f, 2f, 0), ForceMode.Impulse);
            }
            else
            { 
                player.transform.rotation = Quaternion.Euler(0, 180, 0);

                rigid.AddForce(new Vector3(-5f, 2f, 0), ForceMode.Impulse);
            
            }
            
            rigid.constraints = RigidbodyConstraints.FreezeRotation;

            presaEscalada = null;
            isPresaSelected = false;
            isColliding = false;
            ropeAttached = false;
            ropeRenderer.enabled = false;

        }

    }


    private void Presionado_E(InputAction.CallbackContext obj)
    {
    
        if (isPresaSelected == true && ropeAttached == false)
        { 
        
            print("pulsado E desde Ropemanager");
            
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
                //SpringJoint ropeJoint = hit.transform.gameObject.AddComponent<SpringJoint>();
                ropeJoint = hit.transform.GetComponent<HingeJoint>();

                if (ropeJoint != null)
                { 
                    ropeRenderer.enabled = true;
                    camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.66f;

                    float x = 0;
                    if(playerMovement.isFacingRight == true)
                    {
                        x = 10;
                        //if (destinoPos.x >= 0)
                        //{ 
                            
                        //}
                        //else if (destinoPos.x < 0)
                        //{

                        //    x = -10;

                        //}

                    }
                    else
                    { 
                        x = -10;
                        // if (destinoPos.x >= 0)
                        //{ 
                            
                        //}
                        //else if (destinoPos.x < 0)
                        //{

                        //    x = 10;

                        //}
                    
                    }

                    playerMovement.isUsedGancho = true;
                    rigid.AddForce(new Vector3(x, 15f, 0), ForceMode.Impulse);
                    await UniTask.Delay(TimeSpan.FromMilliseconds(300));

                    rigid.drag = 0;
                    rigid.constraints = RigidbodyConstraints.None;
                    ropeJoint.connectedBody = rigid;
                    ropeJoint.autoConfigureConnectedAnchor = true;
                
                    Vector3 coordHit = hit.point;
                    coordHit.x += 1;

                    isColliding = false;
                    ropeAttached = true;
                    encontrado = true;

                    ropeRenderer.SetPosition(0, player.transform.position);
                    ropeRenderer.SetPosition(1, coordHit);
                    
                    //ropePositions.Add(coordHit);
                    playerMovement.ropeHook = coordHit;
                    
                    //await UniTask.Delay(1000);
                    //rigid.velocity = Vector2.zero;
                    //rigid.drag = 0.3f;
                    playerMovement.isSwinging = true;
            
                }
            }

           

        
        
        }
        else
        { 
        
    //        int numHits = Physics.RaycastNonAlloc(player.transform.position, destinoPos, hitsResultados, ropeMaxCastDistance, ropeLayerMask);

        
    //        if (numHits > 0)
    //        { 
    //            for (ushort i = 0; i < hitsResultados.Length; i++)
    //            { 
    //                //if (hitsResultados[i] == null) continue;

    //#if UNITY_EDITOR
    //                print("impulso" + playerMovement.isFacingRight + " dest.x="+ destinoPos.x + " name=" + hitsResultados[i].transform.name);
    //#endif
    //                punteroAnim.Play("puntero_clicked");
    //                playerMovement.EnableFlip();
    ////                if(playerMovement.isFacingRight == true)
    ////                { 

    ////                    if (destinoPos.x < 0)
    ////                    { 
    ////#if UNITY_EDITOR
    ////                        print("pa la izquierda");
    ////#endif
    ////                        rigid.AddRelativeForce(new Vector2(-10, 30f), ForceMode.Impulse);
                       
    ////                        playerMovement.Flip();
                    
    ////                    }
    ////                    else
    ////                    {
    ////#if UNITY_EDITOR
    ////                        print("pa la derecha");
    ////#endif
    ////                        rigid.AddRelativeForce(new Vector2(10, 30f), ForceMode.Impulse);
    ////                        playerMovement.Flip();


    ////                    }

                    
    ////                }
    ////                else
    ////                { 
    ////                    if (destinoPos.x < 0)
    ////                    { 
    ////#if UNITY_EDITOR
    ////                        print("pa la izquierda");
    ////#endif
    ////                        rigid.AddRelativeForce(new Vector2(-10, 30f), ForceMode.Impulse);
    ////                        playerMovement.Flip();
                    
    ////                    }
    ////                    else
    ////                    {
    ////#if UNITY_EDITOR
    ////                        print("pa la derecha");
    ////#endif
    ////                        rigid.AddRelativeForce(new Vector2(10, 30f), ForceMode.Impulse);
    ////                        playerMovement.Flip();


    ////                    }
                
    ////                }
                
    //                //await UniTask.Delay(TimeSpan.FromMilliseconds(200));

                
    //                //isColliding = false;
    //                //ropeAttached = true;
    //                //encontrado = true;
    //                //ropeRenderer.SetPosition(0, player.transform.position);
    //                //ropeRenderer.SetPosition(1, hitsResultados[i].point);

    //                //ropeHingeAnchorRb.transform.position = hitsResultados[i].point;
    //                //ropePositions.Add(hitsResultados[i].point);
    //                //playerMovement.ropeHook = hitsResultados[i].point;
    //                //wrapPointsLookup.Add(hitsResultados[i].point, 0);

    //                //ropeJoint.projectionDistance = Vector2.Distance(player.transform.position, hitsResultados[i].point); //distance
    //                //ropeJoint.enableCollision = true;
    //                //ropeHingeAnchorSprite.enabled = true;
    //                ////await UniTask.Delay(1000);
    //                ////rigid.velocity = Vector2.zero;
    //                ////rigid.drag = 0.3f;
    //                //playerMovement.isSwinging = true;


    //            }   

    //        }
        
        }

        

        isGanchoClicked = false;

        if (encontrado == false)
        { 
            
            ropeRenderer.enabled = false;
            ropeAttached = false;
            //ropeJoint.enableCollision = false;
            
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
        //ropeJoint.enableCollision = false;
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
