using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using DataTypes.Events;
using Unity.Entities.UniversalDelegates;

namespace CPU
{
    public enum ENormal
    {
        Analytical,
        GeometricDinamic,
        GeometricStatic,
        Unity
    }

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SurfaceCreator : MonoBehaviour
    {
        [SerializeField]
        private NoiseSettings m_Settings = default;


        [Header("Mesh Data")]
        [SerializeField]
        private Mesh m_Mesh = default;

        [SerializeField]
        [Range(1, 255)]
        private int m_Resolution = 64;

        [Space(5)]
        [Header("Noise Params")]

        [SerializeField]
        private ENormal m_NormalType = default;

        [SerializeField]
        private bool m_ColoringForStrength = false;

        [SerializeField]
        private bool m_Damping = false;

        [SerializeField]
        private bool m_DisplayNormals = default;

        [SerializeField]
        private Vector2Event m_Offset = default;

        [SerializeField]
        private Vector2 m_OffsetValue = default;

        [SerializeField]
        [Range(1f, 16)]
        private float m_Frequency = 1f;

        [SerializeField]
        [Range(1, 16)]
        private int m_Octaves = 1;

        [SerializeField]
        [Range(0f, 4f)]
        private float m_Strength = 1f;

        [SerializeField]
        [Range(1f, 4f)]
        private float m_Lacunarity = 2f;

        [SerializeField]
        [Range(0f, 1f)]
        private float m_Persistence = 0.5f;

        [SerializeField]
        [Range(0f, 4f)]
        private float m_Amplitude = 1f;

        [SerializeField]
        [Range(0, 100f)]
        private float m_DerivativeStrenght = 10f;

        [SerializeField]
        private Gradient m_Gradient = default;


        [SerializeField]
        [HideInInspector]
        private MeshFilter m_MeshFilter = default;

        private Vector3[] m_Vertices = default;
        private Vector3[] m_Normals = default;
        private Vector2[] m_Uv = default;
        private int[] m_Triangles = default;
        private Color[] m_Colors = default;
        

        private int m_PreviousResolution = default;

        private void Awake()
        {
            ResizeMesh(m_Resolution);
            m_Settings.Register(OnSettingsChanged);
            m_Offset?.Register(Refresh);
            OnSettingsChanged();
        }

        private void OnSettingsChanged()
        {
            m_ColoringForStrength = m_Settings.m_ColoringForStrength;
            m_Damping = m_Settings.m_Damping;

            m_Offset.Unregister(Refresh);
            m_Offset = m_Settings.m_Offset;
            m_Offset.Register(Refresh);

            m_Strength = m_Settings.m_Strength;
            m_Frequency = m_Settings.m_Frequency;
            m_Lacunarity = m_Settings.m_Lacunarity;
            m_Persistence = m_Settings.m_Persistence;
            m_Amplitude = m_Settings.m_Amplitude;
            m_Octaves = m_Settings.m_Octaves;
            m_Gradient = m_Settings.m_Gradient;

            Refresh();
        }

        private void OnDestroy()
        {
            m_Settings.Unregister(OnSettingsChanged);
            m_Offset?.Unregister(Refresh);
        }

        [ContextMenu("Refresh")]
        private void Refresh()
        {
            Debug.Log("Refreshing");
            Noise.m_Max = float.MinValue;
            Noise.m_Min = float.MaxValue;

            Noise.m_DerivativeCalculationDuration = 0;
            Noise.m_GeometricalDerivativeDuration = 0;
            Noise.m_NoiseCalculationDuration = 0;


            //Vector2 point00 = new Vector2(-0.5f, -0.5f);
            //Vector2 point10 = new Vector2(0.5f, -0.5f);
            //Vector2 point01 = new Vector2(-0.5f, 0.5f);
            //Vector2 point11 = new Vector2(0.5f, 0.5f);
            float stepSize = 1f / m_Resolution;

            for (int v = 0, y = 0; y <= m_Resolution; y++)
            {
                
                //Vector2 point0 = Vector2.Lerp(point00, point01, y * stepSize);
                //Vector2 point1 = Vector2.Lerp(point10, point11, y * stepSize);
                for (int x = 0; x <= m_Resolution; x++, v++)
                {
                    var point = new Vector2(x * stepSize + m_OffsetValue.x, y * stepSize - m_OffsetValue.y);
                    //Vector2 point = Vector2.Lerp(point0, point1, x * stepSize);
                    NoiseSample sample = Noise.Sample(point, m_Frequency, m_Octaves, m_Strength, m_Lacunarity, m_Persistence, m_Amplitude, m_Damping ? 1f:0f, m_ColoringForStrength ? 1f : 0f, m_NormalType);
                    
                    m_Colors[v] = m_Gradient.Evaluate(sample.m_ColorValue);
                    m_Vertices[v].y = sample.m_Value;

                    //if(x == )

                    if(m_NormalType == ENormal.Analytical || m_NormalType == ENormal.GeometricDinamic)
                        m_Normals[v] = new Vector3(-sample.m_Derivative.x * m_DerivativeStrenght, 1f, -sample.m_Derivative.y * m_DerivativeStrenght).normalized;//sample.m_Derivative.normalized;
                }
            }


            m_Mesh.Clear();
            m_Mesh.vertices = m_Vertices;
            m_Mesh.triangles = m_Triangles;
            m_Mesh.uv = m_Uv;

            if (m_NormalType == ENormal.GeometricStatic)
            {
                RecalculateNormals();
            }
            m_Mesh.normals = m_Normals;
            m_Mesh.colors = m_Colors;
            if(m_NormalType == ENormal.Unity)
            {
                m_Mesh.RecalculateNormals();
                m_Mesh.RecalculateTangents();
            }

            Debug.Log(string.Format("Noise:{0} GeometricDinamic:{1} Analytical:{2}", Noise.m_NoiseCalculationDuration, Noise.m_GeometricalDerivativeDuration, Noise.m_DerivativeCalculationDuration));
        }

        private void OnValidate()
        {
            if (m_MeshFilter == null)
                m_MeshFilter = GetComponent<MeshFilter>();

            if (m_Mesh == null)
            {
                m_Mesh = new Mesh();
                m_Mesh.name = "Generated Mesh";
                m_MeshFilter.mesh = m_Mesh;
            }

            if (m_PreviousResolution != m_Resolution)
            {
                ResizeMesh(m_Resolution);
                m_PreviousResolution = m_Resolution;
            }


            Refresh();
        }

        private void Update()
        {

        }


        private void ResizeMesh(int size)
        {
            //get vertices/uv/normals
            var vertexCount = (size + 1) * (size + 1);

            m_Vertices = new Vector3[vertexCount];
            m_Normals = new Vector3[vertexCount];
            m_Uv = new Vector2[vertexCount];
            m_Colors = new Color[vertexCount];
            m_Triangles = new int[m_Resolution * m_Resolution * 6];

            float stepSize = 1f / size;

            for (int v = 0, z = 0; z <= size; z++)
            {
                for (int x = 0; x <= size; x++, v++)
                {
                    m_Vertices[v] = new Vector3(x * stepSize - 0.5f, 0f , z * stepSize - 0.5f);
                    m_Uv[v] = new Vector2(x * stepSize, z * stepSize);
                    m_Normals[v] = Vector3.up;
                    m_Colors[v] = Color.white;
                }
            }

            for (int t = 0, v = 0, y = 0; y < size; y++, v++)
            {
                for (int x = 0; x < size; x++, v++, t += 6)
                {
                    m_Triangles[t] = v;
                    m_Triangles[t + 1] = v + size + 1;
                    m_Triangles[t + 2] = v + 1;
                    m_Triangles[t + 3] = v + 1;
                    m_Triangles[t + 4] = v + size + 1;
                    m_Triangles[t + 5] = v + size + 2;
                }
            }
            
            m_Mesh.vertices = m_Vertices;
            m_Mesh.triangles = m_Triangles;
            m_Mesh.uv = m_Uv;
            m_Mesh.normals = m_Normals;
            m_Mesh.colors = m_Colors;
            
        }

        private void OnDrawGizmosSelected()
        {
            if (m_DisplayNormals && m_Vertices != null)
            {
                Gizmos.color = Color.yellow;
                for (int v = 0; v < m_Vertices.Length; v++)
                {
                    var scaledVer = Vector3.Scale(m_Vertices[v], transform.localScale);
                    var pos = scaledVer + transform.position;
                    //var scaledPos = Vector3.Scale(pos, transform.localScale);
                    Gizmos.DrawRay(pos, m_Normals[v]);
                }
            }
        }

        private void RecalculateNormals()
        {
            long start = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            for (int v = 0, z = 0; z <= m_Resolution; z++)
            {
                for (int x = 0; x <= m_Resolution; x++, v++)
                {
                    var dx = GetXDerivative(x, z, m_Resolution);
                    var dz = GetZDerivative(x, z, m_Resolution);
                    m_Normals[v] = new Vector3(-dx, 1f, -dz).normalized;

                    //Debug.Log(string.Format("dx:{0} dz:{1} normal:{2} normal.x:{3} normal.z:{4}", dx, dz, m_Normals[v].ToString(), m_Normals[v].x, m_Normals[v].z));
    
                }
            }
            var duration = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - start;
            Debug.Log("Normal Duration:" + duration);
        }

        private float GetXDerivative(int x, int z, int resolution)
        {
            int index = (int)(z * (resolution + 1f) + x);
            int indexLeftAddition = x > 0 ? -1 : 0;
            int indexRightAddition = x < resolution ? 1 : 0;
            float scale = x <= 0 || x >= resolution ? 1f : 0.5f;

            float left = m_Vertices[index + indexLeftAddition].y;
            float right = m_Vertices[index + indexRightAddition].y;

            return (right - left) * scale * m_DerivativeStrenght * 40f;
        }

        private float GetZDerivative(int x, int z, int resolution)
        {
            int indexBackwardAddition = z > 0f ? -1 : 0;
            int indexForwardAddition = z < resolution ? 1 : 0;
            float scale = z <= 0f || z >= resolution ? 1f : 0.5f;

            float forward = m_Vertices[(int)((z + indexForwardAddition) * (resolution +1) + x)].y;
            float back = m_Vertices[(int)((z + indexBackwardAddition) * (resolution +1) + x)].y;

            return (forward - back) * scale * m_DerivativeStrenght * 40f;
        }
    }
}

