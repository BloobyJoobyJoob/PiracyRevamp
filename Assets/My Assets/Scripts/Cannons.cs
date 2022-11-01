using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Unity.Netcode;

[System.Serializable]
public class Cannon
{
    public Transform transform;
    public ParticleSystem ps;
    public float postDelay = 1;
}

public class Cannons : NetworkBehaviour
{
    public Cannon[] cannons;
    public CinemachineImpulseSource CinemachineImpulseSource;

    public float recoilDistance = 2;
    public float recoilTime = 0.2f;
    public float cannonAlignTime = 1;
    public GameObject cannonBall;
    public float fireDelay;
    public float fireForce;
    public Vector3 localOffset;
    public Vector3 worldOffset;

    bool shooting = true;
    public float impulseStrength;

    public Transform cam;
    public override void OnNetworkSpawn()
    {
        Invoke("EnableCannons", 1);
        cam = GameObject.FindGameObjectWithTag("Main Virtual Cam").transform;
    }

    void EnableCannons()
    {
        shooting = false;
    }

    public void TryFireCannons()
    {
        if (IsOwner)
        {
            FireCannonsServerRPC();
        }

        if (shooting)
        {
            return;
        }
        else
        {
            shooting = true;
            StartCoroutine(BeginShooting(Time.time));
        }
    }

    IEnumerator BeginShooting(float timeStarted)
    {
        foreach (Cannon cannon in cannons)
        {
            if (IsOwner)
            {
                CinemachineImpulseSource.GenerateImpulse(impulseStrength * (cam.position - transform.position).normalized);
            }

            cannon.ps.Play();

            Transform t = cannon.transform;

            GameObject cb = Instantiate(cannonBall, t.TransformPoint(t.localPosition + localOffset) + worldOffset, t.rotation);

            cb.GetComponent<Rigidbody>().AddForce(fireForce * t.forward);

            yield return new WaitForSeconds(cannon.postDelay);
        }

        yield return new WaitForSeconds(fireDelay - (Time.time - timeStarted));

        shooting = false;
    }
    [ServerRpc]
    private void FireCannonsServerRPC()
    {
        FireCannonsClientRPC();
    }

    [ClientRpc]
    private void FireCannonsClientRPC()
    {
        if (!IsOwner)
        {
            TryFireCannons();
        }
    }
}
