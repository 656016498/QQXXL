using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BayatGames.SaveGamePro;
using UniRx;
using EasyExcelGenerated;
using DG.Tweening;

public class SevenWithdrawPanel : UIBase
{
    [SerializeField] private IButton CloseBtn;
    [SerializeField] private IButton GoToBtn;
    [SerializeField] private IButton GotBtn;
    [SerializeField] private IButton NoActiveBtn;
    [SerializeField] private IButton WithdrawBtn;
    [SerializeField] private SliderControl MSlidercontrol;
    [SerializeField] private Text TaskDir;
    [SerializeField] private HorizontalSlider MHorizontalSlider;
    [SerializeField] private Transform mItems;
    [SerializeField] private Text RemainTime;
    [SerializeField] private Text SchduleInfo;
    private Dictionary<Transform, int> mDic_trans_days = new Dictionary<Transform, int>();


    [Header("测试按钮")]
    public IButton testBtn;
    static bool isFirstShow = false;
    private void Start()
    {

        MHorizontalSlider.NowShowItem += ListenShowingObj;
        for (int i = 0; i < mItems.childCount; i++)
        {
            var item = mItems.GetChild(i);
            var day = i + 1;
            mDic_trans_days.Add(item, day);
        }
        MHorizontalSlider.SetShowDays(SevenWithdrawDataMgr.Instance.GetRunningDays);
        ItemInit();
        InitRemainTime();
        SettingBtn();
        isFirstShow = true;
    }

    private void SettingBtn()
    {
        CloseBtn.onClick.AddListener(()=> { Hide(); });
        GotBtn.onClick.AddListener(()=> { GotoLogic();});
        GoToBtn.onClick.AddListener(() =>
        {
            GotoClick();
            Debug.LogError("已领取");
        });
        NoActiveBtn.onClick.AddListener(() =>
        {
            Debug.LogError("未激活");
        });
        WithdrawBtn.onClick.AddListener(WitdhrawBtnLogic);
        testBtn.onClick.AddListener(() =>
        {
            SevenWithdrawDataMgr.Instance.AddTaskData(1);
            RefreshUI(mShowingDays, mShowingTrans);
        });
       
    }

    private void GotoLogic()
    {
        Debug.Log("已领取");
    }

    //点击前往按钮
    private void GotoClick()
    {
        UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);
        Hide();
        return;
        Debug.Log("当前展示第" + mShowingDays + "天");
        var info = SevenWithdrawDataMgr.Instance.GetInfo(mShowingDays);
     
