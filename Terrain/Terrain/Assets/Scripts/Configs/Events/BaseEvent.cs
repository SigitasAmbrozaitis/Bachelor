using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace DataTypes.Events
{
    public class BaseEvent<T> : Event, ISerializationCallbackReceiver
    {

        [SerializeField]
        private T m_RuntimeValue = default;

        [SerializeField]
        private T m_EditTimeValue = default;


        private T m_PreviousValue = default;

        public T value
        {
            get => m_RuntimeValue;
            set
            {
                m_RuntimeValue = value;
                Raise();
            }
        }

        public T previousValue
        {
            get => m_PreviousValue;
        }

        public void OnAfterDeserialize()
        {
            
        }

        public void OnBeforeSerialize()
        {
            if (!Application.isPlaying)
                m_RuntimeValue = m_EditTimeValue;
        }

#if UNITY_EDITOR

        //[SerializeField]
        //private bool m_LiveRaise = default;
        private void OnValidate()
        {
            //if (m_LiveRaise)
            //    value = Application.isPlaying ? m_RuntimeValue : m_EditTimeValue;
        }
#endif

    }
}

