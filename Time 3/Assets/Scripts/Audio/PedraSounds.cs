using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedraSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance passo;
    private FMOD.Studio.EventInstance atento;
    private FMOD.Studio.EventInstance fugindo;
    private FMOD.Studio.EventInstance susto;
    public GameObject player;
    public CreaturePatrol creaturePatrolScript;
    private CreaturePatrol.AlertnessLevel currentAlertLevel;

    // Start is called before the first frame update
    void Awake()
    {
        passo = FMODUnity.RuntimeManager.CreateInstance("event:/criaturaPedra/passos");
        atento = FMODUnity.RuntimeManager.CreateInstance("event:/criaturaPedra/atento");
        fugindo = FMODUnity.RuntimeManager.CreateInstance("event:/criaturaPedra/fugindo");
        susto = FMODUnity.RuntimeManager.CreateInstance("event:/criaturaPedra/susto");

        currentAlertLevel = creaturePatrolScript.alertness;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerDistanceParameter();
        CheckAssertLevel();
    }

    private void Step() //chamada pelo animator
    {
        passo.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(passo,  transform.parent.GetComponent<Transform>(),  transform.parent.GetComponent<Rigidbody>());
    }

    private float DistanceToPlayer()
    {
        Vector3 center = transform.position;
        Vector3 playerPos = player.transform.position;

        float dist = Vector3.Distance(center, playerPos);
        return dist;
    }

    private float CalculatePlayerDistanceParameter()
    {
        Transform sphere = transform.Find("som");
        if (sphere == null)
        {
            Debug.Log("xablau na esfera da criatura rochosa");
            return 0.0f;
        }
        float maxDist = sphere.GetComponent<SphereCollider>().radius*sphere.localScale.z; //VERIFICAR SE É REALMENTE UMA BOLA
        if (maxDist <= 0.0f)
        {
            Debug.Log("Erro: shablau no raio da esfera!");
            return 0.0f;
        }

        float dist = DistanceToPlayer()/maxDist;
        //Debug.Log(dist);
        return dist;
    }

    private void UpdatePlayerDistanceParameter()
    {
        float playerDist = CalculatePlayerDistanceParameter();
        passo.setParameterByName("PlayerDistance", playerDist);
        atento.setParameterByName("PlayerDistance", playerDist);
        fugindo.setParameterByName("PlayerDistance", playerDist);
        susto.setParameterByName("PlayerDistance", playerDist);
    }

    private void CheckAssertLevel()
    {
        if (currentAlertLevel != creaturePatrolScript.alertness)
        {
            currentAlertLevel = creaturePatrolScript.alertness;
            //Debug.Log(currentAlertLevel);
            if (currentAlertLevel == CreaturePatrol.AlertnessLevel.suspicious)
            {
                atento.start();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(atento,  transform.parent.GetComponent<Transform>(),  transform.parent.GetComponent<Rigidbody>());

            }
            else if (currentAlertLevel == CreaturePatrol.AlertnessLevel.running)
            {
                fugindo.start();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(fugindo,  transform.parent.GetComponent<Transform>(),  transform.parent.GetComponent<Rigidbody>());
            }
        }
    }

    public void PlaySusto()
    {
        Debug.Log("SUSTO!");
        susto.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(susto, transform.parent.GetComponent<Transform>(),  transform.parent.GetComponent<Rigidbody>());
    }
}
