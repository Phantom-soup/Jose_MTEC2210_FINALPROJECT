using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class newplayercontrol : MonoBehaviour
{
    //Ground Check

    //Movement
    public float moveSpeed = 90;
    float xMove;

    //Jumping
    public float jumpPower = 20;
    public float buttonTime = 1f;
    public float gravityScale = 5;
    public float fallGravityScale = 15;

    float jumpTime;
    float buttonPressedTime;
    bool jumping;


    Rigidbody2D rb;

    public bool isGrounded;
    public float offset = 0.1f;
    public Vector2 surfacePosition;
    ContactFilter2D filter;
    Collider2D[] results = new Collider2D[1];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    //Ground Check
        Vector2 point = transform.position + Vector3.down * offset;
        Vector2 size = new Vector2(transform.localScale.x, transform.localScale.y);

        if (Physics2D.OverlapBox(point, size, 0, filter.NoFilter(), results) > 0) 
        { 
            isGrounded = true;
            surfacePosition = Physics2D.ClosestPoint(transform.position, results[0]);
        }
        else
        {
            isGrounded = false;
        }

        //Movement
        xMove = Input.GetAxis("Horizontal");
        
    //Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            jumpTime = 0;
        }
        if(Input.GetKeyUp(KeyCode.Space) | jumpTime > buttonTime)
        {
            jumping = false;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xMove * moveSpeed * Time.deltaTime, rb.velocity.y);

        if (jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpTime += Time.deltaTime;
        }
    }
}