using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[System.Serializable]
public class Ocean
{
    private const int unityMaxVerts = 65535;

    [Tooltip("This is the resolution of the ocean")]
    [SerializeField]
    private int _resolution;
    public int Resolution { get => _resolution; private set => _resolution = value; }

    [Tooltip("This is the length & width of the ocean")]
    [SerializeField]
    private Vector2Int _size;
    public Vector2Int Size { get => _size; private set => _size = value; }

    [SerializeField]
    [Tooltip("Attach the mesh filter to hold the ocean mesh here")]
    private MeshFilter _meshFilter;
    public MeshFilter MeshFilter { get => _meshFilter; private set => _meshFilter = value; }

    public Ocean(int resolution, Vector2Int size, MeshFilter meshFilter)
    {
        Resolution = resolution;
        Size = size;
        MeshFilter = meshFilter;
    }


    [Tooltip("Will initialize new ocean based on ocean 's parameters")]

    // Genorate ocean mesh, material need to be applied manually
    public static Ocean InitializeOceanMesh(Ocean ocean)
    {
        // Make Vars
        List<Vector3> verts = new();
        List<int> tris = new();

        float xPerStep = ocean.Size.x / ocean.Resolution;
        float yPerStep = ocean.Size.y / ocean.Resolution;

        // Make Verts
        for (int y = 0; y < ocean.Resolution + 1; y++)
        {
            for (int x = 0; x < ocean.Resolution + 1; x++)
            {
                verts.Add(new Vector3(x * xPerStep, 0, y * yPerStep));
            }
        }

        // Make Tris
        for (int row = 0; row < ocean.Resolution; row++)
        {
            for (int collumn = 0; collumn < ocean.Resolution; collumn++)
            {
                int i = (row * ocean.Resolution) + row + collumn;

                tris.Add(i);
                tris.Add(i + ocean.Resolution + 1);
                tris.Add(i + ocean.Resolution + 2);

                tris.Add(i);
                tris.Add(i + ocean.Resolution + 2);
                tris.Add(i + 1);
            }
        }

        if (verts.Count >= unityMaxVerts)
        {
            // This stops the ocean vertice count going over Unity max
            Debug.LogWarning("Ocean mesh has too many verts: " + verts.Count + ". Please lower the ocean resolution or size to have less than " + unityMaxVerts + " verts");
            return null;
        }
        else
        {
            Mesh mesh = new();
            mesh.Clear();

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();

            ocean.MeshFilter.mesh = mesh;
            return new Ocean(ocean.Resolution, ocean.Size, ocean.MeshFilter);
        }
    }

    // This section is for the buoyancy to work
    // Get the y height of the mesh at an x & z point
    // Vector2 point needs to be relative to the position of the mesh

    // To get the height, I need to run my own gradient funciton that operates in tandem with the shader
    [Tooltip(" Height is relative to the objects position. You need to add the world hieght of the ocean to this value")]
    public static float GetHeightOfMeshAtPoint(Vector2 worldPoint, Material oceanMaterial)
    {
        float height = oceanMaterial.GetFloat("height");
        float speed = oceanMaterial.GetFloat("speed");
        float scale = oceanMaterial.GetFloat("scale");

        // Minimum value passed into octiveFactor parameter should equal the smallest octiveFactor value inside the waterShaderGraph
        // Anything less will calculating bouyancy to a deatail more than what is being visually displayed

        float noise1 = 0;
        float noise2 = 0;
        float noise3 = 0;
        float noise4 = 0;
        float noise5 = 0;
        float noise6 = 0;

        noise1 = GetNoiseAtPosition(new float3(worldPoint.x, 0, worldPoint.y), scale, speed, 32);

        noise2 = GetNoiseAtPosition(new float3(worldPoint.x, 0, worldPoint.y), scale, speed, 16);
        noise3 = GetNoiseAtPosition(new float3(worldPoint.x, 0, worldPoint.y), scale, speed, 8);
        noise4 = GetNoiseAtPosition(new float3(worldPoint.x, 0, worldPoint.y), scale, speed, 4);
        noise5 = GetNoiseAtPosition(new float3(worldPoint.x, 0, worldPoint.y), scale, speed, 2);
        noise6 = GetNoiseAtPosition(new float3(worldPoint.x, 0, worldPoint.y), scale, speed, 1);


        float combinedNoise = noise1 + noise2 + noise3 + noise4 + noise5 + noise6;

        const float defaultVertHeight = 0;

        return (height * combinedNoise) + defaultVertHeight;
    }

    public static float GetStaticHeight()
    {
        return 0;
    }

    // This is the equivalent in code, of the noise subgraph function in the ocean shadergraph
    private static float GetNoiseAtPosition(float3 worldPosition, float noiseScale = 1.5f, float speed = 0.14f, float octaveFactor = 32)
    {
        float2 uv = new(worldPosition.x, worldPosition.z);
        float2 offset = speed * octaveFactor * Shader.GetGlobalFloat("_CustomTime");
        float2 tilingAndOffset = ShaderNodes.TilingAndOffsetNode(uv, new(1, 1), offset);

        float scale = noiseScale / octaveFactor;

        float noise = ShaderNodes.GradientNoiseNode(tilingAndOffset, scale);

        return noise * octaveFactor;
    }
}
