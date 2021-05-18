using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManeger : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManeger animManeger;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool stealth_Input;

    private void Awake()
    {
        animManeger = GetComponent<AnimatorManeger>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Stealth.performed += i => stealth_Input = true;
            playerControls.PlayerActions.Stealth.canceled += i => stealth_Input = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandeMovementInput();
        HandleSteathInput();
        //HandleJumpInput();
        //handleActionInput();
    }

    private void HandeMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animManeger.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSteath);
    }

    private void HandleSteathInput()
    {
        if (stealth_Input)
        {
            playerLocomotion.isSteath = true;
        }
        else
        {
            playerLocomotion.isSteath = false;
        }
    }
}
