using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullandRelease : MonoBehaviour
{
    Vector2 startPos;
    public float force = 600;
    private Transform box;
    private Transform pointer;
    private bool isDragging = false;
    private Vector3 originalRot = new Vector3(0, 0, 180);
    private Vector3[] rotations = new Vector3[15] {new Vector3(0,0,149.26f), new Vector3(0,0,138.53f), new Vector3(0, 0, 127.4f), new Vector3(0, 0, 113.8f), new Vector3(0, 0, 99.4f), new Vector3(0, 0, 86.3f), new Vector3(0, 0, 72), new Vector3(0, 0, 60),
        new Vector3(0, 0, 50.4f),new Vector3(0,0,40.9f),new Vector3(0,0,30.2f),new Vector3(0,0,19.2f),new Vector3(0,0,11.4f), new Vector3(0,0,3.3f),new Vector3(0,0,-4.2f)};
    private Vector3[] boxPos = new Vector3[15] {new Vector3(-6.82f,1.11f,0), new Vector3(-7.1f,1.52f,0),new Vector3(-7.25f,1.89f,0), new Vector3(-7.34f,2.32f,0),new Vector3(-7.41f, 2.73f,0),new Vector3(-7.45f,3.14f,0),new Vector3(-7.45f,3.55f,0),new Vector3(-7.28f,3.96f,0),
    new Vector3(-7.17f,4.37f,0), new Vector3(-7.04f,4.82f,0),new Vector3(-6.78f,5.23f,0),new Vector3(-6.5f,5.6f,0),new Vector3(-6.18f,6.01f,0),new Vector3(-5.75f,6.44f,0),new Vector3(-5.32f,6.78f,0) };
    private int counter;
    private bool dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        box = GameObject.Find("intervalbox").transform;
        pointer = GameObject.Find("Wheel").transform;
        pointer.transform.rotation = Quaternion.Euler(originalRot);
    }
#if UNITY_IOS || UNITY_ANDROID
    private void Update()
    {
        //Vector2 v3;

        if (Input.touchCount != 1)
        {
            if (Input.touchCount == 0)
            {
                dragging = false;
            }

            return;
        }

        Touch touch = Input.touches[0];
        Vector2 pos = touch.position;
        if (!isDragging && touch.position.x < (Screen.width / 2))
        {
            StartCoroutine(WaitingForSecond());
        }
        if (touch.phase == TouchPhase.Began && touch.position.x < (Screen.width / 2))
        {
            dragging = true;
        }
        if (dragging && touch.phase == TouchPhase.Moved)
        {
            Vector2 p = Camera.main.ScreenToWorldPoint(touch.position);

            float radius = 1f;
            Vector2 dir = p - startPos;
            if (dir.sqrMagnitude > radius)
            {
                dir = dir.normalized * radius;
            }

            transform.position = startPos + dir;
        }
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
            if (!IsCorrect())
            {
                IntervalManager.instance.failures++;
                force = 5;
            }
            //fire the bird
            GetComponent<Rigidbody2D>().isKinematic = false;
            Vector2 dir = startPos - (Vector2)transform.position;
            GetComponent<Rigidbody2D>().AddForce(dir * force);
            GetComponent<Animator>().SetBool("airborne", true);
            Destroy(this);
        }

    }
#else
    private void OnMouseUp()
    {
        if(!IsCorrect())
        {
            IntervalManager.instance.failures++;
            force = 5;
        }
        //fire the bird
        GetComponent<Rigidbody2D>().isKinematic = false;
        Vector2 dir = startPos - (Vector2)transform.position;
        GetComponent<Rigidbody2D>().AddForce(dir * force);
        GetComponent<Animator>().SetBool("airborne", true);
        Destroy(this);
    }

    private void OnMouseDrag()
    {
        //move the bird
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float radius = 1f;
        Vector2 dir = p - startPos;
        if (dir.sqrMagnitude > radius)
        {
            dir = dir.normalized * radius;
        }

        transform.position = startPos + dir;

        //move the box
        if (!isDragging)
        {
            StartCoroutine(WaitingForSecond());
        }
    }
#endif
    IEnumerator WaitingForSecond()
    {
        counter = 0;
        isDragging = true;
        for (int x = 0; x < rotations.Length; x++)
        {
            box.transform.position = boxPos[x];
            pointer.transform.rotation = Quaternion.Euler(rotations[x]);
            yield return new WaitForSeconds(.7f);
            counter++;
        }
        isDragging = false;
    }

    private bool IsCorrect()
    {
        int intervalToInteger;
        print(IntervalManager.instance.intervalName);
        char[] characters = IntervalManager.instance.intervalName.ToCharArray();
        if(characters.Length == 3)
        {
            //then it's augmented, minor, perfect, or diminished
            if(characters[0] == 'a')
            {
                //then it's augmented
                if(characters[1] == '2')
                {
                    intervalToInteger = 2;
                }
                else if(characters[1] == '4')
                {
                    intervalToInteger = 6;
                }
                else
                {
                    intervalToInteger = 11;
                }
            }
            else if(characters[0] == 'm')
            {
                if (characters[1] == '2')
                {
                    intervalToInteger = 0;
                }
                else if (characters[1] == '3')
                {
                    intervalToInteger = 3;
                }
                else if (characters[1] == '6')
                {
                    intervalToInteger = 9;
                }
                else
                {
                    intervalToInteger = 12;
                }
            }
            else if(characters[0] == 'P')
            {
                if(characters[1] == '4')
                {
                    intervalToInteger = 5;
                }
                else if(characters[1] == '5')
                {
                    intervalToInteger = 8;
                }
                else
                {
                    intervalToInteger = 14;
                }
            }
            else
            {
                //then it's diminished 5th
                intervalToInteger = 7;
            }
        }
        else
        {
            //then it's major
            if (characters[2] == '2')
            {
                intervalToInteger = 1;
            }
            else if (characters[2] == '3')
            {
                intervalToInteger = 4;
            }
            else if (characters[2] == '6')
            {
                intervalToInteger = 10;
            }
            else
            {
                intervalToInteger = 13;
            }
        }
        if (intervalToInteger == counter)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
