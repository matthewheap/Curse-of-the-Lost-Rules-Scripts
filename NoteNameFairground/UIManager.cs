using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FairGame
{


    public class UIManager : MonoBehaviour
    {
        public Text levelText;
        public Text systemText;
        
        // Start is called before the first frame update
        void Start()
        {
            UpdateUI();
            systemText.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void UpdateUI()
        {
            levelText.text = "Rank: " + TotalGameManager.instance.fairRank + ", Level: " + TotalGameManager.instance.fairLevel;
        }
        public void Quit()
        {
            ES3.Save<int>("fairLevel", TotalGameManager.instance.fairLevel);
            ES3.Save<int>("fairRank", TotalGameManager.instance.fairRank);
            Destroy(FairGameManager.instance.audioSource);
            SceneManager.LoadScene("MainHub");
        }


    }
}
