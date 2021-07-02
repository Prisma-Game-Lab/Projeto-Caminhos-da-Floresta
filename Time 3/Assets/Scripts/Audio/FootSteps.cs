using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class FootSteps : MonoBehaviour
{
    private TerrainDetector terrainDetector;
    FMOD.Studio.EventInstance footSteps;
    //private ThirdPersonController playerController;
    private bool canPlayFootSteps = true;

    public float footStepsCooldownTime;
    private PlayerLocomotion locomotion;

    private void Awake()
    {
        //playerController = gameObject.GetComponent<ThirdPersonController>();
        terrainDetector = new TerrainDetector();
        footSteps = FMODUnity.RuntimeManager.CreateInstance("event:/footSteps");
        locomotion = GetComponent<PlayerLocomotion>();
        Assert.IsNotNull(locomotion);
        //footSteps.start();

    }

    private void Step(int mode) //chamada pelo animator
    {
        if (locomotion.isGrounded)
        {
            int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);
            int parameterValue = textureIndexToParameterValue(terrainTextureIndex);

            if (canPlayFootSteps)
            {
                footSteps.setParameterByName("terrain", parameterValue);
                footSteps.setParameterByName("mode", mode);
                footSteps.start();
            }
            StartCoroutine(FootStepsCooldown());
        }
    }

    private IEnumerator FootStepsCooldown()
    {
        canPlayFootSteps = false;
        yield return new WaitForSeconds(footStepsCooldownTime);
        canPlayFootSteps = true;
    }

    private int textureIndexToParameterValue(int terrainTextureIndex)
    {
        /*
        No FMOD:
        0 - grama
        1 - terra
        2 - cascalho
        3 - pedra
        4 - folhas
        5 - troncos
        6 - agua
        */

        switch (terrainTextureIndex)
        {
            case 0:
                return 0;
            case 1:
                return 3;
            case 2:
                return 6;
            case 3:
                return 2;
            case 4:
                return 5;
            case 5:
                return 1;
            case 6:
                return 4;
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