using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSpriteSpinner : MonoBehaviour
{
    float speed = 400;

    void FixedUpdate()
    {
        float angleRot = speed * Time.deltaTime;
        transform.Rotate(Vector3.forward * angleRot, Space.World);
    }
}
