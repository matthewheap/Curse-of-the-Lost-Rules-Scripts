using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;


public class IntervalManager : MonoBehaviour
{
    static bool created = false;
    public static IntervalManager instance;
    public Sprite[] intervalCards;
    //public int level;
    public string intervalName;
    public int failures;
    Scene thisScene;
    public GameObject audioSource;




    void Awake()
    {
       // if (!created)
       // {
           // DontDestroyOnLoad(gameObject);
        //    created = true;
            instance = this;
       // }
       // else
       // {
        //    Destroy(gameObject);
        //}
        GameObject[] audios = GameObject.FindGameObjectsWithTag("toDelete");
        if (audios.Length > 1)
        {
            Destroy(audios[1]);
        }
        var sprites = Resources.LoadAll<Sprite>("Sprites/Intervals/IntervalSprites");
        intervalCards = new Sprite[sprites.Length];
        //level = 0;
        failures = 0;
        for (int i = 0; i < intervalCards.Length; i++)
        {
            intervalCards[i] = sprites[i];
        }
    }
    public void NextScene()
    {
        TotalGameManager.instance.intervalLevel++;
        ES3.Save<int>("intervalLevel", TotalGameManager.instance.intervalLevel);
        StartCoroutine(waitThree());
    }

    IEnumerator waitThree()
    {
        yield return new WaitForSeconds(3);
        if (TotalGameManager.instance.intervalLevel < 4)
        {
            thisScene = SceneManager.GetActiveScene();
            AnalyticsEvent.LevelComplete("IntervalBirds" + (TotalGameManager.instance.intervalLevel - 1).ToString());
            SceneManager.LoadScene("IntervalBirds" + TotalGameManager.instance.intervalLevel);
        }
        else
        {
            AnalyticsEvent.AchievementUnlocked("Beat Intervals");
            AnalyticsEvent.LevelComplete("Naming Intervals");
            //Destroy(audioSource);
            Destroy(GameObject.FindGameObjectWithTag("toDelete"));
            SceneManager.LoadScene("IntervalVideo");
        }
    }

}
