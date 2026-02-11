using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTController : MonoBehaviour
{
    Vector3 initialPos;
    bool inView = false;
    public float speed = 3;
    public int direction = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (inView)
        {
            float factor = 1;
            if (direction == -1)
            {
                factor = 2.5f;
            }
            else
            {
                factor = 1;
            }
            float movementY = factor * speed * Time.deltaTime * direction;
            transform.position += new Vector3(0, movementY, 0); 
        }
    }
    private void OnWillRenderObject()
    {
        inView = true;
    }
    private void OnBecameInvisible()
    {
        inView = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
        {
            direction = 1;
        }
        else if (collision.gameObject.tag == "Ceiling")
        {
            direction = -1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor" || collision.tag == "Wall")
        {
            direction = 1;
        }
        else if (collision.tag == "Ceiling")
        {
            direction = -1;
        }
    }
}
