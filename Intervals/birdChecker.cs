using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdChecker : MonoBehaviour
{
    Rigidbody2D rb;
    bool leftSlingshot = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < .5f && leftSlingshot)
        {
            Destroy(gameObject);
        }
        if(rb.velocity.sqrMagnitude > 10)
        {
            leftSlingshot = true;
        }
    }

}
