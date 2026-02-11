using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManagerCredits : MonoBehaviour
{
    public GameObject assetsPanel;
    public Text assetText;

    private void Start()
    {
        assetsPanel.SetActive(false);
        assetText.text = "Assets Used";
    }

    // Start is called before the first frame update
    public void ResetGame()
    {
        TotalGameManager.instance.ResetEverything();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    public void ShowAssets()
    {
        if (assetText.text == "Assets Used")
        {
            assetsPanel.SetActive(true);
            assetText.text = "Close";
        }
        else
        {
            assetsPanel.SetActive(false);
            assetText.text = "Assets Used";
        }
        
    }
}
