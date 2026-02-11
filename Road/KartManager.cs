using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class KartManager : MonoBehaviour
{
    //bring in text file that has all the sharps in order up to A#. Do the same for all the flats in order up to
    //Gb. Then have two random numbers - 0/1 to determine flat or sharp key. Then 0-4 to determine how many sharps. This should be in a function that
    //can be called again. Each checkpoint should look back here to see what number. Each box should check to see if it's the right "" and if so, advance
    //the number and speedpad. If not spin the car
    // Start is called before the first frame update
    public string[] sharps = new string[] { "F#", "C#", "G#", "D#", "A#" };
    public string[] flats = new string[] { "Bb", "Eb", "Ab", "Db", "Gb" };
    public int index; //the number of sharps or flats (-1)
    public int choice; //whether it's sharp or flat
    public int augType; //what type of +6 0 = ger, 1 = fr, 2 = it
    public int indexPlace; //where the player is in the key sig.
    public int laps;
    public string scaleName;
    public Text timerText;
    public int timer = 60;
    public int timerStart = 65;
    public int timerIncrease = 10;
    public Text instructionText;
    public Text keysGot;
    public Text newKey;
    public Text lapText;
    private bool inTimer = false;
    public bool selected = false;
    public bool antiCheat = false;
    public TextAsset scalesText;
    public TextAsset notesText;
    private ScaleGenerator sg;
    public string[,] scales; //list of possible scales
    public string[] noteNames; //list of notes
    public string[,] ger6;
    public string[,] fr6;
    public string[,] it6;
    public bool paused = false;
    private int fail;
    public GameObject failPanel;
    public Text failText;
    public Text pauseText;
    public Rigidbody car;
    private Scene thisScene;
    public AudioSource audio;
    public AudioClip rightOne;
    public AudioClip wrongOne;
    public GameObject quitButton;
    public Text lapFlash;
    public GameObject mobileCanvas;

    //for before integration
    public bool level1;
    public bool level2;



    private void Awake()
    {
#if UNITY_IOS || UNITY_ANDROID
        mobileCanvas.gameObject.SetActive(true);
#else
        mobileCanvas.gameObject.SetActive(false);
#endif
        if (TotalGameManager.instance.levelTwo)
        {
            level2 = true;
            level1 = false;
            if (ES3.KeyExists("RaceLevel1"))
            {
                if (ES3.Load<bool>("RaceLevel1"))
                {
                    ES3.Save<int>("RaceLaps", 1);
                    ES3.Save<int>("RaceTimer", timerStart);
                }
            }
            ES3.Save<bool>("RaceLevel1", false);
        }
        else
        {
            level1 = true;
            level2 = false;
            if(ES3.KeyExists("RaceLevel1"))
            {
                if(!ES3.Load<bool>("RaceLevel1"))
                {
                    ES3.Save<int>("RaceLaps", 1);
                    ES3.Save<int>("RaceTimer", timerStart);
                }
            }
            ES3.Save<bool>("RaceLevel1", true);
        }
        fail = 0;
        lapFlash.gameObject.SetActive(false);
        thisScene = SceneManager.GetActiveScene();
       /* if(level1)
        {
            AnalyticsEvent.LevelStart("Key Signature Race", thisScene.buildIndex);
        }
        else
        {
            AnalyticsEvent.LevelStart("+6 Race", thisScene.buildIndex);
        }*/
        
        failPanel.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(true);
        sg = FindObjectOfType<ScaleGenerator>();
        scales = sg.makeScale(scalesText);
        noteNames = sg.makeNote(notesText);
        ger6 = sg.MakeAug6(scales, 0);
        fr6 = sg.MakeAug6(scales, 1);
        it6 = sg.MakeAug6(scales, 2);
        if (level1)
        {
            MakeDecisions();
        }
        else
        {
            MakeDecisionsLvl2();
        }
        if (ES3.KeyExists("RaceLaps") && ES3.KeyExists("RaceTimer"))
        {
            laps = ES3.Load<int>("RaceLaps");
            timer = ES3.Load<int>("RaceTimer");
        }
        else
        {
            laps = 1;
            timer = timerStart;
        }
        indexPlace = 0;
        timerText.text = "Timer: " + timer;
        lapText.text = "Lap " + laps;
        keysGot.text = "Collected: ";
        newKey.gameObject.SetActive(false);
    }
    private void MakeDecisions()
    {
        choice = Random.Range(0, 2);
        index = Random.Range(0, 5);
        if(choice == 0)
        {
            switch (index)
            {
                case 0: scaleName = "F Major";
                    break;
                case 1: scaleName = "Bb Major";
                    break;
                case 2: scaleName = "Eb Major";
                    break;
                case 3: scaleName = "Ab Major";
                    break;
                case 4: scaleName = "Db Major";
                    break;
                default: scaleName = "I'm Broken";
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    scaleName = "G Major";
                    break;
                case 1:
                    scaleName = "D Major";
                    break;
                case 2:
                    scaleName = "A Major";
                    break;
                case 3:
                    scaleName = "E Major";
                    break;
                case 4:
                    scaleName = "B Major";
                    break;
                default:
                    scaleName = "I'm Broken";
                    break;
            }
        }
        instructionText.text = "Choose the sharps/flats IN ORDER of the " + scaleName + " key signature.";
    }
    private void MakeDecisionsLvl2()
    {
        choice = Random.Range(0, scales.GetLength(0)); //choice will be which scale
        augType = Random.Range(0, 3); //chooses which type of aug6
        if (augType == 0 || augType == 1)
        {
            index = 3;
        }
        else
        {
            index = 2;
        }
        scaleName = scales[choice, 0];
        if(augType == 0)
        {
            instructionText.text = "Choose the notes from the German Augmented 6th Chord in " + scaleName + " major.";
        }
        else if(augType == 1)
        {
            instructionText.text = "Choose the notes from the French Augmented 6th Chord in " + scaleName + " major.";
        }
        else
        {
            instructionText.text = "Choose the notes from the Italian Augmented 6th Chord in " + scaleName + " major.";
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!inTimer && timer >0 && !paused)
        {
            StartCoroutine(TimerDown());
        }
        if(timer == 0 || laps == 4 && !paused)
        {
            EndGame();
        }
        if(paused)
        {
            car.velocity = new Vector3(0, 0, 0);
        }
        
    }
    private void FailText()
    {
        EndPause();
        if (level1)
        {
            failText.text = "Let's think about our circle of fifths - remember Father Christmas Got Dad An Electric Blanket! We know C Major has no" +
                " sharps or flats, and that G Major has 1 sharp (F#) and F Major has 1 flat (Bb). Going clockwise around the circle adds sharps (so D" +
                " Major has 2 sharps), going backwards (and adding flats to the names) adds flats (so after F we go back to Bb, then Eb). The sharps are " +
                "in order from F-C-G-D-A-E-B and the flats go backwards Bb-Eb-Ab-Db-Gb-Cb). Draw out the circle and use it as a guide - when you learn this" +
                " you will be so good at theory! Everything will fall into place! Keep trying! (Click 'Unpause' to close this window)";
        }
        else
        {
            failText.text = "We're in major keys here, so let's think of scale degrees. All three types have flat-6, 1, and sharp-4 (so think of the scale " +
                "and say...the 6th scale degree in C Major is A...flatted is Ab...etc.). The German Augmented 6th adds a flat-3 (in major) and the French adds " +
                "a flat-2. Pause if you need to and work it out. Practice makes perfect! (Click 'Unpause' to close this window)";
        }
        failPanel.gameObject.SetActive(true);
    }
    public void EndPause()
    {
        if(!paused)
        {
            pauseText.text = "Unpause";
            quitButton.gameObject.SetActive(false);
            paused = true;
        }
        else
        {
            pauseText.text = "Pause";
            failPanel.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(true);
            paused = false;
        }
    }
    private void EndGame()
    {
        paused = true;
        failPanel.gameObject.SetActive(true);
        if(laps == 4)
        {
            failText.text = "Congratulations! You won with " + timer + " seconds remaining!";
            if (level1)
            {
                AnalyticsEvent.LevelComplete("Key Signature Race", thisScene.buildIndex);
                AnalyticsEvent.LevelComplete("Key Race");
                TotalGameManager.instance.finishedKS = true;
                ES3.Save<bool>("finishedKS", true);
            }
            else
            {
                AnalyticsEvent.LevelComplete("+6 Race", thisScene.buildIndex);
                AnalyticsEvent.LevelComplete("A6 Race");
                TotalGameManager.instance.finishedA6 = true;
                ES3.Save<bool>("finishedA6", true);
            }
        }
        else
        {
            failText.text = "Sorry - you ran out of time! Try again!";
        }
        StartCoroutine(WaitFive());

    }
    IEnumerator WaitFive()
    {
        yield return new WaitForSeconds(5);
        if(laps==4)
        {
            SceneManager.LoadScene("RaceVideo"); 
        }
        else
        {
            quitButton.gameObject.SetActive(false);
            if (level1)
            {
                AnalyticsEvent.LevelFail("Key Signature Race", thisScene.buildIndex);
            }
            else
            {
                AnalyticsEvent.LevelFail("+6 Race", thisScene.buildIndex);
            }
            ES3.Save<int>("RaceLaps", 1);
            ES3.Save<int>("RaceTimer", timerStart);
            SceneManager.LoadScene("MainHub");
        }

    }
    IEnumerator TimerDown()
    {
        inTimer = true;
        yield return new WaitForSeconds(1);
        timer--;
        timerText.text = "Timer: " + timer;
        inTimer = false;
    }
    IEnumerator ShowText()
    {
        newKey.text = scaleName;
        newKey.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        newKey.gameObject.SetActive(false);
    }
    IEnumerator DisappearBox(GameObject box)
    {
        yield return new WaitForFixedUpdate();
        box.SetActive(false);
        yield return new WaitForSeconds(3);
        box.SetActive(true);
    }
    public bool CheckAnswer(string answer, GameObject box)
    {
        StartCoroutine(DisappearBox(box));
        if (level1)
        {
            if (choice == 0) //if it's flats
            {
                if (flats[indexPlace] == answer)
                {
                    //go faster
                    audio.clip = rightOne;
                    audio.Play();
                    keysGot.text += flats[indexPlace];//display string on screen
                    timer += timerIncrease;
                    if (indexPlace < index)
                    {
                        indexPlace++;
                    }
                    else
                    {
                        ResetProblem();
                    }
                    return true;
                }
                else
                {
                    //spin out
                    audio.clip = wrongOne;
                    audio.Play();
                    fail++;
                    if (fail >= 4)
                    {
                        fail = 0;
                        FailText();
                    }
                    return false;
                }
            }
            else
            {
                if (sharps[indexPlace] == answer)
                {
                    audio.clip = rightOne;
                    audio.Play();
                    keysGot.text += sharps[indexPlace];//display string on screen
                    timer += timerIncrease;
                    if (indexPlace < index)
                    {
                        indexPlace++;
                    }
                    else
                    {
                        ResetProblem();
                    }
                    return true;
                }
                else
                {
                    audio.clip = wrongOne;
                    audio.Play();
                    fail++;
                    if (fail >= 4)
                    {
                        fail = 0;
                        FailText();
                    }
                    return false;
                }
            }
        }
        else
        {
            if(augType == 0)
            {
                if(ger6[choice,indexPlace] == answer)
                {
                    audio.clip = rightOne;
                    audio.Play();
                    keysGot.text += ger6[choice, indexPlace];
                    timer += timerIncrease;
                    if (indexPlace < index)
                    {
                        indexPlace++;
                    }
                    else
                    {
                        ResetProblem();
                    }
                    return true;
                }
                else
                {
                    audio.clip = wrongOne;
                    audio.Play();
                    fail++;
                    if (fail >= 4)
                    {
                        fail = 0;
                        FailText();
                    }
                    return false;
                }
            }
            else if(augType == 1)
            {
                if (fr6[choice, indexPlace] == answer)
                {
                    audio.clip = rightOne;
                    audio.Play();
                    keysGot.text += fr6[choice, indexPlace];
                    timer += timerIncrease;
                    if (indexPlace < index)
                    {
                        indexPlace++;
                    }
                    else
                    {
                        ResetProblem();
                    }
                    return true;
                }
                else
                {
                    audio.clip = wrongOne;
                    audio.Play();
                    fail++;
                    if (fail >= 4)
                    {
                        fail = 0;
                        FailText();
                    }
                    return false;
                }
            }
            else
            {
                if (it6[choice, indexPlace] == answer)
                {
                    audio.clip = rightOne;
                    audio.Play();
                    keysGot.text += it6[choice, indexPlace];
                    timer += timerIncrease;
                    if (indexPlace < index)
                    {
                        indexPlace++;
                    }
                    else
                    {
                        ResetProblem();
                    }
                    return true;
                }
                else
                {
                    audio.clip = wrongOne;
                    audio.Play();
                    fail++;
                    if (fail >= 4)
                    {
                        fail = 0;
                        FailText();
                    }
                    return false;
                }
            }
        }
    }
    public void Quitter()
    {
        SceneManager.LoadScene("MainHub");
    }
    private void ResetProblem()
    {
        indexPlace = 0;
        if(level1)
        {
            MakeDecisions();
        }
        else
        {
            MakeDecisionsLvl2();
        }
        StartCoroutine(ShowText());
        keysGot.text = "Collected: ";
    }

}