        switch (info.Type)
        {
            case 1:
                Hide();
                break;
            case 2: ; break;
            case 3:
                Hide();
                break;
            case 4: break;
            case 5: ; break;
            case 6: break;
            case 7:
                Hide();
                break;
            default:
                break;
        }
        Hide();
    }

    public void ShowFinger(Transform targetPos)
    {
        //ShowFingerManager.Instance.ShowFinger(targetPos);
    }
    private void WitdhrawBtnLogic()
    {
        var info = SevenWithdrawDataMgr.Instance.GetInfo(mShowingDays);
        float gold = info.withdrawValue;
        Withdraw(info.withdrawKey, (v) => {
            if (v == "1" || v.Contains("213"))
            {
                //弹出提示
                ShowPublicTip.Instance.Show("提现成功，请留意微信信息！");
                var ui = UIManager.Instance.ShowPopUp<WithdrawSucceedUI>();
                if (v.Contains("213")) ui.OnShow2(gold.ToString());
                else ui.OnShow(gold.ToString());

                XDebug.LogError("点击提现按钮:" + mShowingInfo.State);
                mShowingInfo.State = SevenWithdrawDataMgr.State.GotReward;
                SevenWithdrawDataMgr.Instance.SaveData();
                RefreshUI(mShowingDays, mShowingTrans);

                //提现成功打点
                UmengDisMgr.Instance.CountOnPeoples("tx7_get", string.Format("{0}", mShowingDays));
            }
            else
            {
                //弹出提示
                if (v.Contains("402") || v.Contains("209"))
                {

                    ShowPublicTip.Instance.Show("该额度只能提现1次!");
                    
                }
                else if (v.Contains("214"))
                {
                    ShowPublicTip.Instance.Show("提现过于频繁，请稍后再试！");
                }
                else if (v == "")
                {

                }
                else
                {
                    ShowPublicTip.Instance.Show("提现失败！");
                }

            }
        });
          
       
    }

    //真提现
    public void Withdraw(string pushKey, Action<string> PushState)
    {
        //真提现方法
        WeChatContral.Instance.Withdraw(pushKey, PushState);

    }

    /// <summary>
    /// 当前正在显示的对象
    /// </summary>
    /// <param name="mtrans"></param>
    private void ListenShowingObj(Transform mtrans)
    {
        Debug.LogError(">>>" + mtrans.name);
        mDic_trans_days.TryGetValue(mtrans, out int days);
        RefreshUI(days, mtrans);
    }

    private void ItemInit()
    {
        foreach (var item in mDic_trans_days)
        {
            var info = SevenWithdrawDataMgr.Instance.GetInfo(item.Value);
            item.Key.Find("Days").GetText().text = string.Format("第{0}天", item.Value);
            //item.Key.Find("item").GetText().text=info.
        }
    }

    private const string UnlockTip = "完成上一天任务后解锁";
    private const string UnlockTip2 = "新的任务次日解锁!";
    private SevenWithdrawDataMgr.TaskInfo mShowingInfo;
    private int mShowingDays;
    private Transform mShowingTrans;
    private void RefreshUI(int days, Transform mtrans)
    {
        Debug.LogError(days);
        var info = SevenWithdrawDataMgr.Instance.GetInfo(days);
        mShowingInfo = info;
        mShowingDays = days;
        mShowingTrans = mtrans;
        info.UpdateState();
        var schdule = SevenWithdrawDataMgr.Instance.GetAllSchdle;
        //MSlidercontrol.UpdateInfo(schdule, (schdule * 100).ToString("0.00") + "%");
        //MSlidercontrol.UpdateInfo(schdule, (schdule * 100).ToString("0.00") + "%");
        var allCount = SevenWithdrawDataMgr.Instance.AllCount;
        var completeCount = SevenWithdrawDataMgr.Instance.CompleteCount;
        MSlidercontrol.UpdateInfo(schdule, string.Format("{0}/{1}",completeCount,allCount));
        GoToBtn.transform.HideCanvasGroup();
        WithdrawBtn.transform.HideCanvasGroup();
        GotBtn.transform.HideCanvasGroup();
        NoActiveBtn.transform.HideCanvasGroup();
        if (info.State == SevenWithdrawDataMgr.State.NoActive)
        {
            var lastInfo = SevenWithdrawDataMgr.Instance.GetInfo(days - 1);
            if (lastInfo != null && lastInfo.State == SevenWithdrawDataMgr.State.GotReward)
            {
                TaskDir.text = UnlockTip2;
            }
            else
            {
                TaskDir.text = UnlockTip;
            }
            NoActiveBtn.transform.ShowCanvasGroup();
            //SchduleInfo.text = null;
        }
        else
        {
            //SchduleInfo.text = string.Format("({0}/{1})", info.NowValue, info.TargetValue);
            TaskDir.text =string.Format("任务："+info.Dir+"  ({1}/{2})",info.TargetValue,info.NowValue,info.TargetValue);
            switch (info.State)
            {
                case SevenWithdrawDataMgr.State.NoActive:
                    NoActiveBtn.transform.ShowCanvasGroup();
                    break;
                case SevenWithdrawDataMgr.State.NoComplete:
                    GoToBtn.transform.ShowCanvasGroup();
                    break;
                case SevenWithdrawDataMgr.State.Arrived:
                    WithdrawBtn.transform.ShowCanvasGroup();
                    break;
                case SevenWithdrawDataMgr.State.GotReward:
                    GotBtn.transform.ShowCanvasGroup();
                    break;
                default:
                    break;
            }
        }
    }


    private void InitRemainTime()
    {
        RefreshRemainTime();
        Observable.Interval(System.TimeSpan.FromMinutes(1))
             .Subscribe(_ =>
             {
                 RefreshRemainTime();
             });
    }
    private void RefreshRemainTime()
    {
        var endTime = SevenWithdrawDataMgr.Instance.GetEndTime;
        var nowTime = GameTime.GameClock.NowTime;
        var remainTime = (endTime - nowTime);
        RemainTime.text = string.Format("活动剩余时间:{0}天{1}小时", remainTime.Days, remainTime.Hours);
    }


    public override void Show()
    {
        if (isFirstShow)
        {
            MHorizontalSlider.SetShowDays(SevenWithdrawDataMgr.Instance.GetRunningDays);
            ItemInit();
        }
        base.Show();
        //展示banner
        GameADControl.Instance.Banner(true);
        //打点
        UmengDisMgr.Instance.CountOnNumber("tx7_show");
    }
    public override void Hide()
    {
        //隐藏
        GameADControl.Instance.Banner(false);
        GameADControl.Instance.ShowIntAd("tx7_half");
        base.Hide();
    }
}

