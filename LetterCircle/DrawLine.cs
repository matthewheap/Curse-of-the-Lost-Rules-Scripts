using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;
    public bool isMousePressed;
    public List<Vector3> pointsList;
    private Vector3 mousePos;
    private LetterWheelMan gm;

    // Structure for line points
    struct myLine
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
    };
    //    -----------------------------------    
    void Awake()
    {
        // Create line renderer component and set its property
        line = gameObject.GetComponent<LineRenderer>();
        gm = gameObject.GetComponent<LetterWheelMan>();
        line.positionCount = 0;
        line.startWidth = .05f;
        line.endWidth = .05f;
        line.useWorldSpace = true;
        isMousePressed = false;
        pointsList = new List<Vector3>();
        //        renderer.material.SetTextureOffset(
    }
    //    -----------------------------------    
    void Update()
    {
        // If mouse button down, remove old line and set its color to green
        if (Input.GetMouseButtonDown(0))
        {
            isMousePressed = true;
            line.positionCount = 0;
            pointsList.RemoveRange(0, pointsList.Count);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
        }
        // Drawing line when mouse is moving(presses)
        if (isMousePressed)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) //if we hit a letter, draw a line there 
            {
                gm.turnOn.Add(hit.collider);
                gm.currentWord += hit.transform.GetComponent<TextMeshPro>().text;
                hit.collider.enabled = false;
                gm.wordLength++;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                pointsList.Add(mousePos);
                print(pointsList.Count);
                line.positionCount = pointsList.Count;
                line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
            }
            else
            {
                if (!pointsList.Contains(mousePos) && pointsList.Count > 0)
                {
                    
                    pointsList.Remove(pointsList[pointsList.Count - 1]);
                    pointsList.Add(mousePos);
                    line.positionCount = pointsList.Count;
                    line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
                }
            }
        }
    }


}
