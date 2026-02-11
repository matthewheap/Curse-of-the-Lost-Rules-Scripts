using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelController : MonoBehaviour
{
    bool inView = false;
    bool comingDown = false;
    bool goingBack = false;
    Vector3 initialPos;
    public float speed = 1;
    public float height = 1f;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (inView)
        {
            float movementX = speed * Time.deltaTime * direction;
            float movementY = speed * Time.deltaTime * direction;
            if (!comingDown && !goingBack)
            {
                if (transform.position.y <= initialPos.y + height)
                {
                    transform.position += new Vector3(movementX, movementY, 0);
                }
                else
                {
                    comingDown = true;
                    direction = -1;
                }
            }
            if (comingDown && !goingBack)
            {
                if (transform.position.y >= initialPos.y)
                {
                    transform.position += new Vector3(0, movementY, 0);
                }
                else
                {
                    comingDown = false;
                    goingBack = true;
                }
            }
            if (goingBack && !comingDown)
            {
                if (transform.position.x >= initialPos.x)
                {
                    transform.position += new Vector3(movementX, 0, 0);
                }
                else
                {
                    goingBack = false;
                    direction = 1;
                }
            }
            
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
}
