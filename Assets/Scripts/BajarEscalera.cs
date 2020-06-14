using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using System;

public class BajarEscalera : MonoBehaviour
{


    [SerializeField] private GameLogic gameLogic = null;
    [SerializeField] private GameObject player = null;
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private bool isSiguienteNivel = true;
    [SerializeField] private Transform posicionFinal = null;
    [SerializeField] private bool isBajada = false;
    [SerializeField] private bool isFinalDestinationFacingRight = false;

    [SerializeField] private Transform giroPlayer = null;
    [SerializeField] private Vector3 izquierda = new Vector3(0, 180, 0);
    [SerializeField] private Vector3 derecha = new Vector3(0, 0, 0);

    [SerializeField] private string fase = "";
    [SerializeField] private string zona = "";
    


    // Start is called before the first frame update
    void Start()
    {
        
    }


    private async void OnTriggerEnter(Collider other)
    {
    
        //print("other name=" + other.name + "tag=" + other.tag);

        if (other.CompareTag("Player"))
        { 

        
            if (isSiguienteNivel && playerMovement.isTeleporting == false)
            { 
                
                if (isBajada == true)
                { 

                    player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, -30f);
                }

                
                playerMovement.isTeleporting = true;

                await UniTask.Delay(TimeSpan.FromMilliseconds(500));

                playerMovement.rigid.useGravity = false;
                playerMovement.rigid.velocity = Vector3.zero;
            
            
                playerMovement.transition.EnterTransition();
                await UniTask.Delay(600);

                
                player.transform.position = posicionFinal.position;

                if (isFinalDestinationFacingRight == true)
                {

                    player.transform.rotation = Quaternion.Euler(derecha);
                    playerMovement.isFacingRight = true;
                }
                else
                {
                     player.transform.rotation = Quaternion.Euler(izquierda);
                    playerMovement.isFacingRight = false;

                }


                playerMovement.transition.ExitTransition();
                await UniTask.Delay(350);

                playerMovement.rigid.useGravity = true;
                gameLogic.ShowZona(fase, zona);

                await UniTask.Delay(500);

                playerMovement.bajarEscalera = true;
                await UniTask.Delay(1000);
                playerMovement.bajarEscalera = false;
                
                playerMovement.isTeleporting = false;
                

            
            
            }

        }


    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        { 
            player.transform.rotation = Quaternion.Euler(
                player.transform.rotation.x,
                player.transform.rotation.y,
                0
                );
        
        
        }


    }


}
