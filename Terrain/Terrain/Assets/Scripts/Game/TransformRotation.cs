using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataTypes.Events;
using Unity.Mathematics;

namespace Game
{
    public class TransformRotation : MonoBehaviour
    {
        [SerializeField]
        private Vector3Event m_Rotation = default;

        private void Awake()
        {
            //m_OriginalRotation = transform.localRotation;
            m_Rotation?.Register(OnRotationChange);
            OnRotationChange();
        }

        private void OnDestroy()
        {
            m_Rotation?.Unregister(OnRotationChange);
        }

        private void OnRotationChange()
        {
            var rot = m_Rotation.value;
            rot.x = math.clamp(rot.x, -5f, 5f);
            transform.localRotation = Quaternion.Euler(rot);
        }
    }
}
