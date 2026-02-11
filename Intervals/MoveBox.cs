using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    public Transform box;
    public Transform pointer;
    private bool isDragging = false;
    Vector3 rotation;
    
    // Start is called before the first frame update
    void Start()
    {
        box.gameObject.SetActive(false);
        rotation = pointer.transform.rotation.eulerAngles;
        rotation.z = 180;
        pointer.transform.rotation = Quaternion.Euler(rotation);
    }

    private void FixedUpdate()
    {
        if(isDragging)
        {
            box.gameObject.SetActive(true);
            box.transform.position = new Vector3(-6.82f, 1.11f, 0);
            pointer.transform.rotation = Quaternion.Euler(new Vector3(0,0,149.26f));
        }
        else
        {
            box.gameObject.SetActive(false);
            pointer.transform.rotation = Quaternion.Euler(rotation); 
        }
    }

    private void OnMouseDrag()
    {
        print("dragging");
        isDragging = true;
    }
    private void OnMouseUp()
    {
        isDragging = false;
    }
}
