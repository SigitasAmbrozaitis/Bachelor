using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes.Events;
using Delegates;

namespace CPU
{
    [CreateAssetMenu(menuName ="Noise/Settings")]
    public class NoiseSettings : ScriptableObject
    {
        private VoidDelegate m_Callback = default;


        public bool m_ColoringForStrength = false;
        public bool m_Damping = false;

        public Vector2Event m_Offset = default;

        [Range(1f, 512)] public float m_Frequency = 1f;
        [Range(1, 16)] public int m_Octaves = 1;
        [Range(0f, 4f)] public float m_Strength = 1f;
        [Range(1f, 4f)] public float m_Lacunarity = 2f;
        [Range(0f, 1f)] public float m_Persistence = 0.5f;
        [Range(0f, 4f)] public float m_Amplitude = 1f;
        [Range(0, 100f)] public float m_DerivativeStrenght = 10f;

        public Gradient m_Gradient = default;

        public void Register(VoidDelegate del)
        {
            m_Callback += del;
        }

        public void Unregister(VoidDelegate del)
        {
            m_Callback -= del;
        }

        private void OnValidate()
        {
            m_Callback?.Invoke();
        }
    }

}
