using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class FloorVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip start1;
    public VideoClip start2;
    public VideoClip end;


    private void Start()
    {
        if (TotalGameManager.instance.floorRank == 4 || TotalGameManager.instance.floorRank == 7)
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "FloorEnd.mp4");
            vp.clip = end;
        }
        else if (TotalGameManager.instance.levelTwo)
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "SecondInstructions.mp4");
            vp.clip = start2;
        }
        else
        {
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "FloorInstructions.mp4");
            vp.clip = start1;
        }
    }

    public void stopVideo() 
    {
        vp.Stop();

        if (TotalGameManager.instance.floorRank == 4 || TotalGameManager.instance.floorRank == 7)
        {
            SceneManager.LoadScene("MainHub");
        }
        else if (TotalGameManager.instance.levelTwo && TotalGameManager.instance.floorRank < 5)
        {
            TotalGameManager.instance.floorRank = 5;
            SceneManager.LoadScene("FloorPuzzle");
        }
        else if (!TotalGameManager.instance.levelTwo && TotalGameManager.instance.floorRank >= 5)
        {
            TotalGameManager.instance.floorRank = 1;
            SceneManager.LoadScene("FloorPuzzle");
        }
        else
        {
            SceneManager.LoadScene("FloorPuzzle");
        }
    }

 
}