public class SevenWithdrawDataMgr:Singlton<SevenWithdrawDataMgr>
{
    public SevenWithdrawDataMgr()
    {
        LoadData();
    }

    public void Init() { }

    public class TaskInfo
    {
        public int Days;                    //第几天:配置表的key
        public int Type;                    //任务ID
        public int NowValue;                //进度值
        public int TargetValue;             //目标值
        public string Dir;                  //任务描述       
        public State State;                 //任务完成情况
        public DateTime ActiveTime;         //激活时间
        public string withdrawKey;         //提现key
        public float withdrawValue;        //提现金额

        public void UpdateState()
        {
            if (State== State.NoComplete)
            {
                if (NowValue >= TargetValue)
                {
                    State = State.Arrived;
                    ShowDailyInfoDataControl.Instance.SetNormalShowSevenPanel();
                }
            }            
        }                                              
    }    

    public enum State
    {
         NoActive,              //未激活
         NoComplete,            //未完成
         Arrived,               //已完成
         GotReward              //已领取奖励
    }

    public class Data
    {
        public bool ActriveIsOpen;           //活动是否开启
        public DateTime StartTime;              //开始时间
        public DateTime EndTime;                //结束时间
        public int RunningDays;                 //正在运行的任务
        public Dictionary<int, TaskInfo> Dic_taskInfo;        
    }
    private Data GetData;
    private const string Key = "SevenWithdrawDataMgr";

    /// <summary>
    /// 活动是否开启
    /// </summary>
    public bool ActiveIsOpen
    {
        get
        {
            return GetData.ActriveIsOpen;
        }
    }    

    /// <summary>
    /// 获取信息
    /// </summary>
    /// <param name="days"></param>
    /// <returns></returns>
    public TaskInfo GetInfo(int days)
    {
        GetData.Dic_taskInfo.TryGetValue(days, out TaskInfo info);
        return info;
    }

    public int GetRunningDays
    {
        get
        {
            return GetData.RunningDays;
        }
    }

    /// <summary>
    /// 获取剩余天数
    /// </summary>
    public DateTime GetEndTime
    {
        get
        {
            return GetData.EndTime;
        }
    }

    /// <summary>
    /// 获取当前完成的总进度
    /// </summary>
    public float GetAllSchdle
    {
        get
        {
            var n = 0;
            foreach (var item in GetData.Dic_taskInfo)
            {
                if (item.Value.State== State.Arrived||item.Value.State== State.GotReward)
                {
                    n += 1;
                }
            }
            var all = GetData.Dic_taskInfo.Count;
            return n / (float)all;
        }
    }
    //返回当前总任务数量
    public int AllCount
    {
        get
        {
            var all = GetData.Dic_taskInfo.Count;
            return all;
        }
    }

