using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameTag : MonoBehaviour
{
    private Transform target;
    public float initialDist;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Main Virtual Cam").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt((transform.position * 2) - target.position);

        float dist = Vector3.Distance(transform.position, target.position);
        transform.localScale = Vector3.one * dist / initialDist;
    }
}
