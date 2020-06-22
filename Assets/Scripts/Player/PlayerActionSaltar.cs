using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionSaltar : MonoBehaviour
{
    
    [Header("MAnagers")]
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private PlayerInputs playerInputs = null;


    [Header("config")]
    [SerializeField] private float jumpTimerSet = 4f;
    [SerializeField] private float restJump = 0.01f;
    [SerializeField] private float jumpForce = 21f;
    [SerializeField] private Rigidbody rigid = null;


    [Header("Impulsores")]
    [SerializeField] private Transform impulsores = null;
    [SerializeField] private Vector3 impulsoresPosOriginal = Vector3.zero;
    [SerializeField] public ParticleSystem propulsion = null;

    [Header("Barras Propulsion")]
    [SerializeField] private Image barraPropulsionBackground = null;
    [SerializeField] private Image barraPropulsionFront = null;
    [SerializeField] private Color colorinicial = Color.white;
    [SerializeField] private Color colordestino = Color.white;

    private float jumpTimer;
    private bool isAttemptingToJump;
    private float progressColor = 0;


    private void Awake()
    {
        jumpTimer = jumpTimerSet;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Saltar_Started()
    { 
        if (playerMovement.isTeleporting == true) return;
        if (playerMovement.isSwinging == true) return;
        if (jumpTimer <= 0) return;

#if UNITY_EDITOR
        print("saltar");
#endif
        

        
        
        if (propulsion.isPlaying == false)
        { 
            propulsion.Play();
        
        }
        
        isAttemptingToJump = true;
    
    
    }

    public void Saltar_Canceled()
    {
#if UNITY_EDITOR
        //print("cancelado saltar");
#endif

        isAttemptingToJump = false;
        if (propulsion.isPlaying == true)
        {
            propulsion.Stop();

        }




    }



    private void Update()
    {
        UpdateColors();
        CheckJump();
    }



    private void CheckJump()
    {

        //print("isAttemptingToJump=" + isAttemptingToJump);
        if (isAttemptingToJump == true)
        {

            if (jumpTimer <= 0)
            { 
                propulsion.Stop();
                return;

            }
            
            Vector3 direction = new Vector3(rigid.position.x + playerInputs.inputMovement.x , playerInputs.inputMovement.y * jumpForce, rigid.position.z);
            rigid.AddRelativeForce(direction, ForceMode.Impulse);
            jumpTimer -= restJump;
            
            
            if (jumpTimer <= 0)
            { 
                jumpTimer = 0;
            
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

    private void UpdateColors()
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
    
    
    
    }

   


}
