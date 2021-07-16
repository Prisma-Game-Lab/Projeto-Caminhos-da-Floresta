using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private void Awake()
    {
        videoPlayer.time = 0;
    }

    public void QueueSceneChange(string sceneName)
    {
        videoPlayer.Play();
        StartCoroutine(ChangeSceneOnVideoEnd(sceneName));
    }
    private IEnumerator ChangeSceneOnVideoEnd(string sceneName)
    {
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        SceneManager.LoadScene(sceneName);
    }
}
