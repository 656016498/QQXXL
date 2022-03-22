using BayatGames.SaveGamePro;
using EasyExcelGenerated;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SignPanel : UIBase
{
    public IButton signBtn;
    public IButton onLineBtn;
    public IButton exitBtn;
    public Transform signContent;//签到content
    public Transform onLineContent;//在线红包content
    public List<SignItem> signItems;
    public List<OnlineItem> onLineItems;
    private Action mCallback_sign;
    private Action mCallback_exit;
    private Action mCallback_onLine;
    [Header("奖励类型刷新")]
    public Text awardTypeTxt;
    [Header("dotweenani")]
    public DOTweenAnimation datweenAni; 
    void SignBtnClick()
    {
        if (!WeChatContral.Instance.mWexinIsLogin.Value && SignDataControl.Instance.mdata.indexSign ==2&& SignDataControl.Instance.mdata.canSign)
        {
            WeChatContral.Instance.MLogin(() => {
                Debug.Log("登陆成功");
            });
            return;
        }
        mCallback_sign.Run();
    }
    void ExitBtnClick()
    {
        mCallback_exit.Run();
    }
    void OnlineBtnClick()
    {
        mCallback_onLine.Run();
    }
    void Start()
    {
        BtnSetting();
        if (SignDataControl.Instance.OnLineMax)
        {
            onLineBtn.transform.GetChild(0).GetComponent<Text>().text = string.Format("今日已领完");
            return;
        }
        SignDataControl.Instance.nowTimeType.Subscribe(Value => {
            switch (Value)
            {
                case SignDataControl.TimeType.Stop:
                    onLineBtn.transform.GetChild(0).GetComponent<Text>().text = string.Format("可领取");
                    datweenAni.DOPlay();
                    onLineBtn.transform.GetChild(1).ShowCanvasGroup();
                    break;
                case SignDataControl.TimeType.Playing:
                    datweenAni.DOPause();
                    onLineBtn.transform.GetChild(1).HideCanvasGroup();
                    SignDataControl.Instance.ActiveRemainTime.Subscribe(mValue => { 
                        onLineBtn.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}\n后可领取", mValue);
                    });
                    break;
                default:
                    break;
            }
        });
    }

    public override void Show()
    {
        RefrishUi();
        base.Show();
        GameADControl.Instance.Banner(true);

        //打点
        UmengDisMgr.Instance.CountOnNumber("daily_show");
    }

    public override void Hide()
    {
        base.Hide();
        GameADControl.Instance.Banner(false);
    }

 
    public void AddListenToBtn(Action clickClose, Action clickSign,Action clickOnlie)
    {
        mCallback_exit = clickClose;
        mCallback_sign = clickSign;
        mCallback_onLine = clickOnlie;
    }
    
    private void BtnSetting()
    {
        signBtn.onClick.AddListener(SignBtnClick);
        exitBtn.onClick.AddListener(ExitBtnClick);
        onLineBtn.onClick.AddListener(OnlineBtnClick);
    }

    //刷新ui
    public void RefrishUi()
    {
        //刷新签到模块
        if (signItems.Count == 0)
        {
            for (int i = 0; i < signContent.childCount; i++)
            {
                var msignItem = signContent.GetChild(i).GetComponent<SignItem>();
                if (!signItems.Contains(msignItem))
                {
                    signItems.Add(msignItem);
                }
                msignItem.RefrishUI();
            }
        }
        else
        {
            foreach (var item in signItems)
            {
                item.RefrishUI();
            }
        }
        //刷新在线红包
        if (onLineItems.Count == 0)
        {
            for (int i = 0; i < onLineContent.childCount; i++)
            {
                var mOnline = onLineContent.GetChild(i).GetComponent<OnlineItem>();
                if (!onLineItems.Contains(mOnline))
                {
                    onLineItems.Add(mOnline);
                }
                mOnline.RefeishUi();
            }
        }
        else
        {
            foreach (var item in onLineItems)
            {
                item.RefeishUi();
            }
        }

        //signBtn.interactable = SignDataControl.Instance.mdata.canSign;
        signBtn.GetComponent<Image>().color = SignDataControl.Instance.mdata.canSign ? new Color(255/255f, 255/255f, 255/255f, 255/255f) : new Color(191/255f, 191/255f, 191/255f, 128/255f);

        UpDataAwardText();
    }
    //返回当前签到奖励类型
    public SignItem mSignType
    {
        get
        {

            if (signItems.Count != 0)
            {
                var mSignItem = signItems[SignDataControl.Instance.mdata.indexSign-1];
                return mSignItem;
            }
            else
            {
                return null;
            }
                
                
        }
    }

    //刷新奖励类型
    private void UpDataAwardText()
    {
        var nextAwardDay = SignDataControl.Instance.GetNextCash();  
        var dayIndex = nextAwardDay-SignDataControl.Instance.mdata.signDay;
       
        XDebug.Log("nextAwardDay" + nextAwardDay);
        XDebug.Log("signDay" + SignDataControl.Instance.mdata.signDay);
        XDebug.Log("相隔" + dayIndex + "天");

        if (nextAwardDay == 0) { awardTypeTxt.transform.HideCanvasGroup();return; }
        if (SignDataControl.Instance.TodayIsCasn())
        {
            awardTypeTxt.text = string.Format("<color=#ff4345>今天</color><color=#8f504e>签到,可获得</color><color=#ff4345>现金奖励</color>");
        }
        else
        {
            if (dayIndex == 1)
            {
                awardTypeTxt.text = string.Format("<color=#ff4345>明天</color><color=#8f504e>签到,可获得</color><color=#ff4345>现金奖励</color>");
            }
            else if (dayIndex == 2)
            {
                awardTypeTxt.text = string.Format("<color=#ff4345>后天</color><color=#8f504e>签到,可获得</color><color=#ff4345>现金奖励</color>");
            }
            else
            {
                awardTypeTxt.text = string.Format("<color=#ff4345>{0}天后</color><color=#8f504e>签到,可获得</color><color=#ff4345>现金奖励</color>", SignDataControl.Instance.Rel(dayIndex));
            }
        }
            
    }
}
public class SignData
{
    public int indexSign;//签到日期
    public DateTime timeLastSign;//上次签到时间
    public bool canSign;//是否可以每日签到 
    public int targeSignNum;//目标签到天数
    public Dictionary<int,bool> onLineGetDic;
    public int onLineTime;//倒计时
    public int signDay;//签到标识 
    public  int[] mOnLineList;
    public SignData()
    {
        indexSign = 1;
        timeLastSign = GameTime.GameClock.NowTime;
        canSign = true;
        targeSignNum = 15;
        mOnLineList = new int[] { 60, 180, 300, 600, 1800 };
        onLineTime = mOnLineList[0];
        signDay = 1;
       
        InitDic();
    }
    public  void InitDic()
    {
        onLineGetDic = new Dictionary<int, bool>();
        for (int i = 0; i < 5; i++)
        {
            onLineGetDic.Add(i + 1, false);
        }
    }
}

