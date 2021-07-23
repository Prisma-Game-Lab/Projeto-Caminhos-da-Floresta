using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseCanvas;
    public PlayerControls playerControls;
    private bool isPasued = false;
    private void Awake() {
        Assert.IsNotNull(pauseCanvas);
        playerControls = new PlayerControls();
        playerControls.Pause.Pause.performed += OnPause;
        playerControls.Enable();
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("PAUSEEEE");
        isPasued =  !isPasued;
        pauseCanvas.SetActive(isPasued);
        Time.timeScale = isPasued? 0f : 1f;
    }
}
