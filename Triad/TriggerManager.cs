using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Analytics;

namespace TriadGame
{


    public class TriggerManager : MonoBehaviour
    {
        public bool triggered = false;
        public Player player;

        public TextMeshPro noteName;
        public Text nameTheTriad;
        public Sprite clickedSprite;
        public Transform triadBox;
        public Oscillator osc;
        public Oscillator osc2;
        public Oscillator osc3;

        private int answerCounter;
        private int correctCheck;
        public int index;
        // Start is called before the first frame update

        private string[] answer = new string[3];
        private string[] potentialNotes = new string[8];
        private float[] freqs = new float[3];

        private GameObject clicked;
        private GameObject sceneMan;
        bool inAction = false;

        private void Start()
        {
            answerCounter = 0;
            nameTheTriad.gameObject.SetActive(false);
            correctCheck = 0;
            sceneMan = GameObject.Find("SceneMan");
            TriadManager tm = FindObjectOfType<TriadManager>();
        }

        private void Update()
        {
            if (player.stopped && gameObject == player.thisTrigger)
            {
                clicked = CheckClick();
                if (answerCounter < 3 && clicked != null && clicked.GetComponent<SpriteRenderer>().sprite != clickedSprite)
                {
                    clicked.GetComponent<SpriteRenderer>().sprite = clickedSprite;
                    freqs[answerCounter] = osc.transformCharacterToPitch(clicked.name);
                    if (clicked.name == answer[0] || clicked.name == answer[1] || clicked.name == answer[2])
                    {
                        correctCheck++;
                    }
                    answerCounter++;
                }
                if (answerCounter == 3 && !inAction)
                {
                    freqs = osc.OrderFrequencies(freqs[0], freqs[1], freqs[2]);
                    inAction = true;
                    if (correctCheck == 3)
                    {
                        StartCoroutine(waitTwo(true));
                    }
                    else if (correctCheck < 3)
                    {
                        StartCoroutine(waitTwo(false));
                    }
                }
            }
        }

        GameObject CheckClick()
        {
            if (Input.GetMouseButtonDown(0)) //checks to see what the click is
            {
                
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && !triggered)
            {
                triggered = true;
                player.thisTrigger = gameObject;
                player.stopped = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                TriadGenerator();
            }
        }

