using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class ShipController : NetworkBehaviour
{
    private Rigidbody rb;

    public CinemachineImpulseSource CinemachineImpulseSource;
    public Transform cam;
    public float hitImpulseStrength;

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
    private ScreenDamage indicator;

    public float damageAmmount;
    private void Start()
    {
        Cannons = GetComponent<Cannons>();

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        em = ps.emission;
    }

    public override void OnNetworkSpawn()
    {
        cam = GameObject.FindGameObjectWithTag("Main Virtual Cam").transform;

        indicator = Camera.main.GetComponent<ScreenDamage>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Mathf.Clamp(movement.y, 0, 1) * moveForce * transform.forward, ForceMode.Acceleration);
        rb.AddTorque(Mathf.Clamp(movement.x * rotateForce * new Vector2(rb.velocity.x, rb.velocity.z).sqrMagnitude, -maxRotateForce, maxRotateForce) * -Vector3.up, ForceMode.Acceleration);
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (IsOwner)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Cannonball"))
            {
                CinemachineImpulseSource.GenerateImpulseWithVelocity((cam.position - collision.gameObject.transform.position).normalized * hitImpulseStrength);

                indicator.CurrentHealth -= 15;
            }
            else
            {
                indicator.CurrentHealth -= (collision.impulse / Time.fixedDeltaTime).magnitude * damageAmmount;
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Cannonball"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsOwner)
        {
            indicator.CurrentHealth -= (collision.impulse / Time.fixedDeltaTime).magnitude * damageAmmount * 0.5f;
        }
    }
}
 