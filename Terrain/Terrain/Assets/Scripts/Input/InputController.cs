using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DataTypes.Events;
using Unity.Mathematics;


namespace Game
{
    public class InputController : MonoBehaviour
    {
        private static float3 m_VecW = new float3(0f, 0f, 1f);
        private static float3 m_VecS = new float3(0f, 0f, -1f);
        private static float3 m_VecA = new float3(-1f, 0f, 0f);
        private static float3 m_VecD = new float3(1f, 0f, 0f);


        [Header("Result Events")]
        [SerializeField]
        private Vector2Event m_OffsetEvent = default;

        [SerializeField]
        private Vector3Event m_RotationEvent = default;

        [Space(5)]
        [Header("Settings")]


        [SerializeField]
        [Range(0f, 100f)]
        private float m_RotationHorizontalSpeed = default;

        [SerializeField]
        [Range(0f, 100f)]
        private float m_RotationVerticalSpeed = default;

        [SerializeField]
        [Range(-10f, 10f)]
        private float m_OffsetForwardSpeed = default;

        [SerializeField]
        [Range(-10f, 10f)]
        private float m_OffsetSidewaysSpeed = default;

        [SerializeField]
        [Range(-1f, 1f)]
        private float m_GlobalSpeed = default;

        private void Update()
        {
            UpdateRotation();
            UpdatePosition();

        }

        private void UpdateRotation()
        {
            float x = m_RotationHorizontalSpeed * Input.GetAxis("Mouse X");
            float y = -m_RotationVerticalSpeed * Input.GetAxis("Mouse Y");

            float3 previousValue = m_RotationEvent.value;

            m_RotationEvent.value = new Vector3((previousValue.x + y) % 360f, (previousValue.y + x) % 360f, 0f);
        }

        private void UpdatePosition()
        {
            var forwardSpeed = m_OffsetForwardSpeed * Time.deltaTime * m_GlobalSpeed;
            var sidewaysSpeed = m_OffsetSidewaysSpeed * Time.deltaTime * m_GlobalSpeed;

            float3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                direction += m_VecW;

            if (Input.GetKey(KeyCode.S))
                direction += m_VecS;

            if (Input.GetKey(KeyCode.A))
                direction += m_VecA;

            if (Input.GetKey(KeyCode.D))
                direction += m_VecD;

            direction = math.normalizesafe(direction);
            direction.z *= forwardSpeed;
            direction.x *= sidewaysSpeed;

            var yRot = math.radians(m_RotationEvent.value.y);

            var sinY = math.sin(yRot);
            var cosY = math.cos(yRot);

            var x = cosY * direction.x - sinY * direction.z;
            var z = sinY * direction.x + cosY * direction.z;

            var rotated = new float2(x, z);
            m_OffsetEvent.value = (float2)m_OffsetEvent.value + rotated;

            Debug.Log(string.Format("x:{0} z:{1}", x, z));
            //if(Input.GetButton("Horizontal"))
            //    x = Input.GetAxisRaw("Horizontal") * m_OffsetSidewaysSpeed * Time.deltaTime * m_GlobalSpeed;
            //if (Input.GetButton("Vertical"))
            //    z = Input.GetAxisRaw("Vertical") * m_OffsetForwardSpeed * Time.deltaTime * m_GlobalSpeed;

            //var sidewayVector = x < 0 ? m_ControlledObject.right : m_ControlledObject.right;
            //var forwardVector = z < 0 ? m_ControlledObject.forward : m_ControlledObject.forward;

            //Debug.Log("Forward:" + m_ControlledObject.forward);

            //var move = sidewayVector * x + forwardVector * z;
            //m_OffsetEvent.value += move;
        }

    }
}

