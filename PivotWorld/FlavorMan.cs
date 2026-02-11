using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PivotWorld
{
    public class FlavorMan : MonoBehaviour
    {
        bool triggered = false;
        GameObject sceneMan;
        private void Start()
        {
            sceneMan = GameObject.Find("PivotManager");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            triggered = true;

            var temp = gameObject.name.ToCharArray();
            int lastNum = int.Parse(temp[temp.Length - 1].ToString()); //get just the number
            print("Triggered Flavor Text" + lastNum.ToString());
            sceneMan.GetComponent<PivotManager>().DisplayFlavor(lastNum);
            Destroy(gameObject);
        }
    }
}
