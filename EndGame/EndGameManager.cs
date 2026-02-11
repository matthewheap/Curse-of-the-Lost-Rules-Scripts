using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;
using UnityEngine.Analytics;
using TMPro;

namespace EndGame
{
    public class EndGameManager : MonoBehaviour
    {
        public int difficulty = 3;
        public Player player;
        public Text livesText;
        public GameObject chooseDifficulty;
        public WagnerScript wagner;
        public float questionCounter;
        public float originalQuestionCounter;
        public TextMeshPro answer1;
        public TextMeshPro answer2;
        public GameObject answerButtonL;
        public GameObject answerButtonR;
        public bool inQuestion = false;
        private bool weaponFired = false;
        public GameObject questionSpace;
        private Sprite[] questions;
        private Sprite[] questions2;
        public TextAsset rightAnswers1; //for level 1
        public TextAsset wrongAnswers1;
        public TextAsset rightAnswers2;
        public TextAsset wrongAnswers2;
        private string[] rightAnswerslvl1;
        private string[] wrongAnswerslvl1;
        private string[] rightAnswerslvl2;
        private string[] wrongAnswerslvl2;
        public GameObject mallet;
        public GameObject staff;
        public int weaponSpeed = 50;
        private GameObject weaponInstance;
        public GameObject bach;
        public GameObject bachCanvas;
        public Text bachSpeech;
        public GameObject playerCanvas;
        public Text playerText;
        private Vector3 endTarget;
        private bool noteyMoving = false;
        private bool wasStopped = false;
        public Button nextButton;
        public Button skipButton;
        public Button quitButton;
        public GameObject mobileCanvas;
        private int whoseTurn = 0;
        private int counter = 0;
        private string[] dialog = new string[17] { "Notey! You did it!", "Bach...you're still dead?", "I've been dead for hundreds of years.", "But...but who will return the rules?", "You already have, Notey...", "I have?", "Yes...by displaying your " +
            "theory skills.", "Huh??", "I'm a McGuffin, Notey!", "A McMuffin??", "No...you're the one keeping the rules alive!", "Me??", "All the power of voiceleading is with you and will be forever!", "Don't go, Bach. I like having you here.", "I'm " +
            "not really gone, Notey. I'm inside you.", "I guess this is goodbye.", "Yes, little note. It is." };
        
        
        private int whichAnswer; //going to determine which button is the right one.
        private int index;
        public GameObject mainCanvas;

