using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCheat : MonoBehaviour
{
    private KartManager km;

    private void Start()
    {
        km = FindObjectOfType<KartManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        km.antiCheat = true;
    }
}
