using UnityEngine;
using Unity.Mathematics;

using DataTypes.Events;
namespace Game
{
    public class TransformPosition : MonoBehaviour
    {
        [SerializeField]
        private Vector3Event m_PositionEvent = default;

        private void Awake()
        {
            m_PositionEvent?.Register(OnPosChange);
            OnPosChange();
        }

        private void OnDestroy()
        {
            m_PositionEvent?.Unregister(OnPosChange);
        }

        private void OnPosChange()
        {
            transform.position = m_PositionEvent.value;
        }
    }

}
