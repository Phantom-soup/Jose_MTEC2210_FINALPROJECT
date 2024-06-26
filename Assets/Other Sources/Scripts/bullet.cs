using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody2D rb;
    public GameManager gm;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = gameObject.AddComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){}
        else
        {
            Destroy(gameObject);
        }
    }
}
