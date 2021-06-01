using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManeger playerManeger;
    AnimatorManeger animManeger;
    InputManeger inputManeger;
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
        playerManeger = GetComponent<PlayerManeger>();
        animManeger = GetComponent<AnimatorManeger>();
        inputManeger = GetComponent<InputManeger>();
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

        moveDirection = cameraObj.forward * inputManeger.verticalInput;
        moveDirection = moveDirection + cameraObj.right * inputManeger.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

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

        targetDirection = cameraObj.forward * inputManeger.verticalInput;
        targetDirection = targetDirection + cameraObj.right * inputManeger.horizontalInput;
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

        if(isGrounded == false && isJumping == false)
        {

            animManeger.PlayeTargetAnimation("Falling", true);
        }

        inAirTimer = inAirTimer + Time.deltaTime;
        playerRigidbody.AddForce(transform.forward * leapingVelocity);
        playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);

        
        //if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out RaycastHit hit, groundLayer))
        if(Physics.Raycast(rayCastOrigin, -Vector3.up, 1.2f, groundLayer))
        {
            if(!isGrounded)
            {
                animManeger.PlayeTargetAnimation("Land", true);
            }

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
            animManeger.anim.SetBool("isJumping", true);
            animManeger.PlayeTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeigh);
            Vector3 playerVelovity = moveDirection;
            playerVelovity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelovity;
        }
    }

    public void HandleInteracting()
    {
        isInteracting = true;
        animManeger.anim.SetBool("isInteracting", true);
        animManeger.PlayeTargetAnimation("GetItem", false);
    }

    public void HandleFlute()
    {
        isplayingFlute = true;
        animManeger.anim.SetBool("isplayingFlute", true);
        if(inputManeger.verticalInput != 0 && inputManeger.horizontalInput != 0)
            animManeger.PlayeTargetAnimation("IdleFlute", false);
        else
            animManeger.PlayeTargetAnimation("WalkingFlute", false);
    }
}
