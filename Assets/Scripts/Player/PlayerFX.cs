using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{

    [Header("Flamete")]
    [SerializeField] private ParticleSystem flamete = null;

    [Header("Muerte FX")]
    [SerializeField] public ParticleSystem muerteRobot = null;
    [SerializeField] public ParticleSystem muerteRobotExterior = null;

    [Header("Gravedad FX")]
    [SerializeField] public ParticleSystem gravedadFX = null;

    [Header("Polvo Tocar Suelo FX")]
    [SerializeField] private ParticleSystem polvoRobot = null;

    [Header("Giro Sierra FX")]
    [SerializeField] private ParticleSystem sierraFX = null;

    [Header("Impulsores")]
    [SerializeField] public ParticleSystem propulsion = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

   
    public void PlayFlamete()
    { 
        if (flamete.isPlaying == false)
        { 
        
            flamete.Play();
        
        }
    
    
    }

    public void StopFlamete()
    { 
        print("parado flamete");
        flamete.Stop();
    }


}
