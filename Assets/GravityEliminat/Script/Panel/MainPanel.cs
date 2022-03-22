using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
public class MainPanel : UIBase
{
    #region Top
    public Transform topParent;


    public MagicFly magicFly1;
    //public MagicFly magicFly2;

    public Text loveText;
    public Text LoveCuntDown;
    public Text moneyText;
    public Text dimondText;

    public IButton loveBtn;
    public IButton cashBtn;
    public IButton dimoneBtn;
    public IButton pauseBtn;
    public IButton largeCashBtn;
    #endregion

    #region Left
  public  Treasure treasure;
    //public Button StarBox;//星光宝箱
    //public Image StarBoxpro;
    //public Text StarText;//星光宝箱星星数量
    //public Transform starFull;


    //public Image challengePro;
    //public Text challengeText;
    //public Transform challengeFull;
    #endregion
    #region
    public IButton fastStartBtn;
    [Header("挑战宝箱")]
    public Button ccBtn;
    [Header("存钱罐")]
    public Button pigCoinBtn;
    [Header("每日提现")]
    public Button dayCashBtn;
    [Header("闯关抽奖")]
    public Button passLevelBtn;
    [Header("实物抽奖")]
    public Button objectLotteryBtn;
    [Header("分红")]
    public Button shareRedBtn;
    [Header("任务成就")]
    public Button achiTaskBtn;
    [Header("签到")]
    public Button signBtn;
    [Header("红包测试")]
    public Button redBagTest;
    [Header("提现成功测试按钮")]
    public Button cashSucTestBtn;
    [Header("QA")]
    public Button QaBtn;
    public IButton autoPosBtn;
    #endregion
    public float ScreenFitPos;
    private void Awake()
    {
        base.Awake();

        if ((Screen.height / Screen.width) >= 2)
        {
            ScreenFitPos = 40;
        }
        else
        {
            ScreenFitPos = 0;
        }

        topParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -ScreenFitPos);

        pigCoinBtn.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(pigCoinBtn.transform.parent.GetComponent<RectTransform>().anchoredPosition.x, pigCoinBtn.transform.parent.GetComponent<RectTransform>().anchoredPosition.y- ScreenFitPos);

        signBtn.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(signBtn.transform.parent.GetComponent<RectTransform>().anchoredPosition.x, signBtn.transform.parent.GetComponent<RectTransform>().anchoredPosition.y - ScreenFitPos);


