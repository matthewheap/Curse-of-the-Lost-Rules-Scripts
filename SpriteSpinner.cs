using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpinner : MonoBehaviour
{
    float rotation = 0;
    float speed = 100;

    void FixedUpdate()
    {
        float angleRot = speed * Time.deltaTime;
        transform.Rotate(Vector3.up * angleRot, Space.World);
    }
}
