using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciarExplicacion : MonoBehaviour
{
    
    [SerializeField] private GameLogic gameLogic = null;
    [SerializeField] private CharlaManager charlaManager = null;
    [SerializeField] private string keyLocalization = "";
    [SerializeField] private BoxCollider colider = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        { 
            colider.enabled = false;
            charlaManager.AparecerCharlaExplicacion(keyLocalization);

        }

    }


}
