using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OceanMaster : MonoBehaviour
{
    public static OceanMaster Singleton;

    [HideInInspector]
    public FollowPosition followPosition;

    private void Awake()
    {
        TryGetComponent(out followPosition);

        if (Singleton != null)
        {
            Debug.LogError("More than one oceanMaster");
        }
        else
        {
            Singleton = this;
        }
    }
}
