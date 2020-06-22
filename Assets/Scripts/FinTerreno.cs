using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using System;

public class FinTerreno : MonoBehaviour
{

    [SerializeField] private Transform positionReinicioPlayer = null;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    private async void OnTriggerEnter(Collider other)
    {

# if UNITY_EDITOR
        //print("other.tag=" + other.tag + " name=" + other.name);
#endif
        if (other.CompareTag("Player"))
        { 
                   
            var player = other.GetComponentInParent<PlayerMovement>();
            if (player == null) return;
            if (player.isPlayerFalling == true) return;

            //if (player.ropeManager.isPresaSelected == true)
            //{ 
                
            //    player.ropeManager.QuitarGarfioColisionadoHueco();
            //}

            player.ropeManager.QuitarGarfioColisionadoHueco();

            player.isPlayerFalling = true;
            player.isTeleporting = true;
			player.canMove = false;
            player.rigid.useGravity = true;
            player.rigid.velocity = Vector3.zero;

            player.giroPlayer.gameObject.SetActive(false);
            if (player.isFacingRight == true)
            { 
                player.muerteRobot.Play();
               
            }
            else
            { 
                player.muerteRobotExterior.Play();
            }

            player.playerActionCogerObjecto.CancelarObjetoCogido();

            await UniTask.Delay(TimeSpan.FromMilliseconds(800));

            player.transition.EnterTransition();
            player.SetHealth(100);

            await UniTask.Delay(TimeSpan.FromMilliseconds(300));
            player.transform.position = positionReinicioPlayer.position;
            player.rigid.constraints = RigidbodyConstraints.FreezeRotation;
            
			player.rigid.useGravity = true;

            await UniTask.Delay(1000);

           
            
            
            


            player.transition.ExitTransition();
            //await UniTask.Delay(200);
            player.canMove = true;
            
            player.isTeleporting = false;
            player.giroPlayer.gameObject.SetActive(true);
            player.isPlayerFalling = false;
            


        }

        if (other.CompareTag("CajaSuelo") == true || other.CompareTag("CajaSuelta") == true) 
        { 

            await UniTask.Delay(TimeSpan.FromMilliseconds(300));
            var box0 = other.GetComponentInParent<BoxManager>();
            if (box0 != null)
            { 
                //box0.rigid.useGravity = true;
                if (box0.linkedBox != null)
                { 
                    var box1 = box0.linkedBox.GetComponent<BoxManager>();
                    if (box1.linkedBox != null)
                    { 

                        //box1.rigid.useGravity = true;
                        var box2 = box1.linkedBox.GetComponent<BoxManager>();
                        if (box2 != null)
                        {
                            box2.rigid.useGravity = true;
                            box2.DestruirBox(10);

                        }

                        

                    }
                    box1.DestruirBox(10);
                
                }

                box0.DestruirBox(10);
            
            }

            

            //var junta1 = other.GetComponentInParent<FixedJoint>();
            //if (junta1 != null)
            //{ 
            
            //    var junta2 = junta1.connectedBody;
            //    if (junta2 != null)
            //    { 
            //        junta2.GetComponent<BoxManager>().DestruirBox(10);
                
                
            //    }
            //    junta1.GetComponent<BoxManager>().DestruirBox(10);
            
            //}


            //other.GetComponentInParent<BoxManager>().DestruirBox(10);
        
        }

    }

}
