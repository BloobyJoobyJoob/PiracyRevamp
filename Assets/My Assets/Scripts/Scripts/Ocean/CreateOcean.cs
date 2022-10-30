using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOcean : MonoBehaviour
{
    public Ocean ocean;
    private void Awake()
    {
        Ocean.InitializeOceanMesh(ocean);
    }
}