using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shatter : MonoBehaviour
{
    public Transform explodePosition;
    public float explosionForce;
    public float upwardsMultiplier;

    [SerializeField]
    private Fragment[] fragments;

    public float diesdTime = 4f;
    public float diesdDelay = 4f;

    public void ExplodeFrags(float force, float upwardsMultiplier)
    {
        foreach (Fragment f in fragments)
        {
            if (f.rb == null)
            {
                continue;
            }
            else if (f.rb.CompareTag("skip"))
            {
                continue;
            }

            f.rb.isKinematic = false;

            f.rb.AddForce(new Vector3(force * f.direction.x, force * f.direction.y * upwardsMultiplier, force * f.direction.z), ForceMode.Impulse);
        }

        Invoke("Diesd", diesdDelay);
    }

    public Fragment[] GetFrags()
    {
        return fragments;
    }

    public Fragment[] InitializeFrags(Vector3 explodePosition)
    {
        Fragment[] frags = new Fragment[transform.childCount];

        DestroyRigidbodies();
        Rigidbody[] rbs = CreateRigidBodies();
        Vector3[] directions = GetVectors(explodePosition);

        for (int i = 0; i < transform.childCount; i++)
        {
            frags[i] = new Fragment(directions[i], rbs[i]);
        }

        fragments = frags;
        return frags;
    }

    private void DestroyRigidbodies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out Rigidbody rb))
            {
                DestroyImmediate(rb, true);
            }
        }
    }

    public void DestroyColliders()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out MeshCollider col))
            {
                DestroyImmediate(col, true);
            }
        }
    }

    private Rigidbody[] CreateRigidBodies()
    {
        Rigidbody[] rbs = new Rigidbody[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("skip"))
            {
                continue;
            }

            Rigidbody rb = transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rbs[i] = rb;
        }

        return rbs;
    }

    private Vector3[] GetVectors(Vector3 worldExplodePosition)
    {
        Vector3[] vectors = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("skip"))
            {
                continue;
            }

            Renderer r = transform.GetChild(i).gameObject.GetComponent<Renderer>();

            vectors[i] = (r.bounds.center - worldExplodePosition).normalized;
        }

        return vectors;
    }

    public MeshCollider[] CreateCols()
    {
        DestroyColliders();

        MeshCollider[] colliders = new MeshCollider[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("skip"))
            {
                continue;
            }

            colliders[i] = transform.GetChild(i).gameObject.AddComponent<MeshCollider>();

            colliders[i].convex = true;
        }
        return colliders;
    }

    private void Diesd()
    {
        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            child.DOScale(0, diesdTime).SetEase(Ease.InSine).OnComplete(() => Destroy(child.gameObject));
        }
    }
}

[System.Serializable]
public class Fragment
{
    public Vector3 direction;
    public Rigidbody rb;

    public Fragment(Vector3 _direction, Rigidbody _rb) {
        direction = _direction;
        rb = _rb;
    }
}