using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayFlute : MonoBehaviour
{
    public float radius;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider[] overlaps = new Collider[10];
            int count = Physics.OverlapSphereNonAlloc(transform.position, radius, overlaps, 7);
            Assert.IsTrue(count <= 9, $"{count} colisions");
            for (int i = 0; i < count + 1; i++)
            {
                if (overlaps[i] != null)
                {
                    if (overlaps[i].transform.gameObject.CompareTag("Creature"))
                    {
                        Vector3 directionbetween = (overlaps[i].transform.position - transform.position).normalized;
                        directionbetween.y *= 0; //Zerando o Y do vetor da posicao entre o inimigo e o objeto a ser checado para ignorar o fator de altura

                        Ray ray = new Ray(transform.position, overlaps[i].transform.position - transform.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, radius))
                        {
                            if (hit.transform.gameObject.tag == "Creature")
                            {
                                /* A cria esta dentro do raio */

                                transform.gameObject.GetComponent<ObjectCollider>().objectList.Add("Offering");

                                //recado do amiguinho do som: nao usar SetActive(false), dificulta a minha vida. Obrigado!
                                //hit.transform.gameObject.SetActive(false);

                                hit.transform.gameObject.GetComponentInChildren<PedraSounds>().PlaySusto();

                            }
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
