using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTManager : MonoBehaviour
{

    public float speed = 1;
    bool inView = false;
    int direction = -1;
    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       if(inView)
        {
            float movementX = speed * Time.deltaTime * direction;
            transform.position += new Vector3(movementX, 0, 0);
        }
    }
    private void OnBecameVisible()
    {
        inView = true;
    }
    private void OnBecameInvisible()
    {
        inView = false;
        gameObject.transform.position = initialPos;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            //change direction
            direction *= -1;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
