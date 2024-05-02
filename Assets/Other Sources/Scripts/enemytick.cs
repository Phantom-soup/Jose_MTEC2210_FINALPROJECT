using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemytick : MonoBehaviour
{
    public float speed = 10;
    public GameManager gm;
    private Rigidbody2D rb;

    private void Start()
    {
        gm = gameObject.AddComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            gm.AdjustHealth(-2.2f);
            Destroy(gameObject);
        }
        else if (collision.tag == "Shotzo") {

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
