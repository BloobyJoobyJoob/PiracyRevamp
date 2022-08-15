using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    public float moveSpeed;

    private PlayerInput pi;
    private Rigidbody rb;
    private InputAction move;
    private void Start()
    {
        pi = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
    }
}
