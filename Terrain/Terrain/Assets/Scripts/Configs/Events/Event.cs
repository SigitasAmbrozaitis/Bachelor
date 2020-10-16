using UnityEngine;

using Delegates;

namespace DataTypes.Events
{
    [CreateAssetMenu(menuName = "DataTypes/Events/Event")]
    public class Event : ScriptableObject
    {
        private VoidDelegate m_Callback = default;

        public void Register(VoidDelegate callback)
        {
            m_Callback += callback;
        }

        public void Unregister(VoidDelegate callback)
        {
            if (m_Callback == null)
                return;

            m_Callback -= callback;
        }

        [ContextMenu("Raise")]
        public void Raise()
        {
            m_Callback?.Invoke();
        }
    }
}

