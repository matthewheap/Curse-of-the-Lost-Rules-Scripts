using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

namespace FloorGame
{
    public class UIManager : MonoBehaviour
    {
        public Text levelInfo;
        public Text failText;
        public Image failBackground; 
        string scale;
        int rankSubtractor = 0;
        int mode;
        Tile startingPitch;
        // Use this for initialization
        void Start()
        {
            if(TotalGameManager.instance.levelTwo)
            {
                rankSubtractor = 4;
            }
            UpdateUI(TotalGameManager.instance.floorRank-rankSubtractor, TotalGameManager.instance.floorLevel);
            failBackground.gameObject.SetActive(false);
            if(FloorGameManager.instance.failure > 3)
            {
                int rank = TotalGameManager.instance.floorRank;
                displayFail(rank);
                FloorGameManager.instance.failure = 0;
            }
        }

        // Update is called once per frame

        public void UpdateUI(int newrank, int newlevel)
        {
            if (!TotalGameManager.instance.levelTwo)
            {
                startingPitch = GameObject.Find("SceneManager").GetComponent<FloorGenerator>().tileArray[0, 0];
                mode = GameObject.Find("SceneManager").GetComponent<FloorGenerator>().scaleChoice;
                scale = startingPitch.NoteName;
                if (newrank == 1)
                {
                    levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": " + scale + " Major";
                }
                else if (newrank == 2)
                {
                    levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": " + scale + " Natural Minor";
                }
                else if (newrank == 3)
                {
                    switch (mode)
                    {
                        case 1:
                            levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": " + scale + " Dorian";
                            break;
                        case 2:
                            levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": " + scale + " Phrygian";
                            break;
                        case 3:
                            levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": " + scale + " Lydian";
                            break;
                        case 4:
                            levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": " + scale + " Mixolydian";
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                scale = GameObject.Find("SceneManager").GetComponent<FloorGenerator>().myScale;
                if (newrank == 1)
                {
                    levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": V7/V in " + scale + " Major";
                }
                else
                {
                    levelInfo.text = "Rank " + newrank + ", Level " + newlevel + ": vii°7/V in " + scale + " Major";
                }
            }
        }
        void displayFail(int rank)
        {
            if (rank == 1)
            {
                failText.text = "Major Scales help: There are two ways to think about this. Either think of the major tetrachord (WWH) and build the scale that way" +
                " (WWHWWWH), or (better) think of the key signature. For E Major, for example, the key signature is F#, C#, G#, D#. Those are the only notes that can" +
                " be altered. Every other note will be natural...so we'd have E, F#, G#, A, B, C#, D#, E. You can do it!";
            }
            else if (rank == 2)
            {
                failText.text = "Minor Scales help: There are a few ways to think about this. First, you could build it out of whole and half steps (WWHWWHW). Second, you" +
                " could build it from the major scale, but lower the 3rd, 6th, and 7th scale degrees...so for E Minor you'd take the E Major scale (EF#G#ABC#D#E), take the " +
                "third (G#), sixth (C#), and seventh (D#) and lower them (to G, C, and D)...therefore the scale would be E F# G A B C D E. Third, you could think about the " +
                " relative major...for E Minor, what is a minor third above E? (G!). The two scales share a key signature. You can do it! I believe in you!";
            }
            else if (rank == 3)
            {
                failText.text = "Mode Help: A few ways to approach this. First, you could figure out what the corresponding major scale is. Dorian is based on the second scale degree" +
                ", Phrygian on the third, Lydian on the fourth, and Mixolydian on the fifth. So if you wanted F Mixolydian, you'd think 'what is F the fifth scale degree of?' (Bb!)." +
                "F Mixolydian would have the same key signature as Bb Major. Second, you could think about how the modes alter the scales. For Dorian and Phrygian, think of the minor" +
                " scale (so, for F Dorian, start with F Minor). Then, for Dorian, you raise the 6th scale degree (so for F Dorian, you'd raise the Db of F Minor to a D Natural). For" +
                " Phrygian, you lower the second scale degree (so for F Phrygian, you have Gb instead of G Natural). For Lydian and Mixolydian, think of the major scale. With Lydian, " +
                "raise the fourth scale degree (so for D Lydian, you'd take D Major and raise the G to G#). With Mixolydian, lower the seventh scale degree (so for D Mixolydian, you'd" +
                " take the C# of D Major and make it C Natural. You can do it...you're almost there!";
            }
            else if (rank == 5)
            {
                failText.text = "To find a secondary dominant (in this case V7/V), you first have to figure out what the 'of' is...so here, we're going to figure out what V is. If we're in C Major" +
                    ", V would be G Major. To find the V of that V (which is what the chord we're looking for is), figure out what V in G Major would be (D Major). So V7/V in C Major would be DF#AC" +
                    ". A shortcut is to remember that V7/V is always built on the second scale degree (in this case, D) and is always a dominant chord. So if you figure out the second scale degree and then" +
                    " build a dominant chord on top of it, you'll be right! You can do it!!";
            }
            else
            {
                failText.text = "To find a secondary leading tone chord (in this case vii°7/V), you first have to figure out what the 'of' is...so here we're going to figure out what V is. If we're in" +
                    " C Major, V would be G Major. Then we find the vii°7 chord in that key. In G Major, vii°7 is F#ACEb (note: you can have half diminished secondary leading tones, but let's not worry " +
                    "about that for now!). Another quick way to deal with this is to take the chord you want to tonicize with your secondary dominant, move down a half step, and build the vii°7 chord on that" +
                    " note. So if G is our V, we'd go down to F# and build in minor 3rds - F#ACEb. You can do it!";
            }
            failBackground.gameObject.SetActive(true);
        }
        public void closeFail()
        {
            failBackground.gameObject.SetActive(false);
        }
        public void quitFloor()
        {
            ES3.Save<int>("floorLevel", TotalGameManager.instance.floorLevel);
            ES3.Save<int>("floorRank", TotalGameManager.instance.floorRank);
            Destroy(FloorGameManager.instance.audioSource);
            SceneManager.LoadScene("MainHub");
        }
    }
}