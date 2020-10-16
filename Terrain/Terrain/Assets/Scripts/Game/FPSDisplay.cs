using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class FPSDisplay : MonoBehaviour
    {

        private TMPro.TextMeshProUGUI m_Text = default;
        private float m_DeltaTime = 0.0f;

        public float m_AverageTime = 10f;
        private float m_AveragePassed = default;
        private float m_FrameCount = default;
        private float m_AverageFps = 0f;

        void UpdateCumulativeMovingAverageFPS(float newFPS)
        {
            ++m_FrameCount;
            m_AveragePassed += newFPS;

            if (m_AveragePassed > m_AverageTime)
            {
                m_AverageFps = 1 / (m_AveragePassed / m_FrameCount);
                m_AveragePassed = 0f;
                m_FrameCount = 0;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
            m_Text = GetComponent<TMPro.TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCumulativeMovingAverageFPS(Time.deltaTime);

            m_DeltaTime += (Time.unscaledDeltaTime - m_DeltaTime) * 0.1f;
            float msec = m_DeltaTime * 1000.0f;
            float fps = 1.0f / m_DeltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            string avg = string.Format("({0:0.} avg)", m_AverageFps);
            text += avg;
            text += " (" + Screen.currentResolution.refreshRate + " RR)";
            m_Text.text = text;
        }
    }
}

