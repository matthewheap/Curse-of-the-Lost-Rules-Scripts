using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class HubManager : MonoBehaviour
{
    public static HubManager instance;
    public TextMeshPro door1; //door furthest to the left
    public TextMeshPro door2;
    public TextMeshPro door3;
    public TextMeshPro door4;
    public TextMeshPro door5;
    public TextMeshPro door6;//door furthest to the right
    public TextMeshPro secondary;
    public GameObject Panel;
    public GameObject levelPanel;
    public bool goOn = false;
    public bool goBack = false;
    public Button yesButton;
    public Button noButton;
    public GameObject difficultyPanel;
    public Button finalBattle;
    private bool inDifficulty = false;
    private string defaultReset;
    public Text resetText;
    public GameObject aboutPanel;


    // Start is called before the first frame update
    void Start()
    {
        //check that it exists
        if (instance == null)
        {
            //assign it to the current object
            instance = this;
        }
        Panel.gameObject.SetActive(false);
        aboutPanel.SetActive(false);
        if (!TotalGameManager.instance.choseLevel)
        {
            levelPanel.gameObject.SetActive(true);
        }
        else
        {
            LabelDoors();
            levelPanel.gameObject.SetActive(false);
        }
        GameObject temp = GameObject.FindGameObjectWithTag("Manager");
        if(temp!=null)
        {
            Destroy(temp);
        }
        defaultReset = "You have already completed this puzzle. Do you want to start it again?";
        difficultyPanel.gameObject.SetActive(false);
        finalBattle.gameObject.SetActive(false);
    }
    public void ResetButton()
    {
        StartCoroutine(ResetThings());
    }
    IEnumerator ResetThings()
    {
        resetText.text = "Do you really want to reset EVERYTHING?";
        Panel.gameObject.SetActive(true);
        while (!goOn && !goBack)
        {
            yield return null;
        }
        if (goOn)
        {
            TotalGameManager.instance.ResetEverything();
        }
        else
        {
            Panel.gameObject.SetActive(false);
            goBack = false;
        }
        resetText.text = defaultReset;
    }
    private void Update()
    {
        if(!TotalGameManager.instance.levelTwo)
        {
            if(!inDifficulty && TotalGameManager.instance.finishedTriadLvl1 && TotalGameManager.instance.floorRank == 4 && TotalGameManager.instance.fairRank == 3 && TotalGameManager.instance.intervalLevel == 4 && TotalGameManager.instance.finishedKS && TotalGameManager.instance.finishedLW1) //fix this to add new games
            {
                difficultyPanel.SetActive(true);
                inDifficulty = true;
            }  
        }
        if (TotalGameManager.instance.levelTwo)
        {
            if (!inDifficulty && TotalGameManager.instance.finishedPivot && TotalGameManager.instance.floorRank == 7 && TotalGameManager.instance.secLevel == 4 && TotalGameManager.instance.finishedTriadLvl2 && TotalGameManager.instance.finishedA6 && TotalGameManager.instance.finishedLW2)
            {
                difficultyPanel.SetActive(true);
                inDifficulty = true;
            }
        }
    }

    void LabelDoors()
    {
        
        if (TotalGameManager.instance.levelTwo)
        {
            door1.text = "All Chords";
            door2.text = "Building";
            door3.text = "Naming";
            door4.text = "Pivots";
            door5.text = "Mix & N6";
            door6.text = "+6 Chords";
            secondary.gameObject.SetActive(true);
        }
        else
        {
            door1.text = "    Keys";
            door2.text = "Notes";
            door3.text = "Scales";
            door4.text = "Triads";
            door5.text = "Intervals";
            door6.text = "Key Sigs";
            secondary.gameObject.SetActive(false);
        }
    }
    public void Easy() //buttons for endgame
    {
        TotalGameManager.instance.difficulty = 3;
        SceneManager.LoadScene("BossVideo");
    }
    public void Medium()
    {
        TotalGameManager.instance.difficulty = 2;
        SceneManager.LoadScene("BossVideo");
    }
    public void Hard()
    {
        TotalGameManager.instance.difficulty = 1;
        SceneManager.LoadScene("BossVideo");
    }
    public void noThanks()
    {
        difficultyPanel.SetActive(false);
        finalBattle.gameObject.SetActive(true);
    }
    public void FinalBattle()
    {
        difficultyPanel.SetActive(true);
    }
    public void chooseLevel(string levelName)
    {
        StartCoroutine(WaitForPlayer(levelName));
    }
    public void PushYes()
    {
        goOn = true;
    }
    public void PushNo()
    {
        goBack = true;
    }
    public void levelOne()
    {
        TotalGameManager.instance.levelTwo = false;
        TotalGameManager.instance.choseLevel = true;
        levelPanel.gameObject.SetActive(false);
        LabelDoors();
    }
    public void levelTwo()
    {
        TotalGameManager.instance.levelTwo = true;
        TotalGameManager.instance.choseLevel = true;
        levelPanel.gameObject.SetActive(false);
        LabelDoors();
    }
    public void ShowAboutPanel()
    {
        aboutPanel.SetActive(true);
    }
    public void HideAboutPanel()
    {
        aboutPanel.SetActive(false);
    }
    public void ChooseDifficultyLevel()
    {
        levelPanel.gameObject.SetActive(true);
    }
    IEnumerator WaitForPlayer(string levelName)
    {
        switch (levelName)
        {
            case "Scales":

                if (TotalGameManager.instance.floorRank >= 4)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.floorRank = 1;
                        TotalGameManager.instance.floorLevel = 1;
                        AnalyticsEvent.LevelStart("Scales");
                        SceneManager.LoadScene("FloorVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    SceneManager.LoadScene("FloorVideo");
                }
                break;

            case "Building":
                if (TotalGameManager.instance.floorRank == 7 || (TotalGameManager.instance.floorRank >1 && TotalGameManager.instance.floorRank<=4))
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {

                        TotalGameManager.instance.floorRank = 5;
                        TotalGameManager.instance.floorLevel = 1;
                        AnalyticsEvent.LevelStart("Building");
                        SceneManager.LoadScene("FloorVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    if(TotalGameManager.instance.floorRank>=5)
                    {
                        //keep it there
                    }
                    else
                    {
                           TotalGameManager.instance.floorRank = 5;//FIX ME
                    }
                    
                    SceneManager.LoadScene("FloorVideo");
                }
                break;

            case "Notes":
                if (TotalGameManager.instance.fairRank == 3)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.fairRank = 1;
                        TotalGameManager.instance.fairLevel = 1;
                        AnalyticsEvent.LevelStart("Fairground");
                        SceneManager.LoadScene("FairVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    SceneManager.LoadScene("FairVideo");
                }
                break;

            case "Triads":
                if (TotalGameManager.instance.finishedTriadLvl1 || TotalGameManager.instance.finishedTriadLvl2)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.finishedTriadLvl1 = false;
                        TotalGameManager.instance.finishedTriadLvl2 = false;
                        ES3.Save<bool>("finishedTriadLvl1", false);
                        ES3.Save<bool>("finishedTriadLvl2", false);
                        for (int x = 1; x < TotalGameManager.instance.checkpoints.Length; x++)
                        {
                            TotalGameManager.instance.checkpoints[x] = false;
                        }
                        TotalGameManager.instance.checkpoints[0] = true;
                        ES3.Save<bool[]>("checkpoints", TotalGameManager.instance.checkpoints);
                        AnalyticsEvent.LevelStart("Level 1 Triads");
                        SceneManager.LoadScene("TriadVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    if (ES3.KeyExists("checkpoints"))
                    {
                        TotalGameManager.instance.checkpoints = ES3.Load<bool[]>("checkpoints");
                    }
                    SceneManager.LoadScene("TriadVideo");
                }
                break;

            case "Intervals":
                if (TotalGameManager.instance.intervalLevel == 4)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.intervalLevel = 0;
                        AnalyticsEvent.LevelStart("Intervals");
                        SceneManager.LoadScene("IntervalVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                        SceneManager.LoadScene("IntervalVideo");

                }
                break;

            case "Naming":
                if (TotalGameManager.instance.secLevel == 4)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.secLevel = 0;
                        AnalyticsEvent.LevelStart("Naming Secondary");
                        SceneManager.LoadScene("SecondaryVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                        SceneManager.LoadScene("SecondaryVideo");
                }
                break;
            case "Pivots":
                if (TotalGameManager.instance.finishedPivot)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.finishedPivot = false;
                        ES3.Save<bool>("finishedPivot", false);
                        for (int x = 1; x < TotalGameManager.instance.pivCheckpoints.Length; x++)
                        {
                            TotalGameManager.instance.pivCheckpoints[x] = false;
                        }
                        TotalGameManager.instance.pivCheckpoints[0] = true;
                        ES3.Save<bool[]>("pivCheckpoints", TotalGameManager.instance.pivCheckpoints);
                        AnalyticsEvent.LevelStart("Pivot");
                        SceneManager.LoadScene("PivotVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    SceneManager.LoadScene("PivotVideo");
                }
                break;
            case "EndGame":
                difficultyPanel.gameObject.SetActive(true);
                break;
            case "Key Sigs":
                if (TotalGameManager.instance.finishedKS || TotalGameManager.instance.finishedA6)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.finishedKS = false;
                        TotalGameManager.instance.finishedA6 = false;
                        ES3.Save<bool>("finishedKS", false);
                        ES3.Save<bool>("finishedA6", false);
                        ES3.Save<int>("RaceLaps", 1);
                        ES3.Save<int>("RaceTimer", 65);
                        AnalyticsEvent.LevelStart("Key Race");
                        SceneManager.LoadScene("RaceVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    SceneManager.LoadScene("RaceVideo");
                }
                break;
            case "    Keys":
                if (TotalGameManager.instance.letterRank == 4 || (TotalGameManager.instance.levelTwo && TotalGameManager.instance.letterPlayingLvl1 && TotalGameManager.instance.letterRank > 0) ||
                    (!TotalGameManager.instance.levelTwo && !TotalGameManager.instance.letterPlayingLvl1 && TotalGameManager.instance.letterRank > 0)) //if we'd been playing one level and started the other, it would reset.
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.letterRank = 0;
                        TotalGameManager.instance.letterPlayingLvl1 = true;
                        ES3.Save<bool>("letterPlayingLvl1", true);
                        AnalyticsEvent.LevelStart("Key Sig Wheel");
                        SceneManager.LoadScene("LetterWheelVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    TotalGameManager.instance.letterPlayingLvl1 = true;
                    ES3.Save<bool>("letterPlayingLvl1", true);
                    SceneManager.LoadScene("LetterWheelVideo");
                }
                break;

            case "All Chords":
                if (TotalGameManager.instance.letterRank == 4 || (TotalGameManager.instance.levelTwo && TotalGameManager.instance.letterPlayingLvl1 && TotalGameManager.instance.letterRank > 0) || 
                    (!TotalGameManager.instance.levelTwo && !TotalGameManager.instance.letterPlayingLvl1 && TotalGameManager.instance.letterRank >0))
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.letterRank = 0;
                        TotalGameManager.instance.letterPlayingLvl1 = false;
                        ES3.Save<bool>("letterPlayingLvl1", false);
                        AnalyticsEvent.LevelStart("All Chords");
                        SceneManager.LoadScene("LetterWheelVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    TotalGameManager.instance.letterPlayingLvl1 = false;
                    ES3.Save<bool>("letterPlayingLvl1", false);
                    SceneManager.LoadScene("LetterWheelVideo");
                }
                break;
            case "+6 Chords":
                if (TotalGameManager.instance.finishedA6 || TotalGameManager.instance.finishedKS)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.finishedA6 = false;
                        TotalGameManager.instance.finishedKS = false;
                        ES3.Save<bool>("finishedA6", false);
                        ES3.Save<bool>("finishedKS", false);
                        ES3.Save<int>("RaceLaps", 1);
                        ES3.Save<int>("RaceTimer", 65);
                        AnalyticsEvent.LevelStart("A6 Race");
                        SceneManager.LoadScene("RaceVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    SceneManager.LoadScene("RaceVideo");
                }
                break;

            case "Mix & N6": 
                if (TotalGameManager.instance.finishedTriadLvl2 || TotalGameManager.instance.finishedTriadLvl1)
                {
                    Panel.gameObject.SetActive(true);
                    while (!goOn && !goBack)
                    {
                        yield return null;
                    }
                    if (goOn)
                    {
                        TotalGameManager.instance.finishedTriadLvl1 = false;
                        TotalGameManager.instance.finishedTriadLvl2 = false;
                        ES3.Save<bool>("finishedTriadLvl1", false);
                        ES3.Save<bool>("finishedTriadLvl2", false);
                        for (int x = 1; x < TotalGameManager.instance.checkpoints.Length; x++)
                        {
                            TotalGameManager.instance.checkpoints[x] = false;
                        }
                        TotalGameManager.instance.checkpoints[0] = true;
                        ES3.Save<bool[]>("checkpoints2", TotalGameManager.instance.checkpoints);
                        AnalyticsEvent.LevelStart("Level 2 Triads");
                        SceneManager.LoadScene("TriadVideo");
                    }
                    else
                    {
                        Panel.gameObject.SetActive(false);
                        goBack = false;
                        break;
                    }
                }
                else
                {
                    if (ES3.KeyExists("checkpoints2"))
                    {
                        TotalGameManager.instance.checkpoints = ES3.Load<bool[]>("checkpoints2");
                    }
                    SceneManager.LoadScene("TriadVideo");
                }
                break;
            default:
                print("not available yet");
                break;
        }
    }

}
