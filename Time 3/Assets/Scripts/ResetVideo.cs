using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ResetVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
   private void Awake() {
       videoPlayer.time = 0;
    }
}
