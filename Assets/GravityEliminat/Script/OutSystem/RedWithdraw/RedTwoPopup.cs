using DG.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;



public class RedTwoPopup : UIBase
{
#pragma warning disable 649
   public bool willShow=false;
    [SerializeField]
    private Text txtHave;
    //[SerializeField]
    public Button btnClose, btnClose2, btnDouble, btnWD;
#pragma warning restore 649
    public Text txtAdd, txtPro, txtPigAdd, txtMore, txt1;
    public Image imgPro, imgPro2, imgCan, imgCan2;
    public GameObject pigAdd, effetTW;
    public float speed;
    private float reward;
    private string adid;
    private Action closeCallback, doubleCallback;
    private float startValue, addValue, extraValue;
    public GameObject effect;//星星特效
    [Header("底部进度框")]
    public Transform downPro;
    public Transform up;
    [Header("进度框特效")]
    public Transform fillEffect;
    // Start is called before the first frame update
    public GameObject defult;
    //public Action WDCallBack;
    //public  static bool IsCash = false;
    protected void Start()
    {
        Assert.IsNotNull(txtHave);
        //Assert.IsNotNull(btnClose);
        btnClose.onClick.AddListener(()=> {

            if (DataManager.Instance.data.UnlockLevel > 1)
            {
                Debug.Log("播放插屏");
                GameADControl.Instance.ShowIntAd("win_hb_half");
            }
          
            OnClose();
        });
        btnClose2.onClick.AddListener(OnClose);
        btnDouble.onClick.AddListener(OnDouble);
        btnWD.onClick.AddListener(OnWD);
        //btnClose.gameObject.SetActive(false);
        effetTW.SetActive(false);
        txt1.gameObject.SetActive(false);
        //添加按钮音效
        AudioMgr.Instance.ButtonAddSound(this.transform);
    }