        public void TriadGenerator()
        {
            string[] wrongNotes = new string[2];
            int lvl2Multiplier = 1;
            index = Random.Range(0, 14); //pick which triad we're going to do
            print(index);
            if (TotalGameManager.instance.levelTwo)
            {
                lvl2Multiplier = 4;
                if (gameObject.name == "Triad1" || gameObject.name == "Triad2" || gameObject.name == "Triad3")
                {
                    
                    nameTheTriad.text = "Click the Notes in the ii° triad in " + TriadManager.instance.scales[index, 0] + " major.";
                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.triads[0, index * lvl2Multiplier];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.triads[1, index * lvl2Multiplier];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.triads[2, index * lvl2Multiplier];
                    answer[2] = potentialNotes[2];
                }
                else if (gameObject.name == "Triad4" || gameObject.name == "Triad5" || gameObject.name == "Triad6")
                {
                    nameTheTriad.text = "Click the Notes in the iv triad in " + TriadManager.instance.scales[index, 0] + " major.";
                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.triads[0, (index * lvl2Multiplier) + 1];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.triads[1, (index * lvl2Multiplier) + 1];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.triads[2, (index * lvl2Multiplier) + 1];
                    answer[2] = potentialNotes[2];
                }
                else if (gameObject.name == "Triad7" || gameObject.name == "Triad8" || gameObject.name == "Triad9")
                {
                    nameTheTriad.text = "Click the Notes in the bVI triad in " + TriadManager.instance.scales[index, 0] + " major.";
                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.triads[0, (index * lvl2Multiplier) + 2];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.triads[1, (index * lvl2Multiplier) + 2];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.triads[2, (index * lvl2Multiplier) + 2];
                    answer[2] = potentialNotes[2];
                }
                else if (gameObject.name == "Triad10" || gameObject.name == "Triad11" || gameObject.name == "Triad12")
                {
                    nameTheTriad.text = "Click the Notes in the Neapolitan triad in " + TriadManager.instance.scales[index, 0] + " major.";
                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.triads[0, (index * lvl2Multiplier) + 3];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.triads[1, (index * lvl2Multiplier) + 3];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.triads[2, (index * lvl2Multiplier) + 3];
                    answer[2] = potentialNotes[2];
                }
            }
            else
            {
                if (gameObject.name == "Triad1" || gameObject.name == "Triad2" || gameObject.name == "Triad3")
                {

                    nameTheTriad.text = "Click the Notes in the " + TriadManager.instance.triads[0, index] + " Major Triad";

                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.triads[0, index];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.triads[1, index];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.triads[2, index];
                    answer[2] = potentialNotes[2];
                }
                else if (gameObject.name == "Triad4" || gameObject.name == "Triad5" || gameObject.name == "Triad6")
                {
                    nameTheTriad.text = "Click the Notes in the " + TriadManager.instance.triads[0, index] + " Minor Triad";
                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.minorTriads[0, index];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.minorTriads[1, index];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.minorTriads[2, index];
                    answer[2] = potentialNotes[2];
                }
                else if (gameObject.name == "Triad7" || gameObject.name == "Triad8" || gameObject.name == "Triad9")
                {
                    nameTheTriad.text = "Click the Notes in the " + TriadManager.instance.triads[0, index] + " Diminished Triad";
                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.diminishedTriads[0, index];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.diminishedTriads[1, index];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.diminishedTriads[2, index];
                    answer[2] = potentialNotes[2];
                }
                else if (gameObject.name == "Triad10" || gameObject.name == "Triad11" || gameObject.name == "Triad12")
                {
                    nameTheTriad.text = "Click the Notes in the " + TriadManager.instance.triads[0, index] + " Augmented Triad";
                    nameTheTriad.gameObject.SetActive(true);
                    potentialNotes[0] = TriadManager.instance.augmentedTriads[0, index];
                    answer[0] = potentialNotes[0];
                    potentialNotes[1] = TriadManager.instance.augmentedTriads[1, index];
                    answer[1] = potentialNotes[1];
                    potentialNotes[2] = TriadManager.instance.augmentedTriads[2, index];
                    answer[2] = potentialNotes[2];
                }
            }
            wrongNotes = FindWrongNotes(potentialNotes[1]);//actually may be able to break this out of the if statement
            potentialNotes[3] = wrongNotes[0];
            potentialNotes[4] = wrongNotes[1];
            wrongNotes = FindWrongNotes(potentialNotes[2]);
            potentialNotes[5] = wrongNotes[0];
            potentialNotes[6] = wrongNotes[1];
            potentialNotes[7] = TriadManager.instance.notes[Random.Range(0, TriadManager.instance.notes.Length - 1)];
            while (potentialNotes[7] == potentialNotes[0] || potentialNotes[7] == potentialNotes[1] || potentialNotes[7] == potentialNotes[2] || potentialNotes[7]
                == potentialNotes[3] || potentialNotes[7] == potentialNotes[4] || potentialNotes[7] == potentialNotes[5] || potentialNotes[7] == potentialNotes[6])
            {
                potentialNotes[7] = TriadManager.instance.notes[Random.Range(0, TriadManager.instance.notes.Length - 1)];
            }
            reshuffle(potentialNotes);
            TriadTest(potentialNotes);
        }

