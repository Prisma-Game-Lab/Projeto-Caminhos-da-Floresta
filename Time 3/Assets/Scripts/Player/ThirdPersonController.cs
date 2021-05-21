using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public Animator anim;

    public float speed = 6f;
    public float stealthSpeed = 2f;
    public float turnSmoopthTime = 0.1f;
    float turnSmoopthVelocity;

    /* Variaveis para gravidade e pulo */
    public float gravity = -14f;
    public float jumpForce = 14f;
    public float groundDistance = 0.4f;
    Vector3 velocity;
    public LayerMask groundMask;
    public Transform groundCheck;

    [HideInInspector]
    public bool isGrounded;

    [HideInInspector]
    public bool isMoving;

    FMOD.Studio.EventInstance flautaSound;

    void Awake()
    {
        flautaSound = FMODUnity.RuntimeManager.CreateInstance("event:/flauta");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        isMoving = false;
        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;
            /* Rotarionar o persongaem para a direção em que ele está indo */
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoopthVelocity, turnSmoopthTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDir.normalized * stealthSpeed * Time.deltaTime);
                anim.SetBool("Stealth", true);
                anim.SetBool("Walking", false);
            }

            else
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
                anim.SetBool("Walking", true);
                anim.SetBool("Stealth", false);
            }

        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Stealth", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            anim.SetTrigger("Jump");
        }

        if(Input.GetMouseButtonDown(0)){
            anim.SetTrigger("Usar_flauta");
            flautaSound.start();
        }

    }
}
