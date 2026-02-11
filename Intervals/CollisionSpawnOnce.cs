using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSpawnOnce : MonoBehaviour
{
    public GameObject effect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //spawn effect then remove script
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(this);
    }

}
