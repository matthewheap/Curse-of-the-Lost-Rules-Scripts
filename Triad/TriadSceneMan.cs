using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Analytics;

namespace TriadGame
{

    public class TriadSceneMan : MonoBehaviour
    {
        private Vector3[] checkpointPos = new Vector3[13] { new Vector3(0,.31f,0), new Vector3 (34.12f, 0, 0), new Vector3(66.63f,0,0), new Vector3(116.76f, 3,0),
            new Vector3(161.84f, 10, 0), new Vector3(206,10,0), new Vector3(251.5f, 7,0), new Vector3(302.76f, 7, 0), new Vector3(350.96f, 7.3f, 0),
            new Vector3(396.1f, 1,0), new Vector3(441.4f, 19,0), new Vector3(493.1f, 19, 0), new Vector3(546.5f, 15,0) };
        public Text flavorText;
        private string[] flavorTexts = new string[8] { "Look Out...an Unresolved Leading Tone!", "Phew! Remember...Leading Tones resolve up!!", "Skulls let you know " +
            "unresolved 7ths are near!", "Always resolve the seventh of the chord down, adventurer!", "Watch out for the tritone! Avoid it!!", "Oh no! Parallel Octaves! Gotta" +
            " jump through them!", "These incorrect chords are a mess! Jump into them for a boost, but don't get hit!", "This might require a leap of faith!" }; //add more as necessary
        public Text failText;
        public RectTransform panel;
        public Player player;
        public CinemachineVirtualCamera cam;


        // Start is called before the first frame update
        void Start()
        {
            flavorText.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
            RestartGame();
        }

