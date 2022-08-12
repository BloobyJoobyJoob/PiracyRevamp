using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCenterOfMass : MonoBehaviour
{
    public Vector3 centerOfMassOffset;
    public Rigidbody rigidBody;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass += centerOfMassOffset;
    }
}
