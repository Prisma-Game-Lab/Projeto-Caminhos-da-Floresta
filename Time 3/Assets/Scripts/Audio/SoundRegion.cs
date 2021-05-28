using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRegion : MonoBehaviour
{
    FMOD.Studio.EventInstance ambience;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        ambience = FMODUnity.RuntimeManager.CreateInstance("event:/ambiencia/teste (dist manual)");
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


}
