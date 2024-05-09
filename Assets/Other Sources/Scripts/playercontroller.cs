using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    //Movement
    public float speed = 100;

    public float fSx = 5;
    public float fSy = 5;

    public float xMove;
    public float yMove;

    //Jumping
    bool jumpFlag = false;
    bool canJump = false;
    
    public float jumpPower = 10;
    public float jumpInterval = 0.3f;

    public float jumpPullBack = 5;
    public float fallingGravity = 15;
    public float maxFallSpeed = -9;

    private float jumpTime;

    //Flying
    private float tempFTimer = 0;
    public float flightTimer = 6;
    bool flightLimit = false;
    bool flyState = false;

    //Shooting
    public GameObject BulletClone;
    public Transform launchArea;

    public float chargeTime = 1;
    bool isCharging;
    private float chargingFire;

    //Other
    public Vector2 boxSize;
    public float castDistance;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public GameManager gm;

    private GameObject currentOneWayPlatform;
    private float timeElapsed;
    bool canTakeDamage = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");


        //Flying
        if (Grounded() | Platform())
        {
            if (rb.velocity.y == 0)
            {
                jumpTime = 0;
                canJump = false;
                flyState = false;
                flightLimit = false;
                tempFTimer = 0;
                gm.AdjustFlightTime(flightTimer);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z) && flyState)
            {
                flyState = false;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && flyState == false)
            {
                flyState = true;
            }
        }
        if (flyState)
        {
            tempFTimer += Time.deltaTime;
            
        }

        if (tempFTimer > flightTimer)
        {
            flightLimit = true;
        }

        if (flightLimit)
        {
            flyState = false;
        }


        //Jumping
        if (Input.GetKeyDown(KeyCode.Z))
        {
            canJump = true;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            jumpTime += Time.deltaTime;
            if (canJump)
            {
                jumpFlag = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z) | jumpTime > jumpInterval)
        {
            jumpFlag = false;
            jumpTime = jumpInterval;
        }


        //Shooting
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
        }

        if (Input.GetKey(KeyCode.X))
        {
            if (isCharging)
            {
                chargingFire += Time.deltaTime;

                if (chargingFire < chargeTime)
                {
                    sr.color = Color.gray;
                }
                
                if(chargingFire > chargeTime)
                {
                    sr.color = Color.yellow;
                }
            }
        }



        if (Input.GetKeyUp(KeyCode.X))
        {
            if (chargingFire > chargeTime)
            {
                Instantiate(BulletClone, launchArea.position, transform.rotation);
            }
            chargingFire = 0;
            sr.color = Color.white;
            isCharging = false;
        }


        //Character flip
        if (xMove != 0)
        {
            if (xMove < 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.up * 180);
            }
            else
            {
                transform.rotation = Quaternion.Euler(Vector3.up * 0);
            }
        }



        if (Input.GetKeyDown(KeyCode.Q))
        {
            gm.AdjustHealth(-2.2f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            gm.AdjustHealth(10f);
        }

        if (gm.playerHealth < 0f)
        {
            Destroy(gameObject);
        }

        if (rb.velocity.y <= maxFallSpeed)
        {
            rb.velocity = new Vector2(xMove * fSx * Time.deltaTime, Mathf.Clamp(rb.velocity.y, maxFallSpeed, maxFallSpeed));
        }
    }

    bool Grounded()
    {
        //return Physics2D.Raycast(transform.position, Vector2.down, 0.6f, LayerMask.GetMask("Ground"));
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, LayerMask.GetMask("Ground"));
    }

    bool Platform()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, LayerMask.GetMask("Platform"));
    }

    bool Bonk()
    {
        return Physics2D.Raycast(transform.position, Vector2.up, 0.6f, LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    void FixedUpdate()
    {
        //Flying
        if (flyState)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(xMove * fSx * Time.deltaTime, yMove * fSy * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(xMove * speed * Time.deltaTime, rb.velocity.y);
        }

        //Falling
        if (rb.velocity.y > 0 && flyState == false)
        {
            rb.gravityScale = jumpPullBack;
        }

        if (rb.velocity.y <= 0 && flyState == false)
        {
            rb.gravityScale = fallingGravity;
        }

        //Jumping
        if (jumpFlag)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            
            if(Bonk())
            {
                jumpTime = jumpInterval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemybullet")
        {
            if (canTakeDamage)
            {
                StartCoroutine(BlinkRed());
                gm.AdjustHealth(-2.2f);
                Destroy(collision.gameObject);
            }
        }
    }

    public IEnumerator BlinkRed()
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);

        canTakeDamage = false;
        sr.color = Color.red;
        yield return delay;
        sr.color = Color.white;
        yield return delay;
        sr.color = Color.red;
        yield return delay;
        sr.color = Color.white;
        yield return delay;
        sr.color = Color.red;
        yield return delay;
        sr.color = Color.white;
        canTakeDamage = true;
    }
}