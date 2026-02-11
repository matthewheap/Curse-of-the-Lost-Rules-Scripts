using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class PivotVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip start;
    public VideoClip end;


    private void Start()
    {
        if (TotalGameManager.instance.finishedPivot)
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "PivotEnd.mp4");
            vp.clip = end;
        }
        else
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "PivotInstructions.mp4");
            vp.clip = start;
        }
    }

    public void stopVideo() //need help here
    {
        vp.Stop();

        if (TotalGameManager.instance.finishedPivot)
        {
            SceneManager.LoadScene("MainHub");
        }
        else
        {
            SceneManager.LoadScene("PivotLand");
        }


    }
}
