using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private TerrainDetector terrainDetector;
    FMOD.Studio.EventInstance footSteps;
    private ThirdPersonController playerController;

    private void Awake()
    {
        playerController = gameObject.GetComponent<ThirdPersonController>();
        terrainDetector = new TerrainDetector();
        footSteps = FMODUnity.RuntimeManager.CreateInstance("event:/footSteps");
        //footSteps.start();
    }

    private void Step() //chamada pelo animator
    {
        
    }
    private void Update() {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);
        footSteps.setParameterByName("terrain", terrainTextureIndex);

        FMOD.Studio.PLAYBACK_STATE state;
        footSteps.getPlaybackState(out state);

        if (playerController.isMoving && playerController.isGrounded)
        {
            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
                footSteps.start();
        }
        else
        {
            if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                footSteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        //Debug.Log(terrainTextureIndex);
    }
}