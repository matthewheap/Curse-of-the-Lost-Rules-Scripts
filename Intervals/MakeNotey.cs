using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNotey : MonoBehaviour
{
    public GameObject birdPrefab;
    bool occupied = false;
    private string correctName;


    private void FixedUpdate()
    {
        if(!occupied && !SceneMoving())
        {
            SpawnNext();
            if (!TotalGameManager.instance.levelTwo)
            {
                GetComponent<SceneMan>().MakeInterval();
            }
            else
            {
                GetComponent<SecondarySceneMan>().MakeInterval();
            }
        }
    }
    private void SpawnNext()
    {
        Instantiate(birdPrefab, transform.position, Quaternion.identity);
        occupied = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        occupied = false;
    }

    private bool SceneMoving()
    {
        Rigidbody2D[] bodies = FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
        foreach(Rigidbody2D rb in bodies)
        {
            if(rb.velocity.sqrMagnitude > 2)
            {
                return true;
            }
        }
        return false;
    }
}
