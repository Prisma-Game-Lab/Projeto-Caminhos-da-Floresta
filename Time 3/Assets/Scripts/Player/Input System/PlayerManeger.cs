using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    InputManeger inputManeger;
    CameraManeger cameraManeger;
    PlayerLocomotion playerLocomotion;
    // Start is called before the first frame update
    void Start()
    {
        inputManeger = GetComponent<InputManeger>();
        cameraManeger = FindObjectOfType<CameraManeger>();
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

    private void LateUpdate()
    {
        cameraManeger.HandleAllCameraMovement();
    }
}
