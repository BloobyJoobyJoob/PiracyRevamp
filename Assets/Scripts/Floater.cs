using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour
{
    private Rigidbody rb;
    [Tooltip("Increase this value to make the object float higher. Decrease this value to mkae the object float lower. This value is multiplied by the depth of the floater")]
    public float buoyancyForce = 3;
    private float waveHeight = 0;
    [Tooltip("Attach the material of the ocean this object will be floating on")]
    public Material oceanMaterial;

    [Tooltip("The drag applied when the floaters are in the water")]
    public float waterDrag;
    public float waterAngularDrag;

    [Tooltip("Attach the transform of the ocean. As long as all oceans that this object would be floating on are on the same y level, it shouldn't matter which ocean you attach here")]
    public Transform oceanHeight;

    public Transform[] floatPoints;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb.useGravity == false)
        {
            Debug.LogWarning("This rigidbodies gravity is handled by its floaters");
        }
        else
        {
            rb.useGravity = false;
        }
    }

    void FixedUpdate()
    {
        foreach (Transform floater in floatPoints)
        {
            rb.AddForceAtPosition(Physics.gravity / floatPoints.Length, floater.position, ForceMode.Acceleration);
            waveHeight = Ocean.GetHeightOfMeshAtPoint(new Vector2(floater.position.x, floater.position.z), oceanMaterial);
            waveHeight += oceanHeight.position.y;

            if (floater.position.y < waveHeight)
            {
                float finalForce = Mathf.Clamp01(waveHeight - floater.position.y) * buoyancyForce;
                rb.AddForceAtPosition(new Vector3(0, Mathf.Abs(Physics.gravity.y) * finalForce, 0), floater.position, ForceMode.Acceleration);
                rb.AddForce(finalForce * Time.fixedDeltaTime * waterDrag * -rb.velocity, ForceMode.VelocityChange);
                rb.AddTorque(finalForce * Time.fixedDeltaTime * waterAngularDrag * -rb.angularVelocity, ForceMode.VelocityChange);
            }
        }
    }
}