        public void RestartGame()
        {
            for (int x = checkpointPos.Length - 1; x > -1; x--)
            {
                if (TotalGameManager.instance.checkpoints[x])
                {
                    player.transform.position = checkpointPos[x];
                    break;
                }
            }

            if (player.thisTrigger != null)
            {
                player.thisTrigger.gameObject.SetActive(true);
                player.thisTrigger.GetComponent<Collider2D>().enabled = true;
            }
            DestroyBridge();
            player.animator.SetBool("Dead", false);
        }
        public void DestroyBridge()
        {
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("TriadBox");
            foreach (GameObject temp in tiles)
            {
                Destroy(temp);
            }

        }
        public void DisplayFlavor(int flavorIndex)
        {
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
            flavorText.text = "You won!! You got the Golden Staff!!";
            flavorText.gameObject.SetActive(true);
            StartCoroutine(WaitForEnd());
        }
        IEnumerator WaitForEnd()
        {
            if (TotalGameManager.instance.levelTwo)
            {
                TotalGameManager.instance.finishedTriadLvl2 = true;
                AnalyticsEvent.AchievementUnlocked("Beat Mix and N6");
                AnalyticsEvent.LevelComplete("Level 2 Triads");
                ES3.Save<bool>("finishedTriadLvl2", TotalGameManager.instance.finishedTriadLvl2);
            }
            else
            {
                TotalGameManager.instance.finishedTriadLvl1 = true;
                AnalyticsEvent.AchievementUnlocked("Beat Triads");
                AnalyticsEvent.LevelComplete("Level 1 Triads");
                ES3.Save<bool>("finishedTriadLvl1", TotalGameManager.instance.finishedTriadLvl1);
            }
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene("TriadVideo");
        }
        public void DisplayFail(GameObject whichTriad) 
        {
            player.stopped = true;
            if (whichTriad.name == "Triad1" || whichTriad.name == "Triad2" || whichTriad.name == "Triad3")
            {
                if (TotalGameManager.instance.levelTwo)
                {
                    failText.text = "So we know that a ii starts on the second scale degree, and is usually minor (in a major key). In this case we're borrowing it from the minor key " +
                        "where it's usually diminished. So there are two ways to think about it - either use the key signature of the minor key or take the regular ii chord and make it" +
                        " diminished by lowering the fifth a half step! You can do it!";
                }
                else
                {
                    failText.text = "There are a couple of ways to think about this. You could use intervals or key signatures. For the interval method, think about the distance " +
                        "between the root and third as a Major 3rd, and the third and fifth as a minor 3rd. The root to the fifth should be a perfect fifth. For key signatures, think" +
                        "about the key of the root...say it was A Major...we know we have F#, C#, and G#. Then construct the triad as line-line-line or space-space-space (on the staff)(so," +
                        " something like C-E-G), then add the accidentals as necessary. For A Major, we'd have A, C, and E. Then we'd add the # to the C. Keep Trying!!";
                }
            }
            else if (whichTriad.name == "Triad4" || whichTriad.name == "Triad5" || whichTriad.name == "Triad6")
            {
                if (TotalGameManager.instance.levelTwo)
                {
                    failText.text = "So we know that a IV starts on the fourth scale degree, and is usually major (in a major key). In this case we're borrowing it from the minor key " +
                        "where it's usually minor. So there are two ways to think about it - either use the key signature of the minor key or take the regular IV chord and make it" +
                        " minor by lowering the third a half step! You can do it!";
                }
                else
                {
                    failText.text = "There are a three ways to think about this. You could use intervals, key signatures, or the major triad. For the interval method, think about the distance" +
                        "between the root and third as a Minor 3rd, and the third and fifth as a Major 3rd. The root to the fifth should be a perfect fifth. For key signatures, think" +
                        "about the key of the root...say it was Bb Minor...we know we have Bb, Eb, Ab, Db, Gb. Then construct the triad as line-line-line or space-space-space (on the staff)(so," +
                        " something like C-E-G), then add the accidentals as necessary. For Bb Minor, we'd have Bb, D, and F. Then we'd add the b to the D. If you have the Major Triad," +
                        " simply take the third of the chord and lower it 1 half step! You can do it!!!";
                }
            }
            else if (whichTriad.name == "Triad7" || whichTriad.name == "Triad8" || whichTriad.name == "Triad9")
            {
                if (TotalGameManager.instance.levelTwo)
                {
                    failText.text = "So here we're looking for the bVI - that means we have to find the sixth scale degree and lower it one half step (so in C Major we'd look for A and then" +
                        " lower that to Ab). Then we build a major chord on it. Remember we're just borrowing this chord from the parallel minor, so if in doubt, think what the VI would be " +
                        "in C Minor (Ab Major!). Go for it!!";
                }
                else
                {
                    failText.text = "There are a couple of ways to think about this. You could use intervals, or the major triad. For the interval method, think about the distance" +
                        "between the root and third as a minor 3rd, and the third and fifth as a minor 3rd. The root to the fifth should be a diminished fifth. If you have the Major Triad," +
                        " simply take the third and fifth of the chord and lower both 1 half step! Go get 'em, champ!!!";
                }
            }
            else
            {
                if (TotalGameManager.instance.levelTwo)
                {
                    failText.text = "So we know that the Neapolitan chord is built on the flatted second scale degree (so in C Major that would be Db, but in C# Major it would be D - there doesn't have" +
                        " to be a flat - it's just lowered half a step). It's always a major chord. If you want to build it using scale degrees, it's flat-2, 4, and flat-6. Usually the 3rd is in the bass" +
                        " and the root resolves to the leading tone of the V chord. Keep going!! You're a rockstar!!";
                }
                else
                {
                    failText.text = "There are a couple of ways to think about this. You could use intervals, or the major triad. For the interval method, think about the distance" +
                    "between the root and third as a Major 3rd, and the third and fifth as a Major 3rd. The root to the fifth should be an augmented fifth. If you have the Major Triad," +
                    " simply take the fifth of the chord and raise it 1 half step! You're Almost There!!!!";
                }
            }
            panel.gameObject.SetActive(true);
        }
        public void CloseFail()
        {
            panel.gameObject.SetActive(false);
            player.stopped = false;
            RestartGame();
        }
        public void Quitter()
        {
            if (!TotalGameManager.instance.levelTwo)
            {
                ES3.Save<bool[]>("checkpoints", TotalGameManager.instance.checkpoints);
            }
            else
            {
                ES3.Save<bool[]>("checkpoints2", TotalGameManager.instance.checkpoints);
            }
            SceneManager.LoadScene("MainHub");
        }
        public void ZoomOut(int whichWay)
        {
            if (whichWay == 0)
            {
                cam.m_Lens.OrthographicSize = 10;
            }
            else
            {
                cam.m_Lens.OrthographicSize = 6;
            }
        }
    }
}
