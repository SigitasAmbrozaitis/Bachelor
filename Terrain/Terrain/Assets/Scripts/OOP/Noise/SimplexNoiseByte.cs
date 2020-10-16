using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SimplexNoiseByte
{
    private static readonly byte[] T = { 0x15, 0x38, 0x32, 0x2c, 0x0d, 0x13, 0x07, 0x2a };

    private static byte[] A = {0, 0, 0 };
    private static int i = 0, j = 0, k = 0;
    private static float u = 0f, v = 0f, w = 0f;

    private static float m_Range = 0.5595479f;
    private static float m_Multiplier  = 1f / m_Range;

    public static float GetValue(float x, float y, float z)
    {
        float s = (x + y + z) / 3f;

        i = (int)Mathf.Floor(x + s);
        j = (int)Mathf.Floor(y + s);
        k = (int)Mathf.Floor(z + s);

        s = (i + j + k) / 6f;

        u = x - i + s;
        v = y - j + s;
        w = z - k + s;

        A[0] = A[1] = A[2] = 0;

        byte hi = (byte)(u >= w ? u >= v ? 0 : 1 : v >= w ? 1 : 2);
        byte lo = (byte)(u < w ? u < v ? 0 : 1 : v < w ? 1 : 2);
        return (K(hi) + K((byte)(3 - hi - lo)) + K(lo) + K(0)) * m_Multiplier + m_Range - 0.061328f;
    }

    private static float K(byte a)
    {
        float s = (A[0] + A[1] + A[2]) / 6f;
        float x = u - A[0] + s;
        float y = v - A[1] + s;
        float z = w - A[2] + s;
        float t = 0.6f - x * x - y * y - z * z;

        byte h = Shuffle((byte)(i + A[0]), (byte)(j + A[1]), (byte)(k + A[2]));
        A[a]++;
        if (t < 0)
            return 0;

        byte b5 = (byte)(h >> 5 & 1);
        byte b4 = (byte)(h >> 4 & 1);
        byte b3 = (byte)(h >> 3 & 1);
        byte b2 = (byte)(h >> 2 & 1);
        byte b = (byte)(h & 3);

        float p = b == 1 ? x : b == 2 ? y : z;
        float q = b == 1 ? y : b == 2 ? z : x;
        float r = b == 1 ? z : b == 2 ? x : y;

        p = (b5 == b3 ? -p : p);
        q = (b5 == b4 ? -q : q);
        r = (b5 != (b4 ^ b3) ? -r : r);
        t *= t;

        return 8f * t * t * (p + (b == 0f ? q + r : b2 == 0f ? q : r));
    }

    private static byte Shuffle(byte i, byte j, byte k)
    {
        return (byte)(b(i, j, k, 0) + b(j, k, i, 1) + b(k, i, j, 2) + b(i, j, k, 3) +
            b(j, k, i, 4) + b(k, i, j, 5) + b(i, j, k, 6) + b(j, k, i, 7));
    }

    private static byte b(byte i, byte j, byte k, byte B)
    {
        return T[b(i, B) << 2 | b(j, B) << 1 | b(k, B)];
    }

    private static byte b(byte N, byte B)
    {
        return (byte)(N >> B & 1);
    }

    
}



