using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class TriadVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip start;
    public VideoClip start2;
    public VideoClip end;

    private void Start()
    {
        if ((TotalGameManager.instance.finishedTriadLvl1 && !TotalGameManager.instance.levelTwo) || (TotalGameManager.instance.finishedTriadLvl2 && TotalGameManager.instance.levelTwo))
        {
           // vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "TriadEnd.mp4");
            vp.clip = end;
        }
        else
        {
            if (TotalGameManager.instance.levelTwo)
            {
                //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "TriadTwoStart.mp4");
                vp.clip = start2;
            }
            else
            {
                //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "TriadInstructions.mp4");
                vp.clip = start;
            }
        }

      
    }

    public void stopVideo() 
    {
        vp.Stop();

        if ((TotalGameManager.instance.finishedTriadLvl1 && !TotalGameManager.instance.levelTwo) || (TotalGameManager.instance.finishedTriadLvl2 && TotalGameManager.instance.levelTwo))
        {
            SceneManager.LoadScene("MainHub");
        }
        else
        {
            SceneManager.LoadScene("TriadRace");
        }


    }
}
