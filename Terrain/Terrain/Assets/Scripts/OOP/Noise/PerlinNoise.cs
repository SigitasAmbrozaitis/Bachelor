using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class PerlinNoise
{
    private static int B = 0x100; 
    private static int BM = 0xff;
    private static int N = 0x1000;
    private static int SEED = 70;

    private static int[] p = new int[B + B + 2]; //514
    private static float[][] g2 = new float[B + B + 2][]; //514:3
    private static int start = 1;
    
    public static float GetValue(Vector2 pos)
    {
        return GetValue(pos.x, pos.y);
    }

    public static float GetValue (float x, float y)
    {
        var vec = new float[2];
        vec[0] = x;
        vec[1] = y;

        int  b00, b10, b01, b11;
        int i, j;

        if (start != 0)
        {
            start = 0;
            Initialize();
        }

        Setup(ref vec, 0, out int bx0, out int bx1, out float rx0, out float rx1); //2+ 2& 2-
        Setup(ref vec, 1, out int by0, out int by1, out float ry0, out float ry1);

        i = p[bx0];
        j = p[bx1];

        b00 = p[i + by0];
        b10 = p[j + by0];
        b01 = p[i + by1];
        b11 = p[j + by1];

        var sx = Curve(rx0);
        var sy = Curve(ry0);

        var q = g2[b00]; 
        var u = Dot(rx0, ry0, q);

        q = g2[b10]; 
        var v = Dot(rx1, ry0, q);
        var a = Lerp(sx, u, v);

        q = g2[b01]; 
        u = Dot(rx0, ry1, q);
        q = g2[b11]; 
        v = Dot(rx1, ry1, q);
        var b = Lerp(sx, u, v);

        return Lerp(sy, a, b);
    }


    private static void Setup(ref float[] vec, int i, out int b0, out int b1, out float r0, out float r1)
    {
        var t = vec[i] + N;
        b0 = ((int)t) & BM;
        b1 = (b0 + 1) & BM;
        r0 = t - (int)t;
        r1 = r0 - 1f;
    }

    private static float Lerp(float t, float a, float b)
    {
        return a + t * (b - a);
    }

    private static float Dot(float x, float y, float[] vec)
    {
        return x * vec[0] + y * vec[1];
    }

    private static float Curve(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }

    private static void Normalize(float[] v)
    {
        float s = 1f / (math.sqrt(v[0] * v[0] + v[1] * v[1]));
        v[0] = v[0] * s;
        v[1] = v[1] * s;
    }


    private static void Initialize()
    {
        UnityEngine.Random.seed = SEED;

        for (int i = 0; i < g2.Length; ++i)
        {
            g2[i] = new float[2];
        }

        float BDivider = 1f / (float)B;

        //create 3d gradients
        for (int i = 0; i < B; i++)
        {
            p[i] = i;

            for (int j = 0; j < 2; j++)
            {
                float value = (float)UnityEngine.Random.Range(-B, B) * BDivider;
                //float value = (float)UnityEngine.Random.Range(-B, B) /B;
                g2[i][j] = value;
            }
                
            Normalize(g2[i]);
        }

        //shuffle p values 
        int temp;
        int index = 255;
        while (--index > 0)
        {
            var newIndex = UnityEngine.Random.Range(0, B);

            temp = p[index];
            p[index] = p[newIndex];
            p[newIndex] = temp;
        }


        //extend p value form 255 to 514 indexes, just copy over
        for (int i = 0; i < B + 2; i++)
        {
            p[B + i] = p[i];
            for (int j = 0; j < 2; j++)
            {
                g2[B + i][j] = g2[i][j];
            }
        }
    }
}











