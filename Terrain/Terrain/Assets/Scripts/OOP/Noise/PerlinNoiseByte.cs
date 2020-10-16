using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class PerlinNoiseByte
{
    private static int B = 0x100; 
    private static int BM = 0xff;
    private static int N = 0x1000;
    private static int SEED = 70;
    private static byte[] p = { 151,160,137,91,90,15,
       131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
       190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
       88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
       77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
       102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
       135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
       5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
       223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
       129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
       251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
       49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
       138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
   };

    private static float[][] g3 = new float[B][]; //514:3
    private static int start = 1;

    public static float GetValue(float x, float y, float z)
    {
        var vec = new float[3];
        vec[0] = x;
        vec[1] = y;
        vec[2] = z;

        int i, j;

        int b00, b10, b01, b11;

        float[] q;
        float t, u, v;
        float a, b, c, d;
        float sz, sy;

        if (start != 0)
        {
            start = 0;
            Initialize();
        }

        Setup(ref vec, 0, out int bx0, out int bx1, out float rx0, out float rx1); //2+ 2& 2-
        Setup(ref vec, 1, out int by0, out int by1, out float ry0, out float ry1); //2+ 2& 2-
        Setup(ref vec, 2, out int bz0, out int bz1, out float rz0, out float rz1); //2+ 2& 2-

        i = p[(byte)bx0];
        j = p[(byte)bx1];

        b00 = p[(byte)(i + by0)]; //1+
        b10 = p[(byte)(j + by0)]; //1+
        b01 = p[(byte)(i + by1)]; //1+
        b11 = p[(byte)(j + by1)]; //1+

        t = Curve(rx0);   //3* 1-
        sy = Curve(ry0);  //3* 1-
        sz = Curve(rz0);  //3* 1-

        q = g3[(byte)(b00 + bz0)]; //1+
        u = Dot(rx0, ry0, rz0, q); //3* 2+

        q = g3[(byte)(b10 + bz0)];//1+
        v = Dot(rx1, ry0, rz0, q);//3* 2+
        a = Lerp(t, u, v); //1 * 1 + 1 -

        q = g3[(byte)(b01 + bz0)];//1+
        u = Dot(rx0, ry1, rz0, q);//3* 2+

        q = g3[(byte)(b11 + bz0)];//1+
        v = Dot(rx1, ry1, rz0, q);//3* 2+
        b = Lerp(t, u, v); //1* 1+ 1-

        c = Lerp(sy, a, b); //1* 1+ 1-

        q = g3[(byte)(b00 + bz1)];//1+
        u = Dot(rx0, ry0, rz1, q);//3* 2+

        q = g3[(byte)(b10 + bz1)];//1+
        v = Dot(rx1, ry0, rz1, q);//3* 2+
        a = Lerp(t, u, v);//1* 1+ 1-

        q = g3[(byte)(b01 + bz1)];//1+
        u = Dot(rx0, ry1, rz1, q);//3* 2+

        q = g3[(byte)(b11 + bz1)]; //1+ 
        v = Dot(rx1, ry1, rz1, q);//3* 2+
        b = Lerp(t, u, v);//1* 1+ 1-

        d = Lerp(sy, a, b);//1* 1+ 1-
        var value = Lerp(sz, c, d);

        return value + 0.5f;//1* 1+ 1-
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

    private static float Dot(float x, float y, float z, float[] vec)
    {
        return x * vec[0] + y * vec[1] + z * vec[2];
    }

    private static float Curve(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }

    private static void Normalize(float[] v)
    {
        float s = 1f / (math.sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]));

        v[0] = v[0] * s;
        v[1] = v[1] * s;
        v[2] = v[2] * s;
    }


    private static void Initialize()
    {
        UnityEngine.Random.seed = SEED;

        for (int i = 0; i < g3.Length; ++i)
        {
            g3[i] = new float[3];
        }

        float BDivider = 1f / (float)B;

        //create 3d gradients
        for (int i = 0; i < B; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float value = (float)UnityEngine.Random.Range(-B, B) * BDivider;
                //float value = (float)UnityEngine.Random.Range(-B, B) /B;
                g3[i][j] = value;
            }
            Normalize(g3[i]);
        }
    }
}











