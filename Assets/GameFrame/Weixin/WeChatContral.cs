using BayatGames.SaveGamePro;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class WeChatContral
{
    private static WeChatContral mInstance;
    public static WeChatContral Instance
    {
        get
        {
            if(mInstance==null)
            {
                mInstance = new WeChatContral();
            }

            return mInstance;
        }
    }
    public delegate void AddWithdraw(string key, string time, float rmb, int state);
    public delegate void ClearRecord();
    public event AddWithdraw OnAddWithdraw;
    public event ClearRecord OnClear;
    public ReactiveProperty<string> UserName = new ReactiveProperty<string>();
    public ReactiveProperty<string> UserUrl = new ReactiveProperty<string>();
    public ReactiveProperty<Sprite> UserUrl_sprite = new ReactiveProperty<Sprite>();

    public ReactiveProperty<int> User_ConverCoin = new ReactiveProperty<int>();
    private Action<bool> mCallback_push_conver;
    public ReactiveProperty<bool> mWexinIsLogin = new ReactiveProperty<bool>(false);

    public string userId;

    private static bool mIsInit = false;
    private static bool mIsInit2 = false;

    public string GetMes()
    {
        string mes = "{  \"hongbaoSpeed\":[    {      \"waKey\":\"day7\",      \"hongbaoSpeed\":1.265,      \"createdAt\":\"\"    }  ],  \"numericCalculate\":{    \"loginDay\":2,    \"loginType\":1,    \"userLevel\":1,        \"videoNum\":0,    \"gateLevel\":1,    \"createdAt\":\"\",    \"updatedAt\":\"\"  }}";
        return mes;

    }

    //出现在引导之后
    public void Init()
    {
#if UNITY_IOS || UNITY_IPHONE
        return;
#endif
        mIsInit = true;

        Debug.LogError("清空红包币");

        Wechat.Instance.CleanServerCoinData((string message) =>
        {
            Debug.LogError("清空红包币完成");
            mInit();
        }
        , (string message) =>
        {
            Debug.LogError("服务器数据初始化失败:" + message);
        });

    }

    public void mInit()
    {
#if UNITY_IOS || UNITY_IPHONE 
        return;
#endif
        mIsInit2 = true;
        Debug.Log("WeChatContral初始化");

        UserUrl.Subscribe(url =>
        {
            Debug.Log("头像地址发生改动");

            if (url == null) return;
            //Debug.Log("加载头像");
            ////WWW
            //var r = UnityWebRequest.Get(url);

            ObservableWWW.GetWWW(url)
            .Subscribe(value =>
            {
                Debug.Log("加载头像成功");

                var tex2d = value.texture;
                Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
                UserUrl_sprite.Value = m_sprite;
                SaveLocalUserData();
            });
        });

        
        LoadLocalUserData();

        IsLogin();

    }
    public void Login(Action<bool> callback=null)
    {
        AdControl.Instance.canAwake = false;
        WeChat_AndroidHelps.Callback complete = (string message) =>
        {

            if (callback != null)
            {
                callback(true);
            }
            mWexinIsLogin.Value = true;
            var strs = message.Split('#');
            UserName.Value = strs[0];
            UserUrl.Value = strs[1];
            if (!string.IsNullOrEmpty(strs[2]))
            {
                int v = 0;
                if (int.TryParse(strs[2], out v))
                    User_ConverCoin.Value = v;
            }
            Debug.Log("id:"+ strs[2]);
            if (strs[2]!=null)
            {
                SaveLocalUserData(strs[2]);
                userId = strs[2];
            }

            GetHistory();

        };
        WeChat_AndroidHelps.Callback onfaild = (string message) => {            
            Debug.Log("登陆失败...");
            if (callback != null)
            {
                callback(false);
            }
        };

        Wechat.Instance.Login(complete, onfaild);
    }

    public void IsLogin()
    {
        WeChat_AndroidHelps.Callback complete = (string message) => 
        {
            if(message=="1")
            {
                //登陆状态
                //GetUserData((string usedata) =>
                //{
                //    var strs = usedata.Split('#');
                //    UserName.Value = strs[0];
                //    UserUrl.Value = strs[1];
                //    if (!string.IsNullOrEmpty(strs[2]))
                //    {
                //        int v = 0;
                //        if(int.TryParse(strs[2], out v))
                //            User_ConverCoin.Value = v;
                //    }
                    
                //}, (string failmessage) => { });
                mWexinIsLogin.Value = true;
            }
            else
            {
                Debug.Log("登陆过期...");
            }
        };
        WeChat_AndroidHelps.Callback onfaild = (string message) => { };

        Wechat.Instance.IsLogin(complete, onfaild);
    }

    public void PushCoins(string key,float speed, int maxLv,Action<bool> callback)
    {
        WeChat_AndroidHelps.Callback complete = (string message) => 
        {
            Debug.Log("微信_提交兑换币回调:" + message);
            //int v = 0;
            //if (int.TryParse(message, out v))
            //{
            //    User_ConverCoin.Value = v;
            //    MyGameInfo.Instance.Golds = (float)v / 10000;
            //}
            callback(true);            
        };
        WeChat_AndroidHelps.Callback onfaild = (string message) => { callback(false); };

        Wechat.Instance.PushCoins(key, speed, maxLv, complete, onfaild);
    }

    public void Withdraw(string key,Action<string> callback)
    {
        AdControl.Instance.canAwake = false;
        //Debug.Log(Application.platform);
        if (Application.platform != RuntimePlatform.Android)
        {
            //七天任务
            SevenWithdrawDataMgr.Instance.AddTaskData(1,5);
            callback("1");
            return;
        }
        if (mWexinIsLogin.Value == false)
        {
            Login((bool isLogon) =>
            {
                if (isLogon)
                {
                    callback("");
                    Debug.Log("登录成功");
                    ShowPublicTip.Instance.Show("登陆成功");
                }
                else
                {
                    callback("");
                    ShowPublicTip.Instance.Show("登陆失败");
                    Debug.Log("登录失败");
                }
            });
            return;
        }

        WeChat_AndroidHelps.Callback complete = (string message) =>
        {
            //ShowText("提现成功，请留意微信信息!");
            //七天任务
            SevenWithdrawDataMgr.Instance.AddTaskData(1,5);
            callback("1");
            Debug.Log("提现成功:"+message);
        };
        WeChat_AndroidHelps.Callback onfaild = (string message) => 
        {
            Debug.Log("提现失败:" + message);
            callback(message);
            //if (message.Contains("402")|| message.Contains("209"))
            //{
            //    callback(message);
            //    PuzzleMatchManager.instance.scene.ShowText("该额度只能提现1次!");
            //}
            //else
            //{
            //    callback(message);
            //    PuzzleMatchManager.instance.scene.ShowText("提现失败：本地网络异常！");
            //}
        };

        Wechat.Instance.Withdraw(key, complete, onfaild);
    }

    //微信登陆
    public void MLogin( Action callBack)
    {
        if (mWexinIsLogin.Value == false)
        {
            Login((bool isLogon) =>
            {
                if (isLogon)
                {
                    Debug.Log("登录成功");
                    //ShowText("登陆成功");
                    ShowPublicTip.Instance.Show("登录成功");
                    callBack.Run();
                }
                else
                {
                    //ShowText("登陆失败");
                    Debug.Log("登录失败");
                    ShowPublicTip.Instance.Show("登录失败");
                }
            });
        }
    }
    public void GetHistory(Action callback = null)
    {
        //微信登录成功后获取提现记录,同步到本地信息
        WeChat_AndroidHelps.Callback historyComplete = (string historyMessage) =>
        {
            //Debug.Log("历史记录：" + historyMessage);
            //Debug.Log("historyCallb");
            if (historyMessage != null && historyMessage.Length > 1)
            {
                JObject obj = JObject.Parse(historyMessage);
                if (obj["data"].Count() > 0)
                {
                    //RedWithdrawData.Instance.ClearRecord();
                    OnClear?.Invoke();
                }
                foreach (var item in obj["data"])
                {
                    if (item == null) return;
                    string key = (string)item["waKey"];
                    string rmb = (string)item["fee"];
                    string time = (string)item["updatedAt"];
                    string  state = (string)item["status"];
                    OnAddWithdraw?.Invoke(key, time, float.Parse(rmb), int.Parse(state));
                    //RedWithdrawData.Instance.AddWDRecord(key, time, float.Parse(rmb),int.Parse(state));
                    RedWithdrawData.Instance.isFirstLoging = false;
                }
                callback?.Invoke();
            }
        };
        WeChat_AndroidHelps.Callback historyFail = (string historyMessage) => {
            //ShowText("刷新失败");
            Debug.LogWarning("获取提现记录失败：" + historyMessage); 
        };
        WithdrawHistory(historyComplete, historyFail);
    }
    /// <summary>
    /// 提现历史
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    public void WithdrawHistory(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        Wechat.Instance.WithdrawHistory(complete, onfaild);
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    private void GetUserData(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        Wechat.Instance.GetUserData(complete, onfaild);
    }
    /// <summary>
    /// 获取游戏信息
    /// </summary>
    /// <param name="complete"></param>
    /// <param name="onfaild"></param>
    private void GetGameInfo(WeChat_AndroidHelps.Callback complete, WeChat_AndroidHelps.Callback onfaild)
    {
        Wechat.Instance.GetGameInfo(complete, onfaild);
    }

    public void Share(WeChat_AndroidHelps.Callback complete = null, WeChat_AndroidHelps.Callback onfaild = null)
    {
        Wechat.Instance.Share(complete, onfaild);
    }

#region 本地储存
    public  class UserData
    {
        public string name;
        public Sprite UrlSprite;
        public string userId;
    }

    private void LoadLocalUserData()
    {
        var mdata = SaveGame.Load<UserData>("GAME_WEIXINDATA");
        if(mdata!=null)
        {
            UserName.Value = mdata.name;
            UserUrl_sprite.Value = mdata.UrlSprite;
            mWexinIsLogin.Value = true;
            userId = mdata.userId;
        }
    }

    private void SaveLocalUserData(string id="")
    {
        UserData mdata = new UserData
        {
            name = UserName.Value,
            UrlSprite = UserUrl_sprite.Value,
            userId = id
        };
        SaveGame.Save<UserData>("GAME_WEIXINDATA", mdata);
    }

#endregion
}
