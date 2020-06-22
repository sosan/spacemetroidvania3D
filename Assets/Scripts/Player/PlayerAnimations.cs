using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    [SerializeField] private Animator anim = null;
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private PlayerActionAtaque playerActionAtaque = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isTeleporting == true) return;

        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        

        anim.SetBool("isWalking", playerMovement.isWalking);
        anim.SetBool("isGrounded", playerMovement.isGrounded);
        anim.SetFloat("yVelocity", playerMovement.rigid.velocity.y);
        //anim.SetBool("isWallSliding", playerMovement.isWallSliding);
        anim.SetBool("attack1", playerActionAtaque.isAttacking);
        anim.SetBool("isSwinging", playerMovement.isSwinging);

    }

}
