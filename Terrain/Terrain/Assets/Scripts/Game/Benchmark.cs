using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataTypes.Events;
namespace Game
{
    public class Benchmark : MonoBehaviour
    {
        private delegate float NoiseDelegate(Vector2 pos);

        private enum EBenchmarkType
        {
            Perlin,
            PerlinOptimised
        }

        private static NoiseDelegate[] l_Method =
        {
            PerlinNoise.GetValue,
            CPU.Noise.GetValue
        };

        [SerializeField]
        private LongEvent m_TotalTime;

        [SerializeField]
        private long m_IterationCount;

        [SerializeField]
        private EBenchmarkType m_MethodToBenchmark;

        [ContextMenu("Becnhmark")]
        private void RunBenchmark()
        {
            var method = l_Method[(int)m_MethodToBenchmark];
            long start = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var sum = 0f;
            for (int i = 0; i < m_IterationCount; ++i)
            {
                sum += method(new Vector3(i, -i, 0f));
            }
            m_TotalTime.value = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - start;
        }
    }
}