    //返回当前完成任务数量
    public int CompleteCount
    {
        get
        {
            var n = 0;
            foreach (var item in GetData.Dic_taskInfo)
            {
                if (item.Value.State == State.Arrived || item.Value.State == State.GotReward)
                {
                    n += 1;
                }
            }
            return n;
        }
    }
    private void LoadData()
    {
        GetData = SaveGame.Load<Data>(Key);
        if (GetData==null)
        {
            InitData();
        }
        else
        {
            Debug.LogError("七日:..");
            CheckActiveIsEnd();
            CheckIsOpenNextDay();
        }
        SaveData();
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public void SaveData()
    {
        SaveGame.SaveAsync(Key, GetData);
    }

    private void InitData()
    {

        GetData = new Data();
        var nowTime = GameTime.GameClock.NowTime;
        GetData.StartTime = nowTime.GetTimeSpan_day();
        GetData.EndTime = GetData.StartTime.AddDays(7);
        GetData.RunningDays = 1;
        GetData.Dic_taskInfo = new Dictionary<int, TaskInfo>(7);
        GetData.ActriveIsOpen = true;

        var configlist = ConfigGetMgr.Instance.mSevenWithDrawConfigs();
        for (int i = 0; i < configlist.Count; i++)
        {
            var config = configlist[i];
            var info = new TaskInfo();
            info.Days = config.Day;
            info.Type = config.taskType;
            info.withdrawKey = config.withdraw;
            info.withdrawValue = config.value;
            info.NowValue = 0;
            info.TargetValue = config.target;
            info.Dir = config.taskDir;
            info.State = State.NoActive;
            GetData.Dic_taskInfo.Add(info.Days, info);
        }

        var runningInfo = GetData.Dic_taskInfo[GetData.RunningDays];
        runningInfo.ActiveTime = nowTime;
        runningInfo.State = State.NoComplete;


        Debug.LogWarning("七日活动:" + GetData.StartTime + "->" + GetData.EndTime);
    }

    /// <summary>
    /// 判断是否开启下一天任务
    /// </summary>
    private void CheckIsOpenNextDay()
    {
        Debug.LogError("七日:" + GetData.ActriveIsOpen + ";" + (GetData.RunningDays > GetData.Dic_taskInfo.Count));
        if (!GetData.ActriveIsOpen)
        {
            return;
        }

        var days = GetData.RunningDays;
        if (days>GetData.Dic_taskInfo.Count)
        {
            return;
        }

        var info = GetData.Dic_taskInfo[days];
        var nowTime = GameTime.GameClock.NowTime;
        if (TimeExtension.IsSameDay(info.ActiveTime,nowTime))
        {
            Debug.Log("七天红包:相同时间");
            return;
        }

        if (info.State== State.Arrived||info.State== State.GotReward)
        {
            //任务完成
            GetData.RunningDays += 1;
            Debug.Log("七天红包:加一天");
            if (GetData.RunningDays<GetData.Dic_taskInfo.Count)
            {
                var newInfo = GetData.Dic_taskInfo[GetData.RunningDays];
                newInfo.ActiveTime = nowTime;
                newInfo.State = State.NoComplete;
            }
        }
        SaveData();
    }

    /// <summary>
    /// 检查活动是否结束
    /// </summary>
    private void CheckActiveIsEnd()
    {
        Debug.Log("GetData.ActriveIsOpen:" + GetData.ActriveIsOpen);
        if (!GetData.ActriveIsOpen)
        {
            return;
        }
        var nowTime = GameTime.GameClock.NowTime;
        var endTime = GetData.EndTime;
        if (nowTime>endTime)
        {
            GetData.ActriveIsOpen = false;
        }
        else
        {
            GetData.ActriveIsOpen = true;
        }
    }

    //添加人任务进度
    public void AddTaskData( int addvalue,int taskid=0) 
    {
       
        var minfo = GetData.Dic_taskInfo[GetRunningDays];
        if (minfo.Type != taskid) return;
        if (minfo.NowValue < minfo.TargetValue)
        {
            minfo.NowValue += addvalue;
            Debug.Log("添加任务");
            minfo.UpdateState();

            //打点
            if (minfo.NowValue == minfo.TargetValue)
            {
                UmengDisMgr.Instance.CountOnPeoples("tx7_arrive", string.Format("{0}",minfo.Days));
            }
        }

        SaveData();
    }
#if UNITY_EDITOR

    [UnityEditor.MenuItem("GameEditor/清除七天提现数据")]
    static void Menu()
    {
        SaveGame.Delete(Key);
    }
#endif
}


