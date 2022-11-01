using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class NetworkHelper : MonoBehaviour
{
    public TMP_InputField text;
    public TMP_InputField username;
    public void JoinHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void JoinClient()
    {
        if (text.text != "")
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                text.text,
                7777,
                "0.0.0.0"
                );
        }
        NetworkManager.Singleton.StartClient();
    }
}
