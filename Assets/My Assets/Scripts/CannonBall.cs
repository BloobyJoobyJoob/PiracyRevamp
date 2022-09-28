using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonBall : MonoBehaviour
{
    public float gravityScale;

    public static float globalGravity = -9.81f;
    public float waterDrag = 0.95f;

    public Rigidbody rb;

    public float timeAlive;

    private void Awake()
    {
        Invoke("Shrink", timeAlive);
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= 0)
        {
            rb.velocity *= waterDrag;
            rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
        else
        {
            Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    void Shrink()
    {
        transform.DOScale(0, 1).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
