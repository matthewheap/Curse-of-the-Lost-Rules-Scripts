using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameChooser : MonoBehaviour
{
    public TextMeshPro box1;
    public TextMeshPro box2;
    public TextMeshPro box3;
    private KartManager km;
    
    
    // Start is called before the first frame update
    void Start()
    {
        km = FindObjectOfType<KartManager>();
        box1.text = null;
        box2.text = null;
        box3.text = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        int rightBox = Random.Range(0, 3);
        string[] choices = new string[3];
        if (km.level1)
        {
            if (km.choice == 0)
            {
                choices[0] = km.flats[km.indexPlace];
                int j = Random.Range(0, 5);
                while (j == km.indexPlace)
                {
                    j = Random.Range(0, 5);
                }
                choices[1] = km.flats[j];
                int p = Random.Range(0, 5);
                while (p == km.indexPlace || p == j)
                {
                    p = Random.Range(0, 5);
                }
                choices[2] = km.flats[p];
            }
            else
            {
                choices[0] = km.sharps[km.indexPlace];
                int j = Random.Range(0, 5);
                while (j == km.indexPlace)
                {
                    j = Random.Range(0, 5);
                }
                choices[1] = km.sharps[j];
                int p = Random.Range(0, 5);
                while (p == km.indexPlace || p == j)
                {
                    p = Random.Range(0, 5);
                }
                choices[2] = km.sharps[p];
            }
        }
        else //level 2
        {
            if (km.augType == 0)
            {
                choices[0] = km.ger6[km.choice, km.indexPlace];
                int j = Random.Range(0, km.noteNames.Length);
                string temp = km.noteNames[j];
                while (temp == km.ger6[km.choice, 0] || temp == km.ger6[km.choice, 1] || temp == km.ger6[km.choice, 2] || temp == km.ger6[km.choice, 3])
                {
                    j = Random.Range(0, km.noteNames.Length);
                    temp = km.noteNames[j];
                }
                choices[1] = temp;
                j = Random.Range(0, km.noteNames.Length);
                string temp1 = km.noteNames[j];
                while (temp1 == km.ger6[km.choice, 0] || temp1 == km.ger6[km.choice, 1] || temp1 == km.ger6[km.choice, 2] || temp1 == km.ger6[km.choice, 3] || temp == temp1)
                {
                    j = Random.Range(0, km.noteNames.Length);
                    temp1 = km.noteNames[j];
                }
                choices[2] = temp1;
            }
            else if(km.augType == 1)
            {
                choices[0] = km.fr6[km.choice, km.indexPlace];
                int j = Random.Range(0, km.noteNames.Length);
                string temp = km.noteNames[j];
                while (temp == km.fr6[km.choice, 0] || temp == km.fr6[km.choice, 1] || temp == km.fr6[km.choice, 2] || temp == km.fr6[km.choice, 3])
                {
                    j = Random.Range(0, km.noteNames.Length);
                    temp = km.noteNames[j];
                }
                choices[1] = temp;
                j = Random.Range(0, km.noteNames.Length);
                string temp1 = km.noteNames[j];
                while (temp1 == km.fr6[km.choice, 0] || temp1 == km.fr6[km.choice, 1] || temp1 == km.fr6[km.choice, 2] || temp1 == km.fr6[km.choice, 3] || temp == temp1)
                {
                    j = Random.Range(0, km.noteNames.Length);
                    temp1 = km.noteNames[j];
                }
                choices[2] = temp1;
            }
            else
            {
                choices[0] = km.it6[km.choice, km.indexPlace];
                int j = Random.Range(0, km.noteNames.Length);
                string temp = km.noteNames[j];
                while (temp == km.it6[km.choice, 0] || temp == km.it6[km.choice, 1] || temp == km.it6[km.choice, 2])
                {
                    j = Random.Range(0, km.noteNames.Length);
                    temp = km.noteNames[j];
                }
                choices[1] = temp;
                j = Random.Range(0, km.noteNames.Length);
                string temp1 = km.noteNames[j];
                while (temp1 == km.it6[km.choice, 0] || temp1 == km.it6[km.choice, 1] || temp1 == km.it6[km.choice, 2] || temp == temp1)
                {
                    j = Random.Range(0, km.noteNames.Length);
                    temp1 = km.noteNames[j];
                }
                choices[2] = temp1;
            }
        }
        if (rightBox == 0)
        {
            box1.text = choices[0];
            box2.text = choices[1];
            box3.text = choices[2];
        }
        else if (rightBox == 1)
        {
            box1.text = choices[1];
            box2.text = choices[0];
            box3.text = choices[2];
        }
        else
        {
            box1.text = choices[1];
            box2.text = choices[2];
            box3.text = choices[0];
        }
    }


}
