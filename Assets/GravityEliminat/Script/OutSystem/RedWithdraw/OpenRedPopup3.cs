using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class OpenRedPopup3 : UIBase
{
#pragma warning disable 649

    //[SerializeField]
    public Button btnOpen, btnOpen2;
    [SerializeField]
    private Button btnClose, btnClose2;
#pragma warning restore 649
    public GameObject defult, big;// defult--通关红包 big--其他红包
    public Image imgtype, imgEffet, imgCan, imgCan2;
    public Text txtType, txtNeed;
    private GameObject target;
    //public Sprite typeNew, typeLevel;
    private string adid;
    private Action openCallback;
    private Action closeCallback;
    private int videoReward;
    [Header("提现券进度条")]
    public Image fillPro;
    public Text fillText;
    public IButton withDrawlBtn;//提现按钮
    [Header("Parent")]
    public Transform parent;
    public Text txtMore;
    [Header("通关红包")]
    public Sprite passLevelSp;
    [Header("唤醒红包")]
    public Sprite weakeUpSp;
    public Image hbType;

    protected void Start()
    {
        Assert.IsNotNull(btnOpen);
        Assert.IsNotNull(btnClose);
        btnOpen.onClick.AddListener(OnOpenPrss);
        btnClose.onClick.AddListener(OnClose);
        btnOpen2.onClick.AddListener(OnOpenPrss);
        btnClose2.onClick.AddListener(OnClose);
        imgEffet.transform.DOLocalRotate(new Vector3(0, 0, 359), 6, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);

        withDrawlBtn.onClick.AddListener(() => {
            HideTransObj();
          var mPanel =UIManager.Instance.ShowPopUp<WithdrawUI>();
            mPanel.closeCallBack = ShowTransonsObj;
        });
        //添加按钮音效
        AudioMgr.Instance.ButtonAddSound(this.transform);
    }

    //显示自身
    void ShowTransonsObj() 
    {
        defult.gameObject.SetActive(true);
    }
    void HideTransObj()
    {
        defult.gameObject.SetActive(false);
    }
    private void OnClose()
    {
        CatManager.Instance.CanGet();
        UIManager.Instance.Hide<OpenRedPopup3>();
        closeCallback?.Invoke();
        //隐藏banner
        GameADControl.Instance.Banner(false);
        if (GameManager.Instance.CurrentLevel > 1)
        {
            GameADControl.Instance.ShowIntAd("win_hb_close_int");
        } 
    }
    IEnumerator CloseAnyse()
    {
        transform.DOScale(0.1f, 0.5f).SetEase(Ease.Linear);
        transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.6f);
        UIManager.Instance.Hide<OpenRedPopup3>();
        closeCallback?.Invoke();
    }
    private void OnOpenPrss()
    {
        if (RedWithdrawData.Instance.redData.isFirstBag)
        {
           
            //RedWithdrawData.Instance.UpdateRedIcon(videoReward);

            openCallback?.Invoke();
            UIManager.Instance.Hide<OpenRedPopup3>();
            
        }
        else  if (adid == null || adid == "")
        {
            
            //RedWithdrawData.Instance.UpdateRedIcon(videoReward);
            openCallback?.Invoke();
            UIManager.Instance.Hide<OpenRedPopup3>();
            
        }
        else
        {
           
            AdControl.Instance.ShowRwAd(adid, () =>
            {
                CatManager.Instance.catData.now[0]++;
              

                CatManager.Instance.SaveData();
                Debug.Log("ConfigManEcpm"+ConfigMgr.Instance.ecpm);
                openCallback?.Invoke();
                //RedWithdrawData.Instance.UpdateRedIcon(videoReward);
                UIManager.Instance.Hide<OpenRedPopup3>();

            });
        }

       
    }

    public void OnOpen(string id, int videoRw, Action cbOpen, Action cbClose, float icereward = 0)
    {
        adid = id;
        videoReward = videoRw;
        openCallback = cbOpen;
        closeCallback = cbClose;
      
        big.SetActive(false);
        defult.SetActive(true);

        if (adid!="win_hb_video")//显示幸运红包字样
        {
            hbType.sprite = weakeUpSp;
        }
        else
        {
            hbType.sprite = passLevelSp;
        }
        StartCoroutine(ShowClose());
        var tt = 100 - (float)RedWithdrawData.Instance.redData.redIcon / 10000;
        if (tt > 0)
            txtNeed.text = string.Format("还差{0}元可提现", tt.ToString("f2"));

        btnClose.gameObject.SetActive(false);
        btnClose2.gameObject.SetActive(false);
        //刷新提现券进度条
        RefrishFillPro();

        if (RedWithdrawData.Instance.IsCanCash())
        {
            txtMore.text = string.Format("当前有额度可提现");
        }
        else
        {
            if (RedWithdrawData.Instance.LastBagCash() < 0)
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", 1);
            }
            if (RedWithdrawData.Instance.LastBagCash() > 5)
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", 5);
            }
            else
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", RedWithdrawData.Instance.LastBagCash());
            }
           
        }
    }

    public void OnOpen(string id, Action cbOpen, Action cbClose, float icereward = 0)
    {
        adid = id;
        openCallback = cbOpen;
        closeCallback = cbClose;

        big.SetActive(false);
        defult.SetActive(true);

        if (adid == "game_awaken_video" || adid == "online_hb_video")//显示幸运红包字样
        {
            hbType.sprite = weakeUpSp;
        }
        else
        {
            hbType.sprite = passLevelSp;
        }
        StartCoroutine(ShowClose());
        var tt = 100 - (float)RedWithdrawData.Instance.redData.redIcon / 10000;
        if (tt > 0)
            txtNeed.text = string.Format("还差{0}元可提现", tt.ToString("f2"));

        btnClose.gameObject.SetActive(false);
        btnClose2.gameObject.SetActive(false);
        //刷新提现券进度条
        RefrishFillPro();

        if (RedWithdrawData.Instance.IsCanCash())
        {
            txtMore.text = string.Format("当前有额度可提现");
        }
        else
        {
            if (RedWithdrawData.Instance.LastBagCash() < 0)
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", 1);
            }
            if (RedWithdrawData.Instance.LastBagCash() > 5)
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", 5);
            }
            else
            {
                txtMore.text = string.Format("约差<color=red>{0}</color>个红包可提现", RedWithdrawData.Instance.LastBagCash());
            }

        }
    }

    IEnumerator ShowClose()
    {
        yield return new WaitForSeconds(2f);
        btnClose.gameObject.SetActive(true);
        btnClose2.gameObject.SetActive(true);
    }
    public override void Show()
    {

        //音效
        //Debug.Log("播放红包展示音效");
        base.Show();


        //如果完成新手引导则弹广告
        if (GuideMgr.Instance.GuideIsComplete(9))
        {
            GameADControl.Instance.Banner(true);
        }
    }

    //刷新进度条
    private void RefrishFillPro()
    {
        var mVour = (float)RedWithdrawData.Instance.redData.redIcon/ (RedWithdrawData.Instance.NowCanCash * 10000);
        fillPro.fillAmount = mVour;
        if (mVour >= 1)
        {
            fillText.text = string.Format("{0}%", 100);
            imgCan.gameObject.SetActive(true);
            imgCan2.gameObject.SetActive(true);


        }
        else
        {
            fillText.text = string.Format("{0}%", (mVour * 100).ToString("0.00"));
            imgCan.gameObject.SetActive(false);
            imgCan2.gameObject.SetActive(false);
        }
       
    }
}
