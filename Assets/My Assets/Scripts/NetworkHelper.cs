using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;

public class NetworkHelper : MonoBehaviour
{
    public TMP_InputField text;
    public TMP_InputField username;
    public GameObject Buttons;
    public UnityTransport transport;
    public TextMeshProUGUI code;

    private string joinCode;
    public async void Awake()
    {
        Buttons.SetActive(false);

        await Authenticate();

        Buttons.SetActive(true);
    }

    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void JoinHost()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(7, "australia-southeast1");
        joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        code.text = joinCode;

        NetworkManager.Singleton.StartHost();

        Buttons.SetActive(false);
    }

    public async void JoinClient()
    {
        joinCode = text.text;
        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinCode);

        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
        NetworkManager.Singleton.StartClient();

        Buttons.SetActive(false);
    }
}
