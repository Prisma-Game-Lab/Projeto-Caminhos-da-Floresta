using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCutscene : MonoBehaviour
{
    public VideoPlayerManager videoPlayer;
    public string nextScene;
    void Start()
    {
        videoPlayer.PlayStartThenChangeScene(nextScene);
    }
}
