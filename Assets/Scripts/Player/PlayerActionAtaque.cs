using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionAtaque : MonoBehaviour
{

    [SerializeField] private SphereCollider sierraCollider = null;
    [SerializeField] private ParticleSystem sierraFX = null;
    [SerializeField] public bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

   
    public void Ataque_started()
    {

        isAttacking = true;
        sierraCollider.enabled = true;
        sierraFX.Play();


    }

    public void Ataque_canceled()
    {
       
        isAttacking = false;
        sierraCollider.enabled = false;
        sierraFX.Stop();
    }


}
