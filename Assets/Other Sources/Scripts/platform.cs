using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    public Vector2 boxSize;
    public float castDistance;

    private BoxCollider2D bc;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        if (Grounded())
        {
            bc.enabled = true;
        }
        else
        {
            bc.enabled = false;
        }
    }

    bool Grounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, LayerMask.GetMask("Player"));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
