using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class RaceVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip start;
    public VideoClip end;

    private void Start()
    {
        if (TotalGameManager.instance.finishedKS || TotalGameManager.instance.finishedA6)
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "RaceEnd.mp4");
            vp.clip = end;
        }
        else
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "RaceOpen.mp4");
            vp.clip = start;
        }
    }

    public void stopVideo()
    {
        vp.Stop();

        if (TotalGameManager.instance.finishedA6 || TotalGameManager.instance.finishedKS)
        {
            SceneManager.LoadScene("MainHub");
        }
        else
        {
            SceneManager.LoadScene("RacingGame");
        }
    }
}
