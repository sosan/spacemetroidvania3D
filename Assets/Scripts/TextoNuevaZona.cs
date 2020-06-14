using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using System;
using TMPro;
using ControlesGlobal;
using UnityEngine.InputSystem;

public class TextoNuevaZona : MonoBehaviour
{

    private Controles inputActions;

    [SerializeField] private Animation animHud = null;
    [SerializeField] private ParticleSystem brilloAparecer = null;
    [SerializeField] private ParticleSystem[] fondo = null;
    [SerializeField] private TextMeshProUGUI texto = null;
    [SerializeField] private GameObject textoNivelGO = null;
    [SerializeField] private GameObject panelGo = null;

    [SerializeField] private TextMeshProUGUI textoZonaNivelFijo = null;

    private float durationBrilloAparecerMs = 0;

    private void Awake()
    {


        textoNivelGO.SetActive(false);
        panelGo.SetActive(false);

        durationBrilloAparecerMs = brilloAparecer.main.duration * 1000;
        textoZonaNivelFijo.text = "";
        texto.text = "";
    }



    // Start is called before the first frame update
    private void Start()
    {
        
        

    }

    
   

    public async void ShowTextoAparecer(string keyFase, string keyZona, int tiempoAparecer)
    { 
        
        textoZonaNivelFijo.text = Localization.Get(keyFase) + " - " + Localization.Get(keyZona);

        texto.text = "-- " + Localization.Get(keyFase) + " --<br>" + Localization.Get(keyZona);
        panelGo.SetActive(false);
        
        await UniTask.Delay(1000);
        textoNivelGO.SetActive(true);
        brilloAparecer.Play();
        await UniTask.Delay( TimeSpan.FromMilliseconds(durationBrilloAparecerMs+ 60) );

        animHud.Play("texto_zona_entrada_in");
        fondo[0].Play();
        fondo[1].Play();
        fondo[2].Play();
        
        await UniTask.Delay(tiempoAparecer);
        fondo[0].Stop();
        fondo[1].Stop();
        fondo[2].Stop();
        animHud.Play("texto_zona_entrada_out");


    }

}
