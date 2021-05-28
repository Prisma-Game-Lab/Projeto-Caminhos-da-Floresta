using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRadius : MonoBehaviour
{
    public float maxRadius;
    private Animator animator;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        InRadius(this.transform, maxRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    public static void InRadius(Transform checkingObject, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform.gameObject.tag == "Creature")
                {
                    Vector3 directionbetween = (overlaps[i].transform.position - checkingObject.position).normalized;
                    directionbetween.y *= 0; //Zerando o Y do vetor da posi��o entre o inimigo e o objeto a ser checado para ignorar o fator de altura

                    Ray ray = new Ray(checkingObject.position, overlaps[i].transform.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
                        if (hit.transform.gameObject.tag == "Creature")
                        {
                            /* A cria est� dentro do raio */

                            //Debug.Log("A craitura est� dentro do raio");
                            if (Input.GetMouseButtonDown(0))
                            {
                                checkingObject.gameObject.GetComponent<ObjectCollider>().objectList.Add("Offering");

                                hit.transform.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
}
