using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [Header("Pool")]

    public Mesh[] cloudModels;
    public float cloudMinSize;
    public float cloudMaxSize;

    public GameObject cloudPrefab;

    [Header("Clouds")]

    [Tooltip("Square radius")]
    public float cloudSegmentRadius = 5;
    public float cloudHeightMin = 30;
    public float cloudHeightMax = 30;
    public float cloudsPerLength = 20;
    public float skyLength = 100;

    private Transform[] cloudPool;

    private float cloudDistance;


    private void LayoutPool()
    {
        cloudPool = GetComponentsInChildren<Transform>();

        for (int x = 1; x < Mathf.Sqrt(cloudPool.Length) - 1; x++)
        {
            for (int y = 1; y < Mathf.Sqrt(cloudPool.Length) - 1; y++)
            {
                float size = Random.Range(cloudMinSize, cloudMaxSize);

                cloudPool[(int)(Mathf.Sqrt(cloudPool.Length) * x) + y].transform.localScale = new Vector3(size, size, size);
                cloudPool[(int)(Mathf.Sqrt(cloudPool.Length) * x) + y].transform.eulerAngles = new Vector3(90, Random.Range(-180, 180), 0);

                Vector3 pos = new Vector3((x * cloudDistance) + Random.Range(-cloudSegmentRadius, cloudSegmentRadius), 
                    Random.Range(cloudHeightMin, cloudHeightMax),
                    (y * cloudDistance) + Random.Range(-cloudSegmentRadius, cloudSegmentRadius));

                cloudPool[(int)(Mathf.Sqrt(cloudPool.Length) * x) + y].position = new Vector3(pos.x - (skyLength / 2), pos.y, pos.z - (skyLength / 2));

            }
        }
    }

    public void GeneratePool()
    {
        int childCount = transform.childCount;
        cloudDistance = skyLength / cloudsPerLength;

        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < cloudsPerLength * cloudsPerLength; i++)
        {
            cloudPrefab.GetComponent<MeshFilter>().mesh = cloudModels[Random.Range(0, cloudModels.Length)];
            GameObject cloud = Instantiate(cloudPrefab, transform);

        }

        LayoutPool();
    }
}
