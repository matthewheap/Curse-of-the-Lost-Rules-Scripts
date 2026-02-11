using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriadGame
{
    public class FlavorTextManager : MonoBehaviour
    {
        
        bool triggered = false;
        GameObject sceneMan;
        private void Start()
        {
            sceneMan = GameObject.Find("SceneMan");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            triggered = true;
            
            var temp = gameObject.name.ToCharArray();
            int lastNum = int.Parse(temp[temp.Length - 1].ToString()); //get just the number
            print("Triggered Flavor Text" + lastNum.ToString());
            sceneMan.GetComponent<TriadSceneMan>().DisplayFlavor(lastNum);
            Destroy(gameObject);
        }

    }
}
