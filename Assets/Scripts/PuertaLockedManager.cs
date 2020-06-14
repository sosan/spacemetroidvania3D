using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;


public class PuertaLockedManager : MonoBehaviour
{


    [SerializeField] private Animation anim = null;
    [SerializeField] private GameObject puerta = null;
    [SerializeField] private GameObject health = null;
    [SerializeField] private TextMeshPro textoStatus = null;
    [SerializeField] private ParticleSystem particula = null;
    [SerializeField] private SpriteRenderer[] ruedas = null;
    [SerializeField] private CharlaManager charlaManager = null;
    [SerializeField] private string keyLocalization = "closed_door_explicacion";
    [SerializeField] private bool isPrincipalComponent = false;
    [SerializeField] private PuertaLockedManager puertaLockedManager = null;
    [SerializeField] private Transform bisagra = null;
    //[SerializeField] private Quaternion bisagraDestination = Quaternion.identity;
    [SerializeField] private BoxCollider[] colliderPuertaBloqueo = null;
    

    [SerializeField] public bool isLocked = true;
    private float durationParticula = 0;
    private float durationAnimation = 0;
    private bool isPlayerEntered = false;
    private bool isTutorial = true;



    private void Awake()
    {
        if (isPrincipalComponent == false) return;

        durationParticula = particula.main.duration * 1000;
        durationAnimation = anim.GetClip("girar_rueda_puerta").length * 1000;

        DesactivarTextoRuedas();

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    private async void OnTriggerEnter(Collider other)
    {
        
        if (isPrincipalComponent == false) return;

        if (other.CompareTag("Player") == true)
        { 
            
            if (isLocked == false) return;

            if (particula.isPlaying == true) return; 
            if (isPlayerEntered == true) return; 

            isPlayerEntered = true;

            if (isTutorial == true)
            { 
                charlaManager.AparecerCharlaExplicacion(keyLocalization);
            
            }
            health.SetActive(true);

            anim.Play("girar_rueda_puerta");
            await UniTask.Delay(TimeSpan.FromMilliseconds(320));
            particula.Play();
            

           



        
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (isPrincipalComponent == false) return;

        if (other.CompareTag("Player") == true)
        {
            
            isPlayerEntered = false;
            DesactivarTextoRuedas();
            health.SetActive(false);
        }

    }


    private void OnParticleCollision(GameObject other)
    {
        
        if (other.CompareTag("Player") == true)
        {
            
            if (isLocked == false) return;

            health.SetActive(true);
            var scale = health.transform.localScale;
            scale.x -= 0.1f;
            if (scale.x <= 0)
            { 
                isLocked = false;
                puertaLockedManager.isLocked = false;
                scale.x = 0;
                
                AbrirPuerta();

            
            }
            health.transform.localScale = scale;

        }


    }

    private void AbrirPuerta()
    { 
        
        colliderPuertaBloqueo[0].enabled = false;
        colliderPuertaBloqueo[1].enabled = false;

         DesactivarTextoRuedas();

        anim.Play("abrir_puerta");

    
    }

    private void DesactivarTextoRuedas()
    { 
        textoStatus.color = new Color(textoStatus.color.r, textoStatus.color.g, textoStatus.color.b, 0);
        ruedas[0].color = new Color(ruedas[0].color.r, ruedas[0].color.g, ruedas[0].color.b, 0);
        ruedas[1].color = new Color(ruedas[1].color.r, ruedas[1].color.g, ruedas[1].color.b, 0);
    
    }



}
