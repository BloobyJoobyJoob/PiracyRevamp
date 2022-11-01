using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using AmazingAssets.CurvedWorld;
using Cinemachine;
using TMPro;
using Unity.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    public Rigidbody rb;
    public GameObject[] flags;
    public Transform flagParent;
    public TextMeshProUGUI usernameTag;
    public float interpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkData> netState = new(writePerm: NetworkVariableWritePermission.Server);

    private NetworkVariable<FixedString32Bytes> usernameNetworkVar = new(writePerm: NetworkVariableWritePermission.Server);

    private Vector3 discardVel;
    private Vector3 discardRotVel;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            OceanMaster.Singleton.followPosition.target = transform;
            EndlessTerrain.Singleton.viewer = transform;
            CloudsManager.Singleton.followPosition.target = transform;
            GameObject.FindWithTag("Curved World Manager").GetComponent<CurvedWorldController>().bendPivotPoint = transform;
            GameObject.FindWithTag("Main Virtual Cam").GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0);

            SendUsernameServerRPC(new(GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkHelper>().username.text));
        }
        Instantiate(flags[(int)Mathf.Clamp(OwnerClientId, 0, flags.Length)], flagParent);

        usernameNetworkVar.OnValueChanged += OnUsernameChange;
    }

    public override void OnNetworkDespawn()
    {
        usernameNetworkVar.OnValueChanged -= OnUsernameChange;
    }

    private void OnUsernameChange(FixedString32Bytes prevVal, FixedString32Bytes newVal)
    {
        usernameTag.text = newVal.ToString();
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (IsServer)
            {
                netState.Value = new PlayerNetworkData()
                {
                    Position = rb.position,
                    Rotation = rb.rotation.eulerAngles
                };
            }
            else
            {
                TransmitPlayerNetworkDataServerRPC(new PlayerNetworkData()
                {
                    Position = rb.position,
                    Rotation = rb.rotation.eulerAngles
                });
            }
        }
        else
        {
            rb.MovePosition(Vector3.SmoothDamp(transform.position, netState.Value.Position, ref discardVel, interpolationTime));

            rb.MoveRotation(Quaternion.Euler(
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.x, netState.Value.Rotation.x, ref discardRotVel.x, interpolationTime),
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, netState.Value.Rotation.y, ref discardRotVel.y, interpolationTime),
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, netState.Value.Rotation.z, ref discardRotVel.z, interpolationTime
            )));
        }
    }

    [ServerRpc]
    private void TransmitPlayerNetworkDataServerRPC(PlayerNetworkData state)
    {
        netState.Value = state;
    }
    struct PlayerNetworkData : INetworkSerializable
    {
        private float netPositionX, netPositionY, netPositionZ;
        private float netRotationX, netRotationY, netRotationZ;

        public Vector3 Position
        {
            get => new Vector3(netPositionX, netPositionY, netPositionZ);
            set {
                netPositionX = value.x;
                netPositionY = value.y;
                netPositionZ = value.z;
            }
        }

        public Vector3 Rotation
        {
            get => new Vector3(netRotationX, netRotationY, netRotationZ);
            set {
                netRotationX = value.x;
                netRotationY = value.y;
                netRotationZ = value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref netPositionX);
            serializer.SerializeValue(ref netPositionY);
            serializer.SerializeValue(ref netPositionZ);
            serializer.SerializeValue(ref netRotationX);
            serializer.SerializeValue(ref netRotationY);
            serializer.SerializeValue(ref netRotationZ);
        }
    }

    [ServerRpc]
    private void SendUsernameServerRPC(FixedString32Bytes username)
    {
        usernameNetworkVar.Value = username;
    }
}
