using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ControlesGlobal;
using System;
using UniRx.Async;
using UniRx;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{


    public static PlayerMovement instancePlayer;


    private Controles inputActions;

    [Header("Managers")]
    [SerializeField] private PlayerAfterImagePool imagePool = null;
    [SerializeField] private GameLogic gameLogic = null;
    [SerializeField] private RopeManager ropeManager = null;
    [SerializeField] private PlayerMagnetic playerMagnetic = null;
    [SerializeField] private UIGameManager uIGameManager = null;



    [SerializeField] public Vector2 ropeHook;
    [SerializeField] public bool isSwinging;

    //private float movementXDirection;
    //private float movementYDirection;
    public Vector2 inputMovement = Vector2.zero;

    private float jumpTimer;
    [SerializeField] private float restJump = 0.01f;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private float knockbackStartTime;
    [SerializeField] private float knockbackDuration = 0.2f;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    public bool isFacingRight = true;
    public bool isWalking = false;
    public bool isAttacking = false;
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isTouchingBox = false;
    public bool isBoxCollide = false;
    public bool isBoxTaken = false;
    private bool isWallSliding = false;
    public bool isCompletedRotation = false;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    public bool canMove;
   
    private bool canFlip;
    private bool hasWallJumped;
    public bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private bool isDashing;
    private bool knockback;
    public bool isTeleporting = false;

    public bool isInvulnerable = false;
    public bool isMagnetic = false;

    [SerializeField] private Vector2 knockbackSpeed = new Vector2(10, 10);

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    [SerializeField] public Rigidbody rigid = null;
    [SerializeField] public Animator anim = null;
    [SerializeField] private SphereCollider sierraCollider = null;

    [SerializeField] public float robotCurrentHealth = 1000;
    [SerializeField] public float robotMaxHealth = 1000;

    [SerializeField] private int amountOfJumps = 2;
    [SerializeField] public float movementSpeed = 9.0f;
    [SerializeField] private float jumpForce = 21.0f;
    [SerializeField] private bool saltarconFuerzas = false;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private float wallCheckDistance = 0.65f; 
    [SerializeField] private float boxCheckDistance = 0.65f; 
    [SerializeField] private float wallSlideSpeed = 1f;
    [SerializeField] private float movementForceInAir = 50f;
    [Range(0, 0.3f)] [SerializeField] private float movementSmooth = 0.05f;	
    [SerializeField] private float airDragMultiplier = 0.95f;
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] private float wallHopForce = 10f;
    [SerializeField] private float wallJumpForce = 30f;
    [SerializeField] private float jumpTimerSet = 4f;
    [SerializeField] private float turnTimerSet = 0.1f;
    [SerializeField] private float wallJumpTimerSet = 0.5f;
    [SerializeField] private float ledgeClimbXOffset1 = 0.3f;
    [SerializeField] private float ledgeClimbYOffset1 = 0f;
    [SerializeField] private float ledgeClimbXOffset2 = 0.5f;
    [SerializeField] private float ledgeClimbYOffset2 = 2f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float distanceBetweenImages = 0.1f;
    [SerializeField] private float dashCoolDown = 2.5f;

    [SerializeField] private Vector2 wallHopDirection = new Vector2(1f, 0.5f);
    [SerializeField] private Vector2 wallJumpDirection = new Vector2(1f, 2f);

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform ledgeCheckArriba;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsBox;
    private Collider[] results = new Collider[25];
    private Collider[] resultGround = new Collider[25];
    private RaycastHit[] resultsRayCast = new RaycastHit[25];
    private RaycastHit[] resultsTouchWall = new RaycastHit[25];
    private RaycastHit[] resultsLedge = new RaycastHit[25];
    
    private RaycastHit[] resultsTouchBox = new RaycastHit[25];


    private Vector3 m_Velocity = Vector3.zero;
    private float progressColor = 0;


    
    

    [Header("puertas")]
    public bool isDoorTriggerEnter = false;
    [SerializeField] public DoorOpen lastDoor = null;
    
    [Header("Transicion")]
    [SerializeField] public TransitionManager transition = null;

    [Header("Propulsion FX")]
    [SerializeField] private ParticleSystem propulsion = null;

    [Header("Barras Propulsion")]
    [SerializeField] private Image barraPropulsionBackground = null;
    [SerializeField] private Image barraPropulsionFront = null;
    [SerializeField] private Color colorinicial = Color.white;
    [SerializeField] private Color colordestino = Color.white;

    [Header("Barras Salud")]
    [SerializeField] private Image barraVidaBackground = null;
    [SerializeField] private Image barraVidaFront = null;
    [SerializeField] private Color colorinicialVida = Color.white;
    [SerializeField] private Color colordestinoVida = Color.white;



    [Header("Flamete")]
    [SerializeField] private ParticleSystem flamete = null;

    [Header("Muerte FX")]
    [SerializeField] public ParticleSystem muerteRobot = null;
    [SerializeField] public ParticleSystem muerteRobotExterior = null;

    [Header("Polvo Tocar Suelo FX")]
    [SerializeField] private ParticleSystem polvoRobot = null;

    [Header("Animations")]
    [SerializeField] public Animation animsPlayer = null;
    [SerializeField] public bool bajarEscalera = false;

    [Header("Giro Sierra FX")]
    [SerializeField] private ParticleSystem sierraFX = null;

    [Header("Giro")]
    [SerializeField] public Transform giroPlayer = null;
    [SerializeField] private ParticleSystem[] particulaGiro = null;
    [SerializeField] private ParticleSystemRenderer[] particulaGiroRender = null;


    private void OnEnable()
    {

        inputActions.PlayerMovement.mover.Enable();
        inputActions.PlayerMovement.saltar.Enable();
        inputActions.PlayerMovement.activar.Enable();
        inputActions.PlayerMovement.flamete.Enable();
        inputActions.PlayerMovement.ataque.Enable();
        inputActions.PlayerMovement.cogerObjeto.Enable();

    }


    private void OnDisable()
    {

        inputActions.PlayerMovement.mover.Disable();
        inputActions.PlayerMovement.saltar.Disable();
        inputActions.PlayerMovement.activar.Disable();
        inputActions.PlayerMovement.flamete.Disable();
        inputActions.PlayerMovement.ataque.Disable();
        inputActions.PlayerMovement.cogerObjeto.Disable();

    }


    private void Awake()
    {

        if (instancePlayer == null)
        { 
            instancePlayer = this;
        
        }
        else
        { 
            Destroy(instancePlayer);
        
        }


        


        isDoorTriggerEnter = false;
        inputActions = new Controles();
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

        jumpTimer = jumpTimerSet;

    }

   

    private void Update()
    {
        
        if (uIGameManager.isPaused == true) return;

        CheckMovementDirection();
        UpdateAnimations();
        //CheckJump(); 
        CheckDash();
        CheckKnockback();

        CheckBajarEscalera();

    }

    private void CheckBajarEscalera()
    { 
    
        if (bajarEscalera == true)
        { 
            
            float x = (0.2f * movementSpeed * Time.fixedDeltaTime);
            if (isFacingRight == true)
            { 
                this.transform.position = new Vector3(
                   this.transform.position.x + x,
                   this.transform.position.y,
                    this.transform.position.z
               );
            
            }
            else
            { 
             this.transform.position = new Vector3(
                   this.transform.position.x - x,
                   this.transform.position.y,
                    this.transform.position.z
               );
            
            }

           
        
        
        }
    
    
    }
    
    
    private void FixedUpdate()
    {

        if (uIGameManager.isPaused == true) return;

        ApplyMovement();
        //jl. no se si moverlo con fuerzas o con velocity. no se como quedara mas manejable. quizas hacer una opcion para elegir metodo
        //if (saltarconFuerzas == true)
        //{ 
        //    NormalJumpConFuerzas();
        
        //}
        
        CheckSurroundings();
    }


    

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rigid.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rigid.velocity = new Vector2(0.0f, rigid.velocity.y);
        }
    }


    private void CheckSurroundings()
    {
        if (isSwinging == true) return;

        isGrounded = 1 <= Physics.OverlapSphereNonAlloc(groundCheck.position, groundCheckRadius, resultGround, whatIsGround);
        isTouchingWall = 1 <= Physics.RaycastNonAlloc(wallCheck.position, transform.right, resultsTouchWall, wallCheckDistance, whatIsGround);
        isTouchingLedge = 1 <= Physics.RaycastNonAlloc(ledgeCheck.position, transform.right, resultsLedge, boxCheckDistance, whatIsBox);
        
    }

    private void CheckIfCanJump()
    {

        if (isSwinging == true) return;

        if(isGrounded && rigid.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if(amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
      
    }

    private void CheckMovementDirection()
    {

        if (isTeleporting == true) return;
        if (isSwinging == true) return;
        

        if (inputActions != null)
        { 
            inputMovement = inputActions.PlayerMovement.mover.ReadValue<Vector2>();
        }


        if (inputMovement.x != 0f)
        { 
            canMove = true;
            if (isGrounded)
            { 
                canMove = true;
                isWalking = true;
                //print("istouchingbox=" + isTouchingBox + " isBoxTaken=" + isBoxTaken + " ispressed=" + isPresedTaken + 
                //    " x=" + inputMovement.x + " facindirection=" + facingDirection + " isTouchingLedge=" + isTouchingLedge);
                //istouchingbox=True isBoxTaken=False ispressed=False x=-1 facindirection=-1

                if (isTouchingLedge == true)
                { 

                    canMove = false;
                    isWalking = false;
                    
                    
                }

                if (isBoxTaken == true)
                { 
                    canMove = true;
                    isWalking = true; 
                    
                    
                }

            }
            else
            { 
                
                isWalking = false;
            }
            
        }
        else
        { 
            canMove = true;
            isWalking = false;
            inputMovement.x = 0;
            
        }

        if (isTouchingWall == true)
        { 
        
            
            if(!isGrounded && inputMovement.x != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }

        }



        if (isFacingRight && inputMovement.x < 0)
        {
            Flip();
        }
        else if(!isFacingRight && inputMovement.x > 0)
        {
            Flip();
        }

        //if (isSwinging == false)
        { 
        
            //if(Mathf.Abs(rigid.velocity.x) >= 0.02f)
            //{
            //    isWalking = true;
            //}
            //else
            //{
            //    isWalking = false;
            //}
        
        }

        
    }

    private void UpdateAnimations()
    {
        

        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rigid.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("attack1", isAttacking);

    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        imagePool.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void CheckDash()
    {
        if (isSwinging == true) return;

        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rigid.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    imagePool.GetFromPool();
                    //PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
            
        }
    }


    private void NormalJumpConFuerzas()
    { 
        if (canNormalJump)
        {

            Vector2 direction = Vector2.up * jumpForce;
            rigid.AddRelativeForce(direction, ForceMode.Impulse);
           
        }
    
    }
   

    private void NormalJump()
    {

        //print("Nornmaljump cannomrelajump=" + canNormalJump);
        if (canNormalJump)
        {
            
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        }
    }

    private void ApplyMovement()
    {
        //print(" isSwinging=" + isSwinging + " canMove=" + canMove + " knowck=" +knockback );
        if (isTeleporting == true) return;
        if (gameLogic.isInCharla == true) return;


        if (isSwinging == true)
        {

            if (inputMovement.x == 0)
            { 
            
                rigid.drag = 0.5f;            
            }
            else
            { 
            
                var playerToHookDirection = (ropeHook - (Vector2)this.transform.position).normalized;

                Vector2 perpendicularDirection = Vector2.zero;
                rigid.drag = 0;
                if (inputMovement.x < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)this.transform.position - perpendicularDirection * -2f;
                    //Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                    var force = perpendicularDirection * 10f;
                    
                    rigid.AddForce(force, ForceMode.Force);

                }
                else if (inputMovement.x > 0)
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    //Debug.DrawLine(transform.position, rightPerpPos, Color.black, 0f);
                    var force = perpendicularDirection * 10f;
                    
                    rigid.AddForce(force, ForceMode.Force);

                }

            
            }


            



        }
        else
        { 

            if (isMagnetic == true )
            { 
                if (isCompletedRotation == true)
                { 
                    
                    switch (playerMagnetic.tipoEsquina)
                    {
                        case TipoEsquina.izquierdaAbajo: IzquierdaAbajo(); break;
                        case TipoEsquina.izquierdaArriba: IzquierdaArriba(); break;
                        case TipoEsquina.derechaArriba: DerechaArriba(); break;
                        case TipoEsquina.derechaAbajo: DerechaAbajo(); break;

                    }
                    
                   
                
                }
                
                return;
            
            }

            if (isTouchingBox == true)
            { 
                
                if (canMove == true)
                { 
                    rigid.useGravity = true;
                    float moveX = inputMovement.x * movementSpeed * Time.fixedDeltaTime;
                    Vector2 direction = new Vector2(rigid.position.x + moveX, rigid.position.y);
                    rigid.MovePosition(direction);
                    return;
                
                }
            
            }


            if (isGrounded)
            { 
            
                if(canMove && !knockback)
                {
                    float moveX = inputMovement.x * movementSpeed * Time.fixedDeltaTime;
                    //float moveY = movementYDirection * movementForceInAir * Time.fixedDeltaTime;
                    Vector2 direction = new Vector2(rigid.position.x + moveX, rigid.position.y);
                    rigid.MovePosition(direction);
                    return;

                }
                
            }
            else
            { 
               //print("direction=" + movementXDirection +" knockback" + knockback);
                //rigid.velocity = new Vector2(rigid.velocity.x * airDragMultiplier, rigid.velocity.y);
                //float moveX = movementXDirection * movementSpeed * Time.fixedDeltaTime;
                //float moveY = movementYDirection * movementForceInAir * Time.fixedDeltaTime;
                //Vector2 direction = new Vector2(rigid.position.x + moveX, moveY + rigid.position.y);
                //rigid.MovePosition(direction);
            }

        
        }


       
    }


    private void IzquierdaAbajo()
    { 

        print("movimiento arriba - abajo");
        rigid.useGravity = false;
        float moveX = inputMovement.x * movementSpeed * Time.fixedDeltaTime;
        Vector3 directionRigid = new Vector3(rigid.position.x, rigid.position.y + moveX, rigid.position.z);
        rigid.MovePosition(directionRigid);


    }

    public void IzquierdaArriba()
    { 
        print("movimiento izquierda - derecha");
        rigid.useGravity = true;
        float moveX = inputMovement.x * movementSpeed * Time.fixedDeltaTime;
        Vector3 directionRigid = new Vector3(rigid.position.x + moveX, rigid.position.y , rigid.position.z);
        rigid.MovePosition(directionRigid);
       
    
    
    }

    public void DerechaArriba()
    { 
        
    
    }

    public void DerechaAbajo()
    { 
    
    
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    public void Flip()
    {
        if (isWallSliding == false && knockback == false && isMagnetic == false)
        {
            facingDirection *= -1;
            wallCheckDistance *= -1;
            isFacingRight = !isFacingRight;
            


            transform.Rotate(0f, 180f, 0f);
            
            var rnd = UnityEngine.Random.Range(0, 2);
            if (rnd == 0)
            { 

                rnd = UnityEngine.Random.Range(0, particulaGiro.Length);
                if (isFacingRight == true)
                { 
                    particulaGiroRender[rnd].flip = new Vector3(0, 0, 0);
                
                }
                else
                { 
                    particulaGiroRender[rnd].flip = new Vector3(1, 0, 0);
                
                }
                
                particulaGiro[rnd].Play();
            
            }
            

        }
    }


    public async void PlayerEntrar()
    { 
       
       
        transition.EnterTransition();
        await UniTask.Delay(600);
        transition.ExitTransition();
        await UniTask.Delay(150);
        lastDoor.CloseDoor();
        isInvulnerable = false;
        isTeleporting = false;

        await UniTask.Delay(TimeSpan.FromMilliseconds(300));
        lastDoor.SetTextOpenDoor();

        
        

    
    }

    
    public void ResetHealth()
    { 
        robotCurrentHealth = robotMaxHealth;

        barraVidaBackground.fillAmount = robotCurrentHealth / robotMaxHealth;
    }


    public void SetHealth(int vidaQuitada)
    { 

        robotCurrentHealth -= vidaQuitada;
        if (robotCurrentHealth <= 0)
        { 
            robotCurrentHealth = 0;
        
        }

        barraVidaBackground.fillAmount = robotCurrentHealth / robotMaxHealth;

    
    
    }
    

    private void CheckJump()
    {

        if (jumpTimer <= 1.5f)
        { 
        
            progressColor += Time.deltaTime / 0.2f;
            barraPropulsionBackground.color = Color.Lerp(colorinicial, colordestino,  Mathf.PingPong(progressColor, Time.time ));
            barraPropulsionFront.color = Color.Lerp(colorinicial, colordestino,  Mathf.PingPong(progressColor, Time.time ));
            if (progressColor >= 1)
            { 
                progressColor = 0;
            }
        
        }
        else
        { 
            progressColor = 0;
            barraPropulsionBackground.color = colorinicial;
            barraPropulsionFront.color = colorinicial;
        
        }

        //print("isAttemptingToJump=" + isAttemptingToJump);
        if (isAttemptingToJump == true)
        {

            if (jumpTimer <= 0)
            { 
                propulsion.Stop();
                return;

            }

           

            
            canNormalJump = true;
            
            if (saltarconFuerzas == false)
            { 
                NormalJump();
            
            }
            


            jumpTimer -= restJump;
            
            
            if (jumpTimer <= 0)
            { 
                jumpTimer = 0;
                canNormalJump = false;
            
            }
            barraPropulsionBackground.fillAmount = jumpTimer / jumpTimerSet;
            
        }
        else
        { 
        
            jumpTimer += restJump;
            
            if (jumpTimer >= jumpTimerSet)
            { 
                jumpTimer = jumpTimerSet;
                
            }
            barraPropulsionBackground.fillAmount = jumpTimer / jumpTimerSet;
        
        
        }

     
    }




#region INPUTS
    
    private void Saltar_canceled(InputAction.CallbackContext obj)
    {
#if UNITY_EDITOR
        //print("cancelado saltar");
#endif

        
        isAttemptingToJump = false;
        canNormalJump = false;
        if (propulsion.isPlaying == true)
        { 
            propulsion.Stop();
        
        }
        

    }

//    private void Saltar_performed(InputAction.CallbackContext obj)
//    {
       
//#if UNITY_EDITOR
//        print("performed. isGrounded=" + isGrounded +  " isTouchingWall=" + isTouchingWall );
//#endif


//    }

    private void Saltar_started(InputAction.CallbackContext obj)
    {

        if (isTeleporting == true) return;
        if (isSwinging == true) return;


#if UNITY_EDITOR
        print("saltar");
#endif
        

        if (jumpTimer <= 0)
        { 
            return;

        }
        
        if (propulsion.isPlaying == false)
        { 
            propulsion.Play();
        
        }
        
        isAttemptingToJump = true;

       


    }


    private void Presionado_E(InputAction.CallbackContext obj)
    {
#if UNITY_EDITOR
        //print("presionado E");
#endif

        if (gameLogic.isInCharla == false)
        { 



            if (ropeManager.isPresaSelected == true) return;
            if (isTeleporting == true) return;
        
            if (isDoorTriggerEnter == true)
            { 

                

                if (lastDoor == null) return;
                lastDoor.OpenDoor();
        
        
            }

        
        }
        

        
    }


    private void PararPlayer(InputAction.CallbackContext obj)
    {
        inputMovement.x = 0;

        if (isGrounded)
        {
            canMove = false;
            isWalking = false;
        }
    }

    

    //private void MoverPlayer(InputAction.CallbackContext obj)
    //{

    //    if (isTeleporting == true) return;
    //    if (isSwinging == true) return;

    //    inputMovement = inputActions.PlayerMovement.mover.ReadValue<Vector2>();

    //    //movementXDirection = inputMovement.x;
    //    //movementYDirection = inputMovement.y;

    //    print(inputMovement);

    //    if (inputMovement.x != 0f)
    //    { 
    //        canMove = true;
    //        if (isGrounded)
    //        { 
    //            isWalking = true;
    //        }
    //        else
    //        { 
    //            isWalking = false;
    //        }
            
    //    }
    //    else
    //    { 
    //        canMove = true;
    //        isWalking = false;
            
    //    }

    //    if (isTouchingWall == true)
    //    { 
        
            
    //        if(!isGrounded && inputMovement.x != facingDirection)
    //        {
    //            canMove = false;
    //            canFlip = false;

    //            turnTimer = turnTimerSet;
    //        }

    //    }

        

    //    if (turnTimer >= 0)
    //    {
    //        turnTimer -= Time.deltaTime;

    //        if(turnTimer <= 0)
    //        {
    //            canMove = true;
    //            canFlip = true;
    //        }
    //    }

    //}


    private void Flamete_canceled(InputAction.CallbackContext obj)
    {
        print("parado flamete");
        flamete.Stop();

    }

    private void Flamete_performed(InputAction.CallbackContext obj)
    {

        print("flamete");

        if (flamete.isPlaying == false)
        { 
        
            flamete.Play();
        
        }
    }


    private void Ataque_canceled(InputAction.CallbackContext obj)
    {
       
        isAttacking = false;
        sierraCollider.enabled = false;
        sierraFX.Stop();
    }


    
    private void Ataque_started(InputAction.CallbackContext obj)
    {
        print("attack started");
        isAttacking = true;
        sierraCollider.enabled = true;
        sierraFX.Play();


    }

    private bool isPresedTaken = false;
    [SerializeField] public GameObject objectCollide = null;
    [SerializeField] private Transform parentObjectTaken = null;
    [SerializeField] private Transform parentScene = null;

    private void CogerObjeto_started(InputAction.CallbackContext obj)
    {

        isPresedTaken = true;

        print("apretado boton MAYS. istouching=" + isTouchingBox + " isboxtaken=" + isBoxCollide);
        if (isTouchingBox == true)
        { 

            if (isBoxCollide == true) return;
            if (objectCollide is null == true) return;
            if (objectCollide.GetComponent<BoxManager>().isMovable == false) return;
            

            isBoxCollide = true;
            isBoxTaken = true;
            canMove = true;
            
            if (objectCollide != null)
            { 
                objectCollide.transform.parent = parentObjectTaken;
                //objectCollide.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
            
            
            


        }


    }

    private void CogerObjeto_canceled(InputAction.CallbackContext obj)
    {
        
        //if (isBoxTaken == false) return;
        print("BOTON MAYS CANCELADO sujeta box");
        CancelarObjetoCogido();




    }

    public void CancelarObjetoCogido()
    { 
        var box = parentObjectTaken.GetComponentInChildren<BoxManager>();
        if (box != null)
        {

            if (objectCollide != null)
            {
                objectCollide.transform.parent = parentScene;
            }

            box.transform.parent = parentScene;
            isBoxTaken = false;

        
        }

        
        isPresedTaken = false;
        isBoxCollide = false;
        isTouchingBox = false;
        
        
        objectCollide = null;
    
    
    }

 


   


    #endregion


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        if (isAttemptingToJump == true) return;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.color = Color.red;

        Gizmos.DrawLine(ledgeCheck.position, new Vector3(
            (ledgeCheck.position.x + wallCheckDistance) , 
            ledgeCheck.position.y, 
            ledgeCheck.position.z));

         Gizmos.DrawLine(ledgeCheckArriba.position, new Vector3(
            (ledgeCheckArriba.position.x + wallCheckDistance) , 
            ledgeCheckArriba.position.y, 
            ledgeCheckArriba.position.z));
    }
#endif

}