        // Start is called before the first frame update
        void Awake()
        {
            //difficulty = totalgamemanager.instance.wagnerDifficulty;
            #if UNITY_IOS || UNITY_ANDROID
                  mobileCanvas.gameObject.SetActive(true);
            #else
                   mobileCanvas.gameObject.SetActive(false);
            #endif
            chooseDifficulty.gameObject.SetActive(false);
            questionCounter = 6;
            originalQuestionCounter = questionCounter;
            answer1.gameObject.SetActive(false);
            answer2.gameObject.SetActive(false);
            questionSpace.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            bach.gameObject.SetActive(false);
            bachCanvas.gameObject.SetActive(false);
            playerCanvas.gameObject.SetActive(false);
            difficulty = TotalGameManager.instance.difficulty;
            endTarget = new Vector3(-1.43f, -3.81f, 0); //where Notey has to go to get to last dialog
            //make the question arrays
            var sprites = Resources.LoadAll<Sprite>("Sprites/EndGame/Questions");
            questions = new Sprite[sprites.Length];
            for (int i = 0; i < questions.Length; i++)
            {
                questions[i] = sprites[i];
            }
            var sprites2 = Resources.LoadAll<Sprite>("Sprites/EndGame/Questions2");
            questions2 = new Sprite[sprites2.Length];
            for (int j = 0; j < questions2.Length; j++)
            {
                questions2[j] = sprites2[j];
            }
            rightAnswerslvl1 = rightAnswers1.text.Split('\n'); //returns 40 answers in order
            wrongAnswerslvl1 = wrongAnswers1.text.Split('\n');
            rightAnswerslvl2 = rightAnswers2.text.Split('\n'); //returns 40 answers in order
            wrongAnswerslvl2 = wrongAnswers2.text.Split('\n');
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            if (!inQuestion && wagner.health > 0)
            {
                questionCounter -= Time.deltaTime;
            }
            if(questionCounter<=0 && !inQuestion && wagner.health > 0)
            {
                MakeQuestion();
            }
            if(weaponFired)
            {
                float step = weaponSpeed * Time.deltaTime; // calculate distance to move
                weaponInstance.transform.position = Vector3.MoveTowards(weaponInstance.transform.position, wagner.transform.position, step);

                if (Vector3.Distance(weaponInstance.transform.position, wagner.transform.position) < 0.0001f)
                {
                    Destroy(weaponInstance);
                    weaponFired = false;
                    wagner.health -= 10;
                    UpdateUI();
                    if(wagner.health == 0)
                    {
                        winGame();
                    }
                }
            }
            if (player.stopped)
            {
                wasStopped = true;
                mobileCanvas.gameObject.SetActive(false);
            }
            else
            {
                if (wasStopped)
                {
                    wasStopped = false;

#if UNITY_IOS || UNITY_ANDROID
                    mobileCanvas.gameObject.SetActive(true);
#endif
                }
            }
            if (wagner.health == 0 && noteyMoving)
            {
                float step = weaponSpeed * Time.deltaTime; // calculate distance to move
                player.transform.position = Vector3.MoveTowards(player.transform.position, endTarget, step);
                if (Vector3.Distance(player.transform.position, endTarget) < 0.0001f)
                {
                    noteyMoving = false;
                    player.transform.rotation = Quaternion.identity;
                    if(!player.GetComponent<PlatformerCharacter2D>().m_FacingRight)
                    {
                        print("not facing right");
                        Vector3 theScale = player.transform.localScale;
                        theScale.x *= -1;
                        player.transform.localScale = theScale;
                        player.GetComponent<PlatformerCharacter2D>().m_FacingRight = true;
                    }
                    lastDialog();
                }
            }

        }
        public void UpdateUI()
        {
            livesText.text = "Lives: " + player.lives.ToString() + "\n" + "Wagner's Health: " + wagner.health.ToString();
        }
        public void lastDialog()
        {
            nextButton.gameObject.SetActive(true);
            bach.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            questionSpace.gameObject.SetActive(false);
            DisplayDialog();
        }
        private void DisplayDialog()
        {
            if (counter <= dialog.Length - 1)
            {
                if (whoseTurn == 0)
                {
                    bachCanvas.gameObject.SetActive(true);
                    bachSpeech.text = dialog[counter];
                    whoseTurn = 1;
                    counter++;
                }
                else
                {
                    playerCanvas.gameObject.SetActive(true);
                    playerText.text = dialog[counter];
                    counter++;
                    whoseTurn = 0;
                }
            }
            else
            {
                nextButton.gameObject.SetActive(false);
                skipButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);
                livesText.gameObject.SetActive(false);
                mainCanvas.GetComponent<Image>().CrossFadeAlpha(255, 6, false);
                StartCoroutine(WaitSix());
            }
        }
        public void winGame()
        {
            var exp = wagner.GetComponent<ParticleSystem>();
            wagner.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            exp.Play();
            Destroy(wagner.gameObject, 5f);
            noteyMoving = true;
            player.stopped = true;
            
        }
        public void Quitter()
        {
            SceneManager.LoadScene("MainHub");
        }
        public void NextButton()
        {

                if (whoseTurn == 1)
                {
                    bachCanvas.gameObject.SetActive(false);
                }
                else
                {
                    playerCanvas.gameObject.SetActive(false);
                }
                DisplayDialog();

        }
        public void SkipButton()
        {
            counter = dialog.Length;
            if (whoseTurn == 1)
            {
                bachCanvas.gameObject.SetActive(false);
            }
            else
            {
                playerCanvas.gameObject.SetActive(false);
            }
            DisplayDialog();
        }
        public void ResetQuestion(bool correct)
        {
            questionCounter = originalQuestionCounter;
            answer1.gameObject.SetActive(false);
            answer2.gameObject.SetActive(false);
            questionSpace.gameObject.SetActive(false);
            inQuestion = false;
            player.triggered = false;
            if(!correct)
            {
                wagner.WagnerShootsBig();
            }
            else
            {
                FireWeapon();
            }
        }

