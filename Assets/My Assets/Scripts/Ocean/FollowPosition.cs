using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    [Tooltip("Set this to the smallest common factor of the oceans resolution, ocean size x, and ocean size y")]
    public float followFactor = 2;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x - offset.x, transform.position.y, transform.position.z - offset.y);
        Vector2 posRounded = new Vector2(Mathf.Round(target.position.x / followFactor), Mathf.Round(target.position.z / followFactor));
        Vector2 pos = posRounded * followFactor;
        transform.position = new Vector3(pos.x + offset.x, transform.position.y, pos.y + offset.y);
    }
}