        public string[] FindWrongNotes(string rightNote) //we'll have to change this to reflect incoming bb****
        {
            //return the two wrong notes
            string[] wrongN = new string[2];
            char[] characters = rightNote.ToCharArray();
            if (characters.Length == 1)
            {
                //then it must be a natural
                wrongN[0] = characters[0] + "#";
                wrongN[1] = characters[0] + "b";
            }
            else if (characters[1] == '#' || characters.Length == 3) //if it's a sharp or a double flat (returns normal note name and regular flat
            {
                wrongN[0] = characters[0].ToString();
                wrongN[1] = characters[0] + "b";
            }

            else //must be a flat or double sharp...weird, right?
            {
                wrongN[0] = characters[0].ToString();
                wrongN[1] = characters[0] + "#";
            }
            return wrongN;
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

        void TriadTest(string[] givenNotes)
        {
            Vector3 corner = new Vector3(player.transform.position.x, player.transform.position.y + 2, 0);
            int counter = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    var temp = Instantiate(triadBox, corner + new Vector3(1 * x, 1 * y, 0), Quaternion.identity);
                    temp.name = givenNotes[counter];
                    noteName = temp.GetComponentInChildren<TextMeshPro>();
                    noteName.text = temp.name;
                    counter++;
                }
            }
        }

        void MakeBridge(bool correct)
        {
            nameTheTriad.gameObject.SetActive(false);
            Vector3 xCorner = new Vector3(gameObject.transform.position.x + 3.4f, player.transform.position.y - .5f, 0);
            int counter = 0;
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("TriadBox");
            foreach (GameObject temp in tiles)
            {
                temp.transform.position = xCorner + new Vector3(1 * counter, 0, 0);
                if (!correct) //if it's not right, send the player through the blocks
                {
                    Destroy(temp.GetComponent<BoxCollider2D>());
                }
                counter++;
            }
            if (correct)
            {
                if(gameObject.name == "Triad3" || gameObject.name == "Triad6" || gameObject.name == "Triad9")
                {
                    TriadManager.instance.failures = 0; //reset the failures when we go to a new window
                }
                var tempChar = gameObject.name.ToCharArray(); //this is taking the last character of the trigger (the number) and turning it into the number for the array.
                if (tempChar.Length == 6) 
                {
                    int number = int.Parse(tempChar[tempChar.Length - 1].ToString());
                    if (!TotalGameManager.instance.levelTwo)
                    {
                        AnalyticsEvent.AchievementUnlocked("Beat Triad checkpoint " + number.ToString());
                    }
                    else
                    {
                        AnalyticsEvent.AchievementUnlocked("Beat Mix and N6 checkpoint " + number.ToString());
                    }
                    TotalGameManager.instance.checkpoints[number] = true;
                }
                else if(tempChar.Length == 7) //if it's 10 or 11 or 12
                {
                    int number = int.Parse(tempChar[tempChar.Length - 2].ToString() + tempChar[tempChar.Length-1].ToString());
                    if (!TotalGameManager.instance.levelTwo)
                    {
                        AnalyticsEvent.AchievementUnlocked("Beat Triad checkpoint " + number.ToString());
                    }
                    else
                    {
                        AnalyticsEvent.AchievementUnlocked("Beat Mix and N6 checkpoint " + number.ToString());
                    }
                    TotalGameManager.instance.checkpoints[number] = true;
                }
                if (!TotalGameManager.instance.levelTwo)
                {
                    ES3.Save<bool[]>("checkpoints", TotalGameManager.instance.checkpoints);
                }
                else
                {
                    ES3.Save<bool[]>("checkpoints2", TotalGameManager.instance.checkpoints);
                }
            }
            else
            {
                TriadManager.instance.failures++;
                if(TriadManager.instance.failures == 3)
                {
                    sceneMan.GetComponent<TriadSceneMan>().DisplayFail(gameObject);
                    TriadManager.instance.failures = 0;
                }
            }
            gameObject.SetActive(false);
            triggered = false;
            player.stopped = false;
            TriadManager tm = FindObjectOfType<TriadManager>();
#if UNITY_IOS || UNITY_ANDROID
            tm.mobileCanvas.gameObject.SetActive(true);
#endif
            inAction = false;
            answerCounter = 0;
            correctCheck = 0;
            StopAllCoroutines();
        }

        IEnumerator waitTwo(bool correct) //actually 1.5
        {
            StartCoroutine(osc.playNote(freqs[0], 1.5f));
            StartCoroutine(osc2.playNote(freqs[1], 1.5f));
            StartCoroutine(osc3.playNote(freqs[2], 1.5f));
            yield return new WaitForSeconds(1.5f);
            MakeBridge(correct);
        }
    }

}

