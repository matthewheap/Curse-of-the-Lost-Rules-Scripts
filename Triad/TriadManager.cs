using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Analytics;

namespace TriadGame
{
    public class TriadManager : MonoBehaviour
    {
        public static TriadManager instance;
        //public CinemachineVirtualCamera cam;
        static bool created = false;
        private Scene thisScene;
        //public Player player;
        //public Text failText;
       // public RectTransform panel;
        public string[] notes; //holds note names
        public TextAsset notesIn; //gets note names
        public TextAsset triadsIn; //gets triads in text form
        public TextAsset scalesIn;
        public string[,] triads; //gets all possible major triads (except weird ones)
        public string[,] minorTriads; //"minor
        public string[,] augmentedTriads; //"augmented
        public string[,] diminishedTriads; //"dim
        public string[,] scales;
        //private string[] flavorTexts = new string[8] { "Look Out...an Unresolved Leading Tone!", "Phew! Remember...Leading Tones resolve up!!", "Skulls let you know " +
         //   "unresolved 7ths are near!", "Always resolve the seventh of the chord down, adventurer!", "Watch out for the tritone! Avoid it!!", "Oh no! Parallel Octaves! Gotta" +
          //  " jump through them!", "These incorrect chords are a mess! Jump into them for a boost, but don't get hit!", "This might require a leap of faith!" }; //add more as necessary
        public ScaleGenerator scaleGenerator;
        //public Text flavorText;
        public int failures;
        public GameObject mobileCanvas;
        //public bool finished = false;

        //public bool[] checkpoints = new bool[13];
        //private Vector3[] checkpointPos = new Vector3[13] { new Vector3(0,.31f,0), new Vector3 (34.12f, 0, 0), new Vector3(66.63f,0,0), new Vector3(116.76f, 3,0),
          //  new Vector3(161.84f, 10, 0), new Vector3(206,10,0), new Vector3(251.5f, 7,0), new Vector3(302.76f, 7, 0), new Vector3(350.96f, 7.3f, 0),
          //  new Vector3(396.1f, 1,0), new Vector3(441.4f, 19,0), new Vector3(493.1f, 19, 0), new Vector3(546.5f, 15,0) };


        void Awake()
        {
            // if (!created)
            //  {
            //     DontDestroyOnLoad(gameObject);
            //      created = true;
            //     instance = this;
            // }
            //else
            // {
            //    Destroy(gameObject);
            //}
            instance = this;
            notes = scaleGenerator.makeNote(notesIn);
            #if UNITY_IOS || UNITY_ANDROID
                  mobileCanvas.gameObject.SetActive(true);
            #else
                   mobileCanvas.gameObject.SetActive(false);
            #endif
            if (!TotalGameManager.instance.levelTwo)
            {
                triads = scaleGenerator.TriadMaker(triadsIn);
                minorTriads = scaleGenerator.TriadChanger(triads, "Minor");
                augmentedTriads = scaleGenerator.TriadChanger(triads, "Augmented");
                diminishedTriads = scaleGenerator.TriadChanger(triads, "Diminished");
            }
            else
            {
                scales = scaleGenerator.makeScale(scalesIn);
                triads = scaleGenerator.GetTriadFromScale(scales);
            }
            thisScene = SceneManager.GetActiveScene();
            AnalyticsEvent.LevelStart(thisScene.name, thisScene.buildIndex);
            //for (int x = 0; x<checkpoints.Length; x++)
            // {
            //    checkpoints[x] = false;
            // }
            // checkpoints[0] = true;
            //checkpoints[11] = true; //fordebug
            failures = 0;
            //panel.gameObject.SetActive(false);

        } 
    }
}
