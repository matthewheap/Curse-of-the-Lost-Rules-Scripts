using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScaleGenerator : MonoBehaviour
{

    // Update is called once per frame
    public string[] makeNote(TextAsset notesIn)//this function makes a note
    {
        string[] notes = notesIn.text.Split(' ');
        return notes;
    }

    public string[,] makeScale(TextAsset scaleIn) //this function makes a scale
    {
        string[] lines = scaleIn.text.Split('\n');
        string[][] tempScale = new string[lines.Length][];
        string[,] newScale = new string[lines.Length, 7];
        int verticalLine = 0;
        foreach (string line in lines)
        {
            tempScale[verticalLine++] = line.Split(' ');
        }
        for (int x = 0; x < tempScale.Length; x++)
        {
            for (int y = 0; y < 7; y++) //fix this eventually to allow any scale length
            {
                //if (tempScale[x][y].Contains("\r"))
                //{
                //   tempScale[x][y].
                //}
                newScale[x, y] = tempScale[x][y];
            }
        }
        return newScale;
    }
    public string[,] MakeSecDom(string[,] scale) //makes V7/V two times in each string
    {
        string[,] secDom = new string[scale.GetLength(0), 7];
        for(int x = 0; x<scale.GetLength(0); x++)
        {
            secDom[x, 0] = scale[x, 1]; //later you can change this to include y+[something]%7 to make it different versions
            secDom[x, 1] = ChangeNote(scale[x, 3], 1);
            secDom[x, 2] = scale[x, 5];
            secDom[x, 3] = scale[x, 0];
            secDom[x, 4] = secDom[x, Random.Range(0, 3)];
            secDom[x, 5] = secDom[x, Random.Range(0, 3)];
            secDom[x, 6] = secDom[x, Random.Range(0, 3)];
        }
        return secDom;
    }
    public string[,] MakeAug6(string[,] scale, int type) //type is 0 = german, 1 = french, 2 = italian
    {
        string[,] itAug6 = new string[scale.GetLength(0), 3];
        string[,] aug6 = new string[scale.GetLength(0), 4];

        if(type == 2)
        {
            for(int x = 0; x<scale.GetLength(0); x++)
            {
                itAug6[x, 0] = ChangeNote(scale[x, 5], 0); //make it flat 6
                itAug6[x, 1] = scale[x, 0];
                itAug6[x, 2] = ChangeNote(scale[x, 3], 1);
            }
            return itAug6;
        }
        else
        {
            for (int x = 0; x < scale.GetLength(0); x++)
            {
                aug6[x, 0] = ChangeNote(scale[x, 5], 0); //make it flat 6
                aug6[x, 1] = scale[x, 0];
                if(type == 0)
                {
                    aug6[x, 2] = ChangeNote(scale[x, 2], 0);
                }
                else
                {
                    aug6[x, 2] = scale[x, 1];
                }
                aug6[x, 3] = ChangeNote(scale[x, 3], 1);
            }
        }
        return aug6;
        
    }
    public string[,] MakeSecLT(string[,] scale)
    {
        string[,] secLT = new string[scale.GetLength(0), 7];
        for (int x = 0; x < scale.GetLength(0); x++)
        {
            print(x);
            secLT[x, 0] = ChangeNote(scale[x, 3], 1); //later you can change this to include y+[something]%7 to make it different versions
            print(secLT[x, 0]);
            secLT[x, 1] = scale[x, 5];
            print(secLT[x, 1]);
            secLT[x, 2] = scale[x, 0];
            print(secLT[x, 2]);
            secLT[x, 3] = ChangeNote(scale[x, 2], 0);
            print(secLT[x, 3]);
            secLT[x, 4] = secLT[x, Random.Range(0, 3)];
            secLT[x, 5] = secLT[x, Random.Range(0, 3)];
            secLT[x, 6] = secLT[x, Random.Range(0, 3)];
        }
        return secLT;
    }

    public string[,] scaleSerializer(string[,] scale, int choice)
    {
        string[,] tempScale = new string[scale.GetLength(0), 7];
        for (int x = 0; x < scale.GetLength(0); x++)
        {
            for (int y = 0; y < 7; y++)
            {
                tempScale[x, y] = scale[x, ((y + choice) % 7)];
            }
        }
        return tempScale;
    }

    public string[,] TriadMaker(TextAsset triadsIn)
    {
        string[,] temp = new string[3, 14];
        string[] notes = triadsIn.text.Split(' ');
        int index = 0;
        for (int y = 0; y < 14; y++)
        {
            for(int x = 0; x<3; x++)
            {
                temp[x, y] = notes[index];
                index++;
            }
        }
        return temp;
    }
    public string[,] GetTriadFromScale(string[,] scales) //makes iio, iv, bVI, N6
    {
        string[,] temp = new string[3, scales.GetLength(0) * 4];
        print(scales.GetLength(0));
        string flatSix;
        for (int x = 0; x < scales.GetLength(0) * 4; x++)
        {
            flatSix = ChangeNote(scales[x / 4, 5], 0);
            temp[0, x] = scales[x / 4, 1]; //trying to get them into the order where the triads are triad[x,y] where x is <3 and y is the length of all the keys (*4)
            temp[1, x] = scales[x / 4, 3];
            temp[2, x] = flatSix;
            temp[0, x + 1] = scales[x / 4, 3];
            temp[1, x + 1] = flatSix;
            temp[2, x + 1] = scales[x / 4, 0];
            temp[0, x + 2] = flatSix;
            temp[1, x + 2] = scales[x / 4, 0];
            temp[2, x + 2] = ChangeNote(scales[x / 4, 2], 0);
            temp[0, x + 3] = ChangeNote(scales[x / 4, 1], 0);
            temp[1, x + 3] = scales[x / 4, 3];
            temp[2, x + 3] = flatSix;
            x += 3;
        }
        return temp;
    }
    public string[,] TriadChanger(string[,] triad, string type) //creates minor, augmented, and diminished versions of a basic triad
    {
        string[,] changedTriad = new string[3, 14];
        for (int y = 0; y < 14; y++) //when you make them equal, apparently they're linked?? So we have to do this
        {
            for (int x = 0; x < 3; x++)
            {
                changedTriad[x, y] = triad[x,y];
            }
        }
        string temp;
        if(type == "Minor")
        {
            for (int x = 0; x < 14; x++)
            {
                temp = ChangeNote(changedTriad[1, x], 0);
                changedTriad[1, x] = temp;//move the middle note down 1;
                
            } 
        }
        if(type == "Augmented") //move the last note up 1;
        {
            for (int x = 0; x < 14; x++)
            {
                temp = ChangeNote(changedTriad[2, x], 1);
                changedTriad[2, x] = temp; //print(changedTriad[0, x] + changedTriad[1, x] + changedTriad[2, x]);     
            }
        }
        if(type== "Diminished")
        {
            for (int x = 0; x < 14; x++)
            {
                //print(changedTriad[0, x] + changedTriad[1, x] + changedTriad[2, x]);
                temp = ChangeNote(changedTriad[1, x], 0);
                changedTriad[1, x] = temp;//move the middle note down 1;
                //print(changedTriad[2, x]);
                temp = ChangeNote(changedTriad[2, x], 0);
                changedTriad[2, x] = temp;//move the last note down 1;
                //print(changedTriad[0, x] + changedTriad[1, x] + changedTriad[2, x]); 
            }           
        }
        return changedTriad;
    }
    private string ChangeNote(string noteIn, int direction) //0 = down, 1 = up - Raises or lowers a note.
    {
        char[] characters = noteIn.ToCharArray();
        string newNote;
        if (direction == 0) //so if we want the note to be lowered
        {
            if (characters.Length == 1) //then it must be not # or b
            {
                newNote = characters[0].ToString() + "b";
                return newNote;
            }
            else if(characters.Length == 2 && characters[1] == '#') //if it's sharp
            {
                newNote = characters[0].ToString(); //only return the first character (i.e. the note name)
                return newNote;
            }
            else
            {
                newNote = noteIn + "b"; //make it double flat
                return newNote;
            }
        }
        else
        {
            if (characters.Length == 1) //then it must be not # or b
            {
                newNote = characters[0].ToString() + "#";
                return newNote;
            }
            else if (characters.Length == 2 && characters[1] == 'b') //if it's flat
            {
                newNote = characters[0].ToString(); //only return the first character (i.e. the note name)
                return newNote;
            }
            else
            {
                newNote = characters[0].ToString() + "x"; //make it double sharp
                return newNote;
            }
        }
    }
}
