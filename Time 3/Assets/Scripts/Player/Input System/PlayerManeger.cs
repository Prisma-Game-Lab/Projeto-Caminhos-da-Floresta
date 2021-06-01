using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    InputManeger inputManeger;
    CameraManeger cameraManeger;
    Animator anim;
    PlayerLocomotion playerLocomotion;

    //public bool isInteracting;

    void Start()
    {
        inputManeger = GetComponent<InputManeger>();
        cameraManeger = FindObjectOfType<CameraManeger>();
        anim = GetComponent<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        inputManeger.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManeger.HandleAllCameraMovement();
        playerLocomotion.isInteracting = anim.GetBool("isInteracting");
        playerLocomotion.isJumping = anim.GetBool("isJumping");
        anim.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}
