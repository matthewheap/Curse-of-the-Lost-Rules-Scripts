using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;


namespace FairGame
{
    public class FairGameManager : MonoBehaviour
    {
        [SerializeField] TextAsset notesText;

        public static FairGameManager instance;
        public Sprite[] noteCards; //for storing note pictures
        public Sprite[] balloonStates;
        public string[] notes; //list of note names
        //public int level;
        //public int rank;
        public Text levelText;
        public Text SystemText;
        public string[] goodJob = new string[] { "Nice!", "Awesome!", "Hooray!", "Huzzah!", "Booyah!", "Cool!", "You're the Best!", "On Fire!!", "Go For It!", "Keep Going!", "Magic!", "Fantastic" };
        public string[] badJob = new string[] { "Aww...", "Not Quite", "Almost!", "You can do it!", "Not There Yet", "So Close!", "Keep Trying!", "Try Again", "Come on, now!", "Tragic!", "Hmm..." };
        static bool created = false;
        private UIManager uIManager;
        public GameObject audioSource;

        //for the beginning of everything
        void Awake()
        {
            if (instance == null)
            {
                //assign it to the current object
                instance = this;
            }
            //make sure it's the current object
            else if (instance != this)
            {
                //destroy the current game object - we only need 1 and we already have it
                instance.uIManager = FindObjectOfType<UIManager>();
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            uIManager = FindObjectOfType<UIManager>();
            notes = GetComponent<ScaleGenerator>().makeNote(notesText);
            var sprites = Resources.LoadAll<Sprite>("Sprites/Fair/Notes");
            noteCards = new Sprite[sprites.Length];
            var bal = Resources.LoadAll<Sprite>("Sprites/Fair/Balloon");
            balloonStates = new Sprite[bal.Length];
            for (int i = 0; i < noteCards.Length; i++)
            {
                noteCards[i] = sprites[i];
            }
            for (int j = 0; j < balloonStates.Length; j++)
            {
                balloonStates[j] = bal[j];
            }
            //TotalGameManager.instance.fairLevel = 1;
            //TotalGameManager.instance.fairRank = 1;

        }


        public void levelUp()
        {
            TotalGameManager.instance.fairLevel++;
            if (TotalGameManager.instance.fairLevel == 11 && TotalGameManager.instance.fairRank < 3)
            {
                TotalGameManager.instance.fairRank++;
                TotalGameManager.instance.fairLevel = 1;
                AnalyticsEvent.AchievementUnlocked("Beat Fair Level" + (TotalGameManager.instance.fairRank - 1).ToString());
            }
            uIManager.systemText.text = goodJob[Random.Range(0, goodJob.Length - 1)];
            uIManager.systemText.gameObject.SetActive(true);
            ES3.Save<int>("fairRank", TotalGameManager.instance.fairRank);
            ES3.Save<int>("fairLevel", TotalGameManager.instance.fairLevel);
            restartLevel();
        }
        public void levelDown(string correctNote)
        {
            TotalGameManager.instance.fairLevel--;
            if (TotalGameManager.instance.fairLevel == 0)
            {
                TotalGameManager.instance.fairLevel = 1;
            }
            if (correctNote == "xxxx")
            {
                uIManager.systemText.text = badJob[Random.Range(0, badJob.Length - 1)];
            }
            else
            {
                uIManager.systemText.text = "The Correct Note was " + correctNote;
            }
            uIManager.systemText.gameObject.SetActive(true);
            ES3.Save<int>("fairRank", TotalGameManager.instance.fairRank);
            ES3.Save<int>("fairLevel", TotalGameManager.instance.fairLevel);
            restartLevel();
        }
        public void outOfTime()
        {
            var tempy = FindObjectsOfType<Button>();
            for (int x = 0; x < tempy.Length; x++)
            {
                if (tempy[x].GetComponentInChildren<Text>().text == "Quit")
                {
                        //do nothing
                }
                else
                {
                   tempy[x].image.sprite = balloonStates[2]; //burst all the balloons
                }
            }
            if (TotalGameManager.instance.fairRank < 3)
            {
                levelDown("xxxx");
            }
            else
            {
                ES3.Save<int>("fairRank", TotalGameManager.instance.fairRank);
                ES3.Save<int>("fairLevel", TotalGameManager.instance.fairLevel);
                uIManager.systemText.text = "Congrats! You got " + (TotalGameManager.instance.fairLevel - 1) + " correct!";
                uIManager.systemText.gameObject.SetActive(true);
                StartCoroutine(waitThree());
            }
        }
        public void restartLevel()
        {
            StartCoroutine(waitFive());  
        }
        public IEnumerator waitFive() //actually waits 3
        {
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("NoteNameFairground");
        }
        public IEnumerator waitThree() //leaves the game
        {
            AnalyticsEvent.AchievementUnlocked("Finished FairGround");
            AnalyticsEvent.LevelComplete("Fairground");
            yield return new WaitForSeconds(3.0f);
            Destroy(audioSource);
            SceneManager.LoadScene("FairVideo");
        }
    }
}
