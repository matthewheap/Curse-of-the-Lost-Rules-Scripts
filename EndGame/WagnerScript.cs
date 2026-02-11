using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndGame
{
    public class WagnerScript : MonoBehaviour
    {
        private bool phase2 = false;
        public int health = 80;
        public EndGameManager sceneMan;
        public Vector3 originalPos;
        public float firingSpeedPhase1;
        public float firingSpeedPhase2;
        public float movingSpeed = 3;
        public float rotationSpeed = 100;
        private float shootingTimer;
        public GameObject noteBullet;
        int direction = 1;
        public float phaseTimer;
        public float originalPhase = 20;
        public float shootingSpeed = 2f;
        public float originalFiringSpeed;
        public float originalFiringSpeedPhase2;
        private Quaternion originalRotation;
        public Sprite redEyeWagner;
        public Sprite regularWagner;

        // Start is called before the first frame update
        void Start()
        {
            
            firingSpeedPhase1 = sceneMan.difficulty; //set difficulty as 3 = easy; 1 = hard
            originalFiringSpeed = firingSpeedPhase1;
            firingSpeedPhase2 = sceneMan.difficulty *.75f;
            originalFiringSpeedPhase2 = firingSpeedPhase2;
            phaseTimer = originalPhase;
            originalRotation = transform.rotation;
            originalPos = transform.position;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            phaseTimer -= Time.deltaTime;

            if (phaseTimer <= 0)
            {
                if (phase2)
                {
                    phase2 = false;
                    transform.rotation = originalRotation;
                }
                else
                {
                    phase2 = true;
                }
                if (health >= 30)
                {
                    phaseTimer = originalPhase;
                }

            }
            if(health < 30)
            {
                phase2 = false;
                movingSpeed = 5;
                gameObject.GetComponent<SpriteRenderer>().sprite = redEyeWagner;
                transform.rotation = originalRotation;
            }
            if (!phase2 && health > 0)
            {
                firingSpeedPhase1 -= Time.deltaTime;
                if (firingSpeedPhase1 <= 0 && health >=30)
                {
                    WagnerShoots();
                }
                else if (firingSpeedPhase1 <= 0 && health < 30)
                {
                    WagnerShootsBig();
                }
                float movementX = movingSpeed * Time.deltaTime * direction;
                transform.position += new Vector3(movementX, 0, 0);
            }
            else if(phase2 && health >0)
            {
                firingSpeedPhase2 -= Time.deltaTime;
                if (firingSpeedPhase2 <= 0 && health >= 30)
                {
                    WagnerShoots();
                }
                else if (firingSpeedPhase2<=0 && health< 30)
                {
                    WagnerShootsBig();
                }
                float movementX = movingSpeed * Time.deltaTime * direction;
                transform.position += new Vector3(movementX, 0, 0);
                float angleRot = rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.forward * angleRot, Space.World);
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Wall")
            {
                //change direction
                direction *= -1;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
        private void WagnerShoots()
        {
            firingSpeedPhase1 = originalFiringSpeed;
            firingSpeedPhase2 = originalFiringSpeedPhase2;
            GameObject missileInstance = Instantiate(noteBullet);
            missileInstance.transform.SetParent(transform);
            missileInstance.transform.position = transform.position;
            missileInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shootingSpeed);
            Destroy(missileInstance, 4f);
        }
        public void WagnerShootsBig()
        {
            if(health < 30)
            {
                firingSpeedPhase2 = originalFiringSpeedPhase2;
                firingSpeedPhase1 = originalFiringSpeed;
            }
            for (int x = 0; x < 5; x++)
            {
                GameObject missileInstance = Instantiate(noteBullet);
                missileInstance.transform.SetParent(transform);
                missileInstance.transform.position = transform.position;
                missileInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shootingSpeed);
                missileInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(x * 100, 0));
                Destroy(missileInstance, 4f);
            }
        }
    }
}
