using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPU
{
    public class PlaneGenerator : MonoBehaviour
    {
        [SerializeField]
        private Mesh m_Mesh = default;

        [SerializeField]
        [Range(2, 512)]
        private int m_Resolution = 2;

        [SerializeField]
        [HideInInspector]
        private MeshFilter m_MeshFilter = default;

        [ContextMenu("Generate")]
        private void Generate()
        {
            var vertexCount = (m_Resolution + 1) * (m_Resolution + 1);
            var vertices = new Vector3[vertexCount];
            var uv = new Vector2[vertexCount];
            var triangles = new int[m_Resolution * m_Resolution * 6];

            for (int v = 0, z = 0; z <= m_Resolution; z++)
            {
                for (int x = 0; x <= m_Resolution; x++, v++)
                {
                    vertices[v] = new Vector3(x/ m_Resolution, 0f, z/ m_Resolution);
                    uv[v] = new Vector2(x/m_Resolution, z/m_Resolution);
                }
            }

            for (int t = 0, v = 0, y = 0; y < m_Resolution; y++, v++)
            {
                for (int x = 0; x < m_Resolution; x++, v++, t += 6)
                {
                    triangles[t] = v;
                    triangles[t + 1] = v + m_Resolution + 1;
                    triangles[t + 2] = v + 1;
                    triangles[t + 3] = v + 1;
                    triangles[t + 4] = v + m_Resolution + 1;
                    triangles[t + 5] = v + m_Resolution + 2;
                }
            }

            if (m_MeshFilter == null)
                m_MeshFilter = GetComponent<MeshFilter>();

            if (m_Mesh == null)
            {
                m_Mesh = new Mesh();
                m_Mesh.name = "Generated Mesh";
                m_MeshFilter.mesh = m_Mesh;
            }

            m_Mesh.Clear();
            m_Mesh.vertices = vertices;
            m_Mesh.triangles = triangles;
            m_Mesh.uv = uv;
            m_Mesh.MarkModified();

        }
    }
}

