using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidHelper : SinglMonoBehaviour<AndroidHelper>
{
    public static bool m_isNoAd = false;


    void Awake()
    {
        //设置gameObject 永不被摧毁
        DontDestroyOnLoad(gameObject);
        AdControl.Instance.InitRoatContrl();

    }


    public void AdNoAdCallback()
    {
        m_isNoAd = true;
        AndroidControl.Instance.NoAdCallback();
    }

    public void AdBuySucCallback()
    {
        AndroidControl.Instance.BuySucCallback();
    }
    

    public void AdCompleteCallback()
    {
        AndroidControl.Instance.CompleteCallback();
    }
    public void AdIntCompleteCallback()
    {
        AndroidControl.Instance.IntCompleteCallback();
        //AndroidControl.Instance.IntCompleteCallback = null;
    }

    public void IntAdExtraParamsCallback(string info)
    {
        AndroidControl.Instance.IntExtraParamsCompleteCallback(info);
        AndroidControl.Instance.GerExcString(info);
    }
    public void DynamicInfoCallback(string info)
    {
        AndroidControl.Instance.DynamicParamsCompleteCallback(info);
    }
    public void ConfigInfoCallback(string info)
    {
        AndroidControl.Instance.ConfigParamsCompleteCallback(info);
    }
    public void NoConfigCallback()
    {
        AndroidControl.Instance.NoUrlConfigCallback();
    }
    public void AdSkipCallback()
    {
        AndroidControl.Instance.SkipCallback();
    }

    public void AdCloseCallback()
    {
        AndroidControl.Instance.CloseCallback();
    }

    public void AdErrorCallback()
    {
        AndroidControl.Instance.ErrorCallback();
    }

    public void AdReadyCallback()
    {
        AndroidControl.Instance.ReadyCallback();
    }
    public void AdIntCloseCallback()
    {
       
    }
    //唤醒红包
    public void AdCompleteGameAwake()
    {
        AndroidControl.Instance.ComplwteAdGameAwakeCallBack();
    }
    //插屏
    public void CallBackAdMessage(string message)
    {
        AndroidControl.Instance.GerExcString(message);
        Debug.Log("执行回调" + message);
    }
    //返回版本号
    public void GetEdition(string info) 
    {
        AndroidControl.Instance.EditionCallBack(info);
    }
    //返回安卓ID
    public void GetAndroidId(string info)
    {
        AndroidControl.Instance.AdAndroidId(info);
    }


    private Dictionary<RewDynType, List<DynUseData>> dynDic = new Dictionary<RewDynType, List<DynUseData>>();

    public void GetDynString(string s)
    {
        string jsonStr = "{ \"datas\": " + s + "}";
        DynListData o = JsonUtility.FromJson<DynListData>(jsonStr);

        foreach (DynData item in o.datas)
        {
            if (!dynDic.ContainsKey((RewDynType)item.coefficientType))
            {
                dynDic.Add((RewDynType)item.coefficientType, new List<DynUseData>());
            }
            string[] p = item.ecpmInterval.Split(',');
            dynDic[(RewDynType)item.coefficientType].Add(new DynUseData(item.coefficientType, p[0].TryParseFloat(), p[1].TryParseFloat(), item.coefficientValue, item.maxValue, item.minValue));
        }
        for (int i = 1; i <= 3; i++)
        {
            if (dynDic.ContainsKey((RewDynType)i))
            {
                List<DynUseData> d = dynDic[(RewDynType)i];
                d.Sort(delegate (DynUseData p1, DynUseData p2)
                {
                    return p2.min.CompareTo(p1.min);//升序                      
                });
            }
        }
    }

    public float maxRed = 0;

    public float minRed = 0;

    public float ToRate(RewDynType t)
    {
        float rate = 1;
        if (dynDic.ContainsKey(t))
        {
            List<DynUseData> d = dynDic[t];
            for (int i = 0; i < d.Count; i++)
            {
                if (ConfigMgr.Instance.ecpm > d[i].min)
                {
                    rate = d[i].coefficientValue;
                    maxRed = d[i].maxValue;
                    minRed = d[i].minValue;
                    Debug.Log("dyn动态系数:" + rate + "----最大" + maxRed + "----最小" + minRed);
                    break;
                }
            }
        }
        else
        {
            return 1;
        } 
        return rate;
    }

}

public class DynListData
{
    public List<DynData> datas;
}


[Serializable]
public class DynData
{

    public string id;
    public string projectId;
    public int coefficientType;
    public string ecpmInterval;
    public float coefficientValue;
    public float maxValue;
    public float minValue;
    public string createdAt;
    public string updatedAt;

}

public class DynUseData
{
    public int coefficientType;
    public float min;
    public float max;
    public float coefficientValue;
    public float maxValue;
    public float minValue;

    public DynUseData(int coefficientType, float min, float max, float coefficientValue, float maxValue, float minValue)
    {
        this.coefficientType = coefficientType;
        this.min = min;
        this.max = max;
        this.coefficientValue = coefficientValue;
        this.maxValue = maxValue;
        this.minValue = minValue;
    }
}

public enum RewDynType
{
    Red = 1, Large = 2, Pig = 3
}


