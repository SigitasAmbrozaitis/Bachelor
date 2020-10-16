using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BenchmarkData")]
public class BenchmarkData : ScriptableObject
{
    [SerializeField]
    private List<float> m_Data = new List<float>();

    [SerializeField]
    private float m_Average = 0f;

    public void Add(float value)
    {
        m_Data.Add(value);

        var sum = 0f;
        foreach (var val in m_Data)
            sum += val;

        m_Average = sum / m_Data.Count;
    }
}
