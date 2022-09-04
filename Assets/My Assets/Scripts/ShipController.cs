using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    private Rigidbody rb;

    public float moveForce;
    public float rotateForce;

    public ParticleSystem ps;
    public float spawnMultiplier;
    public float spawnMin;

    private Vector2 movement = Vector2.zero;

    private ParticleSystem.EmissionModule em;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        em = ps.emission;
    }

    private void FixedUpdate()
    {
        rb.AddForce(Mathf.Abs(movement.y) * moveForce * transform.forward, ForceMode.Acceleration);
        rb.AddTorque(movement.x * rotateForce * new Vector2(rb.velocity.x, rb.velocity.z).sqrMagnitude * -transform.up, ForceMode.Acceleration);
    }

    private void Update()
    {
        em.rateOverTime = spawnMin + (rb.velocity.sqrMagnitude * spawnMultiplier);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
}
