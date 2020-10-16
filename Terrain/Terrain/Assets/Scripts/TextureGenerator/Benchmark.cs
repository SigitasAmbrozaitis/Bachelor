using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Benchmark : MonoBehaviour
{
    [System.Serializable]
    public struct Result
    {
        public float perlin;
        public float perlinByte;
        public float simplex;
    }

    [SerializeField]
    private BenchmarkData m_BenchmarkDataPerlin = null;

    [SerializeField]
    private BenchmarkData m_BenchmarkDataSimplex = null;

    [SerializeField]
    private BenchmarkData m_BenchmarkDataPerlinByte = null;


    [SerializeField]
    private Result m_Result = new Result();

    [SerializeField]
    private TextureGenerator m_Generator = null;

    [SerializeField]
    private bool m_BenchmarkPerlin = false;

    [SerializeField]
    private bool m_BenchmarkSimplex = false;

    [SerializeField]
    private Vector2 m_Dimension = Vector2.one;

    public void BenchmarkNoise()
    {
        if (m_BenchmarkPerlin)
        {
            
            long startTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            for (int i = 0; i < (int)(m_Dimension.x); ++i)
            {
                for (int j = 0; j < (int)(m_Dimension.y); ++j)
                {
                    var value = PerlinNoise.GetValue(i * 0.05f, j * 0.05f);
                }
            }


            long endTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long elapsed = endTime - startTime;
            m_Result.perlin = (float)(elapsed) / (float)(((int)(m_Dimension.x) * (int)(m_Dimension.y)));
            m_BenchmarkDataPerlin.Add(m_Result.perlin);

            startTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            for (int i = 0; i < (int)(m_Dimension.x); ++i)
            {
                for (int j = 0; j < (int)(m_Dimension.y); ++j)
                {
                    var value = PerlinNoiseByte.GetValue(i * 0.05f, j * 0.05f, 0f);
                }
            }


            endTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            elapsed = endTime - startTime;
            m_Result.perlinByte = (float)(elapsed) / (float)(((int)(m_Dimension.x) * (int)(m_Dimension.y)));
            m_BenchmarkDataPerlinByte.Add(m_Result.perlinByte);

        }

        if (m_BenchmarkSimplex)
        {
            long startTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            for (int i = 0; i < (int)(m_Dimension.x); ++i)
            {
                for (int j = 0; j < (int)(m_Dimension.y); ++j)
                {
                    var value = SimplexNoise.GetValue(i * 0.1f, j * 0.1f, 0f);
                }
            }

            long endTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long elapsed = endTime - startTime;
            m_Result.simplex = (float)(elapsed) / (float)(((int)(m_Dimension.x) * (int)(m_Dimension.y)));
            m_BenchmarkDataSimplex.Add(m_Result.simplex);
            Debug.Log("Simplex:" + startTime + " " + endTime + " " + elapsed);
        }



    }

    public void BenchamarkTextureGenerator()
    {
        if (m_BenchmarkSimplex)
        {
            m_Generator.usePerlin = false;
            long startTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            m_Generator.GenerateTexture();
            long endTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            m_Result.simplex = endTime - startTime;
            m_BenchmarkDataSimplex.Add(m_Result.simplex);
        }

        if (m_BenchmarkPerlin)
        {
            m_Generator.usePerlin = true;
            long startTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            m_Generator.GenerateTexture();
            long endTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            m_Result.perlin = endTime - startTime;
            m_BenchmarkDataPerlin.Add(m_Result.perlin);
        }


    }
}
