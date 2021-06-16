using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManeger : MonoBehaviour
{
    InputManeger inputManeger;

    [HideInInspector] public Transform targetTransform;           // the target the camera will follow
    [Tooltip("Objeto que é o Pivot da Camera")]
    public Transform cameraPivot;               // objext that is the pivor of the camera
    [Tooltip("Camera principal que ira seguir o player")]
    public Transform cameraTransform;           // transform of the actual camera;
    public LayerMask collisionLayers;       // The layers we want our camera do collide with
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    [Tooltip("Raio da camera antes de colidir com um objeto")]
    public float cameraCollisionRadius = 0.2f;
    [Tooltip("O quanto a camera ira se afastar de um objeto após colidir com ele")]
    public float cameraCollisionOffSet = 0.2f;
    public float minCollisionOffSet = 0.2f;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle;     //Camera looking up and down
    public float pivotAngle;        //camera looking left and right
    public float minPivotAngle = -13;
    public float maxPivotAngle = 35;

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManeger>().transform;
        inputManeger = FindObjectOfType<InputManeger>();
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        lookAngle = lookAngle + (inputManeger.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManeger.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollision()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = targetPosition - (distance - cameraCollisionOffSet);
        }
        
        if(Mathf.Abs(targetPosition) < minCollisionOffSet)
        {
            targetPosition = targetPosition - minCollisionOffSet;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
