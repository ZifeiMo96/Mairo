using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;

    PlayerControl control;
    
    //Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        control = GetComponent<PlayerControl>();
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(control.xVelocity));
        anim.SetBool("isOnGround", control.isOnGround);
        anim.SetBool("isJump", control.isJump);
        anim.SetBool("isLevelUp", control.isLevelUp);
        anim.SetBool("isCrouch", control.isCrouch);
        anim.SetBool("isFlash", control.isFlash);
        anim.SetBool("isDie", control.isDie);


    }
}
