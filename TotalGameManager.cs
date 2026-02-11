using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TotalGameManager : MonoBehaviour
{
    public static TotalGameManager instance;
    public int difficulty = 1;
    public int floorRank = 1;
    public int floorLevel = 1;
    public int intervalLevel = 0;
    public int secLevel = 0;
    public int fairRank = 1;
    public int fairLevel = 1;
    public int letterRank = 0;
    public bool levelTwo = false;
    public bool choseLevel = false;
    public bool[] checkpoints = new bool[13];
    public bool[] pivCheckpoints = new bool[7];
    public bool finishedTriadLvl1 = false;
    public bool finishedTriadLvl2 = false;
    public bool finishedPivot = false;
    public bool finishedKS = false;
    public bool finishedA6 = false;
    public bool finishedLW1 = false;
    public bool finishedLW2 = false;
    public int endGameDifficulty = 3;
    public bool letterPlayingLvl1 = false;


    // Start is called before the first frame update
    void Awake()
    {
        //check that it exists
        if (instance == null)
        {
            //assign it to the current object
            instance = this;
        }
        //make sure it's the current object
        else if (instance != this)
        {
            //destroy the current game object - we only need 1 and we already have it
            Destroy(gameObject);
        }
        //don't destroy this object when changing scenes
        DontDestroyOnLoad(gameObject);
        if(ES3.KeyExists("floorRank")) 
        {
            floorRank = ES3.Load<int>("floorRank");
        }
        else
        {
            floorRank = 1;
        }
        if (ES3.KeyExists("floorLevel"))
        {
            floorLevel = ES3.Load<int>("floorLevel");
        }
        else
        {
            floorLevel = 1;
        }
        if (ES3.KeyExists("letterRank"))
        {
            letterRank = ES3.Load<int>("letterRank");
        }
        else
        {
            letterRank = 0;
        }
        if(ES3.KeyExists("letterPlayingLvl1"))
        {
            letterPlayingLvl1 = ES3.Load<bool>("letterPlayingLvl1");
        }
        if (ES3.KeyExists("fairRank"))
        {
            fairRank = ES3.Load<int>("fairRank");
        }
        else
        {
            fairRank = 1;
        }
        if (ES3.KeyExists("fairLevel"))
        {
            fairLevel = ES3.Load<int>("fairLevel");
        }
        else
        {
            fairLevel = 1;
        }
        if(ES3.KeyExists("finishedLW1"))
        {
            finishedLW1 = ES3.Load<bool>("finishedLW1");
        }
        else
        {
            finishedLW1 = false;
        }
        if (ES3.KeyExists("finishedLW2"))
        {
            finishedLW2 = ES3.Load<bool>("finishedLW2");
        }
        else
        {
            finishedLW2 = false;
        }
        if (ES3.KeyExists("intervalLevel"))
        {
            intervalLevel = ES3.Load<int>("intervalLevel");
        }
        else
        {
            intervalLevel = 0;
        }
        if (ES3.KeyExists("secLevel"))
        {
            secLevel = ES3.Load<int>("secLevel");
        }
        else
        {
            secLevel = 0;
        }
        for (int x = 0; x < checkpoints.Length; x++) //sets the checkpoint bool
        {
            checkpoints[x] = false;
        }
            checkpoints[0] = true;
        if (ES3.KeyExists("finishedTriadLvl1"))
        {
            finishedTriadLvl1 = ES3.Load<bool>("finishedTriadLvl1");
        }
        else
        {
            finishedTriadLvl1 = false;
        }
        if (ES3.KeyExists("finishedTriadLvl2"))
        {
            finishedTriadLvl2 = ES3.Load<bool>("finishedTriadLvl2");
        }
        else
        {
            finishedTriadLvl2 = false;
        }

        if (ES3.KeyExists("finishedKS"))
        {
            finishedKS = ES3.Load<bool>("finishedKS");
        }
        else
        {
            finishedKS = false;
        }
        if(ES3.KeyExists("finishedA6"))
        {
            finishedA6 = ES3.Load<bool>("finishedA6");
        }
        else
        {
            finishedA6 = false;
        }
        if (ES3.KeyExists("finishedPivot"))
        {
            finishedPivot = ES3.Load<bool>("finishedPivot");
        }
        else
        {
            finishedPivot = false;
        }
        if (ES3.KeyExists("pivCheckpoints"))
        {
            pivCheckpoints = ES3.Load<bool[]>("pivCheckpoints");
        }
        else
        {
            for (int x = 0; x < pivCheckpoints.Length; x++)
            {
                pivCheckpoints[x] = false;
            }
            pivCheckpoints[0] = true;
            finishedPivot = false;
        }

    }
    public void ResetEverything()
    {
        for (int x = 0; x < pivCheckpoints.Length; x++) //pivots
        {
            pivCheckpoints[x] = false;
        }
        pivCheckpoints[0] = true;
        finishedPivot = false;
        ES3.Save<bool[]>("pivCheckpoints", pivCheckpoints);
        ES3.Save<bool>("finishedPivot", finishedPivot);

        for (int x = 0; x < checkpoints.Length; x++) // triads
        {
            checkpoints[x] = false;
        }
        checkpoints[0] = true;
        finishedTriadLvl1 = false;
        finishedTriadLvl2 = false;
        ES3.Save<bool[]>("checkpoints", checkpoints);
        ES3.Save<bool[]>("checkpoints2", checkpoints);
        ES3.Save<bool>("finishedTriadLvl1", finishedTriadLvl1);
        ES3.Save<bool>("finishedTriadLvl2", finishedTriadLvl2);
        finishedLW1 = false;
        finishedLW2 = false;
        ES3.Save<bool>("finishedLW1", false);
        ES3.Save<bool>("finishedLW2", false);
        ES3.Save<int>("RaceLaps", 1);
        ES3.Save<int>("RaceTimer", 65);

        intervalLevel = 0; //intervals
        ES3.Save<int>("intervalLevel", intervalLevel);

        secLevel = 0; //secondary angry bird intervals
        ES3.Save<int>("secLevel", secLevel);

        fairRank = 1; //notename fairground
        fairLevel = 1;
        ES3.Save<int>("fairRank", fairRank);
        ES3.Save<int>("fairLevel", fairLevel);

        floorLevel = 1; //floorgame
        floorRank = 1;
        ES3.Save<int>("floorRank", floorRank);
        ES3.Save<int>("floorLevel", floorLevel);

        letterRank = 0;
        ES3.Save<int>("letterRank", letterRank);

        choseLevel = false;

        SceneManager.LoadScene("TitleScreen");
    }

}
