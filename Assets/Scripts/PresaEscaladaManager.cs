using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresaEscaladaManager : MonoBehaviour
{

    [SerializeField] private Animation anim = null;

    //[SerializeField] private SpriteRenderer selectedRender = null;

    private void Awake()
    {
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ActivarSelectedRender()
    { 
        anim.Play("puntero_presa");
    
    }

    public void DesActivarSelectedRender()
    { 
        anim.Play("puntero_desactivar");
    
    }

    //private void OnTriggerExit(Collider other)
    //{
        
    //    if (other.CompareTag("Player"))
    //    { 
    //        DesActivarSelectedRender();
            
        
    //    }


    //}




}
