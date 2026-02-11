using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriadGame
{
    public class TriadClear : MonoBehaviour
    {
        bool triggered = false;
        GameObject sceneMan;
        private void Start()
        {
            sceneMan = GameObject.Find("SceneMan");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && !triggered)
            {
                triggered = true;
                sceneMan.GetComponent<TriadSceneMan>().DestroyBridge();
                Destroy(gameObject);
            }

        }
    }
}
