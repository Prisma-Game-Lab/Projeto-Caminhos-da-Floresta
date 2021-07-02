using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;

public class FOVDetection : MonoBehaviour
{
    public Transform player;
    public float maxAngle;
    public float maxRadius;

    public float multiplyBy;

    public bool isInFov = false; //is in Field of Vision

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Assert.IsNotNull(player, "couldnt find player");
    }

    public void Update()
    {
        isInFov = InFOV(player, maxAngle, maxRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (!isInFov)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    public bool InFOV (Transform target, float maxAngle, float maxRadius)
    {



        Vector3 directionbetween = (target.position - transform.position).normalized;
        directionbetween.y *= 0; //Zerando o Y do vetor da posi��o entre o inimigo e o objeto a ser checado para ignorar o fator de altura

        float angle = Vector3.Angle(transform.forward, directionbetween);
        if(angle <= maxAngle)
        {
            Ray ray = new Ray(transform.position, target.position - transform.position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, maxRadius))
            {
                if(hit.transform == target)
                {
                    /* o jogador est� dentro do campo de vis�o */
                    return true;
                }
            }
        }

        return false;
    }

}
