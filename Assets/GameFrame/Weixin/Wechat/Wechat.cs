using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public class Wechat
{
    private static Wechat mInstance;
    public static Wechat Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new Wechat();
            }
            return mInstance;
        }
    }
#if UNITY_IOS

    [DllImport("__Internal")]
    public static extern string InvokeIsLogin();
    [DllImport("__Internal")]
    public static extern void InvokeWechatLogin();
    [DllImport("__Internal")]
    public static extern void InvokePushCoins(string key,float speed,int lve);
    [DllImport("__Internal")]
    public static extern void InvokeWithdraw(string key);
#endif
    /// <summary>
    /// 是否登陆
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void IsLogin(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;
        if (Application.platform== RuntimePlatform.Android)
        {
            CallAndroidFunc("IsWXLogin");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            var info= InvokeIsLogin();
            if (info=="1")
            {
                complete(info);
            }
            else
            {
                onfaild(info);
            }
#endif
        }
        
    }

    /// <summary>
    /// 登陆
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void Login(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;
        
        if (Application.platform == RuntimePlatform.Android|| Application.platform == RuntimePlatform.WindowsEditor)
        {
            CallAndroidFunc("wxLogin");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokeWechatLogin();
#endif
        }
    }

    /// <summary>
    /// 提前钱币
    /// </summary>
    /// <param name="coin"></param>
    /// <param name="maxLv"></param>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void PushCoins(string key, float speed, int maxLv, WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;

        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("hbCoin", key, speed, maxLv);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokePushCoins(key,speed,maxLv);
#endif
        }
    }

    /// <summary>
    /// 提现
    /// </summary>
    /// <param name="key"></param>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void Withdraw(string key, WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;

       
        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("wxTixian", key);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            InvokeWithdraw(key);
