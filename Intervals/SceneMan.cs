using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMan : MonoBehaviour
{
    Sprite[] intervalQs;
    private int index;
    public GameObject intervalHolder;
    public Text FailText;
    public GameObject Panel;


    // Start is called before the first frame update
    void Start()
    {
        intervalQs = IntervalManager.instance.intervalCards;
        Panel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(IntervalManager.instance.failures == 3)
        {
            ShowFail();
        }
    }

    public void MakeInterval()
    {
        index = Random.Range(0, intervalQs.Length - 1);
        intervalHolder.GetComponent<SpriteRenderer>().sprite = intervalQs[index];
        IntervalManager.instance.intervalName = intervalQs[index].name;
    }

    public void ResetScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("IntervalBirds" + TotalGameManager.instance.intervalLevel);
    }

    public void QuitScene()
    {
        ES3.Save<int>("intervalLevel", TotalGameManager.instance.intervalLevel);
        //Destroy(IntervalManager.instance.audioSource);
        Destroy(GameObject.FindGameObjectWithTag("toDelete"));
        SceneManager.LoadScene("MainHub");
    }
    public void ShowFail()
    {
        IntervalManager.instance.failures = 0;
        Panel.gameObject.SetActive(true);
        FailText.text = "Don't Worry...you can do it! Remember to count up from the bottom note to get the number (so C-E would be 3: C,D,E). The quality comes from the key signature of the bottom note. If the upper note belongs in the key of the bottom note (so, if E belongs" +
            " in the key of C, which it does!), then it's major (or perfect). If it doesn't belong, you've got to think what it should have been (so if it's C-Eb, we know the note should have been E, so Eb is one half-step lower). If it's a 2nd, 3rd, 6th, or 7th, and the note you have is" +
            " one half-step lower than the note it 'should be', then the quality is minor. If it's two half-steps lower, it's diminished. If it's one half-step higher, it's augmented. With 4ths, 5ths, and 8ths, if it belongs in the key, it's perfect. If it's one half-step lower," +
            " it's diminished, and if it's one half step higher, it's augmented. Keep trying!";
    }
    public void CloseFail()
    {
        Panel.gameObject.SetActive(false);
    }
}