        private void FireWeapon()
        {
            int whichWeapon = Random.Range(0, 2);
            if (whichWeapon == 0)
            {
                weaponInstance = Instantiate(mallet, player.transform.position, Quaternion.identity);
            }
            else
            {
                weaponInstance = Instantiate(staff, player.transform.position, Quaternion.identity);
            }
            weaponFired = true;
        }
        public void Lose()
        {
            chooseDifficulty.gameObject.SetActive(true);
        }
        public void ResetGame()
        {
            //resets everything
            chooseDifficulty.gameObject.SetActive(false);
            answer1.gameObject.SetActive(false);
            answer2.gameObject.SetActive(false);
            questionSpace.gameObject.SetActive(false);
            inQuestion = false;
            player.lives = difficulty + 2;
            wagner.health = 80;
            questionCounter = originalQuestionCounter;
            UpdateUI();
            wagner.movingSpeed = 3;
            wagner.GetComponent<SpriteRenderer>().sprite = wagner.regularWagner;
            player.stopped = false;
            player.animator.SetBool("Dead", false);
            wagner.phaseTimer = wagner.originalPhase;
            wagner.firingSpeedPhase1 = difficulty; //set difficulty as 3 = easy; 1 = hard
            wagner.originalFiringSpeed = wagner.firingSpeedPhase1;
            wagner.firingSpeedPhase2 = difficulty * .75f;
            wagner.originalFiringSpeedPhase2 = wagner.firingSpeedPhase2;
        }
        public void Easy()
        {
            difficulty = 3;
            TotalGameManager.instance.difficulty = 3;
            ResetGame();
        }
        public void Medium()
        {
            difficulty = 2;
            TotalGameManager.instance.difficulty = 2;
            ResetGame();
        }
        public void Hard()
        {
            difficulty = 1;
            TotalGameManager.instance.difficulty = 1;
            ResetGame();
        }
        private void MakeQuestion()
        {
            inQuestion = true;
            whichAnswer = Random.Range(0, 2);
            if (!TotalGameManager.instance.levelTwo)
            {
                index = Random.Range(0, questions.Length);
                questionSpace.GetComponent<SpriteRenderer>().sprite = questions[index];
                if (whichAnswer == 0)
                {
                    answer1.text = rightAnswerslvl1[index];
                    answerButtonL.tag = "correct";
                    answer2.text = wrongAnswerslvl1[index];
                    answerButtonR.tag = "wrong";
                }
                else
                {
                    answer2.text = rightAnswerslvl1[index];
                    answerButtonR.tag = "correct";
                    answer1.text = wrongAnswerslvl1[index];
                    answerButtonL.tag = "wrong";
                }
            }
            else
            {
                index = Random.Range(0, questions2.Length);
                questionSpace.GetComponent<SpriteRenderer>().sprite = questions2[index];
                if (whichAnswer == 0)
                {
                    answer1.text = rightAnswerslvl2[index];
                    answerButtonL.tag = "correct";
                    answer2.text = wrongAnswerslvl2[index];
                    answerButtonR.tag = "wrong";
                }
                else
                {
                    answer2.text = rightAnswerslvl2[index];
                    answerButtonR.tag = "correct";
                    answer1.text = wrongAnswerslvl2[index];
                    answerButtonL.tag = "wrong";
                }
            }
            questionSpace.gameObject.SetActive(true);
            answer1.gameObject.SetActive(true);
            answer2.gameObject.SetActive(true);
        }
        IEnumerator WaitSix()
        {
            AnalyticsEvent.AchievementUnlocked("Beat Wagner");
            AnalyticsEvent.LevelComplete("Wagner");
            yield return new WaitForSeconds(6);
            SceneManager.LoadScene("EndCredits");
        }

    }
}
