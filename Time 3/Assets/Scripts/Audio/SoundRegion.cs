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
        player = GameObject.FindGameObjectWithTag("Player");
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
            PlayAmbience();
        }
    }

    void PlayAmbience()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        ambience.getPlaybackState(out state);

        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
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
                return "event:/ambiencia/0 - aberto";
            case 1: //aberto + rio
                return "event:/ambiencia/1 - aberto + rio";
            case 2: //fechado
                return "event:/ambiencia/2 - fechado";
            case 3: //vento arvore
                return "event:/ambiencia/3 - vento arvore";
            case 4: //ventro montanha
                return "event:/ambiencia/4 - vento montanha";
            case 5: //noite
                return "event:/ambiencia/5 - noite";
            case 6: //sinistro
                return "event:/ambiencia/6 - creepy";
            case 7: //coruja
                return "event:/ambiencia/7 - coruja";
            case 8: //sapo
                return "event:/ambiencia/8 - sapo";
            default:
                Debug.Log("evento de ambiencia nao encontrado!");
                return "event:/ambiencia/aberto";
        }
    }


}
