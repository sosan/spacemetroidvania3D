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
        print("other.tag=" + other.tag + " name=" + other.name);
#endif
        if (other.CompareTag("Player"))
        { 
                    
            var player = other.GetComponentInParent<PlayerMovement>();
            if (player == null) return;

			player.canMove = false;
            player.rigid.useGravity = false;
            player.rigid.velocity = Vector3.zero;
            if (player.isFacingRight == true)
            { 
                player.muerteRobot.Play();
            }
            else
            { 
                player.muerteRobotExterior.Play();
            }

            await UniTask.Delay(TimeSpan.FromMilliseconds(1500));

            player.transition.EnterTransition();
            player.SetHealth(100);
            await UniTask.Delay(800);

            player.isTeleporting = true;
            player.transform.position = positionReinicioPlayer.position;
            

            player.transition.ExitTransition();
            await UniTask.Delay(200);

            player.rigid.useGravity = true;
            player.isTeleporting = false;
			player.canMove = true;

        }

        if (other.CompareTag("CajaSuelo") == true || other.CompareTag("CajaSuelta") == true) 
        { 

            await UniTask.Delay(TimeSpan.FromMilliseconds(700));
            var junta1 = other.GetComponentInParent<FixedJoint>();
            if (junta1 != null)
            { 
            
                var junta2 = junta1.connectedBody;
                if (junta2 != null)
                { 
                    junta2.GetComponent<BoxManager>().DestruirBox(10);
                
                
                }
                junta1.GetComponent<BoxManager>().DestruirBox(10);
            
            }


            other.GetComponentInParent<BoxManager>().DestruirBox(10);
        
        }

    }

}
