using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FairGame
{
    public class BalloonGenerator : MonoBehaviour
    {

        public int index; //figures out which note
        [SerializeField] Image noteQuestion; //the place where the note is displayed.

        private string correctNote; //the right note
        private string[] notes; //all the note names
        private Sprite[] noteCards; //all the possible pictures of notes
        private Sprite[] balloonStates; //three balloon sprites

        private Button tempButton;
        private bool isPopped;

        public float timer; //for counting down
        private int xCorrect;//
        private int yCorrect; //these are the coordinates of the correct balloon

        public Text timeLeft; //how much time is left
        public Text noteName;        
        public RectTransform NoteBalloon;//the prefab balloon

        IEnumerator co; //to store a specific function
        public GameObject canvas; //need this because Unity is persnickety 
        public AudioSource popper;


        void Start()
        {
            xCorrect = Random.Range(0, 4);
            yCorrect = Random.Range(0, 2);//generate where the correct balloon will be.
            notes = FairGameManager.instance.notes;
            noteCards = FairGameManager.instance.noteCards; //these grab the notenames and sprites from FairGameManager...so we only have to do the generation once.
            balloonStates = FairGameManager.instance.balloonStates;
            index = Random.Range(0, noteCards.Length - 1); //what is the note we're going to see
            if (TotalGameManager.instance.fairRank < 3)
            {
                timer = 10;
            }
            else
            {
                timer = 10-(TotalGameManager.instance.fairLevel);
            }
            timeLeft.text = "10";
            noteQuestion.sprite = noteCards[index];
            correctNote = noteCards[index].name;
            isPopped = false;
            BalloonMaker();
            co = StartCountdown(timer);
            StartCoroutine(co);
        }

        // Update is called once per frame

        void BalloonMaker()
        {
            string tempNote;
            for (int y = 0; y<3; y++)
            {
                for(int x = 0; x<5; x++)
                {
                    tempNote = wrongPitch();
                    var temp = Instantiate(NoteBalloon, new Vector3(-350 + (x*300), 204 - (y * 300), 0), Quaternion.identity);
                    temp.transform.SetParent(canvas.transform, false);
                    Button buttonCtrl = temp.GetComponent<Button>(); // see below
                    buttonCtrl.onClick.AddListener(() => balloonPopper(buttonCtrl.GetComponentInChildren<Text>().text, buttonCtrl)); //this is necessary because of stupid prefab stuff - it creates an onClick 
                    if (TotalGameManager.instance.fairRank == 1) //easymode
                    {
                        if (x == xCorrect && y == yCorrect)
                        {
                            temp.name = stripNumber(correctNote);
                            noteName = temp.GetComponentInChildren<Text>();
                            noteName.text = temp.name;
                        }
                        else
                        {
                            temp.name = tempNote;
                            noteName = temp.GetComponentInChildren<Text>();
                            noteName.text = temp.name;
                        }
                    }
                    else
                    {
                        if (x == xCorrect && y == yCorrect)
                        {
                            temp.name = correctNote;
                            noteName = temp.GetComponentInChildren<Text>();
                            noteName.text = temp.name;
                        }
                        else
                        {
                            temp.name = tempNote;
                            noteName = temp.GetComponentInChildren<Text>();
                            noteName.text = temp.name;
                        }
                    }
                }
            }
        }

        public IEnumerator StartCountdown(float countdownValue)
        {
            float currCountdownValue = countdownValue;
            while (currCountdownValue >= 0)
            {
                timeLeft.text = currCountdownValue.ToString();
                yield return new WaitForSeconds(1.0f);
                currCountdownValue--;
            }
            popper.Play();
            isPopped = true;
            FairGameManager.instance.outOfTime();
        }

        string wrongPitch() //this one chooses wrong numbers for the pitches. Look at the second (or third) char
        {
            string tempCorrect = stripNumber(correctNote);
            string temp = notes[Random.Range(0,notes.Length-1)]; //picks a note at random
            if(TotalGameManager.instance.fairRank == 1)
            {
                while(temp == tempCorrect) //makes sure there's only one correct note
                {
                    temp = notes[Random.Range(0, notes.Length - 1)];
                }
            }
            else//pick a random note and number
            {
                temp = notes[Random.Range(0, notes.Length - 1)] + Random.Range(1, 5).ToString(); //this creates a pitch with a number between 1 and 5
                while(temp == correctNote)
                {
                    temp = notes[Random.Range(0, notes.Length - 1)] + Random.Range(1, 5).ToString();
                }  
            }
            return temp;
        }

        string stripNumber(string incomingNote)
        {
            //this one takes the number off the end for the easy round.
            string newNote;
            char[] characters = incomingNote.ToCharArray();
            if(characters.Length==2) //then it must be not # or b
            {
                newNote = characters[0].ToString();
            }
            else
            {
                newNote = characters[0].ToString() + characters[1].ToString();
            }
            return newNote;
        }

        void balloonPopper(string incNote, Button incButton) //this one pops the balloon and puts in the right sprite
        {
            if (!isPopped)
            {
                StopCoroutine(co);
                popper.Play();
                incButton.GetComponentInChildren<Text>().gameObject.SetActive(false);
                if (TotalGameManager.instance.fairRank == 1)
                {
                    if (incNote == stripNumber(correctNote))
                    {
                        incButton.image.sprite = balloonStates[1];
                        FairGameManager.instance.levelUp();
                    }
                    else
                    {
                        incButton.image.sprite = balloonStates[2];
                        FairGameManager.instance.levelDown(stripNumber(correctNote));
                    }
                }
                else 
                {
                    if (incNote == correctNote)
                    {
                        incButton.image.sprite = balloonStates[1];
                        FairGameManager.instance.levelUp();
                    }
                    else
                    {
                        incButton.image.sprite = balloonStates[2];
                        FairGameManager.instance.levelDown(correctNote);
                    }
                }
                isPopped = true;
            }
        }
    }
}
