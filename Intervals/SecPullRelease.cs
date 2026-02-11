using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecPullRelease : MonoBehaviour
{
    Vector2 startPos;
    public float force = 600;
    private Transform box;
    private Transform pointer;
    private bool isDragging = false;
    private Vector3 originalRot = new Vector3(0, 0, 180);
    private Vector3[] rotations = new Vector3[10] {new Vector3(0,0,149.8f), new Vector3(0,0,135.8f), new Vector3(0, 0, 123.2f), new Vector3(0, 0, 107.3f), new Vector3(0, 0, 91.1f), new Vector3(0, 0, 75.3f), new Vector3(0, 0, 57.5f), new Vector3(0, 0, 41.2f),
        new Vector3(0, 0, 29.81f),new Vector3(0,0,16.6f)};
    //private Vector3[] boxPos = new Vector3[10] {new Vector3(-.66f,-5.73f,0), new Vector3(-1.54f,-4.8f,0),new Vector3(-2.17f,-3.48f,0), new Vector3(-2.52f,-2.25f,0),new Vector3(-2.68f, -.93f,0),new Vector3(-2.54f,.42f,0),new Vector3(-1.97f,1.91f,0),new Vector3(-1.47f,3.34f,0),
    //new Vector3(-.59f,4.56f,0), new Vector3(.48f,5.71f,0) };
    private Vector3[] boxPos = new Vector3[10] {new Vector3(-6.63f,1.26f,0), new Vector3(-7.01f,1.75f,0),new Vector3(-7.27f,2.32f,0), new Vector3(-7.43f,2.93f,0),new Vector3(-7.43f, 3.54f,0),new Vector3(-7.43f,4.17f,0),new Vector3(-7.21f,4.78f,0),new Vector3(-7.0f,5.51f,0),
    new Vector3(-6.57f,6.08f,0), new Vector3(-6.14f,6.61f,0)};
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
        if (!isDragging && touch.position.x<(Screen.width/2))
        {
            StartCoroutine(WaitingForSecond());
        }
        if (touch.phase == TouchPhase.Began && touch.position.x < (Screen.width/2))
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
                SecondaryMaker.instance.failures++;
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


private void OnMouseUp()
    {
        if (!IsCorrect())
        {
            SecondaryMaker.instance.failures++;
            force = 5;
        }
        //fire the bird
        GetComponent<Rigidbody2D>().isKinematic = false;
        Vector2 dir = startPos - (Vector2)transform.position;
        GetComponent<Rigidbody2D>().AddForce(dir * force);
        GetComponent<Animator>().SetBool("airborne", true);
        Destroy(this);
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
        int secToInteger;
        char[] characters = SecondaryMaker.instance.intervalName.ToCharArray();
        if(characters[0] == '5')
        {
            secToInteger = ((int)char.GetNumericValue(characters[1]))-2;

        }
        else
        {
            secToInteger = ((int)char.GetNumericValue(characters[1])) + 3;

        }
        if (secToInteger == counter)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
