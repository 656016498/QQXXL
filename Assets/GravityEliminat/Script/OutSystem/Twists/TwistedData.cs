using BayatGames.SaveGamePro;
using EasyExcelGenerated;
using GameTime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TwistedData
{
    private static TwistedData mInstace;
    public static TwistedData Instance
    {
        get
        {
            if (mInstace == null)
            {
                mInstace = new TwistedData();
            }
            return mInstace;
        }
    }
    public TwistedData()
    {
        InitData();
    }
    private string permanentDataKey = "TwistedDataKey";
    private string dayDataKey = "TwistedDayDataKey";
    public TwistePermanentData data;
    public TwistedDayData dayData;
    private List<ScheduleConfig> scheduleConfigs;
    private List<DebrisConfig> debrisConfigs;
    private List<TwistedWDConfig> twistedWDConfigs;
    public TaskConfig taskConfig;
    //当前时间
    public DateTime nowTime;
    //碎片上限
    public int fullIphone = 100;
    public int fullIpad = 100;
    //抽奖上限
    public int fullLucky = 10;
    //提现次数上限
    public int dayWithDrawTimes = 3;
    

    private void InitData()
    {
        //设置服务器时间
        nowTime = GameClock.NowTime;
        data = SaveGame.Load<TwistePermanentData>(permanentDataKey,null);
        if (data==null)
        {
            data = new TwistePermanentData();
            data.lastLucky = DateTime.MinValue;
            data.records = new List<RecordsData>();
            UpdateTaskConfig();
            UpdateTwistedRed();
        }
        else
        {
            //循环任务配置
            if (data.taskId>= ConfigMgr.Instance.mConfigData.GetList<TaskConfig>().Count)
            {
                data.taskId = 1;
            }
            
        }
        dayData = SaveGame.Load<TwistedDayData>(dayDataKey,null);
        if (dayData==null) 
        {
            dayData = new TwistedDayData();
        }
        else
        {
            if (nowTime.Year > data.lastLucky.Year || nowTime.Month > data.lastLucky.Month || nowTime.Day > data.lastLucky.Day)
            {
                dayData.dayLuckys = 0;
            }
        }
        SaveData();
        SaveDayData();
        WeChatContral.Instance.OnAddWithdraw += AddWDRecord;
        WeChatContral.Instance.OnClear += ClearRecord;
        
    }
    //获取当前提现key
    public string GetWDKey(float rmb)
    {
        if (twistedWDConfigs==null)
        {
            twistedWDConfigs = ConfigMgr.Instance.mConfigData.GetList<TwistedWDConfig>();
        }
        var cfg = twistedWDConfigs.Find(x => x.reward == rmb);
        return cfg.keys[dayData.dayWithdraw];
        
    }
    //更新抽奖条件配置
    public void UpdateTaskConfig()
    {
        data.taskId += 1;
        taskConfig = ConfigMgr.Instance.mConfigData.Get<TaskConfig>(data.taskId);
        if (taskConfig!=null)
        {
            data.currConditions.Clear();
            data.conditionNums.Clear();
            if (taskConfig.black1 > 0)
            { data.currConditions.Add(Conditions.condition1); data.conditionNums.Add(taskConfig.black1); }
            if (taskConfig.black2 > 0)
            {
                data.currConditions.Add(Conditions.condition2); data.conditionNums.Add(taskConfig.black2);
            }
            if (taskConfig.black3 > 0)
            {
                data.currConditions.Add(Conditions.condition3); data.conditionNums.Add(taskConfig.black3);
            }
            if (taskConfig.black4 > 0)
            {
                data.currConditions.Add(Conditions.condition4); data.conditionNums.Add(taskConfig.black4);
            }
            if (taskConfig.black5 > 0)
            {
                data.currConditions.Add(Conditions.condition5); data.conditionNums.Add(taskConfig.black5);
            }
            if (taskConfig.black6 > 0)
            {
                data.currConditions.Add(Conditions.condition6); data.conditionNums.Add(taskConfig.black6);
            }
            
            
        }
        if (dayData!=null)
        {
            dayData.dayLuckys += 1;
            SaveDayData();
        }
        SaveData();
        
    }
    //添加任务进度
    public void AddConditions(int id,int add)
    {
        switch (id)
        {
            case 0:
                data.conditionNums[0] -= add;
                break;
            case 1:
                data.conditionNums[1] -= add;
                break;
            case 2:
                data.conditionNums[2] -= add;
                break;
            default:
                break;
        }
    }
    
    //更新iPhone12碎片
    public void AddIPhone12(int add)
    {
        data.debrisIPhone += add;
        SaveData();
    }
    //更新iPad碎片
    public void AddIPad(int add)
    {
        data.debrisIPad += add;
        SaveData();
    }
    public void AddWDRecord(string key,float rmb,int state)
    {
        AddWDRecord(key, DateTime.Now.ToString(), rmb, state);
    }

    //增加提现记录
    public void AddWDRecord(string key, string time, float rmb, int state = 1)
    {
        RecordsData newData = new RecordsData(key, time, rmb, state);
        if (data.records.Contains(newData))
        {
            return;
        }
        data.records.Add(newData);
        data.accumulativeWD += rmb;
        SaveData();
    }
    public void ClearRecord()
    {
        data.records.Clear();
        SaveData();
    }
    //根据ecpm获取当前配置
    public ScheduleConfig GetCurrConfig()
    {
        if (scheduleConfigs == null)
        {
            scheduleConfigs = ConfigMgr.Instance.GetCongigLsit<ScheduleConfig>();
        }
        var ecpm = ConfigMgr.Instance.ecpm;
        for (int i = scheduleConfigs.Count - 1; i >= 0; i--)
        {
            if (ecpm > scheduleConfigs[i].ecpm)
            {
                return scheduleConfigs[i];
            }
        }
        return scheduleConfigs[0];
    }
    //增加一次视频红包进度
    public void GetTwistedPro()
    {
        ScheduleConfig config = GetCurrConfig();
        //净利润
        float profit = (ConfigMgr.Instance.dayEcpm * ConfigMgr.Instance.dayVideos) / 1000-data.accumulativeWD;
        for (int i = config.profits.Length-1; i >=0; i--)
        {
            if (profit>=config.profits[i])
            {
                data.redPro += config.pro[i];
                Debug.Log("摇奖进度：+" + config.pro[i]);
                return;
            }
        }
        //return config.pro[0];
    }
    //刷新当前红包值
    public void UpdateTwistedRed()
    {
        ScheduleConfig config = GetCurrConfig();
        //净利润

        float profit = (ConfigMgr.Instance.dayEcpm * ConfigMgr.Instance.dayVideos) / 1000 - data.accumulativeWD;

        for (int i = config.profits.Length - 1; i >= 0; i--)
        {
            if (profit>= config.profits[i])
            {
                data.redReward= config.showRed[i];
                return;
            }
        }
        
    }
    //获取Iphone碎片
    public int GetIPhoneDebris()
    {
        if (debrisConfigs==null)
        {
            debrisConfigs = ConfigMgr.Instance.mConfigData.GetList<DebrisConfig>();
        }
        for (int i = debrisConfigs.Count - 1; i >= 0; i--)
        {
            if (data.debrisIPhone>= debrisConfigs[i].debris)
            {
                return debrisConfigs[i].reward;
            }
        }
        return 1;
    }
    //获取Ipad碎片
    public int GetIPadDebris()
    {
        if (debrisConfigs == null)
        {
            debrisConfigs = ConfigMgr.Instance.mConfigData.GetList<DebrisConfig>();
        }
        for (int i = debrisConfigs.Count - 1; i >= 0; i--)
        {
            if (data.debrisIPad >= debrisConfigs[i].debris)
            {
                return debrisConfigs[i].reward;
            }
        }
        return 1;
    }
    public void SaveData()
    {
        SaveGame.Save(permanentDataKey, data);
    }
    public void SaveDayData()
    {
        SaveGame.Save(dayDataKey, dayData);
    }
    public class TwistePermanentData
    {
        //抽奖条件1完成数量
        public List<int> conditionNums= new List<int>();
        //条件类型
        public List<Conditions> currConditions=new List<Conditions>();
        //iPhone12碎片
        public int debrisIPhone = 0;
        //iPad碎片
        public int debrisIPad = 0;
        //当前抽奖红包进度
        public float redPro = 0;
        //当前抽奖红包值
        public float redReward = 0;
        //当前任务id
        public int taskId = 0;
        //累计提现金额
        public float accumulativeWD = 0;
        ///上次抽奖时间
        public DateTime lastLucky;
        //收件地址
        public ConsigneeAddress address;
        //提现记录
        public List<RecordsData> records;
    }
    public class TwistedDayData
    {
        //当天抽奖次数
        public int dayLuckys=0;
        //今日提现次数
        public int dayWithdraw=0;
    }
    public class ConsigneeAddress
    {
        public ConsigneeAddress(string n,string p,string a)
        {
            name = n;
            phoneNum = p;
            address = a;
        }
        public string name;
        public string phoneNum;
        public string address;
    }
    public enum Conditions
    {
        condition1,
        condition2,
        condition3,
        condition4,
        condition5,
        condition6
    }
}
