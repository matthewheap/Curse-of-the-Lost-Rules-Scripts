using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class SecondaryMaker : MonoBehaviour
{
    static bool created = false;
    public static SecondaryMaker instance;
    public Sprite[] secCards;
    //public int level;
    public string intervalName;
    public int failures;
    private Scene thisScene;
    public GameObject audioSource;

    void Awake()
    {
        //if (!created)
        //{
        //    DontDestroyOnLoad(gameObject);
        //    created = true;
            instance = this;
        //}
        // else
        //{
        //    Destroy(gameObject);
        //}
        GameObject[] audios = GameObject.FindGameObjectsWithTag("toDelete");
        if (audios.Length > 1)
        {
            Destroy(audios[1]);
        }
        var sprites = Resources.LoadAll<Sprite>("Sprites/SecChordAB/SecSprites");
        secCards = new Sprite[sprites.Length];
        failures = 0;
        for (int i = 0; i < secCards.Length; i++)
        {
            secCards[i] = sprites[i];
        }
    }
    public void NextScene()
    {
        TotalGameManager.instance.secLevel++;
        ES3.Save<int>("secLevel", TotalGameManager.instance.secLevel);
        StartCoroutine(waitThree());
    }

    IEnumerator waitThree()
    {
        yield return new WaitForSeconds(3);
        if (TotalGameManager.instance.secLevel < 4)
        {
            thisScene = SceneManager.GetActiveScene();
            AnalyticsEvent.LevelComplete("IntervalBirds" + (TotalGameManager.instance.intervalLevel - 1).ToString());
            SceneManager.LoadScene("SecondBirds" + TotalGameManager.instance.secLevel);
        }
        else
        {
            AnalyticsEvent.AchievementUnlocked("Beat Naming");
            AnalyticsEvent.LevelComplete("Naming Secondary");
            //Destroy(audioSource);
            Destroy(GameObject.FindGameObjectWithTag("toDelete"));
            SceneManager.LoadScene("SecondaryVideo");
        }
    }

}
