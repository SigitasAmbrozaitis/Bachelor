using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPU
{
    public class NoiseMaterialLink : MonoBehaviour
    {
        [SerializeField]
        private NoiseSettings m_Settings = default;

        [SerializeField]
        private Material m_NoiseMaterial = default;

        private void Awake()
        {
            m_Settings.Register(SetMaterial);
            m_Settings.m_Offset.Register(SetOffset);
            SetMaterial();
            SetOffset();
        }

        private void OnDestroy()
        {
            m_Settings.Unregister(SetMaterial);
            m_Settings.m_Offset.Unregister(SetOffset);
        }

        private void SetMaterial()
        {
            m_NoiseMaterial.SetFloat("_Frequency", m_Settings.m_Frequency);
            m_NoiseMaterial.SetFloat("_Octaves", m_Settings.m_Octaves);
            m_NoiseMaterial.SetFloat("_Strength", m_Settings.m_Strength);
            m_NoiseMaterial.SetFloat("_Lacunarity", m_Settings.m_Lacunarity);
            m_NoiseMaterial.SetFloat("_Persistance", m_Settings.m_Persistence);
            m_NoiseMaterial.SetFloat("_Amplitude", m_Settings.m_Amplitude);
            m_NoiseMaterial.SetFloat("_StrenghtColoring", m_Settings.m_ColoringForStrength ? 1f: 0f);
            m_NoiseMaterial.SetFloat("_Damping", m_Settings.m_Damping ? 1f: 0f);

            SetOffset();
        }

        private void SetOffset()
        {
            m_NoiseMaterial.SetFloat("_OffsetX", m_Settings.m_Offset.value.x);
            m_NoiseMaterial.SetFloat("_OffsetY", m_Settings.m_Offset.value.y);
        }
    }

}
