using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionInicial : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private Transform giroPlayer = null;
    [SerializeField] private Vector3 izquierda = new Vector3(0, 180, 0);
    [SerializeField] private Vector3 derecha = new Vector3(0, 0, 0);
    [SerializeField] private bool isDerecha = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        { 

            if (isDerecha == true)
            { 
            
                giroPlayer.rotation = Quaternion.Euler(derecha);
                playerMovement.isFacingRight = true;
            }
            else
            { 
                giroPlayer.rotation = Quaternion.Euler(izquierda);
                playerMovement.isFacingRight = false;
            
            }

        
        
        }


    }

    

   
}
