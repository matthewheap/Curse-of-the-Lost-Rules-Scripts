using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class BossVideo : MonoBehaviour
{
    public VideoPlayer vp;

    private void Start()
    {
       
            //vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "BossInstructions.mp4");

        
    }



    public void stopVideo() //need help here
    {
        vp.Stop();
        AnalyticsEvent.AchievementUnlocked("Reached Wagner");
        SceneManager.LoadScene("BossFight");

    }
}
