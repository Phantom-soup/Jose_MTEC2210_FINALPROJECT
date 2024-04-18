using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    //Movement
    #region Movement
    [Header("Movement")]

    public float speed = 100;

    public float fSx = 5;
    public float fSy = 5;

    float xMove;
    float yMove;

    #endregion

    //Jumping
    #region Jumping
    [Header("Jumping")]

    bool jumpFlag = false;
    public float rayDist = 0.6f;
    
    public float jumpPower = 10;
    public float jumpInterval = 0.3f;

    public float jumpPullBack = 5;
    public float fallingGravity = 15;

    private float jumpTime;

    #endregion

    //Flying
    #region Flying
    [Header("Flying")]

    public float flightTimer = 6;
    bool flyState = false;
    bool flightLimit = false;

    #endregion

    //Shooting
    #region Shooting
    [Header("Shooting")]

    public GameObject BulletClone;
    public Transform launchArea;

    #endregion

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float timeElapsed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");

        if (Grounded())
        {
            flyState = false;

            if (Input.GetKeyUp(KeyCode.Z))
            {
                Debug.Log("Jump!");
                jumpTime = 0;
                jumpFlag = true;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Z) | jumpTime > jumpInterval)
            {
                jumpFlag = false;
            }

            if (Input.GetKeyDown(KeyCode.Z) && flyState)
            {
                Debug.Log("flyState has become false!");
                flyState = false;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && flyState == false)
            {
                Debug.Log("flyState has become true!");
                flyState = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(BulletClone, launchArea.position, transform.rotation);
        }
        Debug.Log(rb.velocity.y);
    }

    bool Grounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, rayDist, LayerMask.GetMask("Ground"));

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(xMove * speed * Time.deltaTime, rb.velocity.y);

        if (rb.velocity.y > 0)
        {
            rb.gravityScale = jumpPullBack;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravity;
        }

        if (jumpFlag)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpTime += Time.deltaTime;
        }

        if (flyState)
        {
            rb.gravityScale = 1;
            rb.velocity = new Vector2(xMove * speed * Time.deltaTime, yMove * speed * Time.deltaTime);
        }
    }
}