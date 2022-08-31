using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Transform boundsObject;
    public Bounds bounds;

    public float speed;
    public float heightMultiplier;
    public Vector2 direction;

    private void Start()
    {
        InvokeRepeating("CheckContains", 0.1f, 0.5f);
        direction = (speed + (transform.position.y * heightMultiplier)) * Time.deltaTime * direction.normalized;
    }

    private void Update()
    {
        Debug.Log(direction);
        transform.position += new Vector3(direction.x, 0, direction.y);
    }

    private void CheckContains()
    {
        bounds.center = boundsObject.position;

        if (transform.position.x <= bounds.min.x)
        {
            transform.position = new Vector3(bounds.max.x, 0, 0);
        }
        if (transform.position.z <= bounds.min.z)
        {
            transform.position = new Vector3(0, 0, bounds.max.z);
        }
        if (transform.position.x >= bounds.max.x)
        {
            transform.position = new Vector3(bounds.min.x, 0, 0);
        }
        if (transform.position.z >= bounds.max.z)
        {
            transform.position = new Vector3(0, 0, bounds.min.z);
        }
    }
}
