using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using KartGame.KartSystems;
using UnityEngine;
using TMPro;

public class KeyBox : MonoBehaviour
{
    public ArcadeKart.StatPowerup boostStats = new ArcadeKart.StatPowerup
    {
        MaxTime = 5
    };
    public bool isCoolingDown { get; private set; }
    public float lastActivatedTimestamp { get; private set; }

    public float cooldown = 5f;

    public UnityEvent onPowerupActivated;
    public UnityEvent onPowerupFinishCooldown;
    public KartManager manager;
    private string thisName;

    private void Start()
    {
        manager = FindObjectOfType<KartManager>();
        lastActivatedTimestamp = -9999f;
        
    }
    private void Update()
    {
        if (isCoolingDown)
        {
            if (Time.time - lastActivatedTimestamp > cooldown)
            {
                //finished cooldown!
                isCoolingDown = false;
                onPowerupFinishCooldown.Invoke();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCoolingDown) return;
        isCoolingDown = true;
        thisName = this.GetComponentInChildren<TextMeshPro>().text;
        var rb = other.attachedRigidbody;
        if (manager.CheckAnswer(thisName, gameObject))//if it's right
        { 
            if (rb)
            {
                var kart = rb.GetComponent<ArcadeKart>();

                if (kart)
                {
                    lastActivatedTimestamp = Time.time;
                    kart.AddPowerup(this.boostStats);
                    onPowerupActivated.Invoke();
                }
            }
        }
        else
        {
            lastActivatedTimestamp = Time.time;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}
