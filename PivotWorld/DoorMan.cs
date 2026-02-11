using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PivotWorld
{
    public class DoorMan : MonoBehaviour
    {
        public GameObject player;
        private Player playerScript;
        private bool isSpinning = false;
        public GameObject doorExit;
        public float speed = 500;
        public Quaternion originalRotation;
        private QuestionMaker qm;



        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<Player>();
            originalRotation = player.transform.rotation;
            if(playerScript.thisTrigger != null)
            {
                qm = playerScript.thisTrigger.GetComponent<QuestionMaker>();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isSpinning)
            {
                float angleRot = speed * Time.deltaTime;
                player.transform.Rotate(new Vector3 (0,0,1) * angleRot, Space.World);
                if (player.transform.localScale.y >= 0)
                {
                    player.transform.localScale -= new Vector3(.01f, .01f, .01f);
                }
            }
        }
        public void SetExit(bool correct, int level)
        {
            if(correct)
            {
                doorExit = GameObject.Find("DoorwayCorrect" + level.ToString());
            }
            else
            {
                doorExit = GameObject.Find("DoorwayWrong" + level.ToString());
                
            }
        }

        private void OnMouseDown()
        {
            playerScript.stopped = true;
            player.transform.position = transform.position + new Vector3 (0,1.5f,-.5f);
            isSpinning = true;
            if (gameObject.name == "Doorway0")
            {
                //GameObject.Find("PivotManager").GetComponent<PivotManager>().checkpoints[1] = true;
                TotalGameManager.instance.pivCheckpoints[1] = true;
            }
            StartCoroutine(waitThree());
        }

        IEnumerator waitThree()
        {
            yield return new WaitForSeconds(2f);
            player.transform.position = doorExit.transform.position + new Vector3(0,0,-.5f);
            isSpinning = false;
            playerScript.stopped = false;
            player.transform.rotation = originalRotation;
            player.transform.localScale = new Vector3(1, 1, 1);
            if (tag == "rightDoor")
            {
                //QuestionMaker.OnCorrectDoor();
                qm.MakePivot();
            }
            else if (tag == "wrongDoor")
            {
                GameObject.Find("PivotManager").GetComponent<PivotManager>().failures++;
                // QuestionMaker.OnWrongDoor();
                qm.ResetProblem();
            }
            DestroyDoors();
        }
        private void DestroyDoors()
        {
            var temp = GameObject.FindGameObjectsWithTag("wrongDoor");
            var temp1 = GameObject.FindGameObjectsWithTag("rightDoor");
            for (int x = 0; x < temp.Length; x++)
            {
                Destroy(temp[x]);
            }
            for (int y = 0; y < temp1.Length; y++)
            {
                Destroy(temp1[y]);
            }
        }
    }
}
