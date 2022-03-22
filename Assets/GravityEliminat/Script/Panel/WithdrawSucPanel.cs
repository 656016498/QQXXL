using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using BayatGames.SaveGamePro;
using EasyExcelGenerated;
using System;

public class WithdrawSucPanel : UIBase
{
    [Header("玩家头像")]
    public Image icon;
    public Text dir;
    public Transform p;
    static   bool isShow = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Show()
    {
        if (!isShow)
        {
            ShowAni();
            base.Show();
        }
       
    }

    public override void Hide()
    {
        base.Hide();
    }
    Vector3 initPos = new Vector3(0, -200f, 0);
    Vector3 targetPos = new Vector3(0, -697f, 0);
    public void ShowAni()
    {
        isShow = true;
        var mRectransPos = p.GetComponent<RectTransform>();
        mRectransPos.anchoredPosition = initPos;
        mRectransPos.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutBack).OnComplete(
            () => {
                //刷新UI
                Observable.TimeInterval(System.TimeSpan.FromSeconds(3f)).
                Subscribe(_ => {
                    mRectransPos.DOAnchorPos(initPos, 0.3f).SetEase(Ease.Linear)
                    .OnComplete(() => {
                        isShow = false;
                        Hide();
                        WithdrawSucManger.Instance.ShowSucPanel();
                    });
                 });
             });
    }

   public  void RefrishUi(Sprite mIcon,string mWeiChatName,float randomMoney,float totalMoney)   
    {
        if (mIcon!=null)
        {
            Debug.LogError("MMMmIcon" + mIcon.name);
            icon.sprite = mIcon;
        }
       
        dir.text = string.Format("恭喜玩家<color=#5E66BC>{0}</color>成功提现到账<color=#FF2F00>{1}元</color>\n已累计提现<color=#FF2F00>{2}元</color>", mWeiChatName, randomMoney, totalMoney);
    }
}

public class WithdrawSucData
{
    public float totalMoney;
    public Sprite falseIcon;//假头像
    public WithdrawSucData()
    {
        totalMoney = 0;
        falseIcon = null /*Resources.Load<Sprite>("Icon/icon")*/;
    }
}
public class WithdrawSucManger
{
    public static WithdrawSucManger mInstance;
    public static WithdrawSucManger Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new WithdrawSucManger();
            }
            return mInstance;
        }
    }
    public WithdrawSucData mdata;
    private const string Local_Key = "WithdrawSucData_Key_NoMain";
    public bool isInit = false;
    private float [] randomCashNum = new float [6] {0.3f,0.4f,0.5f,1f,10f,100f};
    private float[] tatalCashNum = new float[7] {100f,125f,132f,147f,205f,310f,330f};
    public WithdrawSucManger()
    {
        if (!isInit)
        {
            if (mconfig == null)
            {
                mconfig = ConfigGetMgr.Instance.mRandomNameConfig();
            }
            LoadData();
            isInit = true;
        }
        
    }

    public IDisposable mdispose;
     List<RandomName> mconfig;
    //加载
    private void LoadData()
    {
        mdata = SaveGame.Load<WithdrawSucData>(Local_Key);
        {
            if (mdata == null)
            {
                mdata = new WithdrawSucData();
            }
        }
        SaveData();
    }
    public void SaveData()
    {
        SaveGame.Save(Local_Key,mdata);
    }
    //提现类型
    public enum WithdrawType
    {
        True,//真提现
        False,//假提现
    }

    //随机玩家姓名
    public string RandomName()
    {
        var name = mconfig[UnityEngine.Random.Range(0, mconfig.Count)].Name;
        var tou= name.Substring(0, 1);
        var wei = name.Substring(name.Length-1, 1);
        var returnName = tou + "***" + wei;
        return returnName;
        //string newName; 
        //if (name.Contains(" "))
        //{
        //    newName = name.Replace(name, " ");
        //    return newName;
        //}
        //else
        //{
        //    return name;
        //}     
    }
    //随机玩家头像
    public Sprite RandomIcon()
    {
        var mSp = Resources.Load<Sprite>("Icon/icon");
        return mSp;
       
    }
    //随机玩家提现金额
    public float RandomMoney()
    {
        return
            randomCashNum[UnityEngine.Random.Range(0,randomCashNum.Length)];
            
    }

    //随机玩家总金额
    public float RandomTotalMoney()
    {
        return
            tatalCashNum[UnityEngine.Random.Range(0, tatalCashNum.Length)];
    }
    //添加提现金额
    public void AddTotalMoney(float value)
    {
        mdata.totalMoney += value;
        SaveData();
    }
    //自动展示提现成功界面（假）
    public void ShowSucPanel()
    {
        if (mdispose != null)
        {
            mdispose.Dispose();
        }
        mdispose = Observable.TimeInterval(System.TimeSpan.FromSeconds(60f)).Subscribe(_ => {

            var mpanel = UIManager.Instance.ShowPopUp<WithdrawSucPanel>();
            mpanel.RefrishUi(RandomIcon(), RandomName(), RandomMoney(), RandomTotalMoney());

        });
        
    }
}