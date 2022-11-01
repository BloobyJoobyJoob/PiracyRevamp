using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vegetation
{
    public Mesh mesh;
    public Texture2D texture;
    public int count = 200;

    public float minHeight = -10;
    public float maxHeight = 20;
}
