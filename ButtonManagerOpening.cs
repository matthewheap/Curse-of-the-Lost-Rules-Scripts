using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ButtonManagerOpening : MonoBehaviour
{
    public GameObject fullScreen;

    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        fullScreen.SetActive(false);
#else
        fullScreen.SetActive(true);
#endif
    }
    public void levelOne()
    {
        TotalGameManager.instance.levelTwo = false;
        TotalGameManager.instance.choseLevel = true;
        SceneManager.LoadScene("OpenVideo");

    }
    public void levelTwo()
    {
        TotalGameManager.instance.levelTwo = true;
        TotalGameManager.instance.choseLevel = true;
        SceneManager.LoadScene("OpenVideo");

    }
}
