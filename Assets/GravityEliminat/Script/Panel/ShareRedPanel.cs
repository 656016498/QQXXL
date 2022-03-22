using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BayatGames.SaveGamePro;

public class ShareRedPanel : UIBase
{
    [Header("按钮")]
    public IButton exitBtn;//退出
    public IButton goBtn;//前往
    public IButton withdrawalBtn;//提现
    public IButton yesterdayBtn;//昨日提现统计
    public IButton quesBtn1;
    public IButton quesBtn2;
    [Header("可返利金额")]
    public Text totalCash;
    [Header("今日提现人数")]
    public Text todayCash;
    [Header("昨日人均返利")]
    public Text percapita;
    [Header("下个宝石出现")]
    public Text nextDamind;
    [Header("钻石父物体")]
    public List<Shareitem> mlist;
    [Header("彩色钻石")]
    public Text colorful;
    [Header("测试按钮")]
    public Button testBtn;
    void Start()
    {
        exitBtn.onClick.AddListener(() => { Hide(); });
        goBtn.onClick.AddListener(() => {
            //跳转到当前闯关最高关卡
            Hide();
        });
        withdrawalBtn.onClick.AddListener(() => { ShowPublicTip.Instance.Show("您尚未获得分红方块，未能参与返利!"); });
        yesterdayBtn.onClick.AddListener(() => {
            //展示昨日提现弹窗

        });
        quesBtn1.onClick.AddListener(() => {
            //活动规则弹窗

            ShowRulePanel();
        });
        quesBtn2.onClick.AddListener(() => {
            //活动规则弹窗
            ShowRulePanel();
        });
        testBtn.onClick.AddListener(() => {
            for (int i = 1; i < 6; i++)
            {
                ShareRedDataManger.Instance.AddDiamonds((ShareRedDataManger.DiamondsType)i, 1);
            }
            
        });

    }

    private void ShowRulePanel()
    {
        UIManager.Instance.ShowPopUp<LotteryRulePanel>((LotteryRulePanel mpanel) =>
        {
            mpanel.SetDir("1.分红返利:\n\u3000\u3000拥有分红宝石的用户每天可获得广告返利。<color=red>直接将广告收益以现金的形式返利到玩家微信账号。</color>\n2.返利金额来源\n\u3000\u3000每天用户看广告产生<color=red>收益的40%</color>进入返利现金池,<color=red>用于返还用户。</color>\n3.奖金分配\n\u3000\u3000用户奖金=昨日全网广告收益/全网分红宝石量 * 用户拥有的分红宝石。\n4.获得分红方块的方式\n\u3000\u3000<color=red>关卡固定掉落普通宝石,</color>集齐所有普通宝石自动合成获得分红宝石。<color=red>分红宝石可以重复获得,没有上限!</color>");
            mpanel.AddListenToBtn(
                () => { mpanel.Hide(); },
                () => { mpanel.Hide(); });
        });
    }
    public override void Show()
    {
        RefrishUi();
        base.Show();
    }
    public override void Hide()
    {

        base.Hide();
    }

    public void RefrishUi()
    {
        var mdata = ShareRedDataManger.Instance.mdata;
        totalCash.text = string.Format("<size=55>¥</size>{0}<size=50>元</size>", mdata.dayCashNum);
        todayCash.text = string.Format("今日已有<color=#dd8f2a>{0}</color>人收集成功", mdata.collectNums);
        percapita.text = string.Format("{0}<size=48>元</size>", mdata.lastdayCashNum);
        nextDamind.text = string.Format("下个宝石在第<color=#d55624>{0}</color>关出现", DataManager.Instance.data.GemLevel[0]
 );
        //刷新钻石数量
        if (mlist.Count != 0)
        {
            for (int i = 0; i < mlist.Count; i++)
            {
                mlist[i].RefrishUi();
            }
        }
    }
}
public class ShareRedData
{
    public float dayCashNum;//可返利金额
    public float lastdayCashNum;//昨日人均返利金额
    public int collectNums;//收集成功人数
    public DateTime lastdatime;//昨日分红现金
    public Dictionary<ShareRedDataManger.DiamondsType, int> mdiamonds;

    public ShareRedData(float mdayCashNum,float mlastdayCashNum)
    {
        dayCashNum = mdayCashNum;
        lastdayCashNum = mlastdayCashNum;
        collectNums = 255421;
        lastdatime = GameTime.GameClock.NowTime;
        mdiamonds = new Dictionary<ShareRedDataManger.DiamondsType, int>();
        InitValues();

    }

    void InitValues()
    {
        if (mdiamonds.Count == 0)
        {
            for (int i = 1; i < 6; i++)
            {
                mdiamonds.Add((ShareRedDataManger.DiamondsType)i, 0);
            }
        }
        
    }
}

public class ShareRedDataManger
{
    public const string Local_Key = "ShareRedData_Key"; 
    private static ShareRedDataManger mInstance;
    public static ShareRedDataManger Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new ShareRedDataManger();
            }
            return mInstance;
        }
    }
    bool IsInit = false;
    public ShareRedData mdata;
    public ShareRedDataManger() 
    {
        if (!IsInit)
        {
            Init();
        }

    }
    private void Init()
    {
        LoadData();
    }

    public enum DiamondsType 
    {
        cyan=1,//青色
        purple=2,//紫色
        yellow=3,//黄色
        red=4,//红色
        blue=5,//蓝色
        colorful=6,//彩色
    }

    private void LoadData()
    {
        mdata = SaveGame.Load<ShareRedData>(Local_Key);
        if (mdata == null)
        {
            mdata = new ShareRedData(RandomDayCash, RandomLastDayCash);
        }
        else
        {
            //隔天刷新
            if (!TimeExtension.IsSameDay(mdata.lastdatime, GameTime.GameClock.NowTime))
            {
                mdata.dayCashNum = RandomDayCash;
                mdata.lastdayCashNum = RandomLastDayCash;
                mdata.collectNums = RandomColleceNums;
                mdata.lastdatime = GameTime.GameClock.NowTime;
            }
        }
        SaveData();
    }
    private void SaveData()
    {
        SaveGame.Save(Local_Key, mdata);
    }

    //添加钻石
    public void AddDiamonds(DiamondsType mtype,int addNum)
    {
        if (mdata.mdiamonds.ContainsKey(mtype))
        {
            mdata.mdiamonds[mtype] += addNum;
            SaveData();
        }
    }
    //随机每日分红
    float RandomDayCash
    {
        get
        {
            return UnityEngine.Random.Range(28000,32000);
        }

    }
    //随机昨日人均分红
    float RandomLastDayCash
    {
        get
        {
            return UnityEngine.Random.Range(180,210);
        }
    }
    //随机每日收集成功人数
    int RandomColleceNums
    {
        get
        {
            return UnityEngine.Random.Range(mdata.collectNums,mdata.collectNums+1000);
        }
    }
}