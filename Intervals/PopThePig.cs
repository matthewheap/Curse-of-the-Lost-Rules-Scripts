using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopThePig : MonoBehaviour
{
    public float forceNeeded = 1000;
    private GameObject pigHandler;

    private void Start()
    {
        pigHandler = GameObject.Find("slingshot");
    }

    float CollisionForce(Collision2D coll)
    {
        //estimate a collision's foce
        float speed = coll.relativeVelocity.sqrMagnitude;
        if (coll.collider.GetComponent<Rigidbody2D>())
            return speed * coll.collider.GetComponent<Rigidbody2D>().mass;
        return speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(CollisionForce(collision) >= forceNeeded)
        {
            GetComponent<AudioSource>().Play();    
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.3f); 
        pigHandler.GetComponent<PigCounter>().enemiesLeft--;
        Destroy(gameObject);
    }
}
