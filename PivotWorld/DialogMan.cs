using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PivotWorld {
    public class DialogMan : MonoBehaviour
    {
        public Canvas speechbubble;
        public Player player;
        public Text speechtext;
        public Canvas playerSpeech;
        public Text playerSpeechText;
        private int whoseTurn;
        private int counter;
        private int dialogNum;
        public Button skipButton;
        public Button nextButton;
        private string[] dialog0 = new string[15] { "Hello Notey. I have been waiting for you", "B..B..Bach...you're dead?", "What?! No...I'm force projecting. Well, I mean, I am dead, but not any more than usual.", "How can I get you back?", "I hope that wasn't" +
            " a pun, young note.", "No sir!", "Good! You need to go through the door that is the same number as the position of the pivot chord", "What?", "So if the pivot chord is the 5th chord, you go through the 5th door.", "Got it!", "Then you'll enter a magical place" +
            " where you can choose the keys and Roman Numerals.", "Mmmhmmm!", "You can also click on the score if you want to hear the music.", "Will do!", "Good luck, young Notey! You are my only hope!" };
        private string[] dialog1 = new string[13] {"Notey, you wonderful creature!", "Here I am, Bach.", "You have reached your goal", "Yay!", "Here before you is the Mallet of Applied Dominance!", "...really? That's what we went with?", "Don't blame me...I'm dead, remember...", "" +
            "So why did I have to find all those pivots?", "Practice, young Notey...it's what will make you strong.", "Strong enough for what?", "Strong enough to fight the monster that's keeping me hostage!", "Yikes!", "Don't worry, I believe in you! Take the mallet!"};
        public List<string[]> dialogs = new List<string[]>();
        private bool triggered = false;

        // Start is called before the first frame update
        void Start()
        {
            dialogs.Add(dialog0);
            dialogs.Add(dialog1);
            whoseTurn = 0;
            counter = 0;
            playerSpeech.gameObject.SetActive(false);
            speechbubble.gameObject.SetActive(false);
        }


        public void NextButton()
        {
            if (gameObject == player.thisTrigger)
            {
                if (whoseTurn == 1)
                {
                    speechbubble.gameObject.SetActive(false);
                }
                else
                {
                    playerSpeech.gameObject.SetActive(false);
                }
                DisplayDialog(dialogNum);
            }
        
        }
        public void SkipButton()
        {
            counter = dialogs[dialogNum].Length;
            if (whoseTurn == 1)
            {
                speechbubble.gameObject.SetActive(false);
            }
            else
            {
                playerSpeech.gameObject.SetActive(false);
            }
            DisplayDialog(dialogNum);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!triggered)
            {
                triggered = true;
                skipButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
                player.thisTrigger = gameObject;
                player.stopped = true;
                var temp = gameObject.name.ToCharArray();
                dialogNum = int.Parse(temp[temp.Length - 1].ToString()); //get just the number
               // if (player.thisTrigger == gameObject)
               // {
                    DisplayDialog(dialogNum);
               // }
            }
        }
        private void DisplayDialog(int dialogNum)
        {
            string[] currentDialog = dialogs[dialogNum];
            if (counter <= currentDialog.Length - 1)
            {
                if (whoseTurn == 0)
                {
                    speechbubble.gameObject.SetActive(true);
                    speechtext.text = currentDialog[counter];
                    whoseTurn = 1;
                    counter++;
                }
                else
                {
                    playerSpeech.gameObject.SetActive(true);
                    playerSpeechText.text = currentDialog[counter];
                    counter++;
                    whoseTurn = 0;
                }
            }
            else
            {
                counter = 0;
                whoseTurn = 0;
                skipButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                player.stopped = false;
                gameObject.SetActive(false);
                triggered = false;
                
            }
        }
    }
}
