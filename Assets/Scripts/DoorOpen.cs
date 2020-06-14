using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;

public class DoorOpen : MonoBehaviour
{

    [SerializeField] private GameLogic gameLogic = null;


    [SerializeField] private Animation anim = null;
    [SerializeField] private PlayerMovement playerMov = null;
    [SerializeField] private GameObject puerta = null;
    [SerializeField] private DoorOpen destinationDoor = null;
    
    [SerializeField] private bool aperturaLateral = true;
    [SerializeField] private Transform puertaIzquierdaOriginalPosition = null;
    [SerializeField] private Transform puertaDerechaOriginalPosition = null;

    [SerializeField] private Transform puertaIzquierdaFinalPosition = null;
    [SerializeField] private Transform puertaDerechaFinalPosition = null;

    [Header("Teleport Player")]
    [SerializeField] private Transform teleportInicioPlayer = null;
    [SerializeField] private Transform teleportDestinoPlayer = null;
    [SerializeField] private GameObject player = null;


    [Header("Texto Puerta")]
    [SerializeField] private TextMeshPro textoPuerta = null;
    [SerializeField] private SpriteRenderer[] ruedas = null;
    [SerializeField] private Animation puertaAnimTexto = null;
    [SerializeField] public bool isDoorDestination = false;

    [Header("Strings")]
    [SerializeField] private string fase = "";
    [SerializeField] private string zona = "";
    

    private void Awake()
    {
        
        textoPuerta.text = "";
        
        ruedas[0].color = new Color(ruedas[0].color.r, ruedas[0].color.g, ruedas[0].color.b, 0);
        ruedas[1].color = new Color(ruedas[1].color.r, ruedas[1].color.g, ruedas[1].color.b, 0);
    }

    // Start is called before the first frame update
    void Start()
    {

        if (aperturaLateral == true)
        { 


            CreateAnimAperturaLateral("apertura_lateral", 0.25f, puertaIzquierdaOriginalPosition, puertaDerechaOriginalPosition, puertaIzquierdaFinalPosition, puertaDerechaFinalPosition);
            CreateAnimAperturaLateral("cerrar_lateral", 0.25f, puertaIzquierdaFinalPosition, puertaDerechaFinalPosition, puertaIzquierdaOriginalPosition, puertaDerechaOriginalPosition);
        }
        



        

    }

    private void CreateAnimAperturaLateral(string nombreClip, float duration, Transform puertaIzqPosInicial, Transform puertaDerPosInicial,
        Transform puertaIzqPosFinal, Transform puertaDerPosFinal)
    { 
    
        
        AnimationClip clip = new AnimationClip();
        clip.name = nombreClip;
        clip.legacy = true;
        clip.wrapMode = WrapMode.Once;
        

        AnimationCurve curvex = AnimationCurve.Linear(0, puertaIzqPosInicial.localPosition.x, duration, puertaIzqPosFinal.localPosition.x);
        AnimationCurve curvey = AnimationCurve.Linear(0, puertaIzqPosInicial.localPosition.y, duration, puertaIzqPosFinal.localPosition.y);
        AnimationCurve curvez = AnimationCurve.Linear(0, puertaIzqPosInicial.localPosition.z, duration, puertaIzqPosFinal.localPosition.z);

        clip.SetCurve("puerta_interior_izquierda", typeof(Transform), "localPosition.x", curvex);
        clip.SetCurve("puerta_interior_izquierda", typeof(Transform), "localPosition.y", curvey);
        clip.SetCurve("puerta_interior_izquierda", typeof(Transform), "localPosition.z", curvez);

        curvex = AnimationCurve.Linear(0, puertaDerPosInicial.localPosition.x, duration, puertaDerPosFinal.localPosition.x);
        curvey = AnimationCurve.Linear(0, puertaDerPosInicial.localPosition.y, duration, puertaDerPosFinal.localPosition.y);
        curvez = AnimationCurve.Linear(0, puertaDerPosInicial.localPosition.z, duration, puertaDerPosFinal.localPosition.z);

        clip.SetCurve("puerta_interior_derecha", typeof(Transform), "localPosition.x", curvex);
        clip.SetCurve("puerta_interior_derecha", typeof(Transform), "localPosition.y", curvey);
        clip.SetCurve("puerta_interior_derecha", typeof(Transform), "localPosition.z", curvez);


        anim.AddClip(clip, clip.name);
    
    
    }

    public async void SetTextOpenDoor()
    { 
        
    
        if (isDoorDestination == false)
        { 
            textoPuerta.text = Localization.Get("puertaabrir");
            puertaAnimTexto.Play("girar_rueda_puerta");
            await UniTask.Delay(TimeSpan.FromMilliseconds(  puertaAnimTexto.GetClip("girar_rueda_puerta").length * 1000 ));
            ruedas[0].color = new Color(ruedas[0].color.r, ruedas[0].color.g, ruedas[0].color.b, 0);
            ruedas[1].color = new Color(ruedas[1].color.r, ruedas[1].color.g, ruedas[1].color.b, 0);

        
        }
        else
        { 
            isDoorDestination = false;
        
        }

        

    
    }

    public void SetTextNull()
    { 
        textoPuerta.text = "";
    
    
    }

    public async void OpenDoor()
    { 
        playerMov.isInvulnerable = true;

        SetTextNull();

        playerMov.isTeleporting = true;

       

        if (aperturaLateral == true)
        { 
            anim.Play("apertura_lateral");
        
        }


        await UniTask.Delay(500);
        //playerMov.PlayerEntrar();
        if (teleportDestinoPlayer != null)
        {

            playerMov.isTeleporting = true;

            await UniTask.Delay(TimeSpan.FromMilliseconds(500));

            playerMov.rigid.useGravity = false;
            playerMov.rigid.velocity = Vector3.zero;
            
            
            playerMov.transition.EnterTransition();
            destinationDoor.isDoorDestination = true;
            await UniTask.Delay(600);
            player.transform.position = teleportDestinoPlayer.position;

            playerMov.transition.ExitTransition();
            await UniTask.Delay(250);
            
            gameLogic.ShowZona(destinationDoor.fase, destinationDoor.zona);

            destinationDoor.OpenDestinationDoor();
            playerMov.rigid.useGravity = true;
            playerMov.isTeleporting = false;

            

            if (aperturaLateral == true)
            { 
                anim.Play("cerrar_lateral");
        
            }
            

        }
        

    }


    public async void OpenDestinationDoor()
    {

        if (aperturaLateral == true)
        { 
            anim.Play("apertura_lateral");
            await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            anim.Play("cerrar_lateral");
        }

    }

   


    public void CloseDoor()
    { 

        if (aperturaLateral == true)
        { 
            anim.Play("cerrar_lateral");
            
        
        }
    
        
    
    }

    private void OnTriggerEnter(Collider collision)
    {

#if UNITY_EDITOR
        print("name=" + collision.name + " tag=" + collision.tag);
#endif
        playerMov = collision.GetComponentInParent<PlayerMovement>();
        playerMov.isDoorTriggerEnter = true;
        playerMov.lastDoor = this;
        
        SetTextOpenDoor();


        
    }

    private void OnTriggerExit(Collider collision)
    {

#if UNITY_EDITOR
        //print("name=" + collision.name + " tag=" + collision.tag);
#endif
        collision.GetComponentInParent<PlayerMovement>().isDoorTriggerEnter = false;
        collision.GetComponentInParent<PlayerMovement>().lastDoor = null;
        
        SetTextNull();
    }

}
