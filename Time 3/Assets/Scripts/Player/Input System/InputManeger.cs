using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManeger : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManeger animManeger;

    public Vector2 movementInput;
    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    private void Awake()
    {
        animManeger = GetComponent<AnimatorManeger>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
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
        //HandleJumpInput();
        //handleActionInput();
    }

    private void HandeMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animManeger.UpdateAnimatorValues(0, moveAmount);
    }
}
