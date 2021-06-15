using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManeger : MonoBehaviour
{
    public Animator anim;
    int horizontal;
    int vertical;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayeTargetAnimation(string targetAnimation, bool isInteracting)
    {
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSteath)
    {

        if( horizontalMovement >= 0.5 || horizontalMovement <= -0.5 || verticalMovement >= 0.5 || verticalMovement <= -0.5){
            anim.SetBool("Walking", true);
        }
        else{
            anim.SetBool("Walking", false);
        }

        if(isSteath){

            anim.SetBool("Stealth", true);
        }
        else{
            anim.SetBool("Stealth", false);
        }
    }
}
