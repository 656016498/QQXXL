using BayatGames.SaveGamePro;
using EasyExcelGenerated;
using GameTime;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PiggyBankData
{
    private static PiggyBankData mInstace;
    public static PiggyBankData Instance
    {
        get
        {
            if (mInstace == null)
            {
                mInstace = new PiggyBankData();
            }
            return mInstace;
        }
    }

    public PiggyBankData()
    {
        InitData();
    }
    public ReactiveProperty<bool> PanelNeedUpdate = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> PigRedIconUpdate = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> PushPopup = new ReactiveProperty<bool>();
    public string pigDataKey = "piggyBankDataKey";
    public PigData pigData;
    //服务器时间
    public DateTime nowTime;

    public List<PiggBankKeys> bankKeys;
    //当前提现金额
    public float currWD;
    public List<PiggyBankSection> pigConfigs;
    System.Random random = new System.Random();

    public void InitData()
    {
        //设置服务器时间
        nowTime = GameClock.NowTime;
        pigData = SaveGame.Load<PigData>(pigDataKey, null);
        pigConfigs = ConfigMgr.Instance.mConfigData.GetList<PiggyBankSection>();
        bankKeys = ConfigMgr.Instance.GetCongigLsit<PiggBankKeys>();
        if (pigData == null)
        {
            pigData = new PigData();
            pigData.lastWDTime = nowTime;
            pigData.deposit = 888;
            pigData.records = new List<RecordsData>();
            pigData.cashDic = new Dictionary<float, int>() { {0.3f,0},{0.4f,0 },{0.5f,0 } };
            pigData.IsFirstPig = true;
        }
        Debug.Log("lastWDTime:" + pigData.lastWDTime);
        WeChatContral.Instance.OnAddWithdraw += AddWDRecord;
        WeChatContral.Instance.OnClear += ClearRecord;

        SaveData();
    }

    //更新存钱罐金额(+-)
    public void UpdatePiggyBank(int add)
    {
        pigData.deposit += add;
        if (DateTime.Now.Year > pigData.lastWDTime.Year || DateTime.Now.Month > pigData.lastWDTime.Month || DateTime.Now.Day > pigData.lastWDTime.Day)
        {
            if (pigData.deposit >= 30000)
            {
                var panel=  UIManager.Instance.ShowPopUp<PiggyBankUI>();
                panel.OnRefresh();
            }
        }
        SaveData();
        UIManager.Instance.Refresh<GamePanel>();

    }
    //提现
    public void OnWithDraw(string key, float rmb, int state = 1)
    {

        currWD = rmb;
        pigData.deposit -= (int)(rmb * 10000);
        pigData.lastWDTime = nowTime;
        AddWDRecord(key, nowTime.ToString(), rmb, state);
        RedWithdrawData.Instance.redDayData.todayWithdrawRmb += rmb;


        //提现返还
        pigData.deposit += 1500;
        RedWithdrawData.Instance.SaveDayData();
        UIManager.Instance.Refresh<GamePanel>();
       
    }

    //添加提现次数(用于打点)
    public void AddCashDicTimes(float key)
    {
        if (pigData.cashDic.ContainsKey(key))
        {
            pigData.cashDic[key] += 1;
        }
        SaveData();
    }
    //返回提现成功次数
    public int AddCashSucNums(float key)
    {
        if (pigData.cashDic.ContainsKey(key))
        {
            return pigData.cashDic[key];
        }
        return 0;
    }
    //增加提现记录
    public void AddWDRecord(string key, string time, float rmb, int state = 1)
    {
        RecordsData newData = new RecordsData(key, time, rmb, state);
        //Debug.Log(redData.records.Contains(newData));
        if (pigData.records.Contains(newData))
        {
            return;
        }
        pigData.records.Add(newData);
        SaveData();
    }
    public void ClearRecord()
    {
        pigData.records.Clear();
        SaveData();
    }
    private void SaveData()
    {
        SaveGame.Save(pigDataKey, pigData);
        PanelNeedUpdate.Value = false;
        PanelNeedUpdate.Value = true;
        PigRedIconUpdate.Value = false;
        PigRedIconUpdate.Value = true;
    }

    /// <summary>
    /// 获取存钱罐红包币
    /// </summary>
    /// <param name="videos">今日视频次数</param>
    /// <returns></returns>
    public int GetPigIcon()
    {
        #region
        //int picIcon = PiggyBankData.Instance.pigData.deposit;
        //int min = 0;
        //int max = 0;
        //var _valueLevel = ConfigMgr.Instance._valueLevel;
        ////Debug.Log("picIcon:" + picIcon);
        //if (picIcon <= pigConfigs[1].pigIcon[_valueLevel])
        //{
        //    min = pigConfigs[0].min[_valueLevel];
        //    max = pigConfigs[0].max[_valueLevel];
        //}
        //else
        //{
        //    for (int i = pigConfigs.Count - 1; i >= 0; i--)
        //    {
        //        if (pigConfigs[i].pigIcon[_valueLevel] != 0 && picIcon >= pigConfigs[i].pigIcon[_valueLevel])
        //        {
        //            min = pigConfigs[i].min[_valueLevel];
        //            max = pigConfigs[i].max[_valueLevel];
        //            break;
        //        }
        //    }
        //}

        ////Debug.Log("min:" + min);
        ////Debug.Log("max:" + max);
        //return random.Next(min, max);
        #endregion
        XDebug.Log("随机金猪币" + ConfigMgr.Instance.RedCoin());
        var num = ConfigMgr.Instance.RedCoin() * 0.3f* AndroidHelper.Instance.ToRate(RewDynType.Pig);
        var mreturn = Convert.ToInt16(num);
        XDebug.Log("返回金猪币" + mreturn);
        //if (mreturn < 10)
        //{
        //    mreturn = 10;
        //}
        //else if(mreturn>800)
        //{
        //    mreturn = 800;
        //}
        if (pigData.IsFirstPig)
        {
            mreturn = 800;
            pigData.IsFirstPig = false;
            SaveData();
        }
        //XDebug.LogError("金猪" + mreturn);
        return
           mreturn;
    }

    public class PigData
    {
        //上次提现时间
        public DateTime lastWDTime;
        //存钱罐存款
        public int deposit;
        //提现记录
        public List<RecordsData> records;
        //每个金额对应
        public Dictionary<float, int> cashDic;
        //首次金猪
        public bool IsFirstPig;
    }
}


