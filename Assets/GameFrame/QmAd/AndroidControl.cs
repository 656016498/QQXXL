using System.Collections;
using System.Collections.Generic;
using UniRx;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

using UnityEngine;
using Util;

public class AndroidControl : Singleton<AndroidControl>
{
#if UNITY_IOS

    [DllImport("__Internal")]
    public static extern void InvokeShowBanner();
    [DllImport("__Internal")]
    public static extern void InvokeHideBanner();
    [DllImport("__Internal")]
    public static extern void InvokeShowInterstitial(string positionName);
    [DllImport("__Internal")]
    public static extern void InvokeShowRewardVideo(string positionName);
    [DllImport("__Internal")]
    public static extern void UMOnEvent(string id);
    [DllImport("__Internal")]
    public static extern void UMOnEventMore(string id,string num);
    [DllImport("__Internal")]
    public static extern void InvokeShowMessage();
    [DllImport("__Internal")]
    public static extern void InvokeHideMessage();
    [DllImport("__Internal")]
    public static extern string GetExtraParams(string id);
#endif


    bool m_IsInit = false;

    public delegate void Callback();
    public delegate void Callback2(string info);
    public Callback AdBuySucCallback = null;
    public Callback AdNoAdCallback = null;
    public Callback AdCompleteCallback = null;
    public Callback AdIntCompleteCallback = null;
    public Callback AdSkipCallback = null;
    public Callback AdCloseCallback = null;
    public Callback AdErrorCallback = null;
    public Callback AdReadyCallback = null;
    public Callback2 AdIntExtraParamsCallback = null;
    public Callback2 AdAndroidId = null;
    public Callback2 DynamicInfoCallback = null;
    public Callback2 ConifgInfoCallback = null;
    public Callback NoConfigCallback = null;
    public Callback2 EditionCallBack = null;//版本号
    //新增
    public string serverMessage;
    public Callback AdSetServerMessageback = null;
    public Callback AdSetServerMessagebackError = null;

#pragma warning disable CS0114 // 成员隐藏继承的成员；缺少关键字 override
    public void Init()
#pragma warning restore CS0114 // 成员隐藏继承的成员；缺少关键字 override
    {
        if (!m_IsInit)
        {
            m_IsInit = true;
        }
    }
    public void ShowBanner()
    {
        if (Application.platform == RuntimePlatform.Android)
        { 
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("showBanner");

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokeShowBanner();
#endif
        }
    }
    public void CallUmengOnEvent(string id)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("umengOnEvent", id);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            UMOnEvent(id);
#endif
        }
    }
    public void CallUmengOnEvent(string id, string value)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("umengOnEvent", id, value);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            UMOnEventMore(id,value);
