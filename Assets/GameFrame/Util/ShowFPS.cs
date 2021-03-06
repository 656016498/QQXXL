using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour 
{
    public float Update_Interval = 0.5F;
    private float m_lastInterval;
    private int m_frames = 0;
    private float m_fps;

    void Start()
    {
        m_lastInterval = Time.realtimeSinceStartup;
        m_frames = 0;
        Application.targetFrameRate = 60;
    }
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.normal.textColor = new Color(1, 0, 0);   //设置字体颜色
        fontStyle.fontSize = 40;       //字体大小

        GUILayout.Label("FPS:" + m_fps.ToString("f2"), fontStyle);
    }
    void Update()
    {
        ++m_frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > m_lastInterval + Update_Interval)
        {
            m_fps = m_frames / (timeNow - m_lastInterval);
            m_frames = 0;
            m_lastInterval = timeNow;
        }
    }
}
