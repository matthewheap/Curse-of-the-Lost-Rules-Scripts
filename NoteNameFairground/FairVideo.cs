using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class FairVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip fairStart;
    public VideoClip fairStop;

    private void Start()
    {
        if (TotalGameManager.instance.fairRank == 3)
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "FairEnd.mp4");
            vp.clip = fairStop;
        }
        else
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "FairInstructions.mp4");
            vp.clip = fairStart;
        }
    }


    public void stopVideo() 
    {
        if (TotalGameManager.instance.fairRank == 3)
        {
            SceneManager.LoadScene("MainHub");
        }
        else
        {
            SceneManager.LoadScene("NoteNameFairground");
        }

    }
 
}
