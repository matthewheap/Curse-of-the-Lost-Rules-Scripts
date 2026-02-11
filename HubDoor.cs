using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        string doorText = gameObject.GetComponent<TextMeshPro>().text;
        HubManager.instance.chooseLevel(doorText);
    }
}
