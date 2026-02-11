using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

namespace EndGame
{
    public class Player : MonoBehaviour
    {
        public Rigidbody2D player; //Notey
        public Collider2D playerCol;
        public Animator animator;
        public AudioSource playerAudio;
        public AudioClip jumpSound;
        public AudioClip deathSound;

        public GameObject thisTrigger;
        private PlatformerCharacter2D m_Character;
        private Vector3 position;

        bool jump = false;
        public bool triggered = false;
        public bool stopped = false; //check whether the player is at a trigger
        
        float horizontalMove = 0f;
        public float runSpeed = 40f;
        private float airHeight = 8;
        private float bigAirHeight = 50;
        float slower = .5f;
        public EndGameManager sceneMan;

        public int lives;
        private Vector3 originalNotey;




        // Start is called before the first frame update
        void Start()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            player = GetComponent<Rigidbody2D>();
            playerCol = GetComponent<Collider2D>();
            animator.SetFloat("Speed", 0);
            animator.SetBool("Dead", false);
            lives = sceneMan.difficulty + 2;
            sceneMan.UpdateUI();
            originalNotey = gameObject.transform.position;
            
        }

        private void Update()
        {
            #if UNITY_IOS || UNITY_ANDROID
                  
            #else
             horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //finds if we're trying to move and generates a velocity, kind of...sent to fixed update 
            #endif

            if (Input.GetButtonDown("Jump") && !stopped)
            {
                jump = CrossPlatformInputManager.GetButtonDown("Jump");
                playerAudio.clip = jumpSound;
                playerAudio.Play();
            }

            if (stopped)
            {
                player.velocity = new Vector3(0, 0, 0);
                horizontalMove = 0;
                jump = false;
                sceneMan.mobileCanvas.gameObject.SetActive(false);
            }

        }

        void FixedUpdate()
        {
            if (!stopped)
            {
                m_Character.Move(horizontalMove, false, jump);
                jump = false;
                animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            }
        }

        public void UpdateAxisHValue(int v)
        {
            if (!stopped)
            { horizontalMove += (v * runSpeed); }
        }

        public void UpdateAxisVValue(bool j)
        {
            if (!stopped)
            {
                jump = j;
                if (jump)
                {
                    playerAudio.clip = jumpSound;
                    playerAudio.Play();
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "enemy" && !triggered)
            {
                triggered = true;
                StartCoroutine(WaitTwo());
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "enemy" && !triggered)
            {
                triggered = true;
                StartCoroutine(WaitTwo());
            }
            else if(collision.gameObject.tag == "correct"  && !triggered && sceneMan.inQuestion)
            {
                bool correct = true;
                sceneMan.ResetQuestion(correct);
            }
            else if(collision.gameObject.tag == "wrong" && !triggered && sceneMan.inQuestion)
            {
                bool correct = false;
                sceneMan.ResetQuestion(correct);
            }


        }
        IEnumerator WaitTwo()
        {
            playerAudio.clip = deathSound;
            playerAudio.Play();
            animator.SetBool("Dead", true);
            stopped = true;
            yield return new WaitForSeconds(2);
            stopped = false;
            lives--;
            if(lives >0)
            {
                transform.position = originalNotey;
                horizontalMove = 0;
                animator.SetBool("Dead", false);
                sceneMan.UpdateUI();
            }
            else
            {
                sceneMan.Lose();
            }
            triggered = false;
        }
    }
}
