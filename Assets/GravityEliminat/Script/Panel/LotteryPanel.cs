using BayatGames.SaveGamePro;
using DG.Tweening;
using GameTime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyExcelGenerated;
using EasyExcel;
using UniRx;


public class LotteryPanel : UIBase
{
    [Header("抽奖区域")]
    public RectTransform transTurntableTrans;//转盘区域
    [Header("抽奖按钮")]
    public Button lottreyBtn;//抽奖按钮
    [Header("问号按钮")]
    public IButton quesBtn;
    [Header("退出按钮")]
    public IButton exitBtn;
    [Header("总抽奖券文本")]
    public Text lotteryText;
    [Header("当前消耗券")]
    public Text needLotteryText;
    private List<int> probability = new List<int>();//奖励概率
    List<LotteryConfig> mlotteryConfig;
    [Header("中奖特效")]
    public GameObject drwaEfffect;
    [Header("指针动画")]
    public Animator pointerAni;
    [Header("leftCloud")]
    public Transform leftCloud;
    [Header("rightCloud")]
    public Transform rightCloud;
    private bool isCanShake = true;
    [Header("进度条")]
    public SliderControl mSliderControl;
    [Header("对应金额图片")]
    public List<Sprite> fillNum;
    public Image fillIcon;
    //public Image fillPaperIcon;
    public Text lotryText;
    [Header("抽奖按钮对应图片")]
    public Sprite canDraw;
    public Sprite noDraw;
    public static bool isShowPanel = false;
    public IDisposable mdis;
    public override void Awake()
    {
        Init();
        base.Awake();
    }
    private void Start()
    {
        BtnSet();
    }
    public override void Show()
    {
        ShowAni();
        RefrishUI();
        
        //mSliderControl.UpdateInfo(LotteryDataManger.Instance.mdata.fillPro / 100, string.Format("{0}LotteryDataManger.Instance.mdata.fillPro.ToString("f2")));
       
        base.Show();
        GameADControl.Instance.Banner(true);
        //打点
        UmengDisMgr.Instance.CountOnNumber("lottery_show");
    }
    public override void Hide()
    {
        base.Hide();
        if (LotteryDataManger.Instance.CloseCallBack!=null)
        {
            LotteryDataManger.Instance.CloseCallBack();
            LotteryDataManger.Instance.CloseCallBack= null;
        }
        GameADControl.Instance.Banner(false);
    }
    //刷新ui
    private void RefrishUI()
    {
        var mdata = LotteryDataManger.Instance.mdata;
        lotteryText.text = string.Format("{0}", mdata.totalLottery);
        needLotteryText.text = string.Format("x{0}", mdata.lotteryPaper);
        Debug.Log("刷新UI");
        lottreyBtn.GetComponent<Image>().sprite = LotteryDataManger.Instance.CanLottery ? canDraw : noDraw;
        mSliderControl.SetSlider(LotteryDataManger.Instance.mdata.fillPro / 100);
        mSliderControl.UpdateSlider(LotteryDataManger.Instance.mdata.fillPro / 100);
        RefrishAwardNum();
    }
    //刷新对应转盘提现金额
    public void RefrishAwardNum()
    {
        var ecpm = LotteryDataManger.Instance.mdata.loteryEcpm;
        if (ecpm == 0)
        {
            fillIcon.sprite = fillNum[0];
            //fillPaperIcon.sprite = fillNum[0];
            lotryText.text = string.Format("?元");
        }
        else if (ecpm <= 100)
        {
            fillIcon.sprite = fillNum[1];
            //fillPaperIcon.sprite= fillNum[1];
            lotryText.text = string.Format("{0}元", 0.3f);
        }
        else if (ecpm > 100 && ecpm <= 200)
        {
            fillIcon.sprite = fillNum[2];
            //fillPaperIcon.sprite = fillNum[2];
            lotryText.text = string.Format("{0}元", 0.5f);
        }
        else if (ecpm >200)
        {
            fillIcon.sprite = fillNum[3];
            //fillPaperIcon.sprite = fillNum[3];
            lotryText.text = string.Format("{0}元", 1f);
        }
    }
    public void BtnSet()
    {
        exitBtn.onClick.AddListener(() => { GameADControl.Instance.ShowIntAd("lottery_half");  Hide(); });
        lottreyBtn.onClick.AddListener(() =>
        {
            if (LotteryDataManger.Instance.mdata.fillPro >= 100 && LotteryDataManger.Instance.mdata.todayCashNum >= 1)
            {
                ShowPublicTip.Instance.Show("今日该额度已提现");
                return;
            }
            if (!WeChatContral.Instance.mWexinIsLogin.Value)
            {
                WeChatContral.Instance.MLogin(()=> {
                  
                });
                return;
            }
            

            if (!isCanShake) return;
            if (LotteryDataManger.Instance.CanLottery)
            {
                //AdControl.Instance.ShowRwAd("lottery_video", () => { 
                var selectNum = RandomNum();
                isCanShake = false;
                //播放音效
                PlayTurnSfx();
                StartTurn(selectNum, () =>
                {
                    //停止播放转盘音效
                    StopTurnsfx();
                    //播放特效
                    drwaEfffect.gameObject.SetActive(true);
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(1f)).Subscribe(_ => {
                        drwaEfffect.gameObject.SetActive(false);
                        CallBack((LotteryDataManger.AwardType)selectNum);
                        isCanShake = true;
                        LotteryDataManger.Instance.AddTurnNums(1);
                        //打点
                        UmengDisMgr.Instance.CountOnNumber("lottery_cj", string.Format("{0}", LotteryDataManger.Instance.mdata.turnNums));
                    });
                });
               
                //});
            }
            else
            {
                UIManager.Instance.ShowPopUp<LotteryTipPanel>((LotteryTipPanel mpanel) =>
                {
                    mpanel.SetDir("闯关可快速获得抽奖券");
                    mpanel.AddListenToBtn(
                        () => { mpanel.Hide(); },
                        () => {
                            UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);
                            mpanel.Hide();
                            Hide();
                            //返回首页最高闯关位置
                        });
                });
            }


        });
        quesBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPopUp<LotteryRulePanel>((LotteryRulePanel mpanel) =>
            {
                mpanel.SetDir("1.抽奖100%概率获得奖励。\n2.每次胜利通关即可获得一张抽奖券。\n3.连续抽奖会增加抽奖券消耗,每次增加1张,上限10张。\n4.每天0点重置抽奖券消耗为1张。");
                mpanel.AddListenToBtn(
                    () => { mpanel.Hide(); },
                    () => { mpanel.Hide(); });
            });
        });

    }
    /// <param name="rangeNum">如果不随机，指定一个数值</param>
    /// <param name="callback">完成回调</param>
    /// <param name="sprites">可选 是否替换图片</param>
    void StartTurn(int rangeNum, TweenCallback callback = null, params Sprite[] sprites)
    {

        float angle = 0;
        angle = 360 * 5 + rangeNum * 360 / probability.Count;
        transTurntableTrans.DOLocalRotate(new Vector3(0, 0, angle), 3, RotateMode.FastBeyond360).OnComplete(callback);

    }

    //播放转盘音效
    void PlayTurnSfx()
    {
        //mdis= Observable.TimeInterval(System.TimeSpan.FromSeconds(.3f)).Subscribe(_ =>
        //{
            AudioMgr.Instance.PlaySFX("转盘转动");
        //});
    }
    //停止转盘音效
    void StopTurnsfx()
    {
        //if (mdis != null)
        //{
        //    mdis.Dispose();
        //}
        AudioMgr.Instance.StopPlaySFX("转盘转动");
    }

    void Init()
    {
        mlotteryConfig = LotteryDataManger.Instance.mLotteryConfigs;
        if (probability.Count == 0)
        {
            foreach (var item in mlotteryConfig)
            {

                probability.Add(item.Awardweight);

            }
        }

        for (int i = 0; i < transTurntableTrans.transform.childCount; i++)
        {
            transTurntableTrans.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = string.Format("X{0}", mlotteryConfig[i].AwardNum);
        }
    }
    //随机抽奖数字
    int RandomNum()
    {
       
        Debug.Log("LotteryDataManger.Instance.mdata.fillPro" + LotteryDataManger.Instance.mdata.fillPro);
        if (LotteryDataManger.Instance.mdata.fillPro >= 100)
        {
            //进度条大于100必得1元
            return 7;
        }
        var x = UnityEngine.Random.Range(0, 101);
        Debug.LogError("随机X" + x);

        foreach (var item in mlotteryConfig)
        {
            if (x < item.weightLimit)
            {
                Debug.LogError("当前随机" + item.ID);
                return item.ID;
            }
        }

        return 0;
    }
    //抽奖回调
    public void CallBack(LotteryDataManger.AwardType mtype)
    {
        #region
        //UIManager.Instance.ShowPopUp<LotteryAwardPanel>((LotteryAwardPanel mpanel) => {
        //    mpanel.mtype = AawardPanelType.Lottery;
        //    mpanel.SetIcon((int)mtype);
        //    mpanel.SetDir(LotteryDataManger.Instance.AwardValue(mtype));
        //    mpanel.AddListenToBtn(
        //        () => { mpanel.Hide(); },
        //        () => {
        //            LotteryDataManger.Instance.AddLotteryAward(mtype);
        //            mpanel.Hide();
        //        });

        //});
        #endregion
        switch (mtype)
        {
            case LotteryDataManger.AwardType.wx10:
                XDebug.Log("获得微信10元");
                break;
            case LotteryDataManger.AwardType.hb:
                RewardHb(LotteryDataManger.Instance.RewardMoudle(mtype));
                XDebug.Log("红包");
                break;
            case LotteryDataManger.AwardType.cash:
                XDebug.Log("现金");
                RewardCash(LotteryDataManger.Instance.RewardMoudle(mtype));
                break;
            case LotteryDataManger.AwardType.wx2:
                XDebug.Log("微信2元");
                break;
            case LotteryDataManger.AwardType.more_Hb:
                RewardHb(LotteryDataManger.Instance.RewardMoudle(mtype));
                XDebug.Log("海量红包");
                break;
            case LotteryDataManger.AwardType.big_Cash:
                RewardCash(LotteryDataManger.Instance.RewardMoudle(mtype));
                XDebug.Log("大量现金");
                break;
            case LotteryDataManger.AwardType.wx1:
                //判断当处于哪个金额
                var mcashNum = LotteryDataManger.Instance.mdata.cashNum;
                var mKey = "";
                if (mcashNum == 0.3f) mKey = WithDrawlKeyControl.LotryCashKey03;
                else if(mcashNum == 0.5f) mKey = WithDrawlKeyControl.LotryCashKey05;
                else if (mcashNum == 1f) mKey = WithDrawlKeyControl.LotryCashKey10;
                WeiChatDrawl(mcashNum, mKey, ()=> { LotteryDataManger.Instance.InitFillPro(); });
                XDebug.Log("微信1元");
                break;
            case LotteryDataManger.AwardType.big_Hb:
                XDebug.Log("大量红包");
                RewardHb(LotteryDataManger.Instance.RewardMoudle(mtype));
                break;
            default:
                break;
        }
        //消耗抽奖券
        LotteryDataManger.Instance.UseLotteryPaper();
        RefrishUI();
        //进度条刷新
       
    }

    //微信提现
    private void WeiChatDrawl(float currGold,string currKey,Action callBack)
    {
        //Debug.Log("currGold" + currGold + "currKey" + currKey);
        WithdrawFeedback.Instance.Withdraw(currGold, currKey, RedWithdrawData.Instance.OnVoucherWD, (b) => {
            if (b)
            {
                UmengDisMgr.Instance.CountOnPeoples("lottery_tx", string.Format("{0}_{1}", LotteryDataManger.Instance.mdata.turnNums, currGold));
                LotteryDataManger.Instance.AddTodaySign(1);
                XDebug.Log("提现成功");
                LotteryDataManger.Instance.SetLoteryEcpm(ConfigMgr.Instance.ecpm);
                callBack.Run();
            }
            
        });
    }
    //奖励红包
    private void RewardHb(float moudel)
    {
        var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon()* moudel;
        Debug.Log("RedWithdrawData.Instance.GetRedIcon()" + RedWithdrawData.Instance.GetRedIcon() + "    " + "moudle" + moudel);
        var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
        var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
        popup2.OnOpen(rewardVoucher, (int)rewardRedIcon, "双倍key", () =>
        {
            popup2.effect.SetActive(false);
            Debug.Log("关闭红包二级界面");
            if (!GameManager.Instance.isCash)
            {
                RedWithdrawData.Instance.UpdateRedIcon((int)rewardRedIcon);
            }
           
        });
    }
    //奖励现金
    private void RewardCash(float moudel)
    {
        var mConfig = LargeCashDataControl.Instance.largeCashConfig;
        var mAddValue = mConfig.AD* moudel;
        Debug.Log("mConfig.AD" + mConfig.AD);
        Debug.Log("moudel" + moudel);
        LargeCashDataControl.Instance.AddtoTotal(mAddValue);
        var mControl = LargeCashDataControl.Instance;
        var mPanel2 = UIManager.Instance.ShowPopUp<LargeCashTwoPanel>();
        mPanel2.RefrishUi(mAddValue, mControl.mData.totalNum, mControl.LastMoney);
        mPanel2.OnOpen(LargeHbType.Nomal,
            () => {

            },
            () => {
                ShowPublicTip.Instance.Show("红包满200元才可以提现哦");
            },
            () => {

            });
    }
    bool isPonterShake()
    {
        var angel = 360 / 8;

        return false;
    }
    #region
    Vector3 leftInitPos = new Vector3(-640f,-368f,0f);
    Vector3 rightInitPos = new Vector3(629f,-368f,0f);
    Vector3 leftTargetPos = new Vector3(-280f,-368f,0);
    Vector3 rightTargetPos = new Vector3(269f,-368f,0f);
    #endregion
    //展示动画
    private void ShowAni()
    {
        var leftRect = leftCloud.GetComponent<RectTransform>();
        var rightRect = rightCloud.GetComponent<RectTransform>();
        leftRect.anchoredPosition = leftInitPos;
        rightRect.anchoredPosition = rightInitPos;

        //移动
        leftRect.DOAnchorPos(leftTargetPos, 1f).SetEase(Ease.OutBack);
        rightRect.DOAnchorPos(rightTargetPos,1f).SetEase(Ease.OutBack);

        //震动
        //leftCloud.GetChild(0).dosh
    }
}
public class LotteryData
{
    public DateTime lastDataTime;
    public int lotteryPaper;//消耗抽奖券
    public int limitPaper;//抽奖券消耗上限
    public int totalLottery;//抽奖券总数
    public float fillPro;//进度条进度
    public float loteryEcpm;//转盘对应ecpm 
    public float cashNum;//对应提现金额
    public int turnNums;//转盘次数
    public int todayCashNum;//今日提现次数
    
