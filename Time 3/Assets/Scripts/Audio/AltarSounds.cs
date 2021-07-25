using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarSounds : MonoBehaviour
{
    FMOD.Studio.EventInstance aprox;
    public GameObject player;

    void Start()
    {
        aprox = FMODUnity.RuntimeManager.CreateInstance("event:/altar/aprox");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        UpdateDistance();
        PlayAprox();
    }

    void PlayAprox()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        aprox.getPlaybackState(out state);

        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(aprox, GetComponent<Transform>(), GetComponent<Rigidbody>());
            aprox.start();
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
        aprox.setParameterByName("PlayerDistance", dist);
    }
}
