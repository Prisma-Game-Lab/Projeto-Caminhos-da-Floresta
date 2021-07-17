using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip story;
    public VideoClip openEmpty;
    public VideoClip closeEmpty;
    private void Awake()
    {
        videoPlayer.Play();
        videoPlayer.Pause();
        videoPlayer.frame = 1;
    }
    public void PlayOpen()
    {
        videoPlayer.clip = openEmpty;
        videoPlayer.Play();
    }
    public void PlayClose()
    {
        videoPlayer.clip = closeEmpty;
        videoPlayer.Play();
    }

    public void PlayStartThenChangeScene(string sceneName)
    {
        videoPlayer.clip = story;
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
