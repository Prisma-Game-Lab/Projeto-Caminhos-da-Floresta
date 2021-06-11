using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedraSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance passo;
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        passo = FMODUnity.RuntimeManager.CreateInstance("event:/criaturaPedra/passos");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistance();
    }

    private void Step() //chamada pelo animator
    {
        passo.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(passo, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    private float DistanceToPlayer()
    {
        Vector3 center = transform.position;
        Vector3 playerPos = player.transform.position;

        float dist = Vector3.Distance(center, playerPos);
        return dist;
    }

    private void UpdateDistance()
    {
        Transform sphere = transform.Find("som");
        if (sphere == null)
        {
            Debug.Log("xablau na esfera da criatura rochosa");
        }
        float maxDist = sphere.GetComponent<SphereCollider>().radius*sphere.localScale.z; //VERIFICAR SE É REALMENTE UMA BOLA
        if (maxDist <= 0.0f)
        {
            Debug.Log("Erro: shablau no raio da esfera!");
            return;
        }

        float dist = DistanceToPlayer()/maxDist;
        //Debug.Log(dist);
        passo.setParameterByName("PlayerDistance", dist);
    }
}
