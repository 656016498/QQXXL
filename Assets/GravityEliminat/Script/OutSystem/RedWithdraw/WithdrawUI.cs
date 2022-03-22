using DG.Tweening;
using EasyExcelGenerated;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WithdrawUI : UIBase
{
    //红包币，现金，当前红包卷进度
    public Text txtRedVolume, txtVolume, txtRedCash, txtPro,txtTime,txtHint,txtTimeH,txtNeed,txtHave,texSafety;
    //关闭，问号，提现记录，抽奖, 提现
    public IButton btnClose, btnQuestion, btnRecord, btnLucky,btnWD;
    //红包卷进度条
    public Image imgPro,imgFinger;
    //提现卷按钮
    //public List<Toggle> tglVouchers;
    //红包币提现按钮
    public List<Toggle> tglRedIcon;
    //抽奖提现选框
    public Toggle toggleLucky;
    //抽奖一二三档切换框
    public Toggle toggleGears1, toggleGears2, toggleGears3;
    //抽奖列表
    public List<LuckyItem> luckyItems;
    //抽奖动画时长
    public float luckyTime = 5;
    private List<Transform> luckyItmeTransform;
    //提现卷提现配置
    private List<WithdrawConfig> configVouchers;
    //红包币提现配置
    private List<WithdrawConfig> configRedIcon;

    //当前提现类型
    private WithdrawType currType;
    //当前提现金额
    private float currGold;
    //当前提现Key
    private string currKey;
    //转盘显示配置
    private List<LuckyConfig> luckyConfigs;
    //当前抽奖配置
    private LuckyConfig currLuckyConfig;

    //当前抽奖item值
    private List<float> currLuckyItemValue;
    private Tween twen;

    //当前转盘是否可抽奖
    private bool canLucky;
    private Transform lastLuckyT;
    private bool isStaring;
    public ScrollRect scrollView;
    public RectTransform red;
    private Toggle currLuckyTgl;
    //是否首次登陆
    private bool isFirstLoging;
    private bool isFirstOpen;
    public Action closeCallBack = null;
    [Header("用户协议")]
    public IButton yhxy;
    [Header("隐私政策")]
    public IButton yszc;
    public override void Awake()
    {
        base.Awake();
        currLuckyItemValue = new List<float>();
        luckyItmeTransform = new List<Transform>();
        for (int i = 0; i < luckyItems.Count; i++)
        {
            luckyItmeTransform.Add(luckyItems[i].transform);
        }
     
    }
 
    // Start is called before the first frame update
    protected void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnQuestion.onClick.AddListener(OnQuestion);
        btnRecord.onClick.AddListener(OnRecord);
        btnLucky.onClick.AddListener(OnLucky);
        btnWD.onClick.AddListener(OnWD);
        yhxy.onClick.AddListener(() => { AdControl.Instance.ShowUser(); });
        yszc.onClick.AddListener(() => { AdControl.Instance.ShowProtocol(); });
        #region
        //toggleGears1.onValueChanged.AddListener(OnGears1);
        //toggleGears2.onValueChanged.AddListener(OnGears2);
        //toggleGears3.onValueChanged.AddListener(OnGears3);
        //toggleLucky.onValueChanged.AddListener(OnLuckyTgl);
        //currLuckyConfig = RedWithdrawData.Instance.GetLuckyConfig(1, true);//默认取一档配置，使用当前关卡等级
        //设置转盘
        //luckyConfigs = new List<LuckyConfig>();
        //var allconfig = ConfigMgr.Instance.GetCongigLsit<LuckyConfig>();
        //luckyConfigs.Add(allconfig.Find(x => x.tableType == 2));
        //luckyConfigs.Add(allconfig.Find(x => x.tableType == 3));
        //luckyConfigs.Add(allconfig.Find(x => x.tableType == 4));
        //toggleGears1.isOn = false;
        //toggleGears1.isOn = true;
        #endregion
        //设置提现金额
        isFirstOpen = true;
        SetGold();
        UpdateUI();

        #region
        //LotteryExtension.LotteryAni.mLunboingTrans.Subscribe(t => {
        //    if (t == null) return;
        //    //Debug.Log(t);
        //    t.GetComponent<LuckyItem>().SetHL(true);
        //    if (lastLuckyT)
        //    {
        //        lastLuckyT.GetComponent<LuckyItem>().SetHL(false);
        //    }
        //    lastLuckyT = t;
        //}).AddTo(this);
        #endregion
    }

    private void OnLuckyTgl(bool arg0)
    {
        if (arg0)
        {
            
            currType = WithdrawType.Lucky;
            currGold = RedWithdrawData.Instance.redData.currLuckyRmb;
            currKey = RedWithdrawData.Instance.redData.currLuckyKey;
            Debug.Log("currType:"+ currType);
            Debug.Log("currGold:" + currGold);
            Debug.Log("currKey:" + currKey);
            
        }
        if (imgFinger.gameObject.activeSelf)
        {
            imgFinger.gameObject.SetActive(false);
        }
    }

    private void SetItemValue()
    {
        currLuckyItemValue.Clear();
        currLuckyItemValue.Add(currLuckyConfig.item1);
        currLuckyItemValue.Add(currLuckyConfig.item2);
        currLuckyItemValue.Add(currLuckyConfig.item3);
        currLuckyItemValue.Add(currLuckyConfig.item4);
        currLuckyItemValue.Add(currLuckyConfig.item5);
        currLuckyItemValue.Add(currLuckyConfig.item6);
        currLuckyItemValue.Add(currLuckyConfig.item7);
        currLuckyItemValue.Add(currLuckyConfig.item8);
        //Debug.Log("SetValue,id:" + currLuckyConfig.id);
    }
   

    public void UpdateUI()
    {
        if (!isFirstOpen) return;
        var redD = RedWithdrawData.Instance.redData;
        txtRedVolume.text = string.Format("{0} <size=22>红包币</size>", redD.redIcon.ToString());//红包币
        txtRedCash.text = string.Format("≈ {0}元",((float)redD.redIcon/10000).ToString("f2"));//现金红包
        txtVolume.text = string.Format("当前拥有<color=#fffd6f>{0}</color>张", redD.voucher.ToString());//红包卷
        txtPro.text = string.Format("{0}%",(redD.voucherPro*100).ToString("f2"));//红包卷进度
        imgPro.fillAmount = redD.voucherPro;

        UpdateRedTgl();
        #region
        ////更新提现卷提现按钮状态
        //var dayData = RedWithdrawData.Instance.redDayData;
        //for (int i = tglVouchers.Count-1; i >=0; i--)
        //{
        //    if (dayData.todayWDs.Contains(configVouchers[i].gold))
        //    {
        //        tglVouchers[i].interactable = false;
        //        tglVouchers[i].isOn = false;
        //        tglVouchers[i].transform.Find("Image").gameObject.SetActive(false);
        //    }
        //    else if(redD.voucher>0)
        //    {
        //        tglVouchers[i].isOn = true;
        //    }
        //}

        //if (tglVouchers[0].isOn == true)
        //{
        //    tglVouchers[0].transform.Find("finger").gameObject.SetActive(true);
        //}
        //else
        //{
        //    tglVouchers[0].transform.Find("finger").gameObject.SetActive(false);
        //}
        //更新抽奖相关
        //txtHave.text = string.Format("拥有<color=#ffe049>{0}</color>张提现卷", redD.voucher.ToString());
        //currLuckyTgl.isOn = false;
        //currLuckyTgl.isOn = true;
        #endregion
        #region
        //用服务器时间
        //DateTime now = GameTime.GameClock.NowTime;
        //TimeSpan timeSpan = now - redD.lastLucky;
        //if (timeSpan.TotalSeconds>=RedWithdrawData.Instance.luckyWDTime)
        //{
        //    //tglRedIcon[tglRedIcon.Count-1].isOn = true;
        //    toggleLucky.interactable = false;
        //    toggleLucky.transform.Find("Label").GetComponent<Text>().text = "??<size=22>元</size>";
        //    toggleLucky.transform.Find("Image").gameObject.SetActive(false);
        //    txtHint.gameObject.SetActive(true);
        //    txtTimeH.gameObject.SetActive(false);
        //    if (redD.lastLucky!=DateTime.MinValue)
        //    {
        //        Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
        //            var ui = UIManager.Instance.ShowPopUp<MissWithdrawUI>();
        //            int missicon = RedWithdrawData.Instance.GetMissIcon();
        //            ui.OnShow(missicon, () => {
        //                float mvalue = RedWithdrawData.Instance.redData.redIcon;
        //                DOTween.To(() => mvalue, x => mvalue = x, RedWithdrawData.Instance.redData.redIcon + missicon, 2.5f).OnUpdate(() => {
        //                    txtRedVolume.text = mvalue.ToString("f0");
        //                });

        //                RedWithdrawData.Instance.redData.redIcon += missicon;
        //                RedWithdrawData.Instance.UpdateLuckyTime(DateTime.MinValue);
        //                UmengDisMgr.Instance.CountOnNumber("comfort_hb_get");


        //            });

        //        });

        //        //UpdateUI();
        //    }

        //}
        //else
        //{
        //    toggleLucky.isOn = true;
        //    toggleLucky.interactable = true;
        //    toggleLucky.transform.Find("Label").GetComponent<Text>().text = redD.currLuckyRmb+"<size=22>元</size>";
        //    toggleLucky.transform.Find("Image").gameObject.SetActive(true);
        //    txtHint.gameObject.SetActive(false);
        //    txtTimeH.gameObject.SetActive(true);
        //    CountDownTime(timeSpan);
        //}
        #endregion
    }
    //刷新红包币提现Tog
    private void UpdateRedTgl()
    {
        var red = RedWithdrawData.Instance.redData;
        for (int i = 0; i < configRedIcon.Count; i++)
        {
            tglRedIcon[i].transform.Find("Image").GetChild(0).GetComponent<Text>().text = string.Format("今日{0}次", RedWithdrawData.Instance.DayCashNums(configRedIcon[i].gold));
        }
        //默认显示第一个
        tglRedIcon[0].isOn = true;
        return;
        #region
        //tglRedIcon[0].isOn = false;
        //tglRedIcon[0].isOn = true;
        //for (int i = 0; i < configRedIcon.Count; i++)
        //{
        //    if (red.records.Find(x=>x.key== configRedIcon[i].cost.ToString())!=null)
        //    {
        //        tglRedIcon[i].interactable = false;
        //        tglRedIcon[i+1].isOn = true;
        //    }
        //}
        #endregion
    }
    private void UpdateSecurity(int index)
    {
        
        var config = RedWithdrawData.Instance.GetSecurityNum(index);
        if (config!=null)
        {
            int todayLucky = RedWithdrawData.Instance.redDayData.todayLuckys[GetCurrLuckyTimes()] + 1;
            if (config != null)
            {
                int num = config.luckys - todayLucky;
                if (num == 0)
                {
                    texSafety.text = string.Format("本次抽奖必获得<color=#FFF728>{0}元</color>提现", config.value);
                }
                else
                {
                    texSafety.text = string.Format("再抽<color=#bdfdff><size=40> {0}次 </size></color>，必获得<color=#FFF728><size=40> {1}元 </size></color>提现", num, config.value);
                }
            }
            else
            {
                texSafety.text = "";
                Debug.LogWarning("无法获得保底配置，请检查配置文件。");
            }
            currLuckyConfig = RedWithdrawData.Instance.GetLuckyConfig(index, isFirstLoging);
        }
        else
        {
            texSafety.text = string.Format("今日已无抽奖次数，请尝试其他转盘。");
            currLuckyConfig = null;
        }
        
       
    }
    private void UpdateLucky(int index)
    {
        var config = luckyConfigs[index];
        txtNeed.text = string.Format("抽奖-{0}",config.cost);
        SetLuckyItem(0,config.item1);
        SetLuckyItem(1, config.item2);
        SetLuckyItem(2, config.item3);
        SetLuckyItem(3, config.item4);
        SetLuckyItem(4, config.item5);
        SetLuckyItem(5, config.item6);
        SetLuckyItem(6, config.item7);
        SetLuckyItem(7, config.item8);
        currLuckyConfig = RedWithdrawData.Instance.GetLuckyConfig(index+1, isFirstLoging);
        //Debug.Log("luckyID:"+currLuckyConfig.id);
        UpdateSecurity(index + 1);
        if (true)//MyGameInfo.instance.Alllevel>=config.level[0])
        {
            canLucky = true;
        }
        else
        {
            canLucky = false;
        }

    }
    private void OnGears1(bool arg0)
    {
        if (arg0)
        {
            UpdateLucky(0);
            currLuckyTgl = toggleGears1;
        }
    }

    private void OnGears2(bool arg0)
    {
        if (arg0)
        {
            UpdateLucky(1);
            currLuckyTgl = toggleGears2;
        }
    }

    private void OnGears3(bool arg0)
    {
        if (arg0)
        {
            UpdateLucky(2);
            currLuckyTgl = toggleGears3;
        }
    }
    public void OnTgl03(bool b)
    {
        if (b)
        {
            tglRedIcon[0].transform.Find("finger").gameObject.SetActive(false);
        }
    }
    private void SetLuckyItem(int index,float v)
    {
        
        string value = v.ToString();
        luckyItems[index].SetValue(value);
    }
    private void SetGold()
    {
        var WDConfig = ConfigMgr.Instance.GetCongigLsit<WithdrawConfig>();
        //提现卷配置
        configVouchers = WDConfig.FindAll(x => x.withdrawType == 2);
        for (int i = 0; i < configVouchers.Count; i++)
        {
            tglRedIcon[i].transform.Find("Label").GetComponent<Text>().text = string.Format("{0}<size=22>元</size>", configVouchers[i].gold.ToString());
            tglRedIcon[i].onValueChanged.AddListener(GetToggleAction(configVouchers[i],WithdrawType.Vouchers));
        }
        //红包币配置
        configRedIcon = WDConfig.FindAll(x => x.withdrawType == 2);
        for (int i = 0; i < configRedIcon.Count; i++)
        {
            tglRedIcon[i].transform.Find("Label").GetComponent<Text>().text = string.Format("{0}<size=22>元</size>", configRedIcon[i].gold.ToString());
            tglRedIcon[i].onValueChanged.AddListener(GetToggleAction(configRedIcon[i], WithdrawType.RedIcon));
        }
    }

    private UnityAction<bool> GetToggleAction(WithdrawConfig config,WithdrawType type)
    {
        return (b) => {
            if (b)
            {
                currType = type;
                currGold = config.gold;
                currKey = config.key;
                XDebug.Log("当前提现类型：" + currType);
                XDebug.Log("当前提现金额：" + currGold);
                XDebug.Log("当前提现key：" + currKey);
            }
        };
    }

    //倒计时
    private void CountDownTime(TimeSpan span)
    {

        if (twen!=null&&twen.IsPlaying())
        {
            return;
        }
        var time = RedWithdrawData.Instance.luckyWDTime - (float)span.TotalSeconds;

        twen = DOTween.To(() => time, x => time = x, 0, time).SetEase(Ease.Linear).OnUpdate(() =>
        {
            txtTime.text = string.Format("{0}:{1}", ((int)time/60).ToString("d2"), ((int)time % 60).ToString("d2"));
            
        }).OnComplete(() =>
        {
            tglRedIcon[0].isOn = true;
            toggleLucky.interactable = false;
            toggleLucky.transform.Find("Label").GetComponent<Text>().text = "??<size=22>元</size>";
            toggleLucky.transform.Find("Image").gameObject.SetActive(false);
            txtHint.gameObject.SetActive(true);
            txtTimeH.gameObject.SetActive(false);
        });
    }

    private void OnClose()
    {
        twen.Kill();
        
        UIManager.Instance.Hide<WithdrawUI>();
        if (closeCallBack != null)
        {
            closeCallBack.Run();
            closeCallBack = null;
        }
        GameADControl.Instance.ShowIntAd("tx_half");
    }

    private void OnQuestion()
    {
        var ui = UIManager.Instance.ShowPopUp<ExplainUI>();
        var list = new List<string>();
        list.Add(@"<size=30><color=#8F504E>红包币提现说明：</color></size>
<size=25><color=#8F504E>1、满足提现需求红包币后即可立即提现！
2、提现档位共6个，0.3元和0.4元每日可分别提现5次，0.5元们每日可提现3次，1元、10元、100元每日可分别提现1次！
3、所有档位每天0点刷新次数.
4、可选择任意档位提现，无需按顺序来！</color></size>");
        list.Add(@"<size=30><color=#8F504E>提现失败原因：</color></size>
<size=25><color=#8F504E>1.游戏未绑定微信
2.微信未绑定银行卡
3.今日提现次数达到5次
4.今日提现人数过多,累计金额达到100万元。
5.玩家所在的地区,网络波动导致异常。</color></size>");
        ui.EnterPanelUpdate(list);

        
        
    }

    private void OnRecord()
    {
        var ui = UIManager.Instance.ShowPopUp<WithdrawRecordUI>();
        ui.OnOpen(RedWithdrawData.Instance.redData.records);
        UmengDisMgr.Instance.CountOnNumber("txjl_show");
 
    }

    private void OnLucky()
    {
        return;
        if (!canLucky) {
            //弹出提示
            ShowText("关卡等级不足");
            return;
        }
        if (currLuckyConfig == null)
        {
            //弹出提示
            ShowText("抽奖次数超限\n请尝试其他档次抽奖或明日再来。");
            return;
        }
        if (RedWithdrawData.Instance.redData.voucher < currLuckyConfig.cost)
        {
           //弹出提示
            ShowText("提现券不足，无法抽奖！\n开红包即能获得提现券进度！");
            return;
        }
       
        var times = GetCurrLuckyTimes();
        UmengDisMgr.Instance.CountOnNumber("cjtx_join", (RedWithdrawData.Instance.redDayData.todayLuckys[times] + 1).ToString());
        btnLucky.interactable = false;
        btnClose.interactable = false;
        toggleGears1.interactable = false;
        toggleGears2.interactable = false;
        toggleGears3.interactable = false;
        SetItemValue();
        
        //判断中奖类型
        int target;
        float luckyReward = 0;
        if (currLuckyConfig.rewardType==1|| currLuckyConfig.rewardType == 4)
        {
            luckyReward = currLuckyConfig.value;
        }
        else
        {
            LuckySection secton = RedWithdrawData.Instance.GetLuckySection();
            if (currLuckyConfig.rewardType == 2)
            {
                luckyReward = secton.nearQuota;
            }else 
            {
                luckyReward = secton.bigQuota;
            }
        }

        target = currLuckyItemValue.FindIndex(x => x == luckyReward)+1;


        //Debug.Log("luckyReward:"+luckyReward);
        //Debug.Log("target:" + target);
        //for (int i = 0; i < currLuckyItemValue.Count; i++)
        //{
        //    Debug.Log("target"+i + currLuckyItemValue[i]);
        //}
        LotteryExtension.LotteryAni.PlayOrderSkipAni(luckyItmeTransform, target, Ease.InOutQuad, 5, luckyTime, () => {
            lastLuckyT.GetComponent<LuckyItem>().OnBlink(()=> {
                btnLucky.interactable = true;
                btnClose.interactable = true;
                toggleGears1.interactable = true;
                toggleGears2.interactable = true;
                toggleGears3.interactable = true;
                //RedWithdrawData.Instance.redData.currLuckyRmb = luckyReward;

                //RedWithdrawData.Instance.UpdateVoucher(-currLuckyConfig.cost);
                RedWithdrawData.Instance.redData.voucher -= currLuckyConfig.cost;
                RedWithdrawData.Instance.redDayData.todayVucher -= currLuckyConfig.cost;
                var index = currLuckyConfig.tableType - 2;
                if (index < 0) index = 0;
                RedWithdrawData.Instance.UpdateLuckys(index);

                DateTime now = DateTime.Now;//服务器时间
                TimeSpan timeSpan = now - RedWithdrawData.Instance.redData.lastLucky;
                if (timeSpan.TotalSeconds < RedWithdrawData.Instance.luckyWDTime)
                {
                    var pop = UIManager.Instance.ShowPopUp<ReplaceUI>();
                    pop.OnShwo(luckyReward, () => {
                        twen.Kill();
                        twen = null;
                        RedWithdrawData.Instance.redData.currLuckyRmb = luckyReward;
                        RedWithdrawData.Instance.redData.currLuckyKey = currLuckyConfig.withdrawKey;
                        RedWithdrawData.Instance.UpdateLuckyTime(now);
                        var ui = UIManager.Instance.ShowPopUp<AcquireLuckyUI>();
                        ui.OnShow(luckyReward);
                        scrollView.content.DOAnchorPosY(0, 0.5f);
                        imgFinger.gameObject.SetActive(true);
                        
                        UpdateUI();
                    }, () => {
                        twen.Play();
                        UpdateUI();
                    });


                    twen.Pause();
                }
                else
                {
                    RedWithdrawData.Instance.redData.currLuckyRmb = luckyReward;
                    RedWithdrawData.Instance.redData.currLuckyKey = currLuckyConfig.withdrawKey;
                    RedWithdrawData.Instance.UpdateLuckyTime(now);

                    var ui = UIManager.Instance.ShowPopUp<AcquireLuckyUI>();
                    ui.OnShow(luckyReward);
                    scrollView.content.DOAnchorPosY(0, 0.5f);
                    imgFinger.gameObject.SetActive(true);
                    
                    UpdateUI();
                }
            });
           
        });

    }

    private void OnWD()
    {
        //btnWD.interactable = false;
        switch (currType)
        {
            case WithdrawType.Vouchers:
                //VoucherWithdraw();
                break;
            case WithdrawType.RedIcon:
                RedIconWithdraw();

                break;
            case WithdrawType.Lucky:
                LuckyWithdraw();
                break;
            default:
                btnWD.interactable = true;
                break;
        }
    }
    //提现卷提现
    private void VoucherWithdraw()
    {
        return;
        var cfg = configVouchers.Find(x => x.gold == currGold);
        if (RedWithdrawData.Instance.redData.redIcon>=cfg.cost)
        {
           
            if (cfg.id == 2 && !RedWithdrawData.Instance.redDayData.todayWDs.Contains(configVouchers[0].gold))
            {
                ShowText(string.Format("请先提现{0}元。", configVouchers[0].gold));
                btnWD.interactable = true;
                return;
            }
            if (cfg.id == 3 && !RedWithdrawData.Instance.redDayData.todayWDs.Contains(configVouchers[1].gold))
            {
                ShowText(string.Format("请先提现{0}元。", configVouchers[1].gold));
                btnWD.interactable = true;
                return;
            }
           
            #region
            //if (ConfigMgr.Instance.IsRestrictWD(currGold))
            //{
            //    ShowText(string.Format("很遗憾！\n今日【{0}元】提现用户已达到1500/1500名，\n请明日再试或尝试其他提现额度。", currGold));
            //    btnWD.interactable = true;
            //    return;
            //}
            //Withdraw(currKey, (v) => {
            //    if (v == "1"||v.Contains("213"))
            //    {
            //        //弹出提示

            //        ShowText("提现成功，请留意微信信息！");
            //        var ui = UIManager.Instance.ShowPopUp<WithdrawSucceedUI>();
            //        if (v.Contains("213")) ui.OnShow2(cfg.gold.ToString());
            //        else ui.OnShow(cfg.gold.ToString());


            //        if (v.Contains("213")) RedWithdrawData.Instance.OnVoucherWD(cfg.key, cfg.gold, cfg.cost,4);
            //        else RedWithdrawData.Instance.OnVoucherWD(cfg.key, cfg.gold, cfg.cost);


            //        UpdateUI();
            //    }
            //    else
            //    {
            //        //弹出提示
            //        if (v.Contains("402")||v.Contains("209"))
            //        {
            //            RedWithdrawData.Instance.OnVoucherWD(cfg.key, cfg.gold, cfg.cost);
            //            ShowText("该额度只能提现1次!");
            //            UpdateUI();
            //        }
            //        else if (v.Contains("214"))
            //        {
            //            ShowText("提现过于频繁，请稍后再试！");
            //        }
            //        else if (v=="")
            //        {

            //        }
            //        else
            //        {
            //            ShowText("提现失败！");
            //        }

            //    }
            //    btnWD.interactable = true;
            //});
            #endregion
            WithdrawFeedback.Instance.Withdraw(currGold, currKey, RedWithdrawData.Instance.OnVoucherWD, (b) => { 
                if (b)
                {
                    RedWithdrawData.Instance.redData.voucher -= cfg.cost;
                    RedWithdrawData.Instance.SaveData();
                    UpdateUI();
                }
                btnWD.interactable = true;
            });
        }
        else
        {
            //弹出提示
           
            ShowText("提现券不足，无法提现！\n开红包即可获得获得提现券进度！");
            btnWD.interactable = true;
        }
    }
    //红包币提现
    private void RedIconWithdraw()
    {
        Debug.LogError("currGold" + currGold);
        Debug.LogError("currGold" + currKey);
        var cfg = configVouchers.Find(x => x.gold == currGold);
       
        if (!RedWithdrawData.Instance.IsCanCasn(currGold))
        {
            ShowText("今日该额度提现次数为0");
            return;
        }
        else if (RedWithdrawData.Instance.redData.redIcon >= cfg.cost)
        {
            if (currGold == 10 || currGold == 100)
            {
                ShowText("今日该档位提现次数已到上限");
                return;
            }
            var mkey = RedWithdrawData.Instance.ReturnCashKey(currGold);
            Debug.Log("mkeymkeymkeymkey" + mkey);
            if (mkey == "")
            {
                ShowText("今日提现已达上限");
                return;
            }

            WithdrawFeedback.Instance.Withdraw(currGold, mkey, RedWithdrawData.Instance.OnVoucherWD, (b) =>
            {
                if (b)
                {


                    if (DataManager.Instance.data.cashNum == 0)
                    {
                        if (ConfigMgr.Instance.ecpm >= 50 && ConfigMgr.Instance.ecpm <= 80)
                        {
                            AdControl.Instance.SdkSendEvent(4);
                        }
                        AdControl.Instance.SdkSendEvent(2);
                    }
                    else if (DataManager.Instance.data.cashNum == 1)
                    {
                        AdControl.Instance.SdkSendEvent(3);

                    }
                    DataManager.Instance.data.cashNum++;
                    //提现成功打点
                    UmengDisMgr.Instance.CountOnPeoples("tx_get", string.Format("{0}_{1}", RedWithdrawData.Instance.DayCashNums(currGold), currGold));
                    RedWithdrawData.Instance.redData.redIcon -= cfg.cost;
                    RedWithdrawData.Instance.RedCoin.Value = RedWithdrawData.Instance.redData.redIcon;
                    RedWithdrawData.Instance.SaveData();
                    RedWithdrawData.Instance.UseDayCashNum(currGold);
                    UpdateUI();
                }
                btnWD.interactable = true;
            });
        }
        else
        {
            //弹出提示

            ShowText("红包币不足，无法提现！\n开红包即可获得获得红包币！");
            btnWD.interactable = true;
        }


        #region
        //var redD = RedWithdrawData.Instance.redData;
        //if (redD.redIcon >= currGold * 10000)
        //{
        //    if (currGold == configRedIcon[0].cost)
        //    {
        //        //弹出提示
        //        if (redD.withdrawSiginTimes < RedWithdrawData.Instance.needSiginDay)
        //        {
        //            var ui = UIManager.Instance.ShowPopUp<WDSiginUI>();
        //            ui.OnShow(currGold, () => {
        //                RedWithdrawData.Instance.OnVideoSigin();
        //                ui.UpdateUi();
        //            });

        //        }
        //        else
        //        {
        //            var ui = UIManager.Instance.ShowPopUp<WDSiginUI>();
        //            RedWithdrawData.Instance.OnRedIconWD(currGold.ToString(), currGold); ;
        //            ui.ShowGet();
        //            UpdateRedTgl();
        //        }
        //    }
        //    else
        //    {
        //        var cfg = configRedIcon.Find(x => x.cost == currGold);
        //        ShowText(string.Format("清先提现{0}元", configRedIcon.Find(x => x.id == cfg.id-1).cost));
        //    }

        //    btnWD.interactable = true;
        //}
        //else
        //{
        //    //弹出提示
        //    ShowText("红包币不足，无法提现！\n开红包即可获得获得红包币！");
        //    btnWD.interactable = true;
        //}
        #endregion
    }
    //抽奖提现
    private void LuckyWithdraw()
    {
        if (RedWithdrawData.Instance.redData.redIcon >= currGold * 10000)
        {

            var key = currKey;
            var gold = currGold;
            if (key!=null&& key.Length>1)
            {
                if (ConfigMgr.Instance.IsRestrictWD(currGold))
                {
                    ShowText(string.Format("很遗憾！\n今日【{0}元】提现用户已达到1500/1500名，\n请明日再试或尝试其他提现额度。", currGold));
                    btnWD.interactable = true;
                    return;
                }
                Withdraw(key, (v) => {
                    if (v=="1"||v.Contains("213"))
                    {
                        //弹出提示

                        ShowText("提现成功，请留意微信信息！");
                        var ui = UIManager.Instance.ShowPopUp<WithdrawSucceedUI>();
                        if (v.Contains("213")) ui.OnShow2(gold.ToString());
                        else ui.OnShow(gold.ToString());
                        

                        if (v.Contains("213")) RedWithdrawData.Instance.OnRedIconWD(currKey, gold,4);
                        else RedWithdrawData.Instance.OnRedIconWD(currKey, gold);



                        RedWithdrawData.Instance.UpdateLuckyTime(DateTime.MinValue);
                        var times = GetCurrLuckyTimes();
                        string info = string.Format("抽奖次数：{0}_金额:{1}_奖池类型:{2}", (RedWithdrawData.Instance.redDayData.todayLuckys[times] + 1), gold, currLuckyConfig.level);
                        UmengDisMgr.Instance.CountOnNumber("cjtx_get", info);
                        UpdateUI();
                    }
                    else
                    {
                        //弹出提示
                        if (v.Contains("402")|| v.Contains("209"))
                        {
                            RedWithdrawData.Instance.OnRedIconWD(currKey, gold);
                            RedWithdrawData.Instance.UpdateLuckyTime(DateTime.MinValue);
                            ShowText("该额度只能提现1次!");
                            UpdateUI();
                        }
                        else if (v.Contains("214"))
                        {
                            ShowText("提现过于频繁，请稍后再试！");
                        }
                        else if (v == "")
                        {

                        }
                        else
                        {
                            ShowText("提现失败！");
                        }

                    }
                    btnWD.interactable = true;
                });
                
            }
            else
            {
                //弹出提示
                ShowText(string.Format("很遗憾！\n今日【{0}元】提现用户已达到1500/1500名，\n请明日再试或尝试其他提现额度。", currGold));
                btnWD.interactable = true;
            }
            
        }
        else
        {
            //弹出提示
            ShowText("红包币不足，无法提现！\n开红包即可获得获得红包币！");
            btnWD.interactable = true;
        }
    }
    public void Withdraw(string pushKey, Action<string> PushState)
    {
        //Debug.LogError("pushKey:"+ pushKey);

        //真提现方法
        WeChatContral.Instance.Withdraw(pushKey, PushState);

    }
    private int GetCurrLuckyTimes()
    {
        if (currLuckyConfig == null)
        {
            return 0;
        }
        int times = currLuckyConfig.tableType - 2;
        if (times < 0) times = 0;
        return times;
    }
    private void ShowText(string txt)
    {
        //弹窗文字
        Debug.Log(txt);
        ShowPublicTip.Instance.Show(txt);
    }

    public override void Show()
    {
        GameADControl.Instance.ShowMsg(false);
        UmengDisMgr.Instance.CountOnNumber("tx_show");
        UpdateUI();
        base.Show();
    }
}
