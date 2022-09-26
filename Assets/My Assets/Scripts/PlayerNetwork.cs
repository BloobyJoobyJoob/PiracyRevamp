using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using AmazingAssets.CurvedWorld;
using Cinemachine;

public class PlayerNetwork : NetworkBehaviour
{
    public Rigidbody rb;
    public float interpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkData> netState = new(writePerm: NetworkVariableWritePermission.Server);
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
        }
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
                    Position = transform.position,
                    Rotation = transform.rotation.eulerAngles
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
        private short netPositionX, netPositionY, netPositionZ;
        private short netRotationX, netRotationY, netRotationZ;

        public Vector3 Position
        {
            get => new Vector3(netPositionX, netPositionY, netPositionZ);
            set {
                netPositionX = (short)value.x;
                netPositionY = (short)value.y;
                netPositionZ = (short)value.z;
            }
        }

        public Vector3 Rotation
        {
            get => new Vector3(netRotationX, netRotationY, netRotationZ);
            set {
                netRotationX = (short)value.x;
                netRotationY = (short)value.y;
                netRotationZ = (short)value.z;
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
}
