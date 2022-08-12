using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [Header("IMPORTANT: Set UseGravity on this objects Rigid Body to Zero.")]
    [Header("Gravity will be applied to the object through the floaters")]
    [Space]
    [Space]
    [Tooltip("The Rigidbody of this object. For objects with multiple floaters, make the floaters children of the object, and attach the parent Rigidbody here for every floater.")]
    public Rigidbody rb;
    [Tooltip("Increase this value to make the object float higher. Decrease this value to mkae the object float lower. This value is multiplied by the depth of the floater")]
    public float buoyancyForce = 3;
    private float waveHeight = 0;
    [Tooltip("Attach the material of the ocean this object will be floating on")]
    public Material oceanMaterial;
    [Tooltip("The number of floaters that are attached to this object")]
    public int floatersOnThisObject = 4;
    [Tooltip("The gravityScale to be applied to this floater. (In 95% of cases, this should be kept at a value of one)")]
    public float gravityScaleOnThisFloater = 1;

    [Tooltip("The drag applied when the floater is in the water")]
    public float waterDrag;
    public float waterAngularDrag;

    [Tooltip("Attach the transform of the ocean. As long as all oceans that this object would be floating on are on the same y level, it shouldn't matter which ocean you attach here")]
    public Transform oceanHeight;
    void FixedUpdate()
    {
        rb.AddForceAtPosition(gravityScaleOnThisFloater * Physics.gravity / floatersOnThisObject, transform.position, ForceMode.Acceleration);
        waveHeight = Ocean.GetHeightOfMeshAtPoint(new Vector2(transform.position.x, transform.position.z), oceanMaterial);
        waveHeight += oceanHeight.position.y;

        Debug.DrawLine(transform.position, new Vector3(transform.position.x, waveHeight, transform.position.z));

        if (transform.position.y < waveHeight)
        {
            float finalForce = Mathf.Clamp01(waveHeight - transform.position.y) * buoyancyForce;
            rb.AddForceAtPosition(new Vector3(0, Mathf.Abs(Physics.gravity.y) * finalForce, 0), transform.position, ForceMode.Acceleration);
            rb.AddForce(finalForce * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(finalForce * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
