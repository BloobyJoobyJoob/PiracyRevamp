using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour
{
    public static CloudsManager Singleton;

    [HideInInspector]
    public FollowPosition followPosition;

    public float oceanHeight = 0;

    private void Awake()
    {
        TryGetComponent(out followPosition);

        if (Singleton != null)
        {
            Debug.LogError("More than one CloudsManager");
        }
        else
        {
            Singleton = this;
        }
    }
}
