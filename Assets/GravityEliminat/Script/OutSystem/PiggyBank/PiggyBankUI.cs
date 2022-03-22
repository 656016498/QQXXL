using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PiggyBankUI : UIBase
{
    //关闭，提现，问号，提现记录按钮
    public Button btnClose, btnWD, btnQuestion, btnRecord,btnTomorrow;
    public Toggle toggle03, toggle05, toggle1;
    //现金文本
    public Text txtCash,txtIcon;
    //明日再来
    public Image imgTomorrow;
    //选中按钮
    private string pitchTgl="";

    public Action callBack;
    

    protected  void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnWD.onClick.AddListener(OnWD);
        btnQuestion.onClick.AddListener(OnQuestion);
        btnRecord.onClick.AddListener(OnRecord);
        btnTomorrow.onClick.AddListener(OnTomorrow);
        toggle03.onValueChanged.AddListener(On03);
        toggle05.onValueChanged.AddListener(On05);
        toggle1.onValueChanged.AddListener(On1);
        
        PiggyBankData.Instance.PanelNeedUpdate.Subscribe(b => {
            OnRefresh();
        }).AddTo(this);
        //UmengDisMgr.Instance.CountOnNumber("cqg_show");
        //AdControl.Instance.HideMessageAd();
        //添加按钮音效
        //添加按钮音效
        AudioMgr.Instance.ButtonAddSound(this.transform);
    }

    private void OnTomorrow()
    {
        ShowText("开任意红包，均能获得金猪零钱！\n次日登录，即可立即提现哦！!");
    }

    public void OnRefresh()
    {
        
        var data = PiggyBankData.Instance.pigData;
        string cash = ((float)(data.deposit / 100)/100).ToString("f2");
        txtCash.text = string.Format("≈{0}元", cash);
        txtIcon.text = string.Format("{0} <size=20>金猪币</size>",data.deposit);
        toggle03.isOn = false;
        toggle03.isOn = true;
        //用服务器时间
        DateTime now = GameTime.GameClock.NowTime;
        //Debug.Log("now:"+now);
        Debug.Log("data.lastWDTime:" + data.lastWDTime);
        btnTomorrow.gameObject.SetActive(false);
        btnWD.gameObject.SetActive(false);
        if (now.Year > data.lastWDTime.Year || now.Month > data.lastWDTime.Month || now.Day > data.lastWDTime.Day)
        {
            btnWD.gameObject.SetActive(true);
        }
        else
        {
            btnTomorrow.gameObject.SetActive(true);
            toggle03.transform.Find("Image").gameObject.SetActive(false);
            //toggle03.interactable = false;
            //toggle03.isOn = false;
            toggle05.transform.Find("Image").gameObject.SetActive(false);
            //toggle05.interactable = false;
            //toggle05.isOn = false;
            toggle1.transform.Find("Image").gameObject.SetActive(false);
            //toggle1.interactable = false;
            //toggle1.isOn = false;
        }
    }

    private void OnRecord()
    {
        var ui = UIManager.Instance.ShowPopUp<WithdrawRecordUI>();
        ui.OnOpen(PiggyBankData.Instance.pigData.records);
       
        
    }

    private void OnQuestion()
    {

        var ui = UIManager.Instance.ShowPopUp<ExplainUI>();
        var list = new List<string>();
        list.Add("<size=25><color=#8F504E>1.游戏内观看视频，都会有一笔现金红包存入存钱罐。</color></size>");
        list.Add("<size=25><color=#8F504E>2.每日只可选择任意的金额，提现一次！</color></size>");
        list.Add("<size=25><color=#8F504E>3.次日0点以后，即可上线领取奖励！无需看广告！</color></size>");
        ui.EnterPanelUpdate(list);

    }

    private void OnWD()
    {
        btnWD.interactable = false;
        float reward = float.Parse(pitchTgl);
        //Debug.Log("PiggyBankData.Instance.pigData.deposit" + PiggyBankData.Instance.pigData.deposit);
        //Debug.Log("reward" + reward * 10000);
        Debug.LogError("PIGGGGGG"+reward);
        if (PiggyBankData.Instance.pigData.deposit >= (int)(reward *10000))
        {
            //if (ConfigMgr.Instance.IsRestrictWD(reward))
            //{
            //    ShowText(string.Format("很遗憾！\n今日【{0}元】提现用户已达到1500/1500名，\n请明日再试或尝试其他提现额度。", reward));
            //    return;
            //}
            string key = PiggyBankData.Instance.bankKeys.Find(x => x.money == pitchTgl).key;
            //真提现逻辑
            Withdraw(key, (value) => {
                btnWD.interactable = true;
                if (value=="1")
                {
                    //ShowText("提现成功，请留意微信信息！");
                    AdControl.Instance.SdkSendEvent(1);
                    Debug.LogWarning("提现成功，请留意微信信息！");
                    PiggyBankData.Instance.OnWithDraw(key, reward);
                    var ui = UIManager.Instance.ShowPopUp<WithdrawSucceedUI>();
                    if (value.Contains("213")) ui.OnShow2(reward.ToString());
                    else ui.OnShow(reward.ToString());
                   
                    //if (value.Contains("213")) PiggyBankData.Instance.OnWithDraw(key, reward,4);
                    //else PiggyBankData.Instance.OnWithDraw(key, reward);
                    //添加提现成功次数
                    PiggyBankData.Instance.AddCashDicTimes(reward);
                    UmengDisMgr.Instance.CountOnNumber("cqg_tx_get",string.Format("{0}_{1}", PiggyBankData.Instance.AddCashSucNums(reward), reward));
                    OnRefresh();

                    //显示提现成功跑马灯
                    WithdrawSucManger.Instance.AddTotalMoney(reward);
                    var mpanel = UIManager.Instance.ShowPopUp<WithdrawSucPanel>();
                    var mdata = AudioMgr.Instance.mdate;
                    var mSucData = WithdrawSucManger.Instance.mdata;
                    mpanel.RefrishUi(mdata.icon, mdata.weiChatName, reward, mSucData.totalMoney);
                }
                else
                {
                    if (value.Contains("402")|| value.Contains("209"))
                    {
                        PiggyBankData.Instance.OnWithDraw(key, reward);
                        ShowText("该额度只能提现1次!");
                        OnRefresh();
                    }
                    else if (value.Contains("214"))
                    {
                        ShowText("提现过于频繁，请稍后再试！");
                    }
                    else if (value=="")
                    {

                    }
                    else
                    {
                        ShowText("提现失败！");
                    }
                    //弹出提示
                    //ShowText("提现失败！");

                }
            });
        }
        else
        {
            //弹出游戏提示
            ShowText("存钱罐余额不足！");
            btnWD.interactable = true;
        }
    }

    public void Withdraw(string pushKey, Action<string> PushState)
    {

        //真提现方法
        WeChatContral.Instance.Withdraw(pushKey, PushState);

    }

    private void On1(bool arg0)
    {
        if (arg0)
        {
            pitchTgl = "0.5";
        }
    }

    private void On05(bool arg0)
    {
        if (arg0)
        {
            pitchTgl = "0.4";
        }
    }

    private void On03(bool arg0)
    {
        if (arg0)
        {
            pitchTgl = "0.3";
        }
    }
    private void OnClose()
    {
        if (callBack!=null)
        {
            callBack();
            callBack = null;
        }
        GameADControl.Instance.ShowIntAd("cqg_half");
        UIManager.Instance.Hide<PiggyBankUI>();
    }

    private void ShowText(string tip)
    {
        //弹窗文字
        Debug.Log(tip);
        ShowPublicTip.Instance.Show(tip);
    }

    public override void Show()
    {
        //存钱罐界面展示次数
        UmengDisMgr.Instance.CountOnNumber("cqg_show");
        base.Show();
        GameADControl.Instance.Banner(true);
    }

    public override void Hide()
    {
        GameADControl.Instance.Banner(false);
        base.Hide();
    }
}
