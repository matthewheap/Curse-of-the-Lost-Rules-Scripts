using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SecondarySceneMan : MonoBehaviour
{
    Sprite[] secondaryQs;
    private int index;
    public GameObject intervalHolder;
    public Text FailText;
    public GameObject Panel;


    // Start is called before the first frame update
    void Start()
    {
        secondaryQs = SecondaryMaker.instance.secCards;
        Panel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (SecondaryMaker.instance.failures == 3)
        {
            ShowFail();
        }
    }

    public void MakeInterval()
    {
        index = Random.Range(0, secondaryQs.Length - 1);
        intervalHolder.GetComponent<SpriteRenderer>().sprite = secondaryQs[index];
        SecondaryMaker.instance.intervalName = secondaryQs[index].name;
    }

    public void ResetScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("SecondBirds" + TotalGameManager.instance.secLevel);
    }

    public void QuitScene()
    {
        ES3.Save<int>("secLevel", TotalGameManager.instance.secLevel);
        //Destroy(SecondaryMaker.instance.audioSource);
        Destroy(GameObject.FindGameObjectWithTag("toDelete"));
        SceneManager.LoadScene("MainHub");
    }
    public void ShowFail()
    {
        SecondaryMaker.instance.failures = 0;
        Panel.gameObject.SetActive(true);
        FailText.text = "First we have to figure out what the type of chord is (i.e. dominant or diminished). If you're having trouble with this, remember that the intervals from the root for dominant are M3+m3+m3 and the intervals from the root for fully-diminished" +
            " are m3+m3+m3. Once we've worked that out we can see what the chord would be V or vii°7 of. For V7/something, you can count up a fourth to find the tonicized chord (so in G Major, if we have AC#EG, we count A-B-C-D to find that it's V7/D, and therefore" +
            " V7/V). If it's vii°7 of something, all we have to do is look up a half-step...if the root is F#, the tonicized chord's root will be G (so F#ACEb is vii°7/G in C Major...therefore vii°7/V). Take your time, and you'll be able to do it! Remember that the " +
            "key signature is always going to be the major key for this game. Go for it!!";
    }
    public void CloseFail()
    {
        Panel.gameObject.SetActive(false);
    }
}
