using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class AdControl : Util.Singleton<AdControl>
{
    public bool canAwake = false;
    public bool isShowAd = true;
    public void SendEvent(string id)
    {
        if (isShowAd)
        {
            AndroidControl.Instance.CallUmengOnEvent(id);
        }
        UMCheck.SaveUmeng(id);
    }
    public void SendEvent(string id, string value)
    {
        if (isShowAd)
        {
            AndroidControl.Instance.CallUmengOnEvent(id, value);
        }
        UMCheck.SaveUmeng(id, value);
    }
    /// <summary>
    /// 设置信息流宽高
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetMessageAdWithDP(int x, int y)
    {
        if (isShowAd)
        {
            AndroidControl.Instance.CallAndroidFunc("setMessageAdSizeDP", x, y);
        }
    }

    /// <summary>
    /// 展示插屏广告
    /// </summary>
    /// <param name="id">插屏广告id</param>
    /// <param name="succ">展示成功回调</param>
    /// <param name="fail">展示失败回调</param>
    public void ShowIntAd(string id, AndroidControl.Callback succ = null, AndroidControl.Callback fail = null)
    {
        if (isShowAd)
        {
            canAwake = false;
            AndroidControl.Instance.CallAndroidFunc("showIntAd", id, succ, fail);
            Debug.Log("播放插屏：" + id);
        }
        else
        {
            if (succ != null)
            {
                succ();
            }
        }

    }

    /// <summary>
    /// 展示激励视频广告
    /// </summary>
    /// <param name="id">激励视频广告id</param>
    /// <param name="complete">激励视频看完成功回调// 发放奖励</param>
    /// <param name="close">关闭回调</param>
    /// <param name="fail">失败回调</param>
    public void ShowRwAd(string id, AndroidControl.Callback complete, AndroidControl.Callback close = null, AndroidControl.Callback fail = null)
    {
        XDebug.Log("广告:视频:" + id);
        if (isShowAd)
        {
            canAwake = false;
            AndroidControl.Instance.CallAndroidFunc("showRwAd", id, complete, close, fail);
            //if (MyGameInfo.Instance.IsPB != 0)
            //{
            //    MyGameInfo.Instance.PBVideoNum += 1;
            //    SendEvent("new_break_video", MyGameInfo.Instance.PBVideoNum.ToString());
            //}
        }
        else
        {
            if (complete != null)
            {
                complete();
                //统一添加转盘进度
                LotteryDataManger.Instance.AddFillPro(ConfigMgr.Instance.LotreyFillPro());
            }
        }

    }

    /// <summary>
    /// 开屏广告
    /// </summary>
    /// <param name="id"></param>
    public void ShowSplash()
    {
        if (isShowAd)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("showSplash");

            }
        }

    }
    /// <summary>
    /// 安装提示
    /// </summary>
    /// <param name="id"></param>
    public void ShowOpenOrInstallAppDialog()
    {
        if (isShowAd)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("showOpenOrInstallAppDialog");

            }
        }

    }
    //展示Banner
    public void ShowBanner()
    {
        if (isShowAd)
        {
            AndroidControl.Instance.ShowBanner();
        }

    }
    //展示隐私协议
    public void ShowProtocol()
    {
        if (isShowAd)
        {
            AdControl.Instance.canAwake = false;
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("showProtocol");

            }
        }

    }
    //展示用户协议
    public void ShowUser()
    {
        if (isShowAd)
        {
            AdControl.Instance.canAwake = false;
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("showUser");

            }
        }

    }
    /// <summary>
    /// 关闭banner广告
    /// </summary>
    public void HideBanner()
    {

        if (isShowAd)
        {
            AndroidControl.Instance.HideBanner("hideBanner");
        }

    }
    /// <summary>
    /// 展示信息流广告
    /// </summary>
    /// <param name="id"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="succ"></param>
    /// <param name="close"></param>
    /// <param name="fail"></param>
    public void ShowMessageAd(string id, int x, int y, AndroidControl.Callback succ = null, AndroidControl.Callback close = null, AndroidControl.Callback fail = null)
    {
        if (isShowAd)
        {
            AndroidControl.Instance.CallAndroidFunc("showMessageAd", id, x, y);
        }
        else
        {
            if (succ != null)
            {
                succ();
            }
        }

    }
    /// <summary>
    /// 隐藏信息流
    /// </summary>
    public void HideMessageAd()
    {
        if (isShowAd)
        {
            AndroidControl.Instance.HideMessageAd("hideMessageAd");
        }

    }


    /// <summary>
    /// 渠道购买接口
    /// </summary>
    /// <param name="id"></param>
    /// <param name="complete"></param>
    /// <param name="fail"></param>
    public void BugItem(string id, AndroidControl.Callback complete, AndroidControl.Callback fail = null)
    {



    }

    /// <summary>
    /// UM统计调用
    /// </summary>
    /// <param name="id">对应统计id</param>
    /// <param name="num">对应统计参数</param>
    public void UMStatistics(string id, string num = null)
    {
        if (isShowAd)
        {
            AndroidControl.Instance.CallAndroidOnEvent("UMOnEvent", id, num);
        }

    }


    public void GetAdInfo()
    {
        // 获取广告相关开关信息
    }
    /// <summary>
    /// 震动（0.1秒）
    /// </summary>
    public void CallShake()
    {
        AndroidControl.Instance.CallAndroidFunc("vibrate");
    }

    /// <summary>
    /// 获取展示位的额外控制条件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="succ"></param>
    public void GetAdIntExtraParams(string id, AndroidControl.Callback2 succ)
    {
        AndroidControl.Instance.CallAndroidExtraParams("GetExtraParams", id, succ);
    }
    public void GetConfigParams(AndroidControl.Callback2 succ)
    {
        AndroidControl.Instance.CallAndroidConfigParams("GetDynamicConfig", succ);
    }

    /// <summary>
    /// 获取动态红包信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="succ"></param>
    public void GetAdIntDynamicParams(AndroidControl.Callback2 succ)
    {
        AndroidControl.Instance.CallAndroidDynamicParams("GetDynamicInfo", succ);
    }

    public void GetAdIntDynamicParams2(AndroidControl.Callback2 succ)
    {
        AndroidControl.Instance.CallAndroidDynamicParams("GetDynamicInfo2", succ);
    }

    public void GetConfigUrlParams(AndroidControl.Callback2 succ, AndroidControl.Callback fail)
    {
        AndroidControl.Instance.CallConfigUrlParams("GetDynamicUrl", succ, fail);
    }

    public void Quit()
    {
        if (isShowAd)
        {


            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("quit");
            }
        }
    }
    /// <summary>
    /// 关卡时长统计
    /// </summary>
    /// <param name="levelID"></param>
    /// <param name="type"></param>
    /// <param name="completeTime"></param>
    public void MySendEvent(string levelID, int type, int completeTime)
    {
        if (isShowAd)
        {
            AndroidControl.Instance.CallMyOnEvent(levelID, type, completeTime);
        }
    }

    public void InitRoatContrl() {


        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("GetDynStr");
        }

    }
    public void SdkSendEvent(int eventID)
    {
        if (isShowAd)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (!DataManager.Instance.data.SDKSend.Contains(eventID))
                {
                    DataManager.Instance.data.SDKSend.Add(eventID);
                    AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                    jo.Call("SubmitEvent1To5", eventID);
                }
            }
        }

    }
    public void RYGameTime(long time)
    {
        AndroidControl.Instance.CallRYTime(time);
    }
    public void MyOnLogin()
    {
        AndroidControl.Instance.CallOnLogin();
    }
    //返回安卓ID
    public void GetAndroidID(AndroidControl.Callback2 complete, AndroidControl.Callback close = null, AndroidControl.Callback fail = null)
    {
        AndroidControl.Instance.CallAndroidFunc("GetAndroidId", null, complete);
    }
    //返回版本号
    public void GetEdition(AndroidControl.Callback2 complete, AndroidControl.Callback close = null, AndroidControl.Callback fail = null)
    {
        AndroidControl.Instance.CallAndroidEditionFunc("GetEdition", null, complete);
    }

   public int JoinLevelNum = 0;

    //图片插屏
    public void Parameter(string str, int OpenNums = 0, AndroidControl.Callback succ = null)
    {
        if (isShowAd)
        {
            AndroidControl.Callback showResultCallBack = () =>
            {
                // Hide_Normal(null);
                succ();

            };
            AndroidControl.Callback success = () =>
            {
                if (AndroidControl.Instance.serverMessage != "")
                {
                    try
                    {
                        //print("胜利插屏回调");
                        var configStrArr = AndroidControl.Instance.serverMessage.Split('#');
                        int startLevel = int.Parse(configStrArr[0]);
                        int everyLevel = int.Parse(configStrArr[1]);
                        int r = JoinLevelNum - startLevel;


                        if (r >= 0 && r % everyLevel == 0)
                        {
                            AdControl.Instance.ShowIntAd(str, showResultCallBack, null);
                        }
                        else
                        {
                            showResultCallBack();
                        }
                    }
                    catch (System.Exception)
                    {
                        Debug.LogError("胜利插屏异常");
                        showResultCallBack();
                    }
                }
                else
                {
                    showResultCallBack();
                }
            };
            AndroidControl.Callback fail = () =>
            {
                AdControl.Instance.ShowIntAd(str, showResultCallBack, null);
            };

            AdControl.Instance.GetServerMessage(str, success, fail);
        }

    }
  
    public void GetServerMessage(string poisitonName, AndroidControl.Callback succ = null, AndroidControl.Callback fail = null)
    {
        if (isShowAd)
        {
            AndroidControl.Instance.CallGetMessage("GetExtraParams", poisitonName, succ, fail);
        }

    }


   
}
