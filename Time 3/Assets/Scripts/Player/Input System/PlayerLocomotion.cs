using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManeger inputManeger;
    Vector3 moveDirection;
    Transform cameraObj;
    Rigidbody playerRigidbody;

    public float movementSpeed = 7;
    public float steathSpeed = 5;
    public float rotationSpeed = 15;
    public bool isSteath = false;

    private void Awake()
    {
        inputManeger = GetComponent<InputManeger>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObj = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
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
}