    public LotteryData()
    {
        fillPro = 0;
        lotteryPaper = 1;
        limitPaper = 10;
        totalLottery = 0;
        lastDataTime = GameClock.NowTime;
        loteryEcpm = 0;
        cashNum = 0;
        turnNums = 0;
        todayCashNum = 0;
    }
}
public class LotteryDataManger
{
    public const string Local_Key = "LotteryData_Key";

    private static LotteryDataManger mInstance;
    public static LotteryDataManger Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new LotteryDataManger();
            }
            return mInstance;
        }
    }
    bool IsInit = false;
    public LotteryData mdata;
    public System.Action CloseCallBack;
    public LotteryDataManger()
    {
        if (!IsInit)
        {
            Init();
        }
          
    }
    public List<LotteryConfig> mLotteryConfigs
    {
        get
        {
            return
            ConfigGetMgr.Instance.mLotteryConfig();
        }
    }
    private void Init()
    {
     LoadData();
    }

    private void LoadData()
    {
        mdata = SaveGame.Load<LotteryData>(Local_Key);
        if (mdata == null)
        {
            mdata = new LotteryData();
        }
        else
        {
            //隔天刷新
            if (!TimeExtension.IsSameDay(mdata.lastDataTime, GameClock.NowTime))
            {
                mdata.lotteryPaper = 1;
                mdata.todayCashNum = 0;
            }
        }
        SaveData();
    }

    private void SaveData()
    {
        SaveGame.Save(Local_Key, mdata);
    }
    //增加消耗券
    public void LotteryAdd()
    {
        mdata.lotteryPaper++;
        if (mdata.lotteryPaper >= mdata.limitPaper)
        {
            mdata.lotteryPaper = mdata.limitPaper;
        }
    }

    //刷新抽奖券
    public void UpdateLotteryPaper(int value)
    {
        mdata.totalLottery = value;
        SaveData();
    }
    //使用消耗券
    public void UseLotteryPaper()
    {
        if (CanLottery)
        {
            mdata.totalLottery --;
        }
       
        SaveData();
    }
    //添加抽奖券
    public void AddLotteryPaper(int num)
    {
        mdata.totalLottery += num;
        SaveData();
        LotteryPanel.isShowPanel = true;
    }
    //是否可以使用转盘
    public bool CanLottery
    {
        get
        {
          return mdata.totalLottery >= mdata.lotteryPaper; 
        }
    }
    //添加转盘次数
    public void AddTurnNums(int num)
    {
        mdata.turnNums += num;
        SaveData();
    }
    //添加转盘奖励
    public void AddLotteryAward(AwardType mtype)
    {
        var addValue = AwardValue(mtype);
        switch (mtype)
        {
            case AwardType.wx10: 
                break;
            case AwardType.hb:
                break;
            case AwardType.cash:
                break;
            case AwardType.wx2:
                break;
            case AwardType.more_Hb:
                break;
            case AwardType.big_Cash:
                break;
            case AwardType.wx1:
                break;
            case AwardType.big_Hb:
                break;
            default:
                break;
        }
    
    }
    //添加转盘奖励数量
    public int AwardValue(AwardType mtype)
    {
        
        foreach (var item in mLotteryConfigs)
        {
            if (item.ID ==(int)mtype)
            {
                return item.AwardNum;
            }
        }
        return 0;
    }
    public enum AwardType
    {
        wx10=1,//微信10
        hb=2,//红包币
        cash=3,//现金
        wx2=4,//微信2元
        more_Hb=5,//海量红包
        big_Cash=6,//大量现金
        wx1=7,//微信1元
        big_Hb=8,//大量红包
    }
    //随机奖励系数
    public float RewardMoudle(AwardType type)
    {
        foreach (var item in mLotteryConfigs)
        {
            if ((int)type == item.ID)
            {
                return item.AwardCoe;
            }
        }
        return 1;
    }
    //添加进度条进度
    public void AddFillPro(float num)
    {
        mdata.fillPro += num;
        SaveData();
    }
    //清0进度条进度
    public void InitFillPro()
    {
        mdata.fillPro = 0;
        SaveData();
    }

    //设置转盘ecpm 
    public void SetLoteryEcpm(float ecpm)
    {
        mdata.loteryEcpm = ecpm;
        SaveData();
        SetCashNum();
    }
    //设置提现金额
    public void SetCashNum()
    {
        if (mdata.loteryEcpm == 0)
        {
            mdata.cashNum = 0;
        }
        else if (0 < mdata.loteryEcpm && mdata.loteryEcpm <=100)
        {
            mdata.cashNum = 0.3f;
        }
        else if (100<mdata.loteryEcpm && mdata.loteryEcpm<=200)
        {
            mdata.cashNum = 0.5f;
        }
        else if ( mdata.loteryEcpm > 200)
        {
            mdata.cashNum = 1f;
        }
        OrderSystemControl.Instance.awardType.Value = mdata.cashNum;
        SaveData();
    }
    //增加转盘ecpm
    public void AddLoteryEcpm(float ecpm)
    {
        mdata.loteryEcpm += ecpm;
        SaveData();
    }

    //添加今日添加签到
    public void AddTodaySign(int num)
    {
        mdata.todayCashNum += num;
        SaveData();
    }
}
public class ConfigGetMgr:Singlton<ConfigGetMgr>
{
    public List<LotteryConfig> mLotteryConfig()
    {

        var configs = ConfigMgr.Instance.mConfigData.GetList<LotteryConfig>();
        if (configs != null)
        {
            return configs;
        }
        else
        {
            return null;
        }
    }

    public List<SevenWithDrawConfig> mSevenWithDrawConfigs()
    {
        var configs = ConfigMgr.Instance.mConfigData.GetList<SevenWithDrawConfig>();
        if (configs != null)
        {
            return configs;
        }
        else
        {
            return null;
        }
    }

    public List<RandomName> mRandomNameConfig()
    {
        var configs = ConfigMgr.Instance.mConfigData.GetList<RandomName>();
        if (configs != null)
        {
            return configs;
        }
        else
        {
            return null;
        }
    }

    public List<SignRedConfig> mSignRedConfigs()
    {
        var configs = ConfigMgr.Instance.mConfigData.GetList<SignRedConfig>();
        if (configs != null)
        {
            return configs;
        }
        else
        {
            return null;
        }
    }

    public List<LargeCashConfig> mLargeCashConfig()
    {
        var configs = ConfigMgr.Instance.mConfigData.GetList<LargeCashConfig>();
        if (configs != null)
        {
            return configs;
        }
        else
        {
            return null;
        }

    }
}