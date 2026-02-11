using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloorGame
{
    public class Player : MonoBehaviour
    {
        Tile[,] safeFloor;
        int row;
        Animator animator;
        GameObject[] deathFloor;
        public Oscillator osc;
        float freq;
        float oldFreq;
        string myScale;
   
        // Use this for initialization
        void Start()
        {
            safeFloor = GameObject.Find("SceneManager").GetComponent<FloorGenerator>().tileArray;
            myScale = GameObject.Find("SceneManager").GetComponent<FloorGenerator>().myScale;
            row = 0;
            oldFreq = 0;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit))
                {
                    char[] characters = hit.transform.name.ToCharArray();
                    int x = (int)char.GetNumericValue(characters[0]);
                    int y = (int)char.GetNumericValue(characters[1]);
                    if(row==y)
                    {
                        animator.Play("Jump");
                        freq = osc.transformCharacterToPitch(safeFloor[x, y].NoteName); //takes the notename and turns it into 0-11, returns the frequency
                        freq = osc.frequencyChecker(oldFreq, freq); //finds out whether the old frequency is lower than the new one
                        StartCoroutine(osc.playNote(freq, .5f));
                        oldFreq = freq;
                        transform.position = new Vector3(x + 1, .21f, row - 3);
                        checkSafety(x, hit.transform.name);
                        row++;
                    }
                    if(row==7 && hit.transform.name == "Exit")
                    {
                        transform.position = new Vector3(4, .21f, 4);
                        freq = osc.transformCharacterToPitch(myScale);
                        freq = osc.frequencyChecker(oldFreq, freq);
                        StartCoroutine(osc.playNote(freq, .5f));
                        oldFreq = freq;
                        StartCoroutine(WaitForIt());
                    }
                }
            }
            if(this.transform.position.y < -8)
            {
                FloorGameManager.instance.resetGame(false);
            }
        }
        IEnumerator WaitForIt()
        {
            yield return new WaitForSeconds(2);
            FloorGameManager.instance.levelUp();
        }
        void checkSafety(int space, string name)
        {
            if (safeFloor[space,row].Correct == true)
            {
                return;
            }
            else
            {
                //deathFloor = GameObject.FindGameObjectsWithTag("Floor");
                //foreach(GameObject floor in deathFloor)
                // {
                //  floor.AddComponent<Rigidbody>();
                //}
                GameObject fallingTile = GameObject.Find(name);
                StartCoroutine(osc.dropNote(freq));
                fallingTile.AddComponent<Rigidbody>();
                //Destroy(GameObject.Find(name));
         
            }

        }

    }
}
