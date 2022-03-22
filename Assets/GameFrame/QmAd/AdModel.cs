using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdModel 
{
    // 红包接口开关
    private bool m_IsRedEnvelope = false;
    // 渠道买量版本开关
    private bool m_IsChannel = false;
    private bool m_IsLoad = false;

    private string m_SDKInfo = "";

  
    public bool IsLoad { get => m_IsLoad; set => m_IsLoad = value; }
    public bool IsRedEnvelope { get => m_IsRedEnvelope; set => m_IsRedEnvelope = value; }
    public bool IsChannel { get => m_IsChannel; set => m_IsChannel = value; }
    public string SDKInfo { get => m_SDKInfo; set => m_SDKInfo = value; }
}