#endif
        }
    }

    public void CallAndroidFunc(string funcName,int x,int y)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName, x, y);
        }
    }
    public void CallAndroidFunc(string funcName, string para, Callback2 ready)
    {
        //AdCompleteCallback = null;
        AdErrorCallback = null;
        AdAndroidId = ready;

#if UNITY_EDITOR
        AdAndroidId("159357258");
#endif

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName);
            
        }
        else
        {
           
        }
    }
    public void CallAndroidEditionFunc(string funcName, string para, Callback2 ready) 
    {
        AdErrorCallback = null;
        //版本号
        EditionCallBack = ready;

#if UNITY_EDITOR
        EditionCallBack("0.1");
#endif

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName);
          
        }
        else
        {
            //CompleteCallback();
        }
    }
    public void CallAndroidFunc(string funcName , string para, int x, int y)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (para.Length <= 0)
            {
                jo.Call(funcName);
            }
            else
            {
                jo.Call(funcName, para, x, y);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokeShowMessage();
#endif
        }
        else
        {
            //CompleteCallback();
        }
    }

    /// <summary>
    /// 激励视频调用安卓ios接口
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="para"></param>
    /// <param name="complete"></param>
    /// <param name="close"></param>
    /// <param name="error"></param>
    public void CallAndroidFunc(string funcName, string para, Callback complete, Callback close, Callback error)
    {
        AdCompleteCallback = complete;
        AdCloseCallback = close;
        AdErrorCallback = error;
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (para.Length <= 0)
            {
                jo.Call(funcName);
            }
            else
            {
                jo.Call(funcName, para);
            }

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
                    InvokeShowRewardVideo(para);
#endif
        }
        else
        {
            CompleteCallback();
        }
    }

    /// <summary>
    /// 插屏开屏调用安卓ios接口
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="para"></param>
    /// <param name="complete"></param>
    /// <param name="error"></param>
    public void CallAndroidFunc(string funcName, string para, Callback complete, Callback error)
    {
        AdIntCompleteCallback = complete;
        AdErrorCallback = error;
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (para.Length <= 0)
            {
                jo.Call(funcName);
            }
            else
            {
                jo.Call(funcName, para);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokeShowInterstitial(para);
#endif
        }
        else
        {
            IntCompleteCallback();
        }
    }

    public void CallAndroidFunc(string funcName, string para, Callback ready)
    {
        //AdCompleteCallback = null;
        AdErrorCallback = null;
        AdReadyCallback = ready;

#if UNITY_EDITOR
        AdReadyCallback();
#endif

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (para.Length <= 0)
            {
                jo.Call(funcName);
            }
            else
            {
                jo.Call(funcName, para);
            }
        }
        else
        {
            //CompleteCallback();
        }
    }

    public void CallAndroidStartMissionFunc(string funcName, string para)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName, para);
        }
    }

    public void CallAndroidEndMissionFunc(string funcName, string para, bool isWin, string failPara)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName, para, isWin, failPara);
        }
    }

    public void CallAndroidUseToolsFunc(string funcName, string para)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName, para);
        }
    }

    public void CallAndroidNoAdFunc(string funcName, string para, Callback noad)
    {
        AdNoAdCallback = noad;

#if UNITY_EDITOR
        AdNoAdCallback();
#endif

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (para.Length <= 0)
            {
                jo.Call(funcName);
            }
            else
            {
                jo.Call(funcName, para);
            }
        }
        else
        {
            AdNoAdCallback();
        }
    }

    public void CallAndroidBuyFunc(string funcName, string para, Callback noad)
    {
        AdBuySucCallback = noad;

#if UNITY_EDITOR
        BuySucCallback();
        return;
#endif

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (para.Length <= 0)
            {
                jo.Call(funcName);
            }
            else
            {
                jo.Call(funcName, para);
            }
        }
        else
        {
            AdBuySucCallback();
        }
    }

    public void CompleteCallback()
    {
        //Debug.Log("AndroidControl call succ");
        //MainGameMgr.Instance.mMainGame.AddOneLookVideos();
        ConfigMgr.Instance.UpdateUserValue();
        AdCompleteCallback?.Invoke();
        Debug.Log("AdCompleteCallbackInvoke");
        //增加转盘精度条
        LotteryDataManger.Instance.AddFillPro(ConfigMgr.Instance.LotreyFillPro());
        AdCompleteCallback = null;
    }

    public void IntCompleteCallback()
    {
        Debug.Log("AndroidControl call succ");
        AdIntCompleteCallback?.Invoke();
        AdIntCompleteCallback= null;
    }

    public void SkipCallback()
    {
        Debug.Log("AndroidControl call Fail");
        if (AdSkipCallback != null)
        {
            AdSkipCallback();
        }
    }

    public void ErrorCallback()
    {
        Debug.Log("AndroidControl call Fail");
        if (AdErrorCallback != null)
        {
            AdErrorCallback();
        }
    }

    public void CloseCallback()
    {
        if (AdCloseCallback != null)
        {
            AdCloseCallback();
        }
    }

    public void ReadyCallback()
    {
        if (AdReadyCallback != null)
        {
            AdReadyCallback();
        }
    }

    public void BuySucCallback()
    {
        if (AdBuySucCallback != null)
        {
            AdBuySucCallback();
        }
    }

    public void NoAdCallback()
    {
        if (AdNoAdCallback != null)
        {
            AdNoAdCallback();
        }
        //UiManager.Instance.ShowWind("NoADView");
    }
    public void NoUrlConfigCallback()
    {
        if (NoConfigCallback != null)
        {
            NoConfigCallback();
        }
    }
    public void IntExtraParamsCompleteCallback(string info)
    {
        Debug.Log("AndroidControl call succ");
        if (AdIntExtraParamsCallback != null)
        {
            AdIntExtraParamsCallback(info);
        }
    }
    public void DynamicParamsCompleteCallback(string info)
    {
        
        if (DynamicInfoCallback != null)
        {
            DynamicInfoCallback(info);
            Debug.Log("DynamicInfoAndroid"+ info);
        }
    }
    public void ConfigParamsCompleteCallback(string info)
    {

        if (ConifgInfoCallback != null)
        {
            ConifgInfoCallback(info);
        }
    }
    public void PlayShock(int val)
    {     
#if !UNITY_EDITOR
        long[] shock = new long[] { 0, val };
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("StartShock", shock, -1);
        }