#endif
        }
    }

    /// <summary>
    /// 提现历史
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void WithdrawHistory(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;

        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("wxTixianHistory");
        }
        else
        {
            complete("{\"data\":[{\"waKey\":\"dayOne03\",\"fee\":0.3,\"count\":1,\"status\":1,\"createdAt\":\"2021 - 09 - 09 16:38:51\",\"updatedAt\":\"2021 - 09 - 09 16:38:51\"}]}");
        }
        
    }

    /// <summary>
    /// 获取用户头像
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void GetUserData(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;

        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("GetWexinName_Url");
        }
        

    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void GetGameInfo(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;

        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("getGameInfo");
        }
        
    }


    public void CleanServerCoinData(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;
        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("CleanHbCoin");
        }
    }


    public void Share(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        WeChat_AndroidHelps.Instance.CompleteCallback = complete;
        WeChat_AndroidHelps.Instance.FaildCallback = onfaild;

        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("weixinShare1");
        }
    }


    /// <summary>
    /// 统计登陆入口
    /// </summary>
    public void CountLogic()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CallAndroidFunc("TiXian_Login");
        }
    }

    /// <summary>
    /// 用户等级
    /// </summary>
    public string playerLv = "expand1";
    /// <summary>
    /// 产出钱币数量
    /// </summary>
    public string CreatCoins = "expand2";
    /// <summary>
    /// 汽车最高等级
    /// </summary>
    public string CarMaxLv = "expand3";
    /// <summary>
    /// 升级宝箱获取奖励的红包币
    /// </summary>
    public string Expand4 = "expand4";
    /// <summary>
    /// 
    /// </summary>
    public string Expand5 = "expand5";
    /// <summary>
    /// 任务ID
    /// </summary>
    public string TaskID = "expand6";
    /// <summary>
    /// 
    /// </summary>
    public string Expand7 = "expand7";

    public string Expand8= "expand8";

    /// <summary>
    /// 统计自定义事件接口
    /// </summary>
    /// <param name="eventID"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void CountEvent(string eventID, string key, string value)
    {
        #region key
        //        key：
        //expand1 用户等级
        //expand2 产出数量
        //expand3 汽车最高等级
        //expand4 产出等级
        //expand5 产出对应汽车等级
        //expand6 任务id
        //expand7 最高车辆等级
        #endregion        
        CallAndroidFunc("TiXian_CountEvent", eventID, key, value);
    }

    private Dictionary<string, string> mDic;
    private Dictionary<string,string> GetDicInfo
    {
        get
        {
            if(mDic==null)
            {
                mDic = new Dictionary<string, string>();
                mDic.Add("expand1", "用户等级");
                mDic.Add("expand2", "产出数量");
                mDic.Add("expand3", "汽车最高等级");
                mDic.Add("expand4", "等级宝箱产出");
                mDic.Add("expand5", "任务id");
                mDic.Add("expand6", "最高车辆等级");
            }
            return mDic;
        }
    }
    private string GetInfo(string key)
    {
        GetDicInfo.TryGetValue(key, out string info);
        return info;
    }

    private Dictionary<string, string> mDic2;
    private Dictionary<string,string> GetDic2
    {
        get
        {
            if(mDic2==null)
            {
                mDic2 = new Dictionary<string, string>();
                mDic2.Add("packrain", "红包雨产出");
                mDic2.Add("packraindoub", "红包雨视频翻倍产出");
                mDic2.Add("synup", "汽车合成升级");
                mDic2.Add("syndoub", "汽车合成双倍奖励");
                mDic2.Add("signpack", "福利签到产出");
                mDic2.Add("signpackdoub", "福利签到双倍产出");
                mDic2.Add("line", "福利在线产出");
                mDic2.Add("linedoub", "福利在线双倍产出");
                mDic2.Add("inve", "投资产出");
                mDic2.Add("ballnoon", "热气球产出");
                mDic2.Add("ballnoondoub", "热气球翻倍产出");
                mDic2.Add("levelbox", "等级宝箱产出");
                mDic2.Add("levelboxdoub", "等级宝箱翻倍产出");
                mDic2.Add("seniorcar", "高级新车合成奖励");
                mDic2.Add("gppack", "金猪获得红包产出");
                mDic2.Add("carup", "汽车位产出红包币");
                mDic2.Add("asspack", "助理产出");
                mDic2.Add("luckturn", "幸运转盘产出");
                mDic2.Add("luckturndoub", "幸运转盘翻倍产出");
                mDic2.Add("task", "任务产出");
                mDic2.Add("taskdoub", "任务翻倍产出");
                mDic2.Add("other", "其他");

            }
            return mDic2;
        }
    }
    private string GetKeyName(string key)
    {
        GetDic2.TryGetValue(key, out string name);
        return name;
    }


    public void CountEvent(string eventID
        , string key1, string value1
        , string key2, string value2)
    {
        Debug.LogError(string.Format("上传数据:eventid:{0}\n{1}:{2}\n{3}:{4}"
           , GetKeyName(eventID), GetInfo(key1), value1, GetInfo(key2), value2));
        
        CallAndroidFunc("TiXian_CountEvent", eventID, key1, value1, key2, value2);
    }
    public void CountEvent(string eventID
        , string key1, string value1
        , string key2, string value2
        , string key3, string value3)
    {
        Debug.LogError(string.Format("上传数据:eventid:{0}\n{1}:{2}\n{3}:{4}\n{5}:{6}" 
            , GetKeyName(eventID), GetInfo(key1), value1, GetInfo(key2), value2, GetInfo(key3), value3));      
        CallAndroidFunc("TiXian_CountEvent", eventID, key1, value1, key2, value2, key3, value3);
    }

    public void CountEvent(string eventID
        , string key1, string value1
        , string key2, string value2
        , string key3, string value3
        , string key4, string value4)
    {
        Debug.LogError(string.Format("上传数据:eventid:{0}\n{1}:{2}\n{3}:{4}\n{5}:{6}\n{7}:{8}"
            , GetKeyName(eventID), GetInfo(key1), value1, GetInfo(key2), value2, GetInfo(key3), value3, GetInfo(key4), value4));
       
        CallAndroidFunc("TiXian_CountEvent", eventID, key1, value1, key2, value2, key3, value3, key4, value4);
    }


    /// <summary>
    /// 统计关卡信息接口
    /// </summary>
    /// <param name="levelID">关卡编号</param>
    /// <param name="type">1:进入,2:成功,3:失败</param>
    /// <param name="completeTime">通关时间</param>
    public void CountLevel(string levelID, int type, int completeTime)
    {
        CallAndroidFunc("TiXian_LevelInfo", levelID, type, completeTime);
    }

    private void CallAndroidFunc(string func, params object[] args)
    {
        //Debug.LogWarning("提交申请");
        if (Application.platform == RuntimePlatform.Android)
        {
            //CheckRun.isBanPlay = true;
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            if (args.Length == 0)
            {
                jo.Call(func);
            }
            else
            {
                jo.Call(func, args);
            }
        }
        else
        {
            if (WeChat_AndroidHelps.Instance.CompleteCallback == null) return;
            try
            {
                if (args.Length > 0)
                {
                    WeChat_AndroidHelps.Instance.OnCompleteCallback(GetFalseAndroidInfo(func, (int)args[0]));
                }
                else
                {
                    WeChat_AndroidHelps.Instance.OnCompleteCallback(GetFalseAndroidInfo(func));
                }
            }
            catch (System.Exception)
            {               
            }            
        }
    }

    private string GetFalseAndroidInfo(string funcName, int addCoins = 0)
    {
        string message = null;
        switch (funcName)
        {
            case "IsWXLogin":
                message = "0";
                break;
            case "wxLogin":
                Debug.Log(">>>>模拟:获取用户信息");

                message = "杰之诺风#http://thirdwx.qlogo.cn/mmopen/vi_32/Od4S1sET6rr3DuoRj5oN8LcvJiaH2G4V8A0umTQ6GiaZQVJXWAC9GuuJynJYZCrSvhm41TWkTccmyZulMFWTialmA/132 #123456000";// + (ConverCoin.Instace.GetCoin + addCoins);
               
                break;
            case "hbCoin":
                message = "";// (ConverCoin.Instace.GetCoin + addCoins).ToString();
                break;
            case "wxTixian":
                message = "1";
                break;
            case "wxTixianHistory":
                message = "{\"data\": [{\"6011\": \"6011\",\"fee\": 0.3,\"count\": 1},{\"level03_1\": \"level03_1\",\"fee\": 0.4,\"count\": 1}]}";
                break;
            case "GetWexinName_Url":
                Debug.Log(">>>>模拟:获取用户信息");
                message = "杰之诺风#http://thirdwx.qlogo.cn/mmopen/vi_32/Od4S1sET6rr3DuoRj5oN8LcvJiaH2G4V8A0umTQ6GiaZQVJXWAC9GuuJynJYZCrSvhm41TWkTccmyZulMFWTialmA/132#";// + ConverCoin.Instace.GetCoin;
                break;
            case "weixinShare1":
                message = "分享成功";
                break;
            default:
                break;
        }
        //Debug.Log(message);
        return message;
    }
}
