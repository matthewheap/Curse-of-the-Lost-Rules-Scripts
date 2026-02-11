using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class SecondaryVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip start;
    public VideoClip end;


    private void Start()
    {
        if (TotalGameManager.instance.secLevel == 4)
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "IntervalEnd.mp4");
            vp.clip = end;
        }

        else
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "NamingInstructions.mp4");
            vp.clip = start;
        }
    }

    public void stopVideo() 
    {
        vp.Stop();

        if (TotalGameManager.instance.secLevel == 4)
        {
            SceneManager.LoadScene("MainHub");
        }
        else
        {
            SceneManager.LoadScene("SecondBirds" + TotalGameManager.instance.secLevel);
        }

    }
}
