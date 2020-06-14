using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;
using ControlesGlobal;
using ModuloEscritura.EscrituraTextMeshPro;
using UnityEngine.InputSystem;

public class CharlaManager : MonoBehaviour
{

    private Controles inputActions;

    [SerializeField] private Animation animHud = null;


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
    

    private void OnEnable()
    {

        inputActions.PlayerMovement.activar.Enable();
        inputActions.PlayerMovement.testeo_activar.Enable();

    }


    private void OnDisable()
    {

        inputActions.PlayerMovement.activar.Disable();
        inputActions.PlayerMovement.testeo_activar.Disable();
        

    }



    private void Awake()
    {
        TextTokenizer.RegisterDefaultTokenizer();
        inputActions = new Controles();
        inputActions.PlayerMovement.activar.performed += SiguienteCharla;
        //inputActions.PlayerMovement.testeo_activar.performed += NuevaCharla;
        inputActions.PlayerMovement.activar.Enable();
        //inputActions.PlayerMovement.testeo_activar.Enable();

        charlaPanel1.SetActive(false);
        textoCharlaPanel1.text = "";
        durationCharlaEntrada = animHud.GetClip("charla_entrada1").length;
        durationCharlaSalida = animHud.GetClip("charla_salida1").length;
        
        
    }


    //jl. borrar solo es de testeo para inicar una nueva charla
    private void NuevaCharla(InputAction.CallbackContext obj)
    { 
        
        AparecerCharla1();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //await UniTask.Delay(1000);
        //AparecerNuevaZona();



    }


    private void AparecerNuevaZona()
    { 
        

    
    
    }

    public async void AparecerCharlaExplicacion(string key)
    {
    
        if (isCharlaEmpezada == true)
        { 
        
            await UniTask.Delay(TimeSpan.FromSeconds(4));
            AparecerCharlaExplicacion(key);
            return;
        }

        isCharlaEmpezada = true;

        botonSiguiente.SetActive(false);

        //isInCharla = true;
        animHud.Play("charla_entrada1");
        await UniTask.Delay(TimeSpan.FromSeconds(durationCharlaEntrada));
        

        ShowTextoCharla(key);
        
        animHud.Play("charla1_idle");
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        animBoton.Play("boton_charla_idle");

        await UniTask.Delay(TimeSpan.FromSeconds(5));
        
        animHud.Play("charla_salida1");
        await UniTask.Delay(TimeSpan.FromMilliseconds(200));
        textoCharlaPanel1.text = "";
        botonSiguiente.SetActive(true);
        isCharlaEmpezada = false;
        isInCharla = false;
    
    }


    private async void ShowTextoCharla(string key)
    { 
        string localization = Localization.Get(key);
        if (escrituraPanel1.IsWriting == true)
        {
                
            escrituraPanel1.Skip();
            await UniTask.Delay(200);
            escrituraPanel1.Write(localization);

        }
        else
        { 
            escrituraPanel1.Write(localization);
            
        }

    
    }


    public async void AparecerCharla1()
    { 
        
        isInCharla = true;
        animHud.Play("charla_entrada1");
        await UniTask.Delay(TimeSpan.FromSeconds(durationCharlaEntrada));
        

        ShowTextoCharla();
        
        animHud.Play("charla1_idle");
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        animBoton.Play("boton_charla_idle");
    
    }

    private async void ShowTextoCharla()
    { 

        string key = "key_parte_" + countPartesCharla;

        if (textoCharlas.TryGetValue(key, out valoresCharla))
        { 

            if (countTrozosCharla == valoresCharla.Count )
            { 
                
                textoBotonCharla.text = Localization.Get("fintrozo");

                
            }
            else if (countTrozosCharla > valoresCharla.Count)
            { 


                if (isCharlaEmpezada == true) return;

                if (escrituraPanel1.IsWriting == true)
                {
                
                    escrituraPanel1.Skip();
                    await UniTask.Delay(300);

                }
            
                animHud.Play("charla_salida1");
                await UniTask.Delay(TimeSpan.FromMilliseconds(200));
                isInCharla = false;
                isCharlaEmpezada = true;
                countTrozosCharla = 1;
                textoCharlaPanel1.text = "";
                return;
            
            }
            else
            { 
            
                textoBotonCharla.text = Localization.Get("siguientetrozo");
            
            }
                
            isCharlaEmpezada = false;
            key = valoresCharla[countTrozosCharla - 1];
            string localization = Localization.Get(key);
            if (escrituraPanel1.IsWriting == true)
            {
                
                escrituraPanel1.Skip();
                await UniTask.Delay(200);
                escrituraPanel1.Write(localization);

            }
            else
            { 
                escrituraPanel1.Write(localization);
            
            }
            
            
            
    
        }

        
    
    }
    
    public async void Clicked_SiguienteCharla()
    { 
    
        if (isInCharla == false) return;
        

        if (countTrozosCharla == ushort.MaxValue) countTrozosCharla = 0;

        
        countTrozosCharla++;
        animHud.Play("boton_charla_pulsado");
        await UniTask.Delay(200);
        
        ShowTextoCharla();
    
    }

    public void SiguienteCharla(InputAction.CallbackContext obj)
    { 

        Clicked_SiguienteCharla();
    
    }




}
