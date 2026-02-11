using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeventhController : MonoBehaviour
{
    // Start is called before the first frame update
    bool popUp = true;
    bool inView = false;
    bool going = false;

    public float speed = 1f;
    public float height = .82f;
    public int direction = 1;
    private float factor;

    private Vector3 initialPos;


    private void Start()
    {
        initialPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (inView)
        {
            if(direction == 1)
            {
                factor = 2;
            }
            if(direction == -1)
            {
                factor = .3f;
            }
            if (!going)
            {
                StartCoroutine(WaitThree());
            }
            if (popUp)
            {
                float movementY = speed * Time.deltaTime * direction * factor;
                if (transform.position.y + movementY > initialPos.y + height)
                {
                    direction = -1;
                }
                transform.position += new Vector3(0, movementY, 0);
                if (transform.position.y <= initialPos.y)
                {
                    popUp = false;
                    going = false;
                    direction = 1;
                }
            }
        }

    }
    IEnumerator WaitThree() //2.5
    {
        going = true;
        yield return new WaitForSeconds(2.5f);
        popUp = true;
    }
    private void OnBecameVisible()
    {
        inView = true;
    }
    private void OnBecameInvisible()
    {
        inView = false;
    }
}
