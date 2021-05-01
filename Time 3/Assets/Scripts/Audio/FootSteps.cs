using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private TerrainDetector terrainDetector;
    FMOD.Studio.EventInstance footSteps;
    //private ThirdPersonController playerController;

    private void Awake()
    {
        //playerController = gameObject.GetComponent<ThirdPersonController>();
        terrainDetector = new TerrainDetector();
        footSteps = FMODUnity.RuntimeManager.CreateInstance("event:/footSteps");
        //footSteps.start();
    }

    private void Step() //chamada pelo animator
    {
        //Debug.Log("PISOU");
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);
        int parameterValue = textureIndexToParameterValue(terrainTextureIndex);
        footSteps.setParameterByName("terrain", parameterValue);
        footSteps.start();
    }

    private int textureIndexToParameterValue(int terrainTextureIndex)
    {
        switch (terrainTextureIndex){
            case 0:
                return 1;
            case 1:
                return 6;
            case 2:
                return 0;
            case 3:
                return 2;
            case 4:
                return 4;
            case 5:
                return 3;
            case 6:
                return 5;
            default:
                return 0;
        }
    }

   /* private void Update() {
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
    }*/
}