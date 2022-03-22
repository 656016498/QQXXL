using BayatGames.SaveGamePro;
using EasyExcelGenerated;
using GameTime;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RedWithdrawData
{
    private static RedWithdrawData mInstace;
    public static RedWithdrawData Instance
    {
        get
        {
            if (mInstace == null)
            {
                mInstace = new RedWithdrawData();
            }
            return mInstace;
        }
    }

    public RedWithdrawData()
    {
        InitData();
    }

    public ReactiveProperty<bool> PanelNeedUpdate = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> GameRedIconUpdate = new ReactiveProperty<bool>();
     
    private string redDataKey = "redDataKey";
    private string redDayDataKey = "redDayDataKey";
    public RedData redData;
    public RedDayData redDayData;
    List<RedSection> redSections;
    List<LuckyConfig> luckyConfigs;
    List<LuckySection> luckySections;
    List<RedSchduleSection> redSchduleSections;
    List<VideoDamping> videoDampings;
    List<WithdrawDamping> withdrawDampings;

    public List<string> key3 = new List<string>() { "dayOne03", "dayOne03_1","dayOne03_2","dayOne03_3","dayOne03_4" };
    public List<string> key4 = new List<string>() { "dayOne04", "dayOne04_1", "dayOne04_2", "dayOne04_3", "dayOne04_4" };
    public List<string> key5 = new List<string>() { "dayOne05", "dayOne05_1", "dayOne5_2" };
    public List<string> key10 = new List<string>() { "dayOne1" };
    System.Random random = new System.Random();
    //跨档奖励进度
    public float extraValue;
    //服务器时间
    public DateTime nowTime;
    //抽奖提现时长(秒)
    public int luckyWDTime = 600;
    //提现连续签到天数
    public int needSiginDay = 60;
    //每日签到视频数
    public int daySiginVideo = 60;
    //限制循环刷新UI
    private bool canUpdateUI = true;
    //游戏数据（用游戏本身的）
    //今日视频
    public int TodayVideo;
    //是否首次登陆
    public bool isFirstLoging;
    //红包币监听
    public ReactiveProperty<double> RedCoin = new ReactiveProperty<double>();
    private void InitData()
    {
        //设置服务器时间
        nowTime = GameClock.NowTime;
        redData = SaveGame.Load<RedData>(redDataKey, null);
        if (redData == null)
        {
            redData = new RedData();
            redData.voucher = 0;
            redData.allVucher = 0;
            redData.voucherPro = 0;
            redData.redIcon = 0;
            redData.totalredIcon = 0;
            redData.currLuckyRmb = 0;
            redData.withdrawRmb = 0;
            redData.withdrawTimes = 0;
            redData.withdrawSiginTimes = 0;
            redData.lastLucky = DateTime.MinValue;
            redData.records = new List<RecordsData>();
            redData.isFirstBag = true;
            redData.dayCashDic = new Dictionary<float, int>() { {0.3f,5 },{0.4f,5 },{0.5f,3 },{1f,1},{10f,1},{100f,1 } };
           
        }
        redDayData = SaveGame.Load<RedDayData>(redDayDataKey, null);
        //Debug.Log("redDayData:" + redDayData);
        if (redDayData == null)
        {
            redDayData = new RedDayData();
            redDayData.todayLuckys = new int[] { 0, 0, 0 };
            redDayData.todayVucher = 0;
            redDayData.todayWithdrawRmb = 0;
            redDayData.todayWithdrawVideos = 0;
            redDayData.lastLoginTime = nowTime;
            redDayData.todayWDs = new List<float>();
        }
        else
        {
            if (nowTime.Year > redDayData.lastLoginTime.Year || nowTime.Month > redDayData.lastLoginTime.Month || nowTime.Day > redDayData.lastLoginTime.Day)
            {
                redData.dayCashDic = new Dictionary<float, int>() { { 0.3f, 5 }, { 0.4f, 5 }, { 0.5f, 3 }, { 1f, 1 }, { 10f, 1 }, { 100f, 1 } };
                redDayData.todayWithdrawRmb = 0;
                //新增每日提现次数
                redDayData.todayWithdrawVideos = 0;
                //判断是否连续
                if (!IsContinuouDay(redData.lastSignTime, GameClock.NowTime))
                {
                    redDayData.todayWithdrawVideos = 0;
                    redData.withdrawSiginTimes = 0;
                    redData.lastSignTime = GameClock.NowTime;
                }
                redDayData.todayLuckys = new int[] { 0, 0, 0 };
                if (redData.allVucher >= 8)
                {
                    redDayData.todayVucher = 5;
                }
                else
                {
                    redDayData.todayVucher = redData.allVucher;
                }
                redDayData.lastLoginTime = nowTime;
                if (redDayData.todayWithdrawVideos >= daySiginVideo)
                {
                    redData.withdrawSiginTimes = 0;
                }
                redDayData.todayWithdrawVideos = 0;
                redDayData.todayWDs = new List<float>();
            }
        }
        SaveData();
        SaveDayData();
        WeChatContral.Instance.OnAddWithdraw += AddWDRecord;
        WeChatContral.Instance.OnClear += ClearRecord;
        redSections = ConfigMgr.Instance.mConfigData.GetList<RedSection>();
        redSchduleSections = ConfigMgr.Instance.mConfigData.GetList<RedSchduleSection>();
        videoDampings = ConfigMgr.Instance.mConfigData.GetList<VideoDamping>();
        withdrawDampings = ConfigMgr.Instance.mConfigData.GetList<WithdrawDamping>();
        RedCoin.Value = redData.redIcon;
    }
    //每日提现消耗次数
    public void UseDayCashNum(float key)
    {
        if (redData.dayCashDic.ContainsKey(key))
        {
            if (redData.dayCashDic[key] > 0)
            {
                redData.dayCashDic[key]--;
            }
           
        }
        SaveData();
    }
    //每日是否可提现
    public bool IsCanCasn(float key)
    {
        if (redData.dayCashDic.ContainsKey(key))
        {
            if (redData.dayCashDic[key] > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    //每日可提现对应次数
    public int DayCashNums(float key)
    {
        if (redData.dayCashDic.ContainsKey(key))
        {
            return redData.dayCashDic[key];
        }
        return 0;
    }
    //返回提现key
    public string ReturnCashKey(float gold)
    {
        //Debug.Log("goldgoldgoldgold" + gold);
        if (gold == 0.3f)
        {
            var mNum = DayCashNums(gold);
            //Debug.Log("mNummNummNum" + mNum);
            return
                 key3[mNum - 1];
        }
       else if (gold == 0.4f)
        {
            var mNum = DayCashNums(gold);
            return
                 key4[mNum - 1];
        }
        else if (gold == 0.5f)
        {
            var mNum = DayCashNums(gold);
            return
                 key5[mNum - 1];
        }
        else if (gold == 1f)
        {
            var mNum = DayCashNums(gold);
            return
                 key10[mNum - 1];
        }
        return "";
    }
    //当前可提现额度
    public float NowCanCash
    {
        get
        {
            foreach (var item in redData.dayCashDic)
            {
                if (item.Value > 0)
                {
                    return item.Key;
                }
            }
            return 0.3f;
        }
    }
    //当前是否可提现
    public bool IsCanCash(int  add =0)
    {
       
       return redData.redIcon + add >= NowCanCash * 10000;
       
    }
    //当前还差几个红包可提现
    public int LastBagCash()
    {
        //每个红包大约多少红包币
        var mbag = ((NowCanCash * 10000) - redData.redIcon) / ConfigMgr.Instance.ecpm;
        if ( mbag < 1)
        {
            return 1;
        }
        return (int)mbag;
    }

    //判断是否连续 
    public static bool IsContinuouDay(DateTime lastTime, DateTime nowData)
    {
        if (IsSameDay(lastTime, nowData))
        {
            return true;
        }
        if (lastTime.AddDays(1).Day == nowData.Day)
        {
            Debug.Log(true);
            return true;
        }
        else
        {
            Debug.Log(false);
            return false;
        }
    }
    
    public static bool IsSameDay(DateTime time1, DateTime time2)
    {
        if (time1 == null)
        {
            return false;
        }
        if (time1.Year != time2.Year || time1.Month != time2.Month || time1.Day != time2.Day)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    //更新红包币金额(+-)
    public void UpdateRedIcon(int add)
    {
        redData.redIcon += add;
        redData.totalredIcon += add;
        RedCoin.Value = redData.redIcon;
        SaveData();
        GameRedIconUpdate.Value = false;
        GameRedIconUpdate.Value = true;
        Debug.Log("红包币+：" + add);

        var mcoin = redData.redIcon;
        //打点
        if (mcoin >= 3000 && mcoin <4000)
        {
            UmengDisMgr.Instance.CountOnPeoples("cqg_tx_arrive", string.Format("{0}_{1}", DayCashNums(0.3f), 0.3));
        }
        else if (mcoin >= 4000 && mcoin< 5000)
        {
            UmengDisMgr.Instance.CountOnPeoples("cqg_tx_arrive", string.Format("{0}_{1}", DayCashNums(0.4f), 0.4));
        }
        else if (mcoin >= 5000 && mcoin <10000)
        {
            UmengDisMgr.Instance.CountOnPeoples("cqg_tx_arrive", string.Format("{0}_{1}", DayCashNums(0.5f), 0.5));
        }
        else if (redData.redIcon >= 10000)
        {
            UmengDisMgr.Instance.CountOnPeoples("cqg_tx_arrive", string.Format("{0}_{1}", DayCashNums(1f), 1));
        }
    }
    //更新红包卷进度
    public void UpdateVoucherPro(float add)
    {
        redData.voucherPro += add;
        if (redData.voucherPro >= 1)
        {
            redData.voucherPro -= 1;
            //.Log("voucherPro" + redData.voucherPro);
            UpdateVoucher(1);
            return;
        }
        SaveData();

    }
    //更新红包卷
    public void UpdateVoucher(int add)
    {
        redData.voucher += add;
        redDayData.todayVucher += add;
        SaveData();
        SaveDayData();
        Debug.Log("红包卷+：" + add);
    }
    //更新抽奖时间
    public void UpdateLuckyTime(DateTime time)
    {
        redData.lastLucky = time;
        SaveData();
    }
    //更新抽奖次数
    public void UpdateLuckys(int index)
    {
        redDayData.todayLuckys[index] += 1;
        SaveDayData();
    }

    //提现卷提现
    public void OnVoucherWD(string key, float rmb, int state = 1)
    {

        redDayData.todayWDs.Add(rmb);
        OnWithdraw(key, rmb, state);
    }
    //红包币提现
    public void OnRedIconWD(string key, float rmb, int state = 1)
    {
        redData.redIcon -= (int)(rmb * 10000);
        RedCoin.Value = redData.redIcon;
        OnWithdraw(key, rmb, state);
    }

    public void OnWithdraw(string key, float rmb, int state = 1)
    {
        AddWDRecord(key, DateTime.Now.ToString(), rmb, state);
        redDayData.todayWithdrawRmb += rmb;
        //SaveData();
        SaveDayData();
    }

    //增加提现记录
    public void AddWDRecord(string key, string time, float rmb, int state = 1)
    {
        RecordsData newData = new RecordsData(key, time, rmb, state);
        //Debug.Log(redData.records.Contains(newData));
        if (redData.records.Contains(newData))
        {
            return;
        }
        redData.records.Add(newData);
        redData.withdrawTimes += 1;
        redData.withdrawRmb += rmb;
        SaveData();
    }
    public void ClearRecord()
    {
        redData.records.Clear();
        SaveData();
    }
    //观看签到视频
    public void OnVideoSigin()
    {
        redDayData.todayWithdrawVideos += 1;
        SaveDayData();
        if (redDayData.todayWithdrawVideos >= daySiginVideo)
        {
            redData.withdrawSiginTimes += 1;
            redData.lastSignTime = GameClock.NowTime;
            SaveData();
        }
    }

    public void SaveData()
    {

        SaveGame.Save(redDataKey, redData);
        PanelNeedUpdate.Value = false;
        PanelNeedUpdate.Value = true;
        ////Debug.Log("save");
        //if (canUpdateUI)
        //{

        //    canUpdateUI = false;
        //    Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ => { 
        //        canUpdateUI = true;
        //        //Debug.LogWarning("canUpdateUI:" + canUpdateUI);
        //    });
        //}
    }
    public void SaveDayData()
    {
        SaveGame.Save(redDayDataKey, redDayData);
    }
    ///设置新人红包
    public void SetFirstBag()
    {
        redData.isFirstBag = false;
        SaveData();
    }
    /// <summary>
    /// 获取红包币
    /// </summary>
    /// <returns></returns>
    public int GetRedIcon()
    {
        Debug.Log("随机红包币" + ConfigMgr.Instance.RedCoin());
        Debug.Log("获取androidEcpm" + ConfigMgr.Instance.ecpm);
        float num = 0;
        if (RedWithdrawData.Instance.redData.isFirstBag)
        {
            num = ConfigMgr.Instance.RedCoin();
        }
        else
        {
            num = ConfigMgr.Instance.RedCoin() * 1.5F;
        }
      
        var mreturn = Convert.ToInt16(num);
        if (mreturn > 800 && !redData.isFirstBag)
        {
            mreturn = 800;
        }
        else if (mreturn<10)
        {
            mreturn = 10;
        }
        XDebug.Log("mreturn" + mreturn);
        return
           mreturn;

        #region
        int icon = RedWithdrawData.Instance.redData.redIcon;
        int min = 0;
        int max = 0;
        if (icon < redSections[0].Section)
        {
            min = redSections[0].Min;
            max = redSections[0].Max;
        }
        else if (icon >= redSections[redSections.Count - 1].Section)
        {
            min = redSections[redSections.Count - 1].Min;
            max = redSections[redSections.Count - 1].Max;
        }
        else
        {
            for (int i = redSections.Count - 1; i >= 0; i--)
            {
                if (icon >= redSections[i].Section)
                {
                    min = redSections[i + 1].Min;
                    max = redSections[i + 1].Max;
                    break;
                }
            }
        }
        return random.Next(min, max);
        #endregion
    }
    /// <summary>
    /// 获取错失红包币
    /// </summary>
    /// <returns></returns>
    public int GetMissIcon()
    {
        int icon = RedWithdrawData.Instance.redData.redIcon;
        int min = 0;
        int max = 0;
        if (icon < redSections[0].Section)
        {
            min = redSections[0].MinComfort;
            max = redSections[0].MaxComfort;
        }
        else if (icon >= redSections[redSections.Count - 1].Section)
        {
            min = redSections[redSections.Count - 1].MinComfort;
            max = redSections[redSections.Count - 1].MinComfort;
        }
        else
        {
            for (int i = redSections.Count - 1; i >= 0; i--)
            {
                if (icon >= redSections[i].Section)
                {
                    min = redSections[i].MinComfort;
                    max = redSections[i].MaxComfort;
                    break;
                }
            }
        }
        Debug.Log("min:" + min);
        Debug.Log("max:" + max);
        return random.Next(min, max);
    }

   

    /// <summary>
    /// 获取转盘配置
    /// </summary>
    /// <param name="index">转盘类型</param>
    /// <param name="isFristDay">是否第一天</param>
    /// <returns></returns>
    public LuckyConfig GetLuckyConfig(int index, bool isFristDay)
    {
        if (luckyConfigs == null)
        {
            luckyConfigs = ConfigMgr.Instance.mConfigData.GetList<LuckyConfig>();
        }

        List<LuckyConfig> configs;
        if (isFristDay && index == 1)
        {
            configs = luckyConfigs.FindAll(x => x.tableType == 1);
        }
        else
        {
            configs = luckyConfigs.FindAll(x => x.tableType == index + 1);
        }
        var luckyTimes = RedWithdrawData.Instance.redDayData.todayLuckys[index - 1] + 1;
        //Debug.Log("luckyTimes:" + luckyTimes);
        //Debug.Log("configs.Count:" + configs.Count);
        if (luckyTimes > configs.Count) return null;


        return configs.Find(x => x.luckys == luckyTimes);

    }

    public LuckyConfig GetSecurityNum(int level)
    {
        if (luckyConfigs == null)
        {
            luckyConfigs = ConfigMgr.Instance.mConfigData.GetList<LuckyConfig>();
        }
        var currConfig = GetLuckyConfig(level, false);
        if (currConfig == null)
        {
            return null;
        }
        var securityConfig = luckyConfigs.Find(x => x.tableType == currConfig.tableType && x.luckys >= currConfig.luckys && x.rewardType == 4);

        if (securityConfig != null)
        {
            return securityConfig;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取接近/大额提现配置
    /// </summary>
    /// <returns></returns>
    public LuckySection GetLuckySection()
    {
        if (luckySections == null)
        {
            luckySections = ConfigMgr.Instance.mConfigData.GetList<LuckySection>();
        }
        int icon = RedWithdrawData.Instance.redData.redIcon;
        for (int i = luckySections.Count - 1; i >= 0; i--)
        {
            if (icon >= luckySections[i].redIcon)
            {

                return luckySections[i];
            }
        }
        return luckySections[0];
    }



    /// <summary>
    /// 获取提现卷进度
    /// </summary>
    /// <param name="haveWithdraw">已提现金额
    /// </param>
    /// <param name="videos">今日视频次数</param>
    /// <returns></returns>
    public float GetVoucherPro()
    {
        var d = RedWithdrawData.Instance.redData;
        float haveWithdraw = d.withdrawRmb;
        int videos = TodayVideo;
        var pro = d.voucherPro * 100;
        var times = RedWithdrawData.Instance.redDayData.todayVucher + 2;
        if (isFirstLoging) times -= 1;
        //获取当前提现次数配置
        var confgs = redSchduleSections.FindAll(x => x.linesId == times);
        if (confgs.Count <= 0)
        {
            return 0;
        }
        //排序
        confgs.Sort((x, y) => x.progressAlue.CompareTo(y.progressAlue));
        var _valueLevel = ConfigMgr.Instance._valueLevel;
        float min = 0;
        float max = 0;
        for (int i = confgs.Count - 1; i >= 0; i--)
        {
            if (pro >= confgs[i].progressAlue)
            {
                min = confgs[i].min[_valueLevel];
                max = confgs[i].max[_valueLevel];
                break;
            }
        }
        //基础进度
        var basePro = ((float)random.Next((int)(min * 100), (int)(max * 100)) / 10000);
        var videoDamping = GetVideoDamping(videos);
        var withdrawDamping = GetWithdrawDamping(haveWithdraw);
        //衰减后进度
        float rPro = basePro * videoDamping * withdrawDamping;
        //跨档补充
        if (rPro + RedWithdrawData.Instance.redData.voucherPro >= 1)
        {
            extraValue = (float)random.Next(9, 12) / 100;
            rPro += extraValue;
            Debug.Log("extraValue:" + extraValue);
        }
        //Debug.Log("basePro:" + basePro);
        //Debug.Log("videoDamping:" + videoDamping);
        //Debug.Log("withdrawDamping:" + withdrawDamping);
        //Debug.Log("rPro:" + rPro);
        return rPro;
    }
    //获取视频衰减
    private float GetVideoDamping(int videos)
    {
        for (int i = 0; i < videoDampings.Count; i++)
        {
            if (videoDampings[i].min <= videos && videos < videoDampings[i].max)
            {
                return (float)videoDampings[i].scale / 100;
            }
        }
        return 1;
    }
    //获取已提现衰减
    private float GetWithdrawDamping(float haveWithdraw)
    {
        for (int i = 0; i < withdrawDampings.Count; i++)
        {
            if (withdrawDampings[i].min <= haveWithdraw && haveWithdraw < withdrawDampings[i].max)
            {
                return (float)withdrawDampings[i].scale / 100;
            }
        }
        return 1;
    }
    public class RedData
    {
        ///提现卷
        public int voucher;
        ///累计获得提现卷
        public int allVucher;
        ///提现卷进度
        public float voucherPro;
        ///红包币
        public int redIcon;
        //累计红包币
        public int totalredIcon;
        //当前转盘提现金额
        public float currLuckyRmb;
        //当前转盘提现Key
        public string currLuckyKey;
        //累计提现金额
        public float withdrawRmb;
        //累计提现次数
        public float withdrawTimes;
        //累计提现视频签到次数
        public int withdrawSiginTimes;
        //累计签到标记
        public DateTime lastSignTime;
        ///上次抽奖时间
        public DateTime lastLucky;

        ///历史提现记录
        public List<RecordsData> records;

        public bool isFirstBag;

        //每日提现次数
        public Dictionary<float, int> dayCashDic;
    }
    public class RedDayData
    {
        ///今日获得提现卷
        public int todayVucher;
        //上次登陆时间
        public DateTime lastLoginTime;
        //今日抽奖次数
        public int[] todayLuckys;
        //今日提现视频签到次数
        public int todayWithdrawVideos;
        //今日提现金额
        public float todayWithdrawRmb;
        ///今日提现记录
        public List<float> todayWDs;
    }


}
public enum WithdrawType
{
    Vouchers,
    RedIcon,
    Lucky
}
