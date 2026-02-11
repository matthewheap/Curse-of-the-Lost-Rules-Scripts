using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class IntervalVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip start;
    public VideoClip end;

    private void Start()
    {
        if (TotalGameManager.instance.intervalLevel == 4)
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "IntervalEnd.mp4");
            vp.clip = end;
        }

        else
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "IntervalInstructions.mp4");
            vp.clip = start;
        }
    }


    public void stopVideo() //need help here
    {
        vp.Stop();

        if (TotalGameManager.instance.intervalLevel == 4)
        {
            SceneManager.LoadScene("MainHub");
        }
        else 
        {
            SceneManager.LoadScene("IntervalBirds" + TotalGameManager.instance.intervalLevel);
        }
    }

 
}
