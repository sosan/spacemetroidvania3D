using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;
using ControlesGlobal;
using ModuloEscritura.EscrituraTextMeshPro;
using UnityEngine.InputSystem;
using Unity.RemoteConfig;


public class GameLogic : MonoBehaviour
{

    private Controles inputActions;

    [Header("Managers")]
    [SerializeField] private TextoNuevaZona textoNuevaZona = null;
    [SerializeField] private SceneLoaderCommander sceneLoader = null;


    [Header("Animaciones")]
    [SerializeField] private Animation animJuego = null;
    [SerializeField] private Animation animHud = null;

    [Header("Player")]
    [SerializeField] private GameObject player = null;
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private Transform playerPositionInitial = null;

    



    [Header("Paneles Charla")]
    
    [SerializeField] private Animation animBoton = null;
    [SerializeField] TypeWriter escrituraPanel1 = null;
    [SerializeField] TypeWriter escrituraPanel2 = null;
    [SerializeField] private GameObject charlaPanel1 = null;
    [SerializeField] private GameObject charlaPanel2 = null;

    [SerializeField] private TextMeshProUGUI textoCharlaPanel1 = null;
    [SerializeField] private TextMeshProUGUI textoCharlaPanel2 = null;

    [SerializeField] private TextMeshProUGUI textoBotonCharla = null;
    [SerializeField] private GameObject botonSiguiente = null;

    private Dictionary<string, List<string>> textoCharlas = new Dictionary<string, List<string>>()
    { 
        {
            "key_parte_1", new List<string> 
            { 
                "key_parte_1_trozo_1", 
                "key_parte_1_trozo_2", 
                "key_parte_1_trozo_3", 
            }

        },
        { 
            "key_parte_2", new List<string> 
            { 
                "key_parte_2_trozo_1", 
                "key_parte_2_trozo_2", 
                "key_parte_2_trozo_3", 
            }
        
        }
        
        
        
        
        
        

    
    };


    public bool isInCharla = false;
    public bool isCharlaEmpezada = false;
    private List<string> valoresCharla = new List<string>();
    private ushort countPartesCharla = 1;
    private ushort countTrozosCharla = 1;

    private float durationCharlaEntrada = 0.85f;
    private float durationCharlaSalida = 0.85f;
    
    
    
    private ushort fase = 1;
    private float durationInitNivel = 0;
    private bool isShowingEscapePanel = false;


    public struct userAttributes
    {

    }

    public struct appAttributes
    {

    }


    private void Awake()
    {
        
        ConfigManager.FetchCompleted += ApplyRemoteSettings;

        ConfigManager.SetEnvironmentID("6e6486d7-889e-40a1-882e-f37c4a7aa3a9");
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());



        durationInitNivel = animJuego.GetClip("inicio_nivel").length * 1000;

    }

    private void ApplyRemoteSettings(ConfigResponse obj)
    {
        
        print("obj=" + obj.requestOrigin);
        switch (obj.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values for " + this + ".");
            break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session for " + this + ".");
            break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly for " + this + ".");
                isDebug = ConfigManager.appConfig.GetBool("isDebug");
            break;
        }


    }

    private void Start()
    {
        

        //jl. provisional
        InitGame();


    }



    public bool isDebug = true;

    public async void InitGame()
    { 
        
        if (isDebug == false)
        { 
            player.transform.position = playerPositionInitial.position;
        
        }

        playerMovement.ResetHealth();
        
        textoNuevaZona.ShowTextoAparecer("fase" + fase, "zona1", 1000);
        animJuego.Play("inicio_nivel");
        await UniTask.Delay(TimeSpan.FromMilliseconds( durationInitNivel));
        
    
    }


    public async void ShowZona(string fase, string zona)
    { 
        textoNuevaZona.ShowTextoAparecer(fase, zona, 1000);
        await UniTask.Delay(TimeSpan.FromMilliseconds( durationInitNivel));
    
    
    }



    



}
