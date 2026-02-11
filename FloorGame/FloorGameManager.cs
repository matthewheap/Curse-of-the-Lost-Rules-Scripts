using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;


namespace FloorGame
{


    public class FloorGameManager : MonoBehaviour
    {

        public static FloorGameManager instance;
        private UIManager uIManager;
       // public int level;
       // public int rank;
        public int failure;
        public int floorCap;
        public int rankCap;
        public GameObject audioSource;


        // Use this for initialization
        //for the beginning of everything
        void Awake()
        {

            //check that it exists
            if (instance == null)
            {
                //assign it to the current object
                instance = this;
            }
            //make sure it's the current object
            else if (instance != this)
            {
                //find an object of type HudManager
                instance.uIManager = FindObjectOfType<UIManager>();
                //destroy the current game object - we only need 1 and we already have it
                Destroy(gameObject);
            }
            //don't destroy this object when changing scenes
            DontDestroyOnLoad(gameObject);
            //find an object of type UIManager
            uIManager = FindObjectOfType<UIManager>();

            //level = 1;
            // rank = 1; 
            failure = 0;
            if (TotalGameManager.instance.levelTwo)
            {
                floorCap = 5;
                rankCap = 6;
            }
            else
            {
                floorCap = 4;
                rankCap = 3;
            }
        }

        public void levelUp()
        {
            TotalGameManager.instance.floorLevel++;
            if(TotalGameManager.instance.floorLevel >= floorCap && TotalGameManager.instance.floorRank < rankCap)
            {
                if(TotalGameManager.instance.levelTwo)
                {
                    AnalyticsEvent.AchievementUnlocked("Beat Building Rank" + (TotalGameManager.instance.floorRank -3).ToString());
                }
                else
                {
                    AnalyticsEvent.AchievementUnlocked("Beat Scales Rank" + TotalGameManager.instance.floorRank.ToString());
                }
                TotalGameManager.instance.floorLevel = 1;
                failure = 0;
                TotalGameManager.instance.floorRank++;
                resetGame(true);
            }
            else if(TotalGameManager.instance.floorLevel == floorCap && (TotalGameManager.instance.floorRank == 3 || TotalGameManager.instance.floorRank == 6))
            {
                uIManager.levelInfo.text = "Congrats!";
                TotalGameManager.instance.floorRank++;
                if (TotalGameManager.instance.levelTwo)
                {
                    AnalyticsEvent.AchievementUnlocked("Beat Building");
                    AnalyticsEvent.LevelComplete("Building");
                }
                else
                {
                    AnalyticsEvent.AchievementUnlocked("Beat Scales");
                    AnalyticsEvent.LevelComplete("Scales");
                }
                ES3.Save<int>("floorLevel", TotalGameManager.instance.floorLevel);
                ES3.Save<int>("floorRank", TotalGameManager.instance.floorRank);
                Destroy(audioSource);
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
                SceneManager.LoadScene("FloorVideo");
            }
            else
            {
                resetGame(true);
            }
            
        }
        public void resetGame(bool exit)
        {
            if (!exit)
            {
                if (TotalGameManager.instance.levelTwo && TotalGameManager.instance.floorLevel > 1)
                {
                    TotalGameManager.instance.floorLevel--;
                }
                else
                {
                    TotalGameManager.instance.floorLevel = 1;
                }
                failure++;
                SceneManager.LoadScene("FloorPuzzle");
            }
            else
            {
                ES3.Save<int>("floorLevel", TotalGameManager.instance.floorLevel);
                ES3.Save<int>("floorRank", TotalGameManager.instance.floorRank);
                SceneManager.LoadScene("FloorPuzzle");
            }
        }

    }
}
