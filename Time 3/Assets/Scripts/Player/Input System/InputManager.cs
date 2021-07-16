using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool stealth_Input;
    public bool jump_Input;
    public bool interact_Input;
    public bool playFlute_input;

    private void Awake()
    {
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

            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.PlayerActions.Interact.performed += i => interact_Input = true;

            playerControls.PlayerActions.PlayFlute.performed += i => playFlute_input = true;
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
        HandleJumpInput();
        handleActionInput();
    }

    private void HandeMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
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

    private void HandleJumpInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJumping();
        }

    }

    public void HandleInteractInput()
    {
        if (interact_Input)
        {
            interact_Input = false;
            playerLocomotion.HandleInteracting();
        }
    }

    public void HandleDeliverInput(){
        if (interact_Input)
        {
            interact_Input = false;
            playerLocomotion.HandleDelivering();
        }
    }

    public void handleActionInput()
    {
        if (playFlute_input)
        {
            playFlute_input = false;
            playerLocomotion.HandleFlute();
        }
    }
}
