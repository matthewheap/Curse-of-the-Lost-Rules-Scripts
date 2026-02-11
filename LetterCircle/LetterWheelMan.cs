using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class LetterWheelMan : MonoBehaviour
{
    public string currentWord; 
    public TextMeshPro spelledWord;
    public int wordLength;
    private int boxesleft = 9;
    private bool checking = false;
    private bool mouseUpTime = false; //make them lift the mouse
    public List<Collider2D> turnOn = new List<Collider2D>(); //list of box colliders to turn on
    private LineRenderer line;
    public bool isMousePressed;
    public List<Vector3> pointsList;
    private Vector3 mousePos;
    private LetterWheelMan gm;
    public GameObject[] wheelLetters; //array containing the gameobjects of the letters
    public GameObject[] boxes; //array containing the gameobjects of the boxes that hide the picture
    private string[] sharps = new string[] { "F#", "C#", "G#", "D#", "A#", "E#", "B#", "Fx" };
    private string[] flats = new string[] { "Bb", "Eb", "Ab", "Db", "Gb", "Cb", "Fb", "Bbb" };
    private string[] letterOptions = new string[8];
    private string[] sharpkeys = new string[] { "G", "D", "A", "E", "B", "F#", "C#" };
    private string[] sharpMinor = new string[] { "E", "B", "F#", "C#", "G#", "D#", "A#" };
    private string[] flatkeys = new string[] { "F", "Bb", "Eb", "Ab", "Db", "Gb", "Cb" };
    private string[] flatMinor = new string[] { "D", "G", "C", "F", "Bb", "Eb", "Ab" };
    private string[] keyOptions = new string[9];
    private string[,] octaOptions;
    private string[,] lvl2Choices = new string[2,9];
    public List<string> answer;
    private Vector3 boxCorner = new Vector3(-1.95f, 5.07f, -.5f);
    public Sprite image1;
    public Sprite image2;
    public Sprite image3;
    public Sprite image4;
    private int rank;
    private int failOrder;
    private int failType;
    private int level2Key;
    public GameObject composer;
    public GameObject failPanel;
    public Text failText;
    public bool sharpKey;
    private bool minor;
    public TextMeshPro instructions;
    public Text levelText;
    private string drawtext = "Draw the key from one note to the next IN ORDER";
    private string boxtext = "Click the box that matches the key you have drawn.";
    private bool textStays = false;
    [SerializeField] TextAsset octaIn;
    private bool level1;

    // Start is called before the first frame update
    private void Awake()
    {
        if (TotalGameManager.instance.levelTwo)
        {
            level1 = false; 
        }
        else
        {
            level1 = true;
        }
        if (level1)
        {
            drawtext = "Draw the key from one note to the next IN ORDER";
            boxtext = "Click the box that matches the key you have drawn.";
        }
        else
        {
            drawtext = "Draw the chord from one note to the next in any order";
            boxtext = "Click the box that matches the chord you have drawn.";
        }
        rank = TotalGameManager.instance.letterRank;
        failPanel.gameObject.SetActive(false);
        failOrder = 0;
        failType = 0;
        octaOptions = makeScale();
        line = gameObject.GetComponent<LineRenderer>();
        instructions.text = drawtext;
        wheelLetters = GameObject.FindGameObjectsWithTag("Letter");
        boxes = GameObject.FindGameObjectsWithTag("TriadBox");
        ResetBoxes();
        line.positionCount = 0;
        line.startWidth = .05f;
        line.endWidth = .05f;
        line.useWorldSpace = true;
        isMousePressed = false;
        pointsList = new List<Vector3>();
        wordLength = 0;
        currentWord = "";
        spelledWord.text = "";

    }

    public string[,] makeScale() //this function makes a scale
    {
        string[] lines = octaIn.text.Split('\n');
        string[][] tempScale = new string[lines.Length][];
        string[,] newScale = new string[lines.Length, 8];
        int verticalLine = 0;
        foreach (string line in lines)
        {
            tempScale[verticalLine++] = line.Split(' ');
        }
        for (int x = 0; x < tempScale.Length; x++)
        {
            for (int y = 0; y < 8; y++) //fix this eventually to allow any scale length
            {
                newScale[x, y] = tempScale[x][y];
            }
        }
        return newScale;
    }
    private void ResetBoxes() 
    {
        failOrder = 0;
        failType = 0;
        boxesleft = 9;
        levelText.text = "Level: " + (rank + 1).ToString();
        answer.Clear();
        if (rank == 0)
        {
            composer.GetComponent<SpriteRenderer>().sprite = image1;
            sharpKey = true;
            minor = false;
        }
        else if (rank == 1)
        {
            composer.GetComponent<SpriteRenderer>().sprite = image2;
            sharpKey = false;
            minor = false;
        }
        else if (rank == 2)
        {
            composer.GetComponent<SpriteRenderer>().sprite = image3; 
            sharpKey = true;
            minor = true;
        }
        else
        {
            composer.GetComponent<SpriteRenderer>().sprite = image4; 
            sharpKey = false;
            minor = true;
        }
        if (level1)
        {
            if (sharpKey)
            {
                for (int x = 0; x < 8; x++)
                {
                    letterOptions[x] = sharps[x];
                }
                if (!minor)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        keyOptions[y] = sharpkeys[y];
                    }
                    keyOptions[7] = sharpkeys[Random.Range(0, 7)];
                    keyOptions[8] = sharpkeys[Random.Range(0, 7)];
                }
                else
                {
                    for (int y = 0; y < 7; y++)
                    {
                        keyOptions[y] = sharpMinor[y];
                    }
                    keyOptions[7] = sharpMinor[Random.Range(0, 7)];
                    keyOptions[8] = sharpMinor[Random.Range(0, 7)];
                }
                
            }
            else
            {
                for (int x = 0; x < 8; x++)
                {
                    letterOptions[x] = flats[x];
                }
                if (!minor)
                {
                    for (int y = 0; y < 7; y++)
                    {
                        keyOptions[y] = flatkeys[y];
                    }
                    keyOptions[7] = keyOptions[Random.Range(0, 7)];
                    keyOptions[8] = keyOptions[Random.Range(0, 7)];
                }
                else
                {
                    for (int y = 0; y < 7; y++)
                    {
                        keyOptions[y] = flatMinor[y];
                    }
                    keyOptions[7] = flatMinor[Random.Range(0, 7)];
                    keyOptions[8] = flatMinor[Random.Range(0, 7)];
                }
            }
        }
        else //level2
        {
            level2Key = Random.Range(0, 7); //picks which octatonic
            lvl2Choices[0, 0] = "It+6 in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 0] = octaOptions[level2Key, 0] + " " + octaOptions[level2Key, 4] + " " + octaOptions[level2Key, 5];

            lvl2Choices[0, 1] = "Ger+6 in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 1] = octaOptions[level2Key, 0] + " " + octaOptions[level2Key, 4] + " " + octaOptions[level2Key, 5] + " " + octaOptions[level2Key, 2];

            lvl2Choices[0, 2] = "Fr+6 in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 2] = octaOptions[level2Key, 0] + " " + octaOptions[level2Key, 4] + " " + octaOptions[level2Key, 5] + " " + octaOptions[level2Key, 1];

            lvl2Choices[0, 3] = "iv in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 3] = octaOptions[level2Key, 0] + " " + octaOptions[level2Key, 3] + " " + octaOptions[level2Key, 5]; //5 is b6

            lvl2Choices[0, 4] = "ii° in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 4] = octaOptions[level2Key, 1] + " " + octaOptions[level2Key, 3] + " " + octaOptions[level2Key, 5];

            lvl2Choices[0, 5] = "bVI in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 5] = octaOptions[level2Key, 0] + " " + octaOptions[level2Key, 2] + " " + octaOptions[level2Key, 5];

            lvl2Choices[0, 6] = "V7/V in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 6] = octaOptions[level2Key, 1] + " " + octaOptions[level2Key, 4] + " " + octaOptions[level2Key, 0] + " " + octaOptions[level2Key, 6];

            lvl2Choices[0, 7] = "vii°7 in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 7] = octaOptions[level2Key, 1] + " " + octaOptions[level2Key, 3] + " " + octaOptions[level2Key, 5] + " " + octaOptions[level2Key, 7];

            lvl2Choices[0, 8] = "vii°7/V in " + octaOptions[level2Key, 0];
            lvl2Choices[1, 8] = octaOptions[level2Key, 0] + " " + octaOptions[level2Key, 2] + " " + octaOptions[level2Key, 4] + " " + octaOptions[level2Key, 6];
            for (int x = 0; x < 9; x++)
            {
                keyOptions[x] = lvl2Choices[0, x];
            }
            for(int x = 0; x<8; x++)
            {
                letterOptions[x] = octaOptions[level2Key, x];
            }
        }
        reshuffle(letterOptions);
        reshuffle(keyOptions);
        int j = 0;
        foreach(GameObject gob in wheelLetters)
        {
            gob.GetComponent<TextMeshPro>().text = letterOptions[j];
            j++;
        }
        j = 0;
        foreach(GameObject bob in boxes)
        {
            if (!minor || !level1)
            {
                bob.GetComponentInChildren<TextMeshPro>().text = keyOptions[j] + " Major";
            }
            else
            {
                bob.GetComponentInChildren<TextMeshPro>().text = keyOptions[j] + " Minor";
            }
            bob.GetComponent<BoxCollider2D>().enabled = false;
            bob.SetActive(true);
            j++;
        }
    }
    void reshuffle(string[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseUpTime = false;
        }
        if (Input.GetMouseButtonDown(0) && !checking)
        {
            isMousePressed = true;
            line.positionCount = 0;
            pointsList.RemoveRange(0, pointsList.Count);
        }
        if(Input.GetMouseButtonDown(0) && checking)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "TriadBox")
            {
                if (level1)
                {
                    if (sharpKey)
                    {
                        for (int x = 0; x < wordLength; x++)
                        {
                            if (answer[x] != sharps[x])
                            {
                                instructions.text = "Wrong Order, Try Again!";
                                failOrder++;
                                if(failOrder==3)
                                {
                                    MakeFailText();
                                }
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        } //if we got here it was in the right order
                        string[] key = hit.collider.GetComponentInChildren<TextMeshPro>().text.Split(' ');
                        if (!minor)
                        {
                            if (key[0] == sharpkeys[wordLength - 1])
                            {
                                hit.collider.gameObject.SetActive(false);
                                instructions.text = "Nice Job! Keep Going!";
                                textStays = true;
                                boxesleft--;
                                if (boxesleft == 0)
                                {
                                    StartCoroutine(WinLevel());
                                }
                                ResetThings();
                                return;
                            }

                            else //it was wrong
                            {
                                instructions.text = "Sorry, that wasn't the right key. Try Again!";
                                failType++;
                                if (failType == 3)
                                {
                                    MakeFailText();
                                }
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        }
                        else
                        {
                            if (key[0] == sharpMinor[wordLength - 1])
                            {
                                hit.collider.gameObject.SetActive(false);
                                instructions.text = "Nice Job! Keep Going!";
                                textStays = true;
                                boxesleft--;
                                if (boxesleft == 0)
                                {
                                    StartCoroutine(WinLevel());
                                }
                                ResetThings();
                                return;
                            }

                            else //it was wrong
                            {
                                instructions.text = "Sorry, that wasn't the right key. Try Again!";
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        }
                    }
                    else //it's flats
                    {
                        for (int x = 0; x < wordLength; x++)
                        {
                            if (answer[x] != flats[x])
                            {
                                instructions.text = "Wrong Order, Try Again!";
                                failOrder++;
                                if (failOrder == 3)
                                {
                                    MakeFailText();
                                }
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        } //if we got here it was in the right order
                        string[] key = hit.collider.GetComponentInChildren<TextMeshPro>().text.Split(' ');
                        if (!minor)
                        {
                            if (key[0] == flatkeys[wordLength - 1])
                            {
                                hit.collider.gameObject.SetActive(false);
                                instructions.text = "Nice Job! Keep Going!";
                                textStays = true;
                                boxesleft--;
                                if (boxesleft == 0)
                                {
                                    StartCoroutine(WinLevel());
                                }
                                ResetThings();
                                return;
                            }
                            else //it was wrong
                            {
                                instructions.text = "Sorry, that wasn't the right key. Try Again!";
                                failType++;
                                if (failType == 3)
                                {
                                    MakeFailText();
                                }
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        }
                        else
                        {
                            if (key[0] == flatMinor[wordLength - 1])
                            {
                                hit.collider.gameObject.SetActive(false);
                                instructions.text = "Nice Job! Keep Going!";
                                textStays = true;
                                boxesleft--;
                                if (boxesleft == 0)
                                {
                                    StartCoroutine(WinLevel());
                                }
                                ResetThings();
                                return;
                            }
                            else //it was wrong
                            {
                                instructions.text = "Sorry, that wasn't the right key. Try Again!";
                                failType++;
                                if (failType == 3)
                                {
                                    MakeFailText();
                                }
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        }
                    }
                }
                else //level 2 logic
                {
                    string correct;
                    string [] boxArray = hit.collider.GetComponentInChildren<TextMeshPro>().text.Split(' ');
                    if (boxArray[0] == "It+6")
                    {
                        correct = lvl2Choices[1, 0];
                    }
                    else if (boxArray[0] == "Ger+6")
                    {
                        correct = lvl2Choices[1, 1];
                    }
                    else if (boxArray[0] == "Fr+6")
                    {
                        correct = lvl2Choices[1, 2];
                    }
                    else if (boxArray[0] == "iv")
                    {
                        correct = lvl2Choices[1, 3];
                    }
                    else if (boxArray[0] == "ii°")
                    {
                        correct = lvl2Choices[1, 4];
                    }
                    else if (boxArray[0] == "bVI")
                    {
                        correct = lvl2Choices[1, 5];
                    }
                    else if (boxArray[0] == "V7/V")
                    {
                        correct = lvl2Choices[1, 6];
                    }
                    else if (boxArray[0] == "vii°7")
                    {
                        correct = lvl2Choices[1, 7];
                    }
                    else
                    {
                        correct = lvl2Choices[1, 8];
                    }
                    print(correct);
                    string[] correctAns = correct.Split(' ');
                    if (answer.Count != correctAns.Length) //if they're not the same length, it's wrong.
                    {
                        instructions.text = "Sorry, that wasn't the right answer. Try Again!";
                        failType++;
                        if (failType == 3)
                        {
                            MakeFailText();
                        }
                        textStays = true;
                        ResetThings();
                        return;
                    }
                    int length = correctAns.Length;
                    print(length);
                    for (int x = 0; x < length; x++)
                    {
                        if (length == 4)
                        {
                            if (answer[x] != correctAns[0] && answer[x] != correctAns[1] && answer[x] != correctAns[2] && answer[x] != correctAns[3]) //if it doesn't equal one of the letters
                            {
                                instructions.text = "Sorry, that wasn't the right answer. Try Again!";
                                failType++;
                                if (failType == 3)
                                {
                                    MakeFailText();
                                }
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        }
                        else //length = 3
                        {
                            if (answer[x] != correctAns[0] && answer[x] != correctAns[1] && answer[x] != correctAns[2]) //if it doesn't equal one of the letters
                            {
                                instructions.text = "Sorry, that wasn't the right answer. Try Again!";
                                failType++;
                                if (failType == 3)
                                {
                                    MakeFailText();
                                }
                                textStays = true;
                                ResetThings();
                                return;
                            }
                        }
                    }
                    hit.collider.gameObject.SetActive(false);
                    instructions.text = "Nice Job! Keep Going!";
                    textStays = true;
                    boxesleft--;
                    if (boxesleft == 0)
                    {
                        StartCoroutine(WinLevel());
                    }
                    ResetThings();
                    return;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && !checking)
        {
            isMousePressed = false;
            if (wordLength > 0)
            {
                CheckWord();
            }
        }
        // Drawing line when mouse is moving(presses)
        if (isMousePressed)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) //if we hit a letter, draw a line there 
            {
                turnOn.Add(hit.collider);
                if(wordLength>0)
                {
                    currentWord += "\n"; //adds a carriage break after the each letter
                }
                currentWord += hit.transform.GetComponent<TextMeshPro>().text;
                answer.Add(hit.transform.GetComponent<TextMeshPro>().text);
                spelledWord.text = currentWord; //keeps the updated word on the left
                hit.collider.enabled = false;
                wordLength++;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                pointsList.Add(mousePos);
                line.positionCount = pointsList.Count;
                line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
            }
            else//this doesn't work
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                if (!pointsList.Contains(mousePos) && pointsList.Count > 0)
                {
                    if (pointsList.Count != 1)
                    {
                        pointsList.Remove(pointsList[pointsList.Count - 1]);
                    }
                    pointsList.Add(mousePos);
                    line.positionCount = pointsList.Count;
                    line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
                }
            }
        }
    }
    public void quitter() //add save here
    {
        TotalGameManager.instance.letterRank = rank;
        ES3.Save<int>("letterRank", rank);
        SceneManager.LoadScene("MainHub"); //add in save feature
    }
    public void closeFail()
    {
        failPanel.gameObject.SetActive(false);
    }
    public void MakeFailText()
    {
        if(failOrder == 3)
        {
            failOrder = 0;
            failText.text = "Remember that the circle of fifths can help you here! Father Christmas Got Dad An Electric Blanket gives you the order of the sharps, and going backwards (Battle Ends And Down Goes Charles' Father is another " +
                "mnemonic) gives you the flats. Remember to go in order!";
        }
        else if(failType == 3 && level1)
        {
            failType = 0;
            failText.text = "Let's think about our circle of fifths - remember Father Christmas Got Dad An Electric Blanket! We know C Major has no" +
                " sharps or flats, and that G Major has 1 sharp (F#) and F Major has 1 flat (Bb). Going clockwise around the circle adds sharps (so D" +
                " Major has 2 sharps), going backwards (and adding flats to the names) adds flats (so after F we go back to Bb, then Eb). The sharps are " +
                "in order from F-C-G-D-A-E-B and the flats go backwards Bb-Eb-Ab-Db-Gb-Cb). Draw out the circle and use it as a guide - when you learn this" +
                " you will be so good at theory! Everything will fall into place! Keep trying!";
        }
        else
        {
            failType = 0;
            failText.text = "What might be helpful with this one is to figure out first the scale with it's diatonic triads, then add the mixture chords (usually iv, iio, bVI), and finally figure out the augmented 6th chords. " +
                "For those, remember we're in major keys here, so let's think of scale degrees. All three types have flat-6, 1, and sharp-4 (so think of the scale " +
                "and say...the 6th scale degree in C Major is A...flatted is Ab...etc.). The German Augmented 6th adds a flat-3 (in major) and the French adds " +
                "a flat-2. Pause if you need to and work it out. Practice makes perfect!";
        }
        failPanel.gameObject.SetActive(true);
    }
    void CheckWord()
    {
        checking = true; 
        instructions.text = boxtext;
        foreach (GameObject bob in boxes)
        {
            bob.GetComponent<Collider2D>().enabled = true;
        }
        //ResetThings();
    }
    IEnumerator WaitToClear()
    {
        int waitTime = 1;
        if(textStays)
        {
            waitTime = 2;
        }
        yield return new WaitForSeconds(waitTime);
        instructions.text = drawtext;
        wordLength = 0;
        currentWord = "";
        spelledWord.text = "";
        answer.Clear();
        line.positionCount = 0;
        pointsList.RemoveRange(0, pointsList.Count);
        checking = false;
        textStays = false;
    }
    IEnumerator WinLevel()
    {
        rank++;
        TotalGameManager.instance.letterRank = rank;
        ES3.Save<int>("letterRank", rank);
        if (rank < 4)
        {
            instructions.text = "Gaze on the composer in a compromising situation...";
        }
        else
        {
            //win go back to hub
            instructions.text = "Congratulations - you beat four levels!";
            if(TotalGameManager.instance.levelTwo)
            {
                TotalGameManager.instance.finishedLW2 = true;
                ES3.Save<bool>("finishedLW2", true);
                AnalyticsEvent.AchievementUnlocked("Finished LetterWheel on Level 2");
                AnalyticsEvent.LevelComplete("All Chords");
            }
            else
            {
                TotalGameManager.instance.finishedLW1 = true;
                ES3.Save<bool>("finishedLW1", true);
                AnalyticsEvent.AchievementUnlocked("Finished LetterWheel on Level 1");
                AnalyticsEvent.LevelComplete("Key Sig Wheel");
            }
        }
        yield return new WaitForSeconds(4);
        if (rank < 4)
        {
            ResetBoxes();
        }
        else
        {
            SceneManager.LoadScene("LetterWheelVideo");
        }
    }
    public void ResetThings()
    {
        for (int x = 0; x < turnOn.Count; x++)
        {
            turnOn[x].enabled = true;
        }
        foreach (GameObject bob in boxes)
        {
            bob.GetComponent<Collider2D>().enabled = false;
        }
        turnOn.Clear();
        StartCoroutine(WaitToClear());
    }
}
