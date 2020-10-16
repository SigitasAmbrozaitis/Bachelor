using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ObjectPrefab = default;

    [SerializeField]
    private Vector3 m_Dimensions = Vector3.one;

    [SerializeField]
    private Vector3 m_DimensionSpeed = Vector3.one;

    [SerializeField]
    private float m_Range = 0f;

    private GameObject m_Parent = default;

    private GameObject[] m_Objects = default;

    public void Generate()
    {

        if (m_Parent == null)
        {
            m_Parent = new GameObject();
            m_Parent.name = "Objects";
        }
            

        Clear();
        m_Objects = new GameObject[(int)(m_Dimensions.x * m_Dimensions.y * m_Dimensions.z)];
        int index = 0;

        for (int i = 0; i < m_Dimensions.x; ++i)
        {
            for(int j = 0; j < m_Dimensions.y; ++j)
            {
                for(int k = 0; k < m_Dimensions.z; ++k)
                {
                    m_Objects[index] = GenerateObject(new Vector3(i, j, k), SimplexNoise.GetValue(i * m_DimensionSpeed.x, j * m_DimensionSpeed.y, k * m_DimensionSpeed.z));

                    ++index;
                }
            }
        }
    }

    private GameObject GenerateObject(Vector3 pos, float noise)
    {
        if (noise < m_Range)
            return null;

        var obj = GameObject.Instantiate(m_ObjectPrefab);
        obj.transform.position = pos;
        obj.transform.SetParent(m_Parent.transform);

        return obj;
    }

    private void Clear()
    {
        if (m_Objects == null)
            return;

        foreach (var item in m_Objects)
        {
            if(item != null)
            {
                DestroyImmediate(item);
            }
        }
    }
}
