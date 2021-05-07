using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    InputManeger inputManeger;
    PlayerLocomotion playerLocomotion;
    // Start is called before the first frame update
    void Start()
    {
        inputManeger = GetComponent<InputManeger>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        inputManeger.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }
}
