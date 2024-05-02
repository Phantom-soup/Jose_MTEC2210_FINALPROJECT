using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShotzo : MonoBehaviour
{
    public GameObject BulletClone;
    public Transform launchArea;

    public float chargeTime = 1;
    bool isCharging;
    private float timeElapsed;


    void Start()
    {
        
    }

 
    void Update()
    {
        if (timeElapsed < chargeTime)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            Instantiate(BulletClone, transform.position, transform.rotation);
            timeElapsed = 0;
        }
    }
}