#endif
    }

    public void CallAndroidFunc(string funcName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName);
        }

    }
    public void HideMessageAd(string funcName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokeHideMessage();
#endif
        }

    }
    public void HideBanner(string funcName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokeHideBanner();
#endif
        }

    }

    /// <summary>
    /// UM统计调用安卓ios
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="para"></param>
    /// <param name="parb"></param>
    public void CallAndroidOnEvent(string funcName,string para,string parb = null)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (parb == null||parb.Equals(""))
            {
                jo.Call(funcName, para);
            }
            else
            {
                jo.Call(funcName, para, parb);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            if (parb == null || parb.Equals(""))
            {
                UMOnEvent(para);
            }
            else
            {
                UMOnEventMore(para,parb);
            }
#endif
        }
    }
    public void CallAndroidExtraParams(string funcName, string para, Callback2 succ)
    {
        AdIntExtraParamsCallback = succ;

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (para.Length <= 0)
            {
                jo.Call(funcName);
            }
            else
            {
                jo.Call(funcName, para);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            succ(GetExtraParams(para));
#endif
        }
    }
    public void CallAndroidDynamicParams(string funcName, Callback2 succ)
    {
        DynamicInfoCallback = succ;
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call(funcName);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            succ(GetExtraParams(para));
#endif
        }
        else
        {
            succ("10:10:10");
        }
    }

    public void CallAndroidConfigParams(string funcName, Callback2 succ)
    {
        ConifgInfoCallback = succ;

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

            jo.Call(funcName);

        }
       
    }

    public void CallConfigUrlParams(string funcName, Callback2 succ,Callback fail)
    {
        ConifgInfoCallback = succ;
        NoConfigCallback = fail;
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

            jo.Call(funcName);

        }
        else
        {
            fail();
            //succ("http://192.168.1.76:58793");
        }

    }
    
    public void CallMyOnEvent(string levelID, int type, int completeTime)
    {
       
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("myOnEvent", levelID, type, completeTime);
        }
    }
    public void CallOnLogin()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("myLogin");
        }
    }
    public void CallRYTime(long time)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("ryGameTime", time);
        }
    }
    /// <summary>
    /// 11.2新增
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="para"></param>
    /// <param name="complete"></param>
    /// <param name="error"></param>
    public void CallGetMessage(string funcName, string para, Callback complete, Callback error)
    {

        AdSetServerMessageback = complete;
        AdSetServerMessagebackError = error;
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

            jo.Call(funcName, para);

        }
        else
        {
            CompleteCallback();
        }
    }
    //获取回馈信息
    public void GerExcString(string message)
    {
        Debug.Log(message + "??????" + AdSetServerMessageback);
        if (message != string.Empty && AdSetServerMessageback != null)
        {
            serverMessage = message;
            // MainMenuView.m_this.ServerMessage = message;
            //AdSetServerMessageback();
            AdSetServerMessageback?.Invoke();
            AdSetServerMessageback = null;
            Debug.Log("回调执行完毕" + serverMessage);
        }
        else if (message == string.Empty)
        {
            Debug.Log("没有取到数据 执行弹广告");
            //AdSetServerMessagebackError();
            AdSetServerMessagebackError?.Invoke();
            AdSetServerMessagebackError = null;
            Debug.Log("没有取到数据 执行结束");
        }
    }
    //唤醒红包回调
    public void ComplwteAdGameAwakeCallBack()
    {
        //var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
        //var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
        var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
        popup1.OnOpen("game_awaken_video", 0, () => {

            Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ =>
            {
            var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
            var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
            //打开回调
            var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
            popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
            {
                if (!GameManager.Instance.isCash)
                {
                    RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                }
                popup2.effect.SetActive(false);
                    Debug.Log("关闭红包二级界面");
            });
            popup1.defult.SetActive(false);
         });
        },
        () =>
        {
            //关闭回调
            Debug.Log("关闭红包一级界面");
            popup1.defult.SetActive(false);
        });

        Debug.Log("打开唤醒红包");
    }
}
