using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{

    Vector3 initialPos;
    Vector3 target;
    bool inView = false;
    public float speed = 3;
    public int distanceUp = -4;
    private int direction = 1;


    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        target = transform.position + new Vector3(0, distanceUp * direction, 0);

    }

    // Update is called once per frame
    void Update()
    {

        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) < 0.0001f)
        {
            direction *= -1;
            target = initialPos + new Vector3(0, distanceUp * direction, 0);
        }

    } 
}
