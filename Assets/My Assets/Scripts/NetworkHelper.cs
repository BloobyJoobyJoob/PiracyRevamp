using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkHelper : MonoBehaviour
{
    public void JoinHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void JoinClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
