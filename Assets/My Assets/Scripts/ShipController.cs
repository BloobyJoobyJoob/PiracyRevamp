using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class ShipController : NetworkBehaviour
{
    private Rigidbody rb;

    public float moveForce;
    public float rotateForce;
    public float maxRotateForce;

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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Mathf.Clamp(movement.y, 0, 1) * moveForce * transform.forward, ForceMode.Acceleration);
        rb.AddTorque(Mathf.Clamp(movement.x * rotateForce * new Vector2(rb.velocity.x, rb.velocity.z).sqrMagnitude, -maxRotateForce, maxRotateForce) * -Vector3.up, ForceMode.Acceleration);
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
