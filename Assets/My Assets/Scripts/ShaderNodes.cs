using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public class ShaderNodes
{
    public static float GradientNoiseNode(Vector2 UV, float Scale)
    {
        return unity_gradientNoise(UV * Scale) + 0.5f;
    }

    private static float2 unity_gradientNoise_dir(float2 p)
    {
        p = p % 289;
        float x = (34 * p.x + 1) * p.x % 289 + p.y;
        x = (34 * x + 1) * x % 289;
        x = math.frac(x / 41) * 2 - 1;
        return math.normalize(math.float2(x - math.floor(x + 0.5f), math.abs(x) - 0.5f));
    }

    private static float unity_gradientNoise(float2 p)
    {
        float2 ip = math.floor(p);
        float2 fp = math.frac(p);
        float d00 = math.dot(unity_gradientNoise_dir(ip), fp);
        float d01 = math.dot(unity_gradientNoise_dir(ip + math.float2(0, 1)), fp - math.float2(0, 1));
        float d10 = math.dot(unity_gradientNoise_dir(ip + math.float2(1, 0)), fp - math.float2(1, 0));
        float d11 = math.dot(unity_gradientNoise_dir(ip + math.float2(1, 1)), fp - math.float2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
        return math.lerp(math.lerp(d00, d01, fp.y), math.lerp(d10, d11, fp.y), fp.x);
    }
    public static float2 TilingAndOffsetNode(float2 UV, float2 Tiling, float2 Offset)
    {
        return UV * Tiling + Offset;
    }
}
