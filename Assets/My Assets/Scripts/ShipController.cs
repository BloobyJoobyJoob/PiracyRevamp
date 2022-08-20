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

    private Vector2 movement = Vector2.zero;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        rb.AddForce(Mathf.Abs(movement.y) * moveForce * transform.forward, ForceMode.Acceleration);
        rb.AddTorque(movement.x * rotateForce * new Vector2(rb.velocity.x, rb.velocity.z).sqrMagnitude * -transform.up, ForceMode.Acceleration);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
}
