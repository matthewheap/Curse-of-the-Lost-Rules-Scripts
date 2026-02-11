using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{
    private GameObject pigHandler;

    private void Start()
    {
        pigHandler = GameObject.Find("slingshot");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemy")
        {
            pigHandler.GetComponent<PigCounter>().enemiesLeft--;
        }
        Destroy(collision.gameObject);
    }
}
