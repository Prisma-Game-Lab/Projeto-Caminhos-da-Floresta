using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance flauta;
    private FMOD.Studio.EventInstance pulo;
    void Start()
    {
        flauta = FMODUnity.RuntimeManager.CreateInstance("event:/flauta");
        pulo = FMODUnity.RuntimeManager.CreateInstance("event:/pulo");
    }

    public void PlayFlauta()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        flauta.getPlaybackState(out state);

        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            flauta.start();
        }
    }
    public void PlayPuloCaralho()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        pulo.getPlaybackState(out state);

        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            pulo.start();
        }
    }

}
