using CPU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTypes.Events;
namespace CPU
{
    public class ActorControl : MonoBehaviour
    {
        [SerializeField]
        private NoiseSettings m_Settings = default;

        [SerializeField]
        private Vector3Event m_ActorPos = default;

        [SerializeField]
        private Vector2Event m_Offset = default;

        [SerializeField]
        [Range(-0.5f, 0.5f)]
        private float m_CameraOffset = default;

        [SerializeField]
        [Range(1, 250)]
        private float m_Scale = default;

        [SerializeField]
        private bool m_Cpu = default;


        private void Awake()
        {
            m_Offset.Register(OnChange);
        }

        private void OnDestroy()
        {
            m_Offset.Unregister(OnChange);
        }

        private void OnChange()
        {
            var pos = m_Offset.value;
            if(m_Cpu)
            {
                pos.y *= -1;

                pos.x += 0.5f;
                pos.y += 0.5f;
            }
            
            var noise = Noise.Sample(pos, m_Settings.m_Frequency, m_Settings.m_Octaves, m_Settings.m_Strength, m_Settings.m_Lacunarity, m_Settings.m_Persistence, m_Settings.m_Amplitude, m_Settings.m_Damping ? 1f : 0f, m_Settings.m_ColoringForStrength ? 1f : 0f, ENormal.Unity);
            m_ActorPos.value = new Vector3(0f, (noise.m_Value + m_CameraOffset) * m_Scale, 0f);
        
        }
    }
}

