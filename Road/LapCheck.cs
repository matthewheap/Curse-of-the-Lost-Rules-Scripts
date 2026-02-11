using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCheck : MonoBehaviour
{
    private KartManager km;

    private void Start()
    {
        km = FindObjectOfType<KartManager>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (km.antiCheat)
        {
            km.laps++;
            ES3.Save<int>("RaceLaps", km.laps);
            ES3.Save<int>("RaceTimer", km.timer);
            if(km.laps<4)
            {
                km.lapText.text = "Lap " + km.laps;
                StartCoroutine(FlashLap());
            }
            km.antiCheat = false;
        }
    }
    IEnumerator FlashLap()
    {
        if (km.laps < 3)
        {
            km.lapFlash.text = "Lap " + km.laps;
        }
        else
        {
            km.lapFlash.text = "Final Lap!";
        }
        km.lapFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        km.lapFlash.gameObject.SetActive(false);
    }
}
