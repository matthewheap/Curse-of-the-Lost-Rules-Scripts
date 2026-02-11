using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Analytics;

namespace PivotWorld //mallet of applied dominance 
{
    public class PivotManager : MonoBehaviour
    {
        public int failures;

        private Vector3[] checkpointPos = new Vector3[7] { new Vector3(0, .31f, -.25f), new Vector3(41.3f, 0, -.25f), new Vector3(87, 15, -.25f), new Vector3(155, 18, -.25f), new Vector3(219, 26, -.25f), new Vector3(285, 31, -.25f), new Vector3(355, 38, -.25f) };
        public Text flavorText;
        public Button NextButton;
        public TextAsset keysIn;
        public Button SkipButton;
        public Button SkipButton2;
        private string[] flavorTexts = new string[8] { "Go on...click on the door!", "Watch out for the bad melodic line...ugh!", "A good melodic line should have some movement...like these platforms!", "A headless snowman lets you know " +
            "unresolved 7ths are near!", "Always resolve the seventh of the chord down, adventurer!", "Watch out for the tritone! Avoid it!!", "Oh no! Parallel Octaves! Gotta" +
            " jump through them!", "These incorrect chords are a mess! Jump into them for a boost, but don't get hit!" }; //add more as necessary
        public Text failText;
        public Text instruction;
        public RectTransform panel;
        public Player player;
        public CinemachineVirtualCamera cam;
        //[System.NonSerialized] public bool[] checkpoints = new bool[7];

        public static Sprite[] scores;
        public static Sprite[] RNs;
        public static AudioClip[] scoreAudio;
        public static bool[] whichDoors;
        public static string[] keys;
        public GameObject mobileCanvas;
        private bool wasStopped = false;

        // Start is called before the first frame update
        void Awake()
        {
            #if UNITY_IOS || UNITY_ANDROID
                  mobileCanvas.gameObject.SetActive(true);
            #else
                   mobileCanvas.gameObject.SetActive(false);
            #endif
            keys = GetComponent<ScaleGenerator>().makeNote(keysIn);
            var sprites = Resources.LoadAll<Sprite>("Sprites/PivotWorld/pivotsprites");
            scores = new Sprite[sprites.Length];
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = sprites[i];
            }
            whichDoors = new bool[scores.Length]; //this will tell us if we've had a door before.
            for (int b = 0; b<whichDoors.Length; b++)
            {
                whichDoors[b] = false; ;
            }
            var sprites2 = Resources.LoadAll<Sprite>("Sprites/PivotWorld/RNs");
            RNs = new Sprite[sprites2.Length];
            for (int j = 0; j < RNs.Length; j++)
            {
                RNs[j] = sprites2[j];
            }
            var sounds = Resources.LoadAll<AudioClip>("Sprites/PivotWorld/pivotSound");
            scoreAudio = new AudioClip[sounds.Length];
            for (int k = 0; k < scoreAudio.Length; k++)
            {
                scoreAudio[k] = sounds[k];
            }
            TotalGameManager.instance.pivCheckpoints[0] = true;
            //for (int x = 0; x < checkpoints.Length; x++)
           // {
            //    checkpoints[x] = false;
            //}
           // checkpoints[0] = true;
            //checkpoints[6] = true; //for debugging
            failures = 0;
            instruction.gameObject.SetActive(false);
            flavorText.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(false);
            SkipButton.gameObject.SetActive(false);
            SkipButton2.gameObject.SetActive(false);
            RestartGame();

        }
        private void Update()
        {
            if(player.stopped)
            {
                wasStopped = true;
                mobileCanvas.gameObject.SetActive(false);
            }
            else
            {
                if(wasStopped)
                {
                    wasStopped = false;
#if UNITY_IOS || UNITY_ANDROID
                    mobileCanvas.gameObject.SetActive(true);
                    player.horizontalMove = 0;
#endif
                }
            }
        }
        private void DestroyQuestion()
        {
            var temp = GameObject.FindGameObjectsWithTag("wrong");
            var temp1 = GameObject.FindGameObjectsWithTag("correct");
            for (int x = 0; x < temp.Length; x++)
            {
                Destroy(temp[x]);
            }
            for (int y = 0; y < temp1.Length; y++)
            {
                Destroy(temp1[y]);
            }

        }
        public void RestartGame()
        {
            /*for (int x = checkpointPos.Length - 1; x > -1; x--)
            {
                if (TotalGameManager.instance.checkpoints[x])
                {
                    player.transform.position = checkpointPos[x];
                    break;
                }
            }*/
            DestroyQuestion();
            for (int x = checkpointPos.Length - 1; x > -1; x--) //until I'm ready to integrate it.
            {
                if (TotalGameManager.instance.pivCheckpoints[x])
                {
                    player.transform.position = checkpointPos[x];
                    break;
                }
            }
            //player.transform.position = checkpointPos[0];
            if (player.thisTrigger != null)
            {
                player.thisTrigger.gameObject.SetActive(true);
                player.thisTrigger.GetComponent<Collider2D>().enabled = true;
            }
            player.animator.SetBool("Dead", false);
            if (failures == 3)
            {
                DisplayFail();
            } 
        }


        public void DisplayFlavor(int flavorIndex)
        {
            print("flavor index = " + flavorIndex);
            flavorText.text = flavorTexts[flavorIndex];
            flavorText.gameObject.SetActive(true);
            StartCoroutine(waitThree());
        }
        IEnumerator waitThree()
        {
            yield return new WaitForSeconds(3.5f);
            flavorText.gameObject.SetActive(false);
        }
        public void WinGame()
        {
            TotalGameManager.instance.finishedPivot = true;
            flavorText.text = "You won!! You got the Mallet of Applied Dominance!!"; 
            flavorText.gameObject.SetActive(true);
            ES3.Save<bool>("finishedPivot", TotalGameManager.instance.finishedPivot);
            StartCoroutine(WaitForEnd());
        }
        IEnumerator WaitForEnd()
        {
            AnalyticsEvent.AchievementUnlocked("Beat Pivots");
            AnalyticsEvent.LevelComplete("Pivot");
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene("PivotVideo");
        }
        public void DisplayFail()
        {
            player.stopped = true;
            
                failText.text = "The first step is to figure out which chord is the last chord before the music starts functioning in the new key. Look for accidentals. A tricky situation can arise if there's a I6/4, which is really just" +
                " a part of the V chord (and therefore functioning in the new key already). So find the first chord that CAN'T be in the new key and go one chord back. Does it work in both? If it does, that's your pivot. Then you just have" +
                " to figure out what the Roman Numerals would be for the pivot bracket - think about it in the original key, then again in the new key! You can do it!";
           
            panel.gameObject.SetActive(true);
        }
        public void CloseFail()
        {
            panel.gameObject.SetActive(false);
            player.stopped = false;
            failures = 0;
            RestartGame();
        }
        public void Quitter()
        {
            ES3.Save<bool[]>("pivCheckpoints", TotalGameManager.instance.pivCheckpoints);
            SceneManager.LoadScene("MainHub");
        }
        
    }
}
