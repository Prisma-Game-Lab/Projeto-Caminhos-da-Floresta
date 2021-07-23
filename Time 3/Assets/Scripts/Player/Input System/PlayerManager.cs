using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    Animator anim;
    PlayerLocomotion playerLocomotion;

    //public bool isInteracting;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        anim = GetComponent<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if(Time.timeScale < 1f) return;
        cameraManager.HandleAllCameraMovement();
        playerLocomotion.isInteracting = anim.GetBool("isInteracting");
        playerLocomotion.isJumping = anim.GetBool("isJumping");
        anim.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}
