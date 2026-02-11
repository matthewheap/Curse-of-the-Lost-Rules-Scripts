using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PigCounter : MonoBehaviour
{
    public int enemiesLeft;
    bool ended = false;
    public Text popped;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        enemiesLeft = enemies.Length;
        popped.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesLeft <= 0 && !ended)
        {
            ended = true;
            popped.gameObject.SetActive(true);
            if (!TotalGameManager.instance.levelTwo)
            {
                IntervalManager.instance.NextScene();
            }
            else
            {
                SecondaryMaker.instance.NextScene();
            }
        }
    }
}
