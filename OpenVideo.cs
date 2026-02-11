using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO;


public class OpenVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip open;

   private void Start()
    {
        vp.clip = open;
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "GameOpen.mp4");     
    }


    public void stopVideo() 
    {
        vp.Stop();
        SceneManager.LoadScene("MainHub");

    }
}
