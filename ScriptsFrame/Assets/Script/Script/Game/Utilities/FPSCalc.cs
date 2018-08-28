using UnityEngine;
using System.Collections;

namespace DevelopEngine
{
	/// <summary>
	/// FPS calculate.
	/// </summary>
	public class FPSCalc : MonoSingleton<FPSCalc> 
	{
        public float fpsMeasuringDelta = 2.0f;

        private float timePassed;
        private int m_FrameCount = 0;
        private float m_FPS = 0.0f;
        private bool isShowFps;

        GUIStyle bb;

        private void Start()
        {
            isShowFps = bool.Parse(Configs.Instance.LoadText("显示帧率", "showFps")); 
            timePassed = 0.0f;
            bb = new GUIStyle();
            bb.normal.background = null;    //这是设置背景填充的
            bb.normal.textColor = new Color(1.0f, 0.5f, 0.0f);   //设置字体颜色的
            bb.fontSize = 200;       //当然，这是字体大小
        }

        private void Update()
        {
            if (!isShowFps)
                return;
            m_FrameCount = m_FrameCount + 1;
            timePassed = timePassed + Time.deltaTime;

            if (timePassed > fpsMeasuringDelta)
            {
                m_FPS = m_FrameCount / timePassed;

                timePassed = 0.0f;
                m_FrameCount = 0;
            }
        }

        private void OnGUI()
        {
            if (isShowFps)
            {
                //居中显示FPS
                GUI.Label(new Rect(100, 100, 1000, 1000), "FPS: " + m_FPS, bb);
            }
        }
    }
}
