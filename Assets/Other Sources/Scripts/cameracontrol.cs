using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontrol : MonoBehaviour
{
    public Transform target;
    public GameObject px;

    public float playerCam;
    public float speed;

    private float lookAhead;

    public Transform leftBounds;
    public Transform upBounds;
    public Transform rightBounds;
    public Transform downBounds;

    void Update()
    {
        transform.position = new Vector3(target.position.x + playerCam, target.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (playerCam * target.rotation.y), Time.deltaTime * speed);
    }
}
