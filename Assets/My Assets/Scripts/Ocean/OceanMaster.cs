using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TimeType
{
    UnscaledTime,
    ScaledTime,
}
public class OceanMaster : MonoBehaviour
{
    public TimeType waterTimeScale;
    void Update()
    {
        Shader.SetGlobalFloat("_CustomTime", waterTimeScale == TimeType.ScaledTime ? Time.time : Time.unscaledTime);
    }
}
