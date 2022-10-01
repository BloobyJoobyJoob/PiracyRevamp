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

    private bool fire = false;

    private ParticleSystem.EmissionModule em;
    private Cannons Cannons;
    private void Start()
    {
        Cannons = GetComponent<Cannons>();

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

        if (fire)
        {
            Cannons.TryFireCannons();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        fire = context.action.IsPressed();
    }
}
