using BayatGames.SaveGamePro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyExcelGenerated;
using System;
using UniRx;

public enum LargeHbType
{
 Nomal,
 Weichat 
}
public class LargeCashTwoPanel : UIBase
{
    [Header("增加金额")]
    public Transform addPrt;
    public Text moneyAdd;
    [Header("总金额")]
    public Text totalMoney;
    [Header("还差..可提现")]
    public Text moneyLast;
    [Header("进度条")]
    public SliderControl mSliderControl;
    [Header("微信按钮")]
    public IButton weiChatBtn;
    [Header("获取按钮")]
    public IButton getBtn;
    public IButton exitBtn;
    [Header("特效")]
    public Transform jinbiEffect;

    Action openCallBack;
    Action closeCallBack;
    Action weiChatCallBack;
    LargeHbType OpenType;

    public RollInfo mRollInfo; 
    void Start()
    {

        getBtn.onClick.AddListener(() => {

            openCallBack.Run();
            var mGamePanel = UIManager.Instance.GetBase<GamePanel>();
            if (mGamePanel != null)
            {
                var mPos1 = mGamePanel.largeCashBtn.transform.GetChild(2).position;
                MoneyFly.instance.Play(3, Vector3.zero, mPos1, () => {
                    XDebug.Log("飞达目的地");
                    Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HbEffect2, UIManager.Instance.GetBase<GamePanel>().largeCashBtn.transform.GetChild(2).position);

                });
            }
            Hide();
        });
        exitBtn.onClick.AddListener(() => {
            closeCallBack();


            var mGamePanel = UIManager.Instance.GetBase<GamePanel>();
            if (mGamePanel!=null)
            {
                var mPos1 = mGamePanel.largeCashBtn.transform.GetChild(2).position;
                MoneyFly.instance.Play(3, Vector3.zero, mPos1, () => {
                    XDebug.Log("飞达目的地");
                    Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HbEffect2, UIManager.Instance.GetBase<GamePanel>().largeCashBtn.transform.GetChild(2).position);
                });
            }
            Hide();
        });
        weiChatBtn.onClick.AddListener(() => {

            weiChatCallBack();

        });
    }

    public override void Show()
    {
        GameADControl.Instance.Banner(false);
        if (DataManager.Instance.data.UnlockLevel>=2)
        {
            GameADControl.Instance.ShowMsg(true);
        }
        base.Show();
    }

    public override void Hide()
    {
        GameADControl.Instance.ShowMsg(false);
        base.Hide();
    }

    public void OnOpen(LargeHbType type, Action mGetCallBack,Action mWeiChatCallBack,Action mCloseCallBack)
    { 
        Init(); 
        OpenType = type;
        openCallBack = mGetCallBack;
        weiChatCallBack = mWeiChatCallBack;
        closeCallBack = mCloseCallBack;
        switch (OpenType)
        {
            case LargeHbType.Nomal:
                getBtn.transform.ShowCanvasGroup();
                addPrt.ShowCanvasGroup();
                jinbiEffect.gameObject.SetActive(true);
                break;
            case LargeHbType.Weichat:
                weiChatBtn.transform.ShowCanvasGroup();
                addPrt.HideCanvasGroup();
                UmengDisMgr.Instance.CountOnNumber("tx200_show");
                break;
            default:
                break;
        }

    }
     
    private void Init()
    {
        getBtn.transform.HideCanvasGroup();
        weiChatBtn.transform.HideCanvasGroup();
        addPrt.HideCanvasGroup();
        jinbiEffect.gameObject.SetActive(false);
    }
    public  void RefrishUi(float add,float total,float last)
    {
        moneyAdd.text = string.Format("+{0}元", add);
        totalMoney.text = string.Format("{0}<size=50>元</size>", total.ToString("f2"));
        moneyLast.text = string.Format("仅差{0}元可提现", last.ToString("f2"));
        mSliderControl.SetSlider((total / 200));
    }
}

/// <summary>
/// 大额提现数据
/// </summary>
public class LargeCashData
{
    public float totalNum;//总金额 
    public int   cashTimes;//领取次数

    public LargeCashData()
    {
        totalNum = 0;
        cashTimes = 1;
    }
}
//大额提现管理类
public class LargeCashDataControl
{
    private  static LargeCashDataControl instance;
    public static LargeCashDataControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LargeCashDataControl();
                
            }
            return instance;
        }

    }
    private const string Local_Key = "LargeCashData_key";
    public LargeCashData mData; 
    public List<LargeCashConfig> mLargeCashConfigs;
    bool isInit = false;
    public ReactiveProperty<float> largeCash = new ReactiveProperty<float>();
    public LargeCashDataControl()
    {
        Init();
    }
    void Init()
    {
        if (!isInit)
        {
            LoadData();
            mLargeCashConfigs = ConfigGetMgr.Instance.mLargeCashConfig();
            largeCash.Value = mData.totalNum;
            isInit = true;
        }
       
    }
    //加档
    private void LoadData()
    {
        mData = SaveGame.Load<LargeCashData>(Local_Key);
        {
            if (mData == null)
            {
                mData = new LargeCashData();
            }
        }
    }
    //存档
    private void SaveData()
    {
        SaveGame.Save(Local_Key, mData);
    }

    //添加金额
    public void AddtoTotal(float num)
    {
        if (mData.cashTimes < mLargeCashConfigs.Count)
        {
            mData.cashTimes++;
        }
        mData.totalNum += num;
        if (mData.totalNum >= 200)
        {
            mData.totalNum = 199.99f;
        }
        largeCash.Value = mData.totalNum;
        SaveData();
        //打点
        if (mData.totalNum >= 100 && mData.totalNum < 130 )
        {
            UmengDisMgr.Instance.CountOnPeoples("tx200_arrive","100");
        }
        else if (mData.totalNum >= 130 && mData.totalNum < 150)
        {
            UmengDisMgr.Instance.CountOnPeoples("tx200_arrive", "130");
        }
        else if (mData.totalNum >= 150 && mData.totalNum < 180)
        {
            UmengDisMgr.Instance.CountOnPeoples("tx200_arrive", "150");
        }
        else if (mData.totalNum >= 180 && mData.totalNum < 200)
        {
            UmengDisMgr.Instance.CountOnPeoples("tx200_arrive", "180");
        }
        else if (mData.totalNum >= 200)
        {
            UmengDisMgr.Instance.CountOnPeoples("tx200_arrive", "200");
        }
    }
    //随机当前金额进度表
    public LargeCashConfig largeCashConfig
    {
        get
        {
            Debug.Log("mLargeCashConfigs" + mLargeCashConfigs.Count);
            foreach (var item in mLargeCashConfigs)
            {

                if (item.CoinNum == mData.cashTimes)
                {
                    //Debug.LogError("eqweqw" + mData.cashTimes + "ddddddd" + item.CoinNum);
                    return item;
                }
            }
            
            return null;
        }

    }


    public float GetLargeCoin() {

        var mAddValue = largeCashConfig.AD* AndroidHelper.Instance.ToRate(RewDynType.Large);
        AddtoTotal(mAddValue);
        return mAddValue;

    }

    //还差多少钱可提现
    public float LastMoney
    {
        get
        {
            return (200 - mData.totalNum);
        }
    }
}