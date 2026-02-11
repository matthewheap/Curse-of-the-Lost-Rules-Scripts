using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

namespace TriadGame
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
        bool triggered = false;
        public bool stopped = false; //check whether the player is at a trigger
        private bool airborne = false;
        bool slowdown = false;
        public bool zoomed = false;
        public bool unzoomed = false;

        private bool bigAir = false;

        float horizontalMove = 0f;
        public float runSpeed = 40f;
        private float airHeight = 8;
        private float bigAirHeight = 50;
        float slower = .5f;
        private GameObject sceneMan;
        public TriadManager triadMan;




        // Start is called before the first frame update
        void Start()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            player = GetComponent<Rigidbody2D>();
            playerCol = GetComponent<Collider2D>();
            animator.SetFloat("Speed", 0);
            animator.SetBool("Dead", false);
            sceneMan = GameObject.Find("SceneMan");
        }

        private void Update()
        {
            if (!stopped)
            {

                #if UNITY_IOS || UNITY_ANDROID
                      
                #else
                    horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //finds if we're trying to move and generates a velocity, kind of...sent to fixed update 
                    
                #endif
            }
            if (transform.position.x >= 549 &&!zoomed)
            {
                zoomed = true;
                sceneMan.GetComponent<TriadSceneMan>().ZoomOut(0);
            }
            if(transform.position.x >= 555 && !unzoomed)
            {
                unzoomed = true;
                sceneMan.GetComponent<TriadSceneMan>().ZoomOut(1);
            }
            if (Input.GetButtonDown("Jump") && !stopped) 
            {
                jump = CrossPlatformInputManager.GetButtonDown("Jump");
                playerAudio.clip = jumpSound;
                playerAudio.Play();
                //animator.SetBool("IsJumping", true);
            }

            if (player.transform.position.y < -10)
            {
                playerAudio.clip = deathSound;
                playerAudio.Play();
                sceneMan.GetComponent<TriadSceneMan>().RestartGame();
            }

            if (stopped) 
            {
                player.velocity = new Vector3(0, 0, 0);
                horizontalMove = 0;
                triadMan.mobileCanvas.gameObject.SetActive(false);
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
            if (airborne)
            {
                if (transform.position.y >= position.y + airHeight-2.1f)
                {
                    airborne = false;
                    slowdown = true;   
                }
                transform.position += new Vector3(0, .5f, 0);
            }
            if (bigAir)
            {
                if (transform.position.y >= position.y + bigAirHeight - 4f)
                {
                    bigAir = false;
                    slowdown = true;
                }
                transform.position += new Vector3(0, 1.5f, 0);
            }
            if (slowdown)
            {

                if (transform.position.y >= position.y + airHeight)
                {
                    slowdown = false;
                    triggered = false;
                    slower = .5f;
                }
                else if (slower >= .02f)
                {
                    slower -= .02f;
                }
                else if (slower <= .01f)
                {
                    slowdown = false;
                    triggered = false;
                    slower = .5f;
                }
                else
                {
                    slower = .5f;
                }
                transform.position += new Vector3(0, slower, 0);
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
            if(collision.tag == "Goal")
            {
                sceneMan.GetComponent<TriadSceneMan>().WinGame();
                stopped = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "enemy" && !triggered)
            {
                triggered = true;
                if (collision.gameObject.name == "Incomplete Note" && transform.position.y >= collision.transform.position.y)
                {
                    position = transform.position;
                    airborne = true;
                }
                else if (collision.gameObject.name == "BigWhirl")
                {
                    position = transform.position;
                    bigAir = true;
                }
                else
                {
                    StartCoroutine(WaitTwo());
                }
                
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
            sceneMan.GetComponent<TriadSceneMan>().RestartGame();

#if UNITY_IOS || UNITY_ANDROID
            triadMan.mobileCanvas.gameObject.SetActive(true);
#endif
            horizontalMove = 0;
            triggered = false;
        }
    }
}
