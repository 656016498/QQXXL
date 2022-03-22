using DG.Tweening;
using EasyExcelGenerated;
using GameTime;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TwistedUI : UIBase
{
    public Button btnClose, btnQaustion, btnRecord, btnTweist,btnAdress,btnTweistNo;
    public Image proIphone, proIpad, proRed;
    public Text txtProIphone, txtProIpad, txtRed, txtProRed, txtTimes,txtWdTimes;
    public List<Transform> conditions;
    public BallsMoving[] balls;
    public CheckBall checkBall;
    public GameObject effet;
    public List<Image> imgDebris;

    private System.Random random = new System.Random();
    [Header("领现金")]
    public Button getBtn;
    public Text dirText;
    public Text peopleNum;
    public Image yes;
    [Header("测试按钮")]
    public Button testBtn;

    // Start is called before the first frame update
    private void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnQaustion.onClick.AddListener(OnQaustion);
        btnRecord.onClick.AddListener(OnRecord);
        btnTweist.onClick.AddListener(OnTweisted);
        btnTweistNo.onClick.AddListener(OnTweisted2);
        btnAdress.onClick.AddListener(OnAddress);
        UpdateUI();
        UmengDisMgr.Instance.CountOnNumber("gashapon_show");
        testBtn.onClick.AddListener(() => {
            for (int i = 0; i < 3; i++)
            {
                TwistedData.Instance.AddConditions(i,20);
                UpdateUI();
            }
        });
        RefrishOrderSystem();
        //OrderSystem.Instance.customerNums.Subscribe(value => {
        //    RefrishOrderSystem();
        //});
    }

   

   

    public void UpdateUI()
    {
        var twistedD = TwistedData.Instance;
        //摇奖机
        var gs = twistedD.data.currConditions;
        var gds = twistedD.data.conditionNums;
        int over = 0;
        #region
        //for (int i = 0; i < conditions.Count; i++)
        //{
        //    Image icon = conditions[i].Find("imgIcon").GetComponent<Image>();
        //    icon.sprite = Resources.Load<Sprite>(string.Format("Game/Granule/{0}", (int)gs[i] + 1));
        //    if (gds[i] > 0)
        //    {
        //        conditions[i].Find("imgFinish").gameObject.SetActive(false);
        //        conditions[i].Find("txtNum").gameObject.SetActive(true);
        //        conditions[i].Find("txtNum").GetComponent<Text>().text = gds[i].ToString();
        //    }
        //    else
        //    {
        //        conditions[i].Find("imgFinish").gameObject.SetActive(true);
        //        conditions[i].Find("txtNum").gameObject.SetActive(false);
        //        over += 1;
        //    }
        //}
        //if (over >= 3)  
        //{
        //    btnTweist.transform.Find("Image").gameObject.SetActive(false);
        //    btnTweist.transform.Find("finger").gameObject.SetActive(true);
        //    btnTweist.interactable = true;
        //}
        //else
        //{
        //    btnTweist.transform.Find("Image").gameObject.SetActive(true);
        //    btnTweist.transform.Find("finger").gameObject.SetActive(false);
        //    btnTweist.interactable = false;
        //}
        #endregion
        //var mdata = OrderSystem.Instance.mdata;
        //if (mdata.nowCustomer >= mdata.targetCustomer)
        //{
        //    btnTweist.transform.Find("Image").gameObject.SetActive(false);
        //    btnTweist.transform.Find("finger").gameObject.SetActive(true);
        //    btnTweist.interactable = true;
        //}
        //else
        //{
        //    btnTweist.transform.Find("Image").gameObject.SetActive(true);
        //    btnTweist.transform.Find("finger").gameObject.SetActive(false);
        //    btnTweist.interactable = false;
        //}
        //红包
        txtRed.text = string.Format("{0}元", twistedD.data.redReward.ToString());
        var pro = twistedD.data.redPro;
        if (pro > 100) pro = 100;
        txtProRed.text = string.Format("{0}%", pro.ToString("f1"));
        proRed.transform.localPosition = new Vector3(0, (pro * 0.9f)-92, 0);

        //碎片
        txtProIphone.text = string.Format("{0}/{1}", twistedD.data.debrisIPhone, twistedD.fullIphone);
        txtProIpad.text = string.Format("{0}/{1}", twistedD.data.debrisIPad, twistedD.fullIphone);
        proIphone.fillAmount = (float)twistedD.data.debrisIPhone / twistedD.fullIphone;
        proIpad.fillAmount = (float)twistedD.data.debrisIPad / twistedD.fullIpad;

        txtTimes.text = string.Format("今日剩余次数:{0}/{1}", twistedD.fullLucky-twistedD.dayData.dayLuckys,twistedD.fullLucky);
        txtWdTimes.text = string.Format("今日剩余提现次数：{0}",twistedD.dayWithDrawTimes-twistedD.dayData.dayWithdraw);
       
    }
    private void OnAddress()
    {
        UIManager.Instance.ShowPopUp<UserAddressUI>();
    }
    private void OnTweisted2()
    {
        ShowText("完成任务即可抽奖。");
        
    }
    private void OnTweisted()
    {
        btnTweist.interactable = false;
        if (TwistedData.Instance.dayData.dayLuckys>= TwistedData.Instance.fullLucky)
        {
            ShowText("今日抽奖次数上限。");
            return;
        }
        UmengDisMgr.Instance.CountOnPeoples("gashapon_join", (TwistedData.Instance.dayData.dayLuckys+1).ToString());
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].Moving();
        }
        int id = GetID();
        effet.SetActive(true);
        Observable.TimeInterval(TimeSpan.FromSeconds(1)).Subscribe(_=> {
            checkBall.Moving(id);
        });
        Observable.TimeInterval(TimeSpan.FromSeconds(2)).Subscribe(_ => {
            effet.SetActive(false);
        });
        
        Observable.TimeInterval(TimeSpan.FromSeconds(3)).Subscribe(_ => {
            
            string info = "";
            Action cb = null;
            if (id == 1)
            {
                int add = TwistedData.Instance.GetIPhoneDebris();
                info = string.Format("IPhone12碎片x{0}", add);
                cb = () => {
                    TwistedData.Instance.AddIPhone12(add);
                    TwistedData.Instance.UpdateTaskConfig();
                    MoveToIphone();
                    UpdateUI();
                };
            }
            if (id == 2)
            {
                int add = TwistedData.Instance.GetIPadDebris();
                info = string.Format("平板碎片x{0}", add);
                cb = () => {
                    TwistedData.Instance.AddIPad(add);
                    TwistedData.Instance.UpdateTaskConfig();
                    MoveToIpad();
                    UpdateUI();
                };
            }
            if (id == 3)
            {
                int add = RedWithdrawData.Instance.GetRedIcon();
                info = string.Format("红包币+{0}", add);
                cb = () => {
                    RedWithdrawData.Instance.UpdateRedIcon(add);
                    TwistedData.Instance.UpdateTaskConfig();
                    UpdateUI();
                    
                };
            }
            if (id == 4)
            {
                float add = TwistedData.Instance.data.redReward;
                info = string.Format("现金{0}元", add);
                cb = () => {
                    string key = TwistedData.Instance.GetWDKey(add);
                    WithdrawFeedback.Instance.Withdraw(add, key, TwistedData.Instance.AddWDRecord, (b) => {
                        if (b)
                        {
                            TwistedData.Instance.dayData.dayWithdraw += 1;
                            TwistedData.Instance.data.redPro =0;
                            TwistedData.Instance.UpdateTaskConfig();
                            TwistedData.Instance.UpdateTwistedRed();
                            string infos = string.Format("提现日期:{0},次数:{1}",GameClock.NowTime.GetTimeSpan_day(), (TwistedData.Instance.dayData.dayLuckys + 1).ToString());
                            UmengDisMgr.Instance.CountOnPeoples("gashapon_cash", infos);
                        }
                        UpdateUI();
                    });

                };
            }
            var ui= UIManager.Instance.ShowPopUp<AcquireTwistedUI>();
            ui.OnShow(info, id, cb);
            //切换下一个主线
          
            RefrishOrderSystem();
        });

        //添加抽奖次数
        //OrderSystem.Instance.AddTwisteNums(1);
        
    }

    private int GetID()
    {
        var twistedD = TwistedData.Instance;
        var ecpm = ConfigMgr.Instance.ecpm;
        if (twistedD.data.redPro>=100&&twistedD.dayData.dayWithdraw<twistedD.dayWithDrawTimes)
        {
            return 4;
        }
        else
        {
            if (ecpm>=100&& twistedD.data.redPro>50&& twistedD.dayData.dayWithdraw < twistedD.dayWithDrawTimes)
            {
                int er = random.Next(100);
                if (er<= twistedD.data.redPro)
                {
                    return 4;
                }
            }
            int r= random.Next(100);
            if (r<=33)
            {
                if (twistedD.data.debrisIPhone>95)
                {
                    return 3;
                }
                return 1;
            }
            else if (r>33&& r<=66)
            {
                if (twistedD.data.debrisIPad > 95)
                {
                    return 3;
                }
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }

    private void OnRecord()
    {
        var ui = UIManager.Instance.ShowPopUp<WithdrawRecordUI>();
        ui.OnOpen(TwistedData.Instance.data.records);
       
    }

    private void OnQaustion()
    {
        UIManager.Instance.ShowPopUp<TwistedQaustionUI>();
    }

    private void MoveToIphone()
    {
        TweenMove(proIphone.rectTransform.position);
    }
    private void MoveToIpad()
    {
        TweenMove(proIpad.rectTransform.position);
    }
    
    private void TweenMove(Vector3 pos)
    {
        int i = 0;
        foreach (var item in imgDebris)
        {
            i++;
            item.gameObject.SetActive(true);
            item.rectTransform.DOMove(pos, 1f).OnComplete(() =>
            {
                item.rectTransform.position = Vector3.zero;
                item.gameObject.SetActive(false);
            }).SetEase(Ease.InBack).SetDelay(0.2f * i);
        }
        
    }
    public void OnHint()
    {
        OnQaustion();
    }
    private void OnClose()
    {
        UIManager.Instance.Hide<TwistedUI>();
    }
    private void ShowText(string str)
    {
        //ShowPublicTip.Instance.Show(str);
        Debug.Log(str);
    }


    //订单系统刷新
    public void RefrishOrderSystem()
    {
        //var id = MainTaskMgr.Instance.GetData.Index;
        //var info = EasyExcelExtension.EffDataMgr.Get<Config_mainTask>(id);

        //dirText.text = info.MainTaskContent;
        //peopleNum.text = MainTaskMgr.Instance.GetData.NowValue + "/" + MainTaskMgr.Instance.GetData.Targetvalue;
        //var mdata = MainTaskMgr.Instance.GetData;
        //bool boo = mdata.NowValue >= mdata.Targetvalue ? true : false;
        //getBtn.transform.HideCanvasGroup();
        //if (boo)
        //{
        //    yes.transform.ShowCanvasGroup();
        //}
        //else
        //{
        //    yes.transform.HideCanvasGroup();
        //}
        //RefrishTwisBtn();


    }

    //抽奖按钮刷新
    public void RefrishTwisBtn()
    {
        //var mdata = MainTaskMgr.Instance.GetData;
        //if (mdata.NowValue >= mdata.Targetvalue)
        //{
        //    btnTweist.transform.Find("Image").gameObject.SetActive(false);
        //    btnTweist.transform.Find("finger").gameObject.SetActive(true);
        //    btnTweist.interactable = true;
        //}
        //else
        //{
        //    btnTweist.transform.Find("Image").gameObject.SetActive(true);
        //    btnTweist.transform.Find("finger").gameObject.SetActive(false);
        //    btnTweist.interactable = false;
        //}
    }

    public override void Show()
    {
        base.Show();
    }
}