public class OnLineData
{
    public int[] mOnLineList;
    public int onLineTime;//倒计时
    public OnLineData()
    {
        mOnLineList = new int[] { 60, 180, 300, 600, 1800 };
        onLineTime = mOnLineList[0];
    }
}
public class SignDataControl
{
    private static SignDataControl mInstance;
    public static SignDataControl Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new SignDataControl();
            }
            return mInstance;
        }
    }
    public const string local_key = "SignData_Key";
    public const string local_OnLinekey = "Online_Key";
    static bool isInit = false;
    public SignData mdata;
    public OnLineData mOnlineData;
    public ReactiveProperty<string> ActiveRemainTime = new ReactiveProperty<string>();
    public ReactiveProperty<TimeType> nowTimeType = new ReactiveProperty<TimeType>(TimeType.Stop);
    public List<SignRedConfig> mSignRedConfigs = new List<SignRedConfig>();
    public IDisposable mdispose;
    /// <summary>
    /// 奖励类型
    /// </summary>
    public enum AwardType
    {
        cash = 1,//现金0.3元
        hb =7,//红包
        pigCash=8//存钱罐
    }
    public enum TimeType
    {
        Stop,
        Playing,
        //CanGet,
    }
    public SignDataControl()
    {
        if (!isInit)
        {
            Init();
            isInit = true;
        }
    }
    public void Init()
    {
        if (!isInit)
        {
            isInit = true;
            Observable.TimeInterval(System.TimeSpan.FromSeconds(.5f)).Subscribe(_ =>
            {
                LoadData();
                StartCutTime();
                if (mSignRedConfigs.Count == 0)
                {
                    XDebug.Log("ConfigGetMgr.Instance.mSignRedConfigs()" + ConfigGetMgr.Instance.mSignRedConfigs().Count);
                    mSignRedConfigs = ConfigGetMgr.Instance.mSignRedConfigs();
                }
            });
        }
         
           
    }
    private void SaveData()
    {
        SaveGame.Save(local_key, mdata);
    }
    private void LoadData()
    {
        mdata = SaveGame.Load<SignData>(local_key);
        if (mdata == null)
        {
            mdata = new SignData();
        }
        else
        {
            Debug.Log("昨日签到" + mdata.timeLastSign);
            Debug.Log("现在时间" + GameTime.GameClock.NowTime);
            //隔天刷新
            if (!TimeExtension.IsSameDay(mdata.timeLastSign, GameTime.GameClock.NowTime))
            {
                Debug.Log("隔天更新时间");
                if (mdata.indexSign >= mdata.targeSignNum + 1 )
                {
                    Debug.Log("隔天更新时间");
                    mdata = new SignData();
                }
                mdata.canSign = true;
                mdata.signDay = mdata.indexSign;
                mdata.InitDic();
            }
        }
        SaveData();
    }

   
    //阿拉伯数字转换语文大写
    public  string Rel(int number)
    {
       
        //结果
        string resule = "";
        //用作替换数字的字符数组
        string[] rep = new string[]
        { "零", "一", "二", "三", "四",
                "五", "六", "七", "八", "九", };
        //用于添加单位的数组
        string[] unit = new string[]
        { "", "十", "百", "千", "万","十"
            ,"百" ,"千" ,"亿","十","百" ,"千"};
        //取数字位数
        int l = number.ToString().Length;
       
        //循环取最后以为数字处理字符转换
        for (int i = 0; i < l; i++)
        {
            //取最后位数值
            int temp = number % 10;
            //取剩余位
            number = number / 10;

            //判断当前最后位为0
            if (temp == 0)
            {
                //判断万位添加单位 万
                if (i == 4)
                    resule += unit[4];
                //判断亿位添加单位 亿
                if (i == 8)
                    resule += unit[8];
                //判断当前最后位是否需要加 零
                if (resule != "" && resule[resule.Length - 1] != '零' && resule[resule.Length - 1] != '万' && resule[resule.Length - 1] != '亿')
                {
                    resule += rep[temp];
                }
                   
            }
            else
            {
                //当前位不是0 添加单位 添加数值
                resule += unit[i];
                resule += rep[temp];
                

            }
        }
        //定义中间变量 倒叙结果
        string str;
        if (resule.Length > 1)
        {
            str = resule.Remove(resule.Length - 1);
        }
        else
        {
            str = resule;
        }
        resule = "";
        for (int i = 0; i < str.Length; i++)
        {
            resule += str[str.Length - 1 - i];
        }
     
        return resule;
    }   

    //添加签到index
    public void AddSignIndex(AwardType mtype)
    { //奖励
        
        switch (mtype)
        {
            case AwardType.hb:
                Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ =>
                {
                    var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
                    var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                    var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                    popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                    {
                        popup2.effect.SetActive(false);
                        Debug.Log("关闭红包二级界面");
                        if (!GameManager.Instance.isCash)
                        {
                            RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                        }
                    });
                    Debug.Log("奖励红包");
                });
                break;
            case AwardType.pigCash:
                Debug.Log("奖励金猪");
                UIManager.Instance.ShowPopUp<LotteryAwardPanel>((LotteryAwardPanel mpanel) => {
                    var MPigCoins = PigCoins;
                    mpanel.mtype = AawardPanelType.Sign;
                    mpanel.SetIcon(-1);
                    mpanel.SetDir(MPigCoins);
                    mpanel.AddListenToBtn(
                        () => { mpanel.Hide(); PiggyBankData.Instance.UpdatePiggyBank(MPigCoins); },
                        () => {
                            mpanel.Hide();
                            //添加金猪币
                            Debug.Log("添加金猪币" + MPigCoins);
                            PiggyBankData.Instance.UpdatePiggyBank(MPigCoins);
                        });
                });
                break;
            case AwardType.cash:
                var gold = 0.3f;
                Withdraw(WithDrawlKeyControl.SignCashKey03, (v) => {
                    if (v == "1" || v.Contains("213"))
                    {
                        //弹出提示
                        ShowPublicTip.Instance.Show("提现成功，请留意微信信息！");
                        var ui = UIManager.Instance.ShowPopUp<WithdrawSucceedUI>();
                        if (v.Contains("213")) ui.OnShow2(gold.ToString());
                        else ui.OnShow(gold.ToString());
                    }
                    else
                    {
                        //弹出提示
                        if (v.Contains("402") || v.Contains("209"))
                        {

                            ShowPublicTip.Instance.Show("该额度只能提现1次!");

                        }
                        else if (v.Contains("214"))
                        {
                            ShowPublicTip.Instance.Show("提现过于频繁，请稍后再试！");
                        }
                        else
                        {
                            ShowPublicTip.Instance.Show("提现失败！");
                        }
                    }
                });
                Debug.Log("奖励现金");
                break;
            default:
                break;
        }
        mdata.indexSign++;
        Debug.Log("当前签到天数" + mdata.indexSign);
        Debug.Log("当前目标签到天数" + mdata.targeSignNum);
        //if (mdata.indexSign >= mdata.targeSignNum+1)
        //{
        //    mdata = new SignData();
        //}
        mdata.canSign = false;
        mdata.timeLastSign = GameTime.GameClock.NowTime;
        SaveData();
        //刷新ui
        var mPamel = UIManager.Instance.GetBase<SignPanel>();
        mPamel.RefrishUi();
    }
    //真提现
    public void Withdraw(string pushKey, Action<string> PushState)
    {
        //真提现方法
        WeChatContral.Instance.Withdraw(pushKey, PushState);
    }
    //设置在线红包领取状态
    public void SetOnline()
    {
        if (mdata.onLineGetDic.ContainsKey(CanGetIndex()) && !mdata.onLineGetDic[CanGetIndex()])
        {
            mdata.onLineGetDic[CanGetIndex()] = true;
        }
        SaveData();
        if(nowTimeType.Value == TimeType.Stop && CanGetIndex()>0)
        {
            StartCutTime();
        }
    }

    //返回第几个在线红包可领取
    public int CanGetIndex()
    {
      foreach (var item in mdata.onLineGetDic)
      {
            if (!item.Value)
                return item.Key;
      }
      return 0; 
    }

    //返回第几个倒计时
    public int RetureDownTime()
    {
        return mdata.mOnLineList[CanGetIndex() - 1];
    }
    //开始倒计时
    public void StartCutTime()
    {
        nowTimeType.Value = TimeType.Playing;
        mdata.onLineTime = RetureDownTime();
        ActiveRemainTime.Value = mdata.onLineTime.Second_TransFrom_Math();
        mdispose = Observable.Interval(System.TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    if (mdata.onLineTime > 0)
                    {
                        mdata.onLineTime--;
                        ActiveRemainTime.Value = mdata.onLineTime.Second_TransFrom_Math();
                    }
                    else if (mdata.onLineTime ==0)
                    {
                        mdispose.Dispose();
                        nowTimeType.Value = TimeType.Stop;
                    }
                });
    }

    //是否当日在线红包领取完
    public bool OnLineMax
    {
        get
        {
            return CanGetIndex() <=0;
        }
    }

   public int  PigCoins
    {
        get
        {
            //if (mdata.indexSign - 1 < mSignRedConfigs.Count)
            //{

            //}
            Debug.Log("mdata.indexSign" + mdata.indexSign);
            Debug.Log("mSignRedConfigs" + mSignRedConfigs.Count);
            var mAwardCount = mSignRedConfigs[mdata.indexSign-1].AwardCount;
            if (mAwardCount != 0)
                return mAwardCount;
            else
                return 0;
        }
    }


    //获取下一个现金奖励在第几天
    public int GetNextCash()
    {
        foreach (var item in mSignRedConfigs)
        {
            if (item.AwardType == (int)AwardType.cash || item.AwardType == (int)AwardType.pigCash)
            {
                if (mdata.signDay < item.ID)
                {
                    XDebug.Log("mdata.indexSign" + mdata.indexSign);
                    return item.ID;
                }
            }
        }

        return 0;
    }
   // 获取当天是不是现金签到
   public bool TodayIsCasn()
    {
       
            if ((mSignRedConfigs[mdata.signDay - 1].AwardType == (int)AwardType.cash || mSignRedConfigs[mdata.signDay - 1].AwardType == (int)AwardType.pigCash) && mdata.canSign)
            {
                return true;
            }
        
        return false;

    }
}
//提现key管理
public class WithDrawlKeyControl
{
    private static WithDrawlKeyControl instance;
    public static WithDrawlKeyControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WithDrawlKeyControl();
            }

            return instance;
        }
    }

    public const string SignCashKey03 = "Sign_0.3"; 
    public const string LotryCashKey03 = "Lokey_0.3";
    public const string LotryCashKey05 = "Lokey_0.5";
    public const string LotryCashKey10 = "Lokey_1";

}