using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRegion : MonoBehaviour
{
    FMOD.Studio.EventInstance ambience;
    public GameObject player;
    public int ambienceType = 0;


    // Start is called before the first frame update
    void Start()
    {
        ambience = FMODUnity.RuntimeManager.CreateInstance(ambienceIntToName());
        //ambience.start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistance();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("entrou");
            PlayAmbience();
        }
    }

    void PlayAmbience()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        ambience.getPlaybackState(out state);

        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            //Debug.Log("tocou");
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(ambience, GetComponent<Transform>(), GetComponent<Rigidbody>());

            ambience.setParameterByName("PlayerDistance", 1);
            ambience.start();
        }
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
        float maxDist = GetComponent<SphereCollider>().radius*transform.localScale.z; //VERIFICAR SE É REALMENTE UMA BOLA
        if (maxDist <= 0.0f)
        {
            Debug.Log("Erro: shablau no raio da esfera!");
            return;
        }

        float dist = DistanceToPlayer()/maxDist;
        //Debug.Log(dist);

        ambience.setParameterByName("PlayerDistance", dist);
    }

    private string ambienceIntToName()
    {
        switch (ambienceType){
            case 0: //aberto
                return "event:/ambiencia/aberto";
            case 1: //aberto + rio
                return "event:/ambiencia/aberto + rio";
            case 2: //fechado
                return "event:/ambiencia/fechado";
            case 3: //vento arvore
                return "event:/ambiencia/vento arvore";
            case 4: //ventro montanha
                return "event:/ambiencia/vento montanha";
            default:
                return "event:/ambiencia/aberto";
        }
    }


}
