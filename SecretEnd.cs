using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretEnd : MonoBehaviour
{
    private void OnMouseDown()
    {
        HubManager.instance.chooseLevel("EndGame");
    }
}
