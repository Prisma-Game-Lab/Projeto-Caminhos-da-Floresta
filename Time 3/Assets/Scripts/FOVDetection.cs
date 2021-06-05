using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FOVDetection : MonoBehaviour
{
    public Transform player;
    public float maxAngle;
    public float maxRadius;

    public float multiplyBy;
    private NavMeshAgent _navMeshAgent;

    public bool isInFov = false; //is in Field of Vision

    private void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        isInFov = InFOV(player, maxAngle, maxRadius);

        if(isInFov == true)
        {
            Debug.Log("O jogador est� no campo de vis�o");
            transform.gameObject.GetComponent<CreaturePatrol>().flee = true;
            //RunFrom();
            Flee();
            transform.gameObject.GetComponent<CreaturePatrol>().flee = false;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, maxRadius);

    //     Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
    //     Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawRay(transform.position, fovLine1);
    //     Gizmos.DrawRay(transform.position, fovLine2);

    //     if (!isInFov)
    //         Gizmos.color = Color.red;
    //     else
    //         Gizmos.color = Color.green;
    //     Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

    //     Gizmos.color = Color.black;
    //     Gizmos.DrawRay(transform.position, transform.forward);
    // }

    public bool InFOV (Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(transform.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {
            if(overlaps[i] != null)
            {
                if(overlaps[i].transform == target)
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
                }
            }
        }
        return false;
    }

    public void RunFrom()
    {
        // store the starting transform
        Transform startTransform = transform;

        //temporarily point the object to look away from the player
        transform.rotation = Quaternion.LookRotation(transform.position - player.position);

        //Then we'll get the position on that rotation that's multiplyBy down the path (you could set a Random.range
        // for this if you want variable results) and store it in a new Vector3 called runTo
        Vector3 runTo = transform.position + transform.forward * multiplyBy;

        //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.

        NavMeshHit hit;    // stores the output in a variable called hit

        // 5 is the distance to check, assumes you use default for the NavMesh Layer name
        NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Default"));
        // just used for testing - safe to ignore
        float nextTurnTime = Time.time + 5;

        // reset the transform back to our start transform
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;

        // And get it to head towards the found NavMesh position
        _navMeshAgent.SetDestination(hit.position);
    }

    public void Flee()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(distance < maxRadius)
        {
            Vector3 dirToPlayer = transform.position - player.transform.position;

            Vector3 newPos = transform.position + dirToPlayer;

            _navMeshAgent.SetDestination(newPos);
        }
    }
}
