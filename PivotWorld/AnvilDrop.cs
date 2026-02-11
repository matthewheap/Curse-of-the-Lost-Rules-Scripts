using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilDrop : MonoBehaviour
{
    Vector3 originalPos;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < originalPos.y -20)
        {
            transform.position = originalPos;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        }
    }
}
