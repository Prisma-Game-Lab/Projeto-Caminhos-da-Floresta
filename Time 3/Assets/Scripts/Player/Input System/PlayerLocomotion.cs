using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    Animator anim;
    InputManager inputManager;
    Vector3 moveDirection;
    Transform cameraObj;
    Rigidbody playerRigidbody;

    [Header("Movement Flags")]
    public bool isSteath = false;
    public bool isGrounded;
    public bool isJumping;
    public bool isInteracting;
    public bool isplayingFlute;

    [Header("Movement Speeds")]
    public float movementSpeed = 7;
    public float steathSpeed = 5;
    public float rotationSpeed = 15;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float raycastHeighOffSet = 0.5f;
    public LayerMask groundLayer;

    [Header("Jump Speeds")]
    public float jumpHeigh = 3;
    [Tooltip("Precisa ser negativa")]
    public float gravityIntensity = -15;



    private void Awake()
    {
        anim = GetComponent<Animator>();
        Assert.IsNotNull(anim);
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObj = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;
        if (isInteracting)
            return;

        moveDirection = cameraObj.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObj.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        anim.SetBool("Walking",moveDirection.sqrMagnitude > 0);
        anim.SetBool("Stealth",isSteath);

        if(isSteath == true)
        {
            moveDirection = moveDirection * steathSpeed;
        }
        else
        {
            moveDirection = moveDirection * movementSpeed;
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;
        if (isInteracting)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObj.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObj.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + raycastHeighOffSet;


        inAirTimer = inAirTimer + Time.deltaTime;
        playerRigidbody.AddForce(transform.forward * leapingVelocity);
        playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);


        //if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out RaycastHit hit, groundLayer))
        if(Physics.Raycast(rayCastOrigin, -Vector3.up, 1.2f, groundLayer))
        {
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void HandleJumping()
    {
        if (isGrounded && !isInteracting)
        {
            anim.SetTrigger("jump");
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeigh);
            Vector3 playerVelovity = moveDirection;
            playerVelovity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelovity;
        }
    }

    public void HandleInteracting()
    {
        isInteracting = true;
        anim.SetTrigger("pickup");
    }

    public void HandleDelivering()
    {
        isInteracting = true;
        anim.SetTrigger("deliver");
    }

    public void HandleFlute()
    {
        isplayingFlute = true;
        anim.SetTrigger("playMelody");
    }
}