    bool IsAddPig;
    public void OnOpen(float voucher, float add, string id, Action closeCb, Action doubleCb = null,bool isAddPig=true) 
    {
        IsAddPig = isAddPig;
        //设置新人红包
        RedWithdrawData.Instance.SetFirstBag();
        addValue = voucher;
        startValue = (float)(RedWithdrawData.Instance.redData.redIcon /*+ add*/) / (RedWithdrawData.Instance.NowCanCash * 10000);
        Debug.Log("addValue" + addValue);
        Debug.Log("startValue" + startValue);
        if (addValue + startValue > 1)
        {
            //extraValue = RedWithdrawData.Instance.extraValue;
            //txtMore.text = string.Format("<size=23>进度达成！<color=#FF0000>奖励进度：{0}%</color></size>", extraValue * 100);
        }
        else
        {
            //txtMore.text = string.Format("提现券进度");
            extraValue = 0;
        }
        reward = add;
        adid = id;
        closeCallback = closeCb;
        doubleCallback = doubleCb;
        //WDCallBack = WDcall;
       
        UpdateUI();
        //StartCoroutine("DoClose");
        //RedWithdrawData.Instance.UpdateVoucherPro(voucher);
        //CheckWD();
        effect.SetActive(true);
        //七天任务
        SevenWithdrawDataMgr.Instance.AddTaskData(1,4);

        if (RedWithdrawData.Instance.IsCanCash((int)reward))
        {
            txtMore.text = string.Format("当前有额度可提现");
        }
        else
        {
            if (RedWithdrawData.Instance.LastBagCash() < 0)
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", 1);
            }
            else  if (RedWithdrawData.Instance.LastBagCash() > 5)
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", 5);
            }
            else
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", RedWithdrawData.Instance.LastBagCash());
            }
        }

        
    }
    private void CheckWD()
    {
        //if (RedWithdrawData.Instance.redData.voucherPro> 0)
        if (imgPro.fillAmount >= 1)
        {
            willShow = true;
            imgCan.gameObject.SetActive(true);
            imgCan2.gameObject.SetActive(true);

            //btnWD.GetComponent<Animator>().enabled = true;
        }
        else
        {

            willShow = false;
            imgCan.gameObject.SetActive(false);
            imgCan2.gameObject.SetActive(false);

        }
    }
    IEnumerator DoClose()
    {
        yield return new WaitForSeconds(2f);
        btnClose.gameObject.SetActive(true);
    }
    private void UpdateUI()
    {
        if (doubleCallback == null)
        {
            btnDouble.gameObject.SetActive(false);
        }
        var icon = RedWithdrawData.Instance.redData.redIcon;
        txtAdd.text = string.Format("{0}", reward.ToString());
        txtHave.text = string.Format("领取后余额≈<size=34><color=#ff3210>{0}元</color></size>", ((float)icon / 10000).ToString("f2"));
        pigAdd.SetActive(IsAddPig);
        if (IsAddPig)
        {
            var pigadd = PiggyBankData.Instance.GetPigIcon();
            if (pigadd > 0)
            {
                txtPigAdd.text = string.Format("+{0}", pigadd);
                PiggyBankData.Instance.UpdatePiggyBank(pigadd);
            }
            else
            {
                pigAdd.SetActive(false);
            }
        }
       
        #region
        //var targetValue = addValue + startValue;
        //if (addValue < 0.05f)
        //{
        //    startValue = targetValue - 0.01f;
        //}
        //var durTime = 1f;
        //imgPro.fillAmount = startValue;
        //imgPro2.fillAmount = startValue;
        //txtPro.text = string.Format("{0}%", (startValue * 100).ToString("0.00"));
        //Observable.Timer(System.TimeSpan.FromSeconds(.5f)).Subscribe(_ =>
        //{
        //    imgPro2.DOFillAmount(targetValue, durTime).SetEase(Ease.Linear).OnComplete(() =>
        //    {
        //        if (extraValue > 0)
        //        {
        //            imgPro.fillAmount = 0;
        //            imgPro2.fillAmount = 0;
        //            var add = targetValue - 1;
        //            effetTW.SetActive(true);
        //            effetTW.transform.DOPath(new Vector3[] { effetTW.transform.position + new Vector3(0, 2, 0), txt1.transform.position }, speed, PathType.CatmullRom).OnComplete(() =>
        //            {
        //                txt1.gameObject.SetActive(true);
        //                txt1.transform.DOMoveY(txt1.transform.position.y + 2, 1.5f).OnComplete(() =>
        //                {
        //                    txt1.gameObject.SetActive(false);
        //                });
        //                effetTW.SetActive(false);
        //            });


        //            imgPro2.DOFillAmount(add, durTime).SetEase(Ease.Linear).OnComplete(() =>
        //            {

        //            });
        //            float mvalue = 0;
        //            DOTween.To(() => mvalue, x => mvalue = x, (add * 100), durTime).OnUpdate(() =>
        //            {
        //                txtPro.text = string.Format("{0}%", (mvalue).ToString("0.00"));
        //            });

        //        }
        //    });
        //    float mvalue1 = startValue * 100;
        //    DOTween.To(() => mvalue1, x => mvalue1 = x, targetValue * 100, durTime).OnUpdate(() =>
        //    {
        //        if (mvalue1 > 100)
        //        {
        //            mvalue1 = 100;
        //        }
        //        txtPro.text = string.Format("{0}%", (mvalue1).ToString("0.00"));
        //    });

        //}).AddTo(this);
        #endregion
        RfriishFillPro(reward);

    }
    //刷新进度条---
    public void RfriishFillPro(float add =0)
    {
        Debug.Log("进去添加" + add);
        var mVour = (float)(RedWithdrawData.Instance.redData.redIcon + add)/ (RedWithdrawData.Instance.NowCanCash * 10000);
        imgPro.DOFillAmount(mVour, 0.5f).SetEase(Ease.Linear).OnComplete(() => { CheckWD(); });
        if (mVour >= 1)
        {
            txtPro.text = string.Format("{0}%", 100);
        }
        else
        {
            txtPro.text = string.Format("{0}%", (mVour * 100).ToString("0.00"));
        }
        
    }
    public void RfriishFillPro1()
    {
        Debug.Log("返回添加");
        var mVour = (float)(RedWithdrawData.Instance.redData.redIcon) / (RedWithdrawData.Instance.NowCanCash * 10000);
        imgPro.DOFillAmount(mVour, 0.5f).SetEase(Ease.Linear);
        if (mVour >= 1)
        {
            txtPro.text = string.Format("{0}%", 100);
        }
        else
        {
            txtPro.text = string.Format("{0}%", (mVour * 100).ToString("0.00"));
        }
    } 
    private void OnDouble()
    {
        AdControl.Instance.ShowRwAd(adid, () =>
        {
            doubleCallback?.Invoke();
        });
        if (adid == "level_up_double_video")
        {
            UmengDisMgr.Instance.CountOnPeoples("level_double_click", (PlayerPrefs.GetInt("next_level") - 1).ToString());
        }
    }

    //显示自身
    void ShowTransonsObj()
    {
        //defult.gameObject.SetActive(true);
        //effect.gameObject.SetActive(true);
        //fillEffect.gameObject.SetActive(true);
        RfriishFillPro(0);

    }
    void HideTransObj()
    {
        //defult.gameObject.SetActive(false);
        //effect.gameObject.SetActive(false);
        //fillEffect.gameObject.SetActive(false);
    }
    private void OnWD()
    {
        if (!GameManager.Instance.isCash)
        {
            RedWithdrawData.Instance.UpdateRedIcon((int)reward);
            GameManager.Instance.isCash = true;
        }
        HideTransObj();
        var mpanel= UIManager.Instance.ShowPopUp<WithdrawUI>();
        mpanel.UpdateUI();
        mpanel.closeCallBack = ShowTransonsObj;  
    }


    private void OnClose()
    {
        
         UIManager.Instance.Hide<RedTwoPopup>();
         closeCallback?.Invoke();
         HintAD();
        GameManager.Instance.isCash = false;
        //动画飞动画
        //1主界面飞  战斗界面飞

        //var mPanel = UIManager.Instance.GetBase<MainPanel>();
        //var mpos = mPanel.cashBtn.transform.parent.GetChild(0).transform.position;
        //var pigPos = mPanel.pigCoinBtn;
        //if (mPanel.gameObject.activeInHierarchy)
        //{
        //    RedFly.instance.Play(3, btnClose2.transform.position, mpos, 
        //        ()=> {
        //            XDebug.Log("飞达目的地");
        //            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HbEffect, mpos);
        //       });

        //    PigFly.instance.Play(3, btnClose2.transform.position, pigPos.transform.position,
        //      () => {
        //          XDebug.Log("飞达目的地");
        //      });

        //}
        //else
        //{
            var mGamePanel = UIManager.Instance.GetBase<GamePanel>();
           if (mGamePanel!=null)
        {
            var mPos1 = mGamePanel.cashBtn.transform.parent.GetChild(2).transform.position;
            var pigPos2 = mGamePanel.pigBtn;
            RedFly.instance.Play(3, btnClose2.transform.position, mPos1, () => {

                XDebug.Log("飞达目的地");
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HbEffect, mPos1);
            });
            PigFly.instance.Play(3, btnClose2.transform.position, pigPos2.transform.position,
             () => {
                 XDebug.Log("飞达目的地");
             });
        }




        if (willShow )
        {
       
            UIManager.Instance.ShowPopUp<WithdrawUI>();
        }
        //}
    }
    private void ShowAD()
    {
        GameADControl.Instance.Banner(false);
        //如果结束新手引导 则显示信息流
        if (DataManager.Instance.data.UnlockLevel>=2)
        {
            GameADControl.Instance.ShowMsg(true);
        }
        
    }
    private void HintAD()
    {
        GameADControl.Instance.ShowMsg(false);
       
    }


    public override void Show()
    {
        base.Show();
        UpdateUI();
        //新手引导
        GuideMgr.Instance.ShowGuide_9(()=> {

            Debug.Log("完成第9步");
        });
        ShowAD();
    }
}