        Observable.Interval(System.TimeSpan.FromSeconds(40)).Subscribe(_ => {

            //Debug.Log();
            magicFly1.BegainFly("Main");
            //magicFly2.BegainFly();


        });

        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ => {
            magicFly1.BegainFly("Main");
        });

    }

    void Start()
    {
        fastStartBtn.onClick.AddListener(()=> {


            if (autoPosBtn.gameObject.activeSelf)
            {
                InfiniteScrollView.Instance.AutomaticPos(() =>
                {

                    UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);

                });
            }
            else { 
                    UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);
            }



        });
        //GameManager.Instance.LoveStar.Subscribe(_ => { });
        TimeClock.NowTimeListening.Subscribe(_ => {

            LoveCuntDown.text = TimeMgr.Instance.LoveString;

        });
        //EventManager.Instance.AddEvent(MEventType.RefreshTresure,RefreshTresure);

        autoPosBtn.onClick.AddListener(() => {

            InfiniteScrollView.Instance.AutomaticPos();
        
        });

        pauseBtn.onClick.AddListener(() => {


            UIManager.Instance.Show<FrontSetPop>(UIType.PopUp);
        });

        //StarBox.onClick.AddListener(() => {

        //    if (DataManager.Instance.data.StarshineStar >= 10)
        //    {
        //        UIManager.Instance.Show<TreasurePop>(UIType.PopUp, TreasureType.Starlight);

        //    }
        //    else { 
            
            
        //    }

        //});

        //GameManager.Instance.StarShineStarSub.Subscribe(_ => {

        //    if (_ < 10)
        //    {
        //        StarText.text = string.Format("{0}/10", _);
        //        starFull.localEulerAngles = new Vector3(0, 0, 0);
        //        starFull.GetComponent<DOTweenAnimation>().DOPause();
        //        starFull.GetChild(0).gameObject.SetActive(false);
        //    }
        //    else {
        //        StarText.text = "点击开启";
        //        starFull.localEulerAngles=new Vector3 (0, 0, -5);
        //        starFull.GetComponent<DOTweenAnimation>().DOPlay();
        //        starFull.GetChild(0).gameObject.SetActive(true);
        //    }
        //    StarBoxpro.fillAmount =(float) _ / 10;

        //});
        GameManager.Instance.DiamondSub.Subscribe(_ => {
            dimondText.text = _.ToString();
        }
        );
        loveBtn.onClick.AddListener(()=> {
            UmengDisMgr.Instance.CountOnNumber("tl_icon");
           AddPopMgr.Instance.isPassivity = false;
            UIManager.Instance.Show<AddPop>(UIType.PopUp,AddEumn.Love);
           
        });
        dimoneBtn.onClick.AddListener(() => { 
        
            UIManager.Instance.Show<AddPop>(UIType.PopUp,AddEumn.Diamond);

        });
        GameManager.Instance.LoveStar.Subscribe(_ => {
            loveText.text = _.ToString();
            if (_ >= 10)
            {
                LoveCuntDown.transform.gameObject.SetActive(false);
            }
            else { 
                LoveCuntDown.transform.gameObject.SetActive(true);
            }
        });
        //提现刷新
        RedWithdrawData.Instance.RedCoin.Subscribe(value => {
            moneyText.text = string.Format("{0}<size=23>元</size>", (value / 10000).ToString("f2"));
        });
        //地图初始化
        InfiniteScrollView.Instance.Init();

        //添加按钮点击事件
        AddLinser();
        ccBtn.onClick.AddListener(() =>
        {
            if (DataManager.Instance.data.ChallengeStar>=10)
            {
                UIManager.Instance.Show<TreasurePop>(UIType.PopUp, TreasureType.Challenge);
            }
        });

        LargeCashDataControl.Instance.largeCash.Subscribe(value => {
            largeCashBtn.transform.GetChild(1).GetComponent<Text>().text = string.Format("{0}<size=10>元</size>", value.ToString("f2"));
        });


        //完成前5关方可自动弹界面
        if (DataManager.Instance.data.UnlockLevel > 5)
        {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(1f)).Subscribe(_ =>
            {
                ShowDailyInfoDataControl.Instance.DailyShowSignPanel();
            });
        }

       

    }

    public override void Show()
    {
        base.Show();
        RefashPage();
        AudioMgr.Instance.PlayMusic("首页BGM");

     
        ShowGuide();

        //首次加载成功进入到游戏界面人数
        UmengDisMgr.Instance.CountOnPeoples("FirstEnterLoadingsuccess");
        GameManager.Instance.GetTicket();

       
    }

    public override void Refresh()
    {
        base.Refresh();
        RefashPage();

    }

    public void ShowGuide()
    {
        //新手引导--第一步
        if (!GuideMgr.Instance.GuideIsComplete(1))
        {
            GuideMgr.Instance.ShowGuide_1(() =>
            {
                XDebug.Log("开始第三步~");
                GuideMgr.Instance.ShowGuide_3(() =>
                {

                    XDebug.Log("开始第四步~");
                });
            });
        }
         //第三步
         else if (!GuideMgr.Instance.GuideIsComplete(3))
        {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ =>
                {
                    GuideMgr.Instance.ShowGuide_3(() => {
                        XDebug.Log("展示第四步~");
                    });
                });
           
        }
    }
    

    public void SetAutoPosBtn(bool show) {
        
        autoPosBtn.gameObject.SetActive(show);
    
    }
    public void RefashPage() {



        treasure.Init();

        
        //旧宝箱刷新
        //if (DataManager.Instance.data.ChallengeStar< 10)
        //{
        //    challengeText.text = string.Format("{0}/10", DataManager.Instance.data.ChallengeStar);
        //    challengeFull.eulerAngles = new Vector3(0, 0, 0);
        //    challengeFull.GetComponent<DOTweenAnimation>().DOPause();
        //    challengeFull.GetChild(0).gameObject.SetActive(false);
        //}
        //else
        //{
        //    challengeText.text = "点击开启";
        //    challengeFull.eulerAngles = new Vector3(0, 0, -5);
        //    challengeFull.GetComponent<DOTweenAnimation>().DOPlay();
        //    challengeFull.GetChild(0).gameObject.SetActive(true);
        //}
        //challengePro.GetComponent<Image>().fillAmount = (float)DataManager.Instance.data.ChallengeStar / 10;

        //显示七日提现
        //dayCashBtn.gameObject.SetActive(ShowDailyInfoDataControl.Instance.IsShowSevenDayCash);
        //ShowDailyInfoDataControl.Instance.DailyShowSevenPanel();
        //ShowDailyInfoDataControl.Instance.CompleteShowSevenPanel();

        //获得抽奖券弹抽奖弹窗
        
    }


    //按钮点击事件
    private void AddLinser()
    {
        cashBtn.onClick.AddListener(() => {
            var mpanel = UIManager.Instance.ShowPopUp<WithdrawUI>();
            mpanel.UpdateUI();
        });
        pigCoinBtn.onClick.AddListener(() => {
            var mpanel = UIManager.Instance.ShowPopUp<PiggyBankUI>();
            mpanel.OnRefresh();
        });
        redBagTest.onClick.AddListener(() => {

            var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
            var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
            var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
            popup1.OnOpen("", rewardRedIcon, () => {
                //打开回调
                var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                {
                    popup2.effect.SetActive(false);
                    Debug.Log("关闭红包二级界面");

                });
                popup1.defult.SetActive(false);
            },
            () =>
            {
                //关闭回调
                Debug.Log("关闭红包一级界面");
                popup1.defult.SetActive(false);
            });
        });

        signBtn.onClick.AddListener(() => {

            UIManager.Instance.ShowPopUp<SignPanel>((SignPanel mpanel) => {

                mpanel.AddListenToBtn(
                    () => { GameADControl.Instance.ShowIntAd("daily_half"); mpanel.Hide(); },
                    () => {
                        if (SignDataControl.Instance.mdata.canSign)
                        {
                            AdControl.Instance.ShowRwAd("daily_sign_video", () => {
                                Debug.Log("mpanel.mSignType.awardType)" + mpanel.mSignType.awardType);
                                SignDataControl.Instance.AddSignIndex(mpanel.mSignType.awardType);
                                //签到打点
                                UmengDisMgr.Instance.CountOnPeoples("daily_suc", string.Format("{0}", SignDataControl.Instance.mdata.signDay));
                            });
                            
                        }
                        else
                        {
                            ShowPublicTip.Instance.Show("今天已签到~");
                        }                       
                     },
                    () => {
                        var nowType = SignDataControl.Instance.nowTimeType.Value;
                        if (nowType == SignDataControl.TimeType.Stop)
                        {
                            AdControl.Instance.ShowRwAd("daily_online_video", () =>
                            {
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
                                SignDataControl.Instance.SetOnline();
                                mpanel.RefrishUi();
                                });
                            }); 
                        }
                        else
                        {
                            ShowPublicTip.Instance.Show("倒计时结束即可领取~");
                        }
                    });

            });

        });
        passLevelBtn.onClick.AddListener(() => {
            UIManager.Instance.ShowPopUp<LotteryPanel>();
        });
        dayCashBtn.onClick.AddListener(() => {
            UIManager.Instance.ShowPopUp<SevenWithdrawPanel>();
        });
        shareRedBtn.onClick.AddListener(() => {
            UIManager.Instance.ShowPopUp<ShareRedPanel>();
        });
        largeCashBtn.onClick.AddListener(() => {

            var mConfig = LargeCashDataControl.Instance.largeCashConfig;
            var mAddValue = mConfig.AD;
            //LargeCashDataControl.Instance.AddtoTotal(mAddValue);
            var mControl = LargeCashDataControl.Instance;
            var mPanel2 = UIManager.Instance.ShowPopUp<LargeCashTwoPanel>();
            mPanel2.RefrishUi(mAddValue, mControl.mData.totalNum, mControl.LastMoney);
            mPanel2.OnOpen(LargeHbType.Weichat,
                () => {

                },
                () => {
                    ShowPublicTip.Instance.Show("红包满200元才可以提现哦");
                },
                () => {

                });

        });

        cashSucTestBtn.onClick.AddListener(() => {
            var mpanel = UIManager.Instance.ShowPopUp<WithdrawSucPanel>();
            //真
            var mdata = AudioMgr.Instance.mdate;
            var mSucData = WithdrawSucManger.Instance.mdata;
            //mpanel.RefrishUi(mdata.icon, mdata.weiChatName, 0.3f, mSucData.totalMoney);
            //假
            mpanel.RefrishUi(WithdrawSucManger.Instance.RandomIcon(), WithdrawSucManger.Instance.RandomName(), WithdrawSucManger.Instance.RandomMoney(), WithdrawSucManger.Instance.RandomTotalMoney());
        });
        QaBtn.onClick.AddListener(() => {
            UIManager.Instance.ShowPopUp<GameQAPanel>();
        });
    }

    //大额红包测试
    public void BigRedTest()
    {
        var mpanel = UIManager.Instance.ShowPopUp<LargeCashPanel>();
        var mConfig = LargeCashDataControl.Instance.largeCashConfig;

        mpanel.OnOpen("", mConfig.FM,
        () => {
            LargeCashDataControl.Instance.AddtoTotal(mConfig.AD);
            var mControl = LargeCashDataControl.Instance;
            var mPanel2 = UIManager.Instance.ShowPopUp<LargeCashTwoPanel>();
            mPanel2.RefrishUi(mConfig.AD, mControl.mData.totalNum, mControl.LastMoney);
            mPanel2.OnOpen(LargeHbType.Nomal,
                () => {

                },
                () => {
                    ShowPublicTip.Instance.Show("红包满200元才可以提现哦");
                },
                () => {

                });
        },
        () => {

        }
        );
    }
}
