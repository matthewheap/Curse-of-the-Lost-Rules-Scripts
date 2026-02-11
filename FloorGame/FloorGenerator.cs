using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FloorGame
{
    public class FloorGenerator : MonoBehaviour
    {
        
        [SerializeField] TextAsset scalesText;
        [SerializeField] TextAsset notesText;

        public string[,] scales; //contains all the notes of the scales as individual strings "C" "D" "E"
        public string[] notes; //a list of possible note names
        public Tile[,] tileArray = new Tile[7,7];
        public int scaleChoice;

        public string myScale; //the scale randomly chosen.
        public string[] wrongNotes;

        private Vector3 floorAxis = new Vector3(1, 0, -3); //the left hand corner of the floor

        public TextMesh noteName;
        public Transform floorTile;

        int index; //for picking a scale

        // Use this for initialization
        void Awake()
        {
            print("Floor Generator: " + TotalGameManager.instance.floorRank);
            scales = GetComponent<ScaleGenerator>().makeScale(scalesText);
            index = (Random.Range(0, scales.GetLength(0)- 1)); //gives the number of the scale
            notes = GetComponent<ScaleGenerator>().makeNote(notesText);
            wrongNotes = new string[2];
            if (TotalGameManager.instance.floorRank == 2)
            {
                scales = GetComponent<ScaleGenerator>().scaleSerializer(scales, 5);
            }
            else if (TotalGameManager.instance.floorRank == 3)
            {
                scaleChoice = Random.Range(1, 5); //this gives me the dorian, phrygian, lydian, and mixolydian modes.
                scales = GetComponent<ScaleGenerator>().scaleSerializer(scales, scaleChoice); //gets whatever the mode is
            }
            else if (TotalGameManager.instance.floorRank == 5)
            {
                myScale = pickScale();
                scales = GetComponent<ScaleGenerator>().MakeSecDom(scales);
            }
            else if (TotalGameManager.instance.floorRank == 6)
            {
                myScale = pickScale();
                scales = GetComponent<ScaleGenerator>().MakeSecLT(scales);
            }
            if (!TotalGameManager.instance.levelTwo) //this didn't work for level 2 because of the way the program calculates secondary dominants
            {
                myScale = pickScale();
            }
            instantiateFloor();
        }


        string pickScale()
        {
            string scaleChoice = scales[index,0];
            return scaleChoice;
        }

        void instantiateFloor()
        {
            int horizontalSlots = 7;
            int verticalSlots = 7;
            int randomPlace = Random.Range(0,horizontalSlots);
            int[] wrongRandom = new int[2];
            for(int y = 0; y<verticalSlots; y++) //nested for loops generate the floor. Each note after the first line is random.
            {
                randomPlace = Random.Range(0, horizontalSlots);
                wrongNotes = FindWrongNotes(scales[index, y]);
                wrongRandom = RandomExcept(0, horizontalSlots, randomPlace);
                for (int x = 0; x<horizontalSlots; x++)
                {
                    if (y == 0 && !TotalGameManager.instance.levelTwo)
                    {
                        noteName.text = myScale;
                        var temp = Instantiate(floorTile, floorAxis + new Vector3(x, 0, y), Quaternion.identity);
                        temp.name = "" + x + y;
                        tileArray[x, y] = new Tile(noteName.text, true);
                        //tileArray[x,y].NoteName = noteName.text;
                        //tileArray[x, y].Correct = true;
                                           
                    }
                    else
                    {

                        if (x == randomPlace)
                        {
                            noteName.text = scales[index,y];
                            var temp = Instantiate(floorTile, floorAxis + new Vector3(x, 0, y), Quaternion.identity);
                            temp.name = "" + x + y;
                            tileArray[x, y] = new Tile(noteName.text, true);
                        }
                        else if (x == wrongRandom[0]) //these two generate close but wrong answers
                        {
                            noteName.text = wrongNotes[0];
                            tileArray[x, y] = new Tile(noteName.text, false);
                            var temp = Instantiate(floorTile, floorAxis + new Vector3(x, 0, y), Quaternion.identity);
                            temp.name = "" + x + y;
                        }
                        else if(x == wrongRandom[1])
                        {
                            noteName.text = wrongNotes[1];
                            tileArray[x, y] = new Tile(noteName.text, false);
                            var temp = Instantiate(floorTile, floorAxis + new Vector3(x, 0, y), Quaternion.identity);
                            temp.name = "" + x + y;
                        }
                        else
                        {
                            noteName.text = notes[Random.Range(0, notes.Length)];
                            var temp = Instantiate(floorTile, floorAxis + new Vector3(x, 0, y), Quaternion.identity);
                            temp.name = "" + x + y;

                            if (noteName.text == scales[index,y] && !TotalGameManager.instance.levelTwo)
                            {
                                tileArray[x, y] = new Tile(noteName.text, true);

                            }
                            else if (TotalGameManager.instance.levelTwo) //have to do this because it's not sequential. 
                            {
                                bool correctNote = false;
                                for (int z = 0; z<7; z++)
                                {
                                    if(noteName.text == scales[index,z])
                                    {
                                        correctNote = true;
                                    }
                                }
                                if(correctNote)
                                {
                                    tileArray[x, y] = new Tile(noteName.text, true);
                                }
                                else
                                {
                                    tileArray[x, y] = new Tile(noteName.text, false);
                                }
                            }
                            else
                            {
                                tileArray[x, y] = new Tile(noteName.text, false);
                            }
                        }
                    }  
                }
            }
            noteName.text = scales[index,0];   
            var exitTile = Instantiate(floorTile, new Vector3(4, 0, 4), Quaternion.identity); //this part adds the exit note
            exitTile.name = "Exit";
        }
        public int[] RandomExcept(int fromNr, int exclusiveToNr, int exceptNr) //picks a random number that isn't the original one
        {
            int[] randomNr = new int[2];
            randomNr[0] = exceptNr;
            randomNr[1] = exceptNr;

            while (randomNr[0] == exceptNr)
            {
                randomNr[0] = Random.Range(fromNr, exclusiveToNr);
            }
            while (randomNr[1] == exceptNr || randomNr[1] == randomNr[0])
            {
               randomNr[1] = Random.Range(fromNr, exclusiveToNr);
            }
            //print("Right: " + exceptNr + ", Wrong1: " + randomNr[0] + ", Wrong2: " + randomNr[1]); //for debugging
            return randomNr;
        }
        public string[] FindWrongNotes(string rightNote)
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
            else if (characters[1] == '#')
            {
                wrongN[0] = characters[0].ToString();
                wrongN[1] = characters[0] + "b";
            }
            else if (characters[1] == 'b' && characters.Length ==2)//must be a flat
            {
                wrongN[0] = characters[0].ToString();
                wrongN[1] = characters[0] + "#";
            }
            else // double flat or double sharp
            {
                wrongN[0] = characters[0] + "#";
                wrongN[1] = characters[0] + "b";
            }
           // print("Wrong note 1: " + wrongN[0] + ", Wrong note 2: " + wrongN[1]); //For debugging
            return wrongN;
        }
    }
}
