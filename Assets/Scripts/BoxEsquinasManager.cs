using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public enum TipoEsquina
{ 
    ninguna,
    izquierdaAbajo,
    izquierdaArriba,
    derechaArriba,
    derechaAbajo
    
}

public class BoxEsquinasManager : MonoBehaviour
{

   

    [SerializeField] private TipoEsquina tipoEsquina = TipoEsquina.ninguna;
    [SerializeField] private Rigidbody rigidPlayer = null;
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private PlayerMagnetic playerMagnetic = null;

    [SerializeField] public SphereCollider izquierdaAbajo = null;
    [SerializeField] public SphereCollider izquierdaArriba = null;
    [SerializeField] public SphereCollider derechaArriba = null;
    [SerializeField] public SphereCollider derechaAbajo = null;



    private void Awake()
    {
        
        //izquierdaAbajo.enabled = true;
        //izquierdaArriba.enabled = false;
        //derechaArriba.enabled = false;
        //derechaAbajo.enabled = false;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    

    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.CompareTag("CajaCollider") == true)
        { 
# if UNITY_EDITOR
            //print("BOX ESQUINAS - tag=" + other.tag  +  " name=" + other.name  + " tipoesquina=" + tipoEsquina);
# endif
            playerMagnetic.isCollideWithMagnetic = true;
            
            playerMagnetic.tipoEsquina = tipoEsquina;

            if (playerMovement.isMagnetic == true)
            { 
                switch (tipoEsquina)
                {
                    case TipoEsquina.izquierdaAbajo: playerMagnetic.IzquierdaAbajo(); break;
                    case TipoEsquina.izquierdaArriba: playerMagnetic.IzquierdaArriba(); break;
                    case TipoEsquina.derechaArriba: playerMagnetic.DerechaArriba(); break;
                    case TipoEsquina.derechaAbajo: playerMagnetic.DerechaAbajo(); break;

                }

            
            
            }
        }



    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CajaCollider") == true)
        { 
            playerMagnetic.isCollideWithMagnetic = false;

            //if (tipoEsquina == TipoEsquina.derechaAbajo)
            //{ 
                

            
            //}

            
        }
    }
    

}
