using EasyExcel;
using EasyExcelGenerated;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMgr
{
    private static ConfigMgr mInstace;
    public static ConfigMgr Instance
    {
        get
        {
            if (mInstace == null)
            {
                mInstace = new ConfigMgr();
            }
            return mInstace;
        }
    }

    public ConfigMgr()
    {
        mConfigData = new EEDataManager();
        mConfigData.Load();
        InitRedConfig();
    }
    public void Init()
    {
        pigConfigs = mConfigData.GetList<PiggyBankSection>();

    }

    public EEDataManager mConfigData;
    public List<PiggyBankSection> pigConfigs;

    List<DynamicRedConfig> dynamicReds;
    System.Random random = new System.Random();


    //总动态数值
    public float ecpm=50;
    public float ltv;
    public int videos;
    //今日动态数组
    public float dayEcpm;
    public float DayLtv;
    public int dayVideos;

    //游戏数据（用游戏本身的）
    //今日视频
    public int TodayVideo;
    //全部视频
    public int AllVideo;

    //通过等级/关卡
    public int Alllevel;

    //用户价值等级
    public int _valueLevel = 2;
    //是否动态调节数值
    private bool isAdjustData = true;
    //判断用户价值视频次数分割线
    private int valueVideo = 20;
    //用户等级刷新频率
    private int updateUserValueInv = 10;
    //public int ValueLevel
    //{
    //    get { return _valueLevel; }
    //    set {
    //        if (value>=0&&value<redSchduleSections[0].max.Length)
    //        {
    //            _valueLevel = value;
    //        }
    //    }
    //}
    //获取配置列表
    public List<T> GetCongigLsit<T>() where T : EERowData
    {
        return mConfigData.GetList<T>();
    }








    #region 动态红包相关
    /// <summary>
    /// 当前等级用户是否超过提现限额
    /// </summary>
    /// <returns></returns>
    public bool IsRestrictWD(float rmb)
    {
        if (dynamicReds == null)
        {
            dynamicReds = mConfigData.GetList<DynamicRedConfig>();
        }
        int id = _valueLevel;
        var config = mConfigData.Get<DynamicRedConfig>(id + 1);
        if (config.dayWD > RedWithdrawData.Instance.redDayData.todayWithdrawRmb + rmb)
        {
            return false;
        }
        return true;
    }
    private int GetDynamicValue(int videos, float v)
    {
        if (dynamicReds == null)
        {
            dynamicReds = mConfigData.GetList<DynamicRedConfig>();
        }
        if (videos >= valueVideo)
        {
            for (int i = dynamicReds.Count - 1; i >= 0; i--)
            {
                if (v >= dynamicReds[i].value2)
                {
                    return dynamicReds[i].id;
                }
            }
        }
        else
        {
            for (int i = dynamicReds.Count - 1; i >= 0; i--)
            {
                if (v >= dynamicReds[i].value1)
                {
                    return dynamicReds[i].id;
                }
            }
        }
        return _valueLevel + 1;
    }

    public void UpdateUserValue1()
    {
        AdControl.Instance.GetAdIntDynamicParams((info) =>
        {
            var strs = info.Split(':');
            Debug.Log("info:" + info);
            float ecpm = float.Parse(strs[0]);
            float ltv = float.Parse(strs[1]);
            int videos = int.Parse(strs[2]);

            float wd = RedWithdrawData.Instance.redDayData.todayWithdrawRmb;
            if (wd == 0)
                wd = 0.3f;
            float v = ltv / wd;
            if (videos < valueVideo)
            {
                _valueLevel = GetDynamicValue(videos, ecpm) - 1;
            }
            else
            {
                _valueLevel = GetDynamicValue(videos, v) - 1;
            }
            UmengDisMgr.Instance.CountOnNumber("dau_ecpm", string.Format("{0}_{1}", AllVideo / 10, ecpm));

        });
    }
    /// <summary>
    /// 更新用户等级--获取ecpm 用户等级 观看视频次数
    /// </summary>
    public void UpdateUserValue()
    {

        AdControl.Instance.GetAdIntDynamicParams((info) =>
        {
            var strs = info.Split(':');
            Debug.Log("DynamicInfoinfo:" + info);
            ecpm = float.Parse(strs[0]);
            ltv = float.Parse(strs[1]);
            videos = int.Parse(strs[2]);
            //设置每日ecpm
            Debug.Log("今天ecpm" + ecpm);
            //玩家进来观看首次视频确认转盘ecpm
            if (videos >=1 && LotteryDataManger.Instance.mdata.loteryEcpm <=0)
            {
                LotteryDataManger.Instance.SetLoteryEcpm(ecpm);
            } 
            float wd = RedWithdrawData.Instance.redDayData.todayWithdrawRmb;
            if (wd == 0)
                wd = 0.3f;
            float v = ltv / wd;
        if (videos > 0 && videos % updateUserValueInv == 0)
        {
            int vl;
            if (videos < valueVideo)
            {
                vl = GetDynamicValue(videos, ecpm) - 1;
            }
            else
            {
                vl = GetDynamicValue(videos, v) - 1;
            }
            if (isAdjustData)
            {
                _valueLevel = vl;
            };
                UmengDisMgr.Instance.CountOnNumber("dau_ecpm", string.Format("{0}_{1}", AllVideo / 10, ecpm));
            }

            if (videos == 1)
            {
                UmengDisMgr.Instance.CountOnNumber("new_user_ecpm", ecpm.ToString());
            }
            AdControl.Instance.GetAdIntDynamicParams2((inf) =>
            {
                var strs2 = info.Split(':');
                dayEcpm = float.Parse(strs[0]);
                DayLtv = float.Parse(strs[1]);
                dayVideos = int.Parse(strs[2]);

                    //赋值每日首次视频
                    Debug.Log("每日视频次数" + dayVideos);
                if (dayVideos == 1)
                {
                    ShowDailyInfoDataControl.Instance.SetDailyVedio(1);
                        //每日首次视频设置七日提现显示（存档）
                        ShowDailyInfoDataControl.Instance.SetDailyFirstEcpm(ecpm);
                }
            });
        });

}
    /// <summary>
    /// 设置用户价值类型
    /// </summary>
    /// <param name="valueLevel">-1：动态，0：D档，1：C档，2：B档，3：A档,4：S档</param>
    public void SetDynamicConfig(string valueLevel)
    {
        int VL = int.Parse(valueLevel);
        if (VL == -1)
        {
            isAdjustData = true;
        }
        else
        {
            _valueLevel = VL;
        }
        Debug.Log("动态值：" + VL);
    }
    #endregion

    ///新ecpm红包数值
    //下放奖励=频次系数*红包币系数*实时ECPM值*（奖励系数+常数）
    public List<EcpmConfig> mEcpmConfigs;
    public List<VedioConfig> mVedioConfigs;
    public List<RedValueConfig> mRedValueConfigs;

    //初始红包表
    public void InitRedConfig()
    {
        mEcpmConfigs = mConfigData.GetList<EcpmConfig>();
        mVedioConfigs = mConfigData.GetList<VedioConfig>();
        mRedValueConfigs = mConfigData.GetList<RedValueConfig>();
        mLoteryEcpmConfigs = mConfigData.GetList<LoteryEcpmConfig>();
        mLoteryFillConfigs = mConfigData.GetList<LoteryFillConfig>();
    }

    //视屏频次系数
    float RandomVedioModulus()
    {
        foreach (var item in mVedioConfigs)
        {
            if (videos < item.VedioMax)
            {
                return item.GetNum;
            }
        }
        XDebug.Log("返回视频频次最大区间");
        //超过120-后取110-120区间
        return
          mVedioConfigs[mVedioConfigs.Count - 1].GetNum;
    }
    //红包币系数
    float RandomHbCoinModulus()
    {
        var mCoin = RedWithdrawData.Instance.redData.totalredIcon;
        foreach (var item in mRedValueConfigs)
        {
            if (mCoin < item.WxMax)
            {
                return item.GetNum;
            }
        }
        XDebug.Log("返回红包币系数最大值");
        return
            mRedValueConfigs[mRedValueConfigs.Count - 1].GetNum;


    }
    //奖励系数 
    float RandomRewardModulus()
    {
        foreach (var item in mEcpmConfigs)
        {
            if (ecpm < item.EcpmMax)
            {
                return item.RewNum;
            }
        }
        XDebug.Log("Ecpm大于500取最大值");
        return
            mEcpmConfigs[mEcpmConfigs.Count - 1].RewNum;

    }
    //Ecpm奖励常数
    float RandomRewardConstant()
    {
        foreach (var item in mEcpmConfigs)
        {
            if (ecpm < item.EcpmMax)
            {
                return item.NorNum;
            }
        }
        XDebug.Log("Ecpm大于500取常数最大值");
        return
            mEcpmConfigs[mEcpmConfigs.Count - 1].NorNum;
    }
    //返回值ECPM常数
    float ReturnConstant()
    {
        if (RandomRewardConstant() - videos < 0)
        {
            return 0;
        }
        else
        {
            XDebug.Log("ecpm常量" + (RandomRewardConstant() - videos));
            return RandomRewardConstant() - videos;
        }
    }
    //随机红包币
    public int  RedCoin()
    {
        TestEcpm();
        Debug.Log("本次ecpm" + ecpm);
        if (RedWithdrawData.Instance.redData.isFirstBag) return 1888;
        Debug.Log("视频频次系数" + RandomVedioModulus());
        Debug.Log("红包系数" + RandomHbCoinModulus());
        Debug.Log("奖励系数" + RandomRewardModulus());
        Debug.Log("Ecpm奖励常数" + ReturnConstant());
        Debug.Log("当前ECPM" + ecpm);
        Debug.Log("第"+ videos+"视频后随机红包币");
        float xs = AndroidHelper.Instance.ToRate(RewDynType.Red);
        var mreturn= (int)((RandomVedioModulus() * RandomHbCoinModulus() * ecpm * (RandomRewardModulus() + ReturnConstant()))* xs);
        Debug.Log("返回随机红包币" + mreturn);
        if (EcpmPanel.ShowPanel)
        {
            var mpanel = UIManager.Instance.ShowPopUp<EcpmPanel>();
            mpanel.RefrishUi(RandomVedioModulus(), RandomHbCoinModulus(), ecpm,RandomRewardModulus(), ReturnConstant(), mreturn, LotreyFillPro(), xs);
        }
        return
            mreturn;

    }
    
    //测试
    public void TestEcpm()
    {
#if UNITY_EDITOR
        if (AdControl.Instance.isShowAd)
        {
            //测试ecpm
            ecpm = 150;
            videos=10;
        }
#endif

    }

    ///转盘进度条进度
    ///进度条奖励=进度条系数*实时ECPM值*ECPM系数/100
    public List<LoteryEcpmConfig> mLoteryEcpmConfigs;
    public List<LoteryFillConfig> mLoteryFillConfigs;

    //进度条系数
    float RandomLotModules()
    {
        var nowValue = LotteryDataManger.Instance.mdata.fillPro;
        foreach (var item in mLoteryFillConfigs)
        {
            if (nowValue < item.WxMax)
            {
                return item.GetNum;
            }
        }
        return
        mLoteryFillConfigs[mLoteryFillConfigs.Count - 1].GetNum;
    }
    //ecpm系数
    float RandomLotEcpmMoudles()
    {
        foreach (var item in mLoteryEcpmConfigs)
        {
            if (ecpm < item.EcpmMax)
            {
                return item.RewNum;
            }
        }
        return
            mLoteryEcpmConfigs[mLoteryEcpmConfigs.Count - 1].RewNum;

    }
    //随机转盘进度条
    public float LotreyFillPro()
    {
        TestEcpm();
        //XDebug.Log("转盘进度条系数" + RandomLotModules());
        //XDebug.Log("ecpm系数" + RandomLotEcpmMoudles());
        //return
        //    (RandomLotModules() * ecpm * RandomLotEcpmMoudles()) / 100;
        //2.ECPM值/100*1/？元
        var num = LotteryDataManger.Instance.mdata.cashNum;
        if (num == 0) num = 1;
        Debug.Log("当前额度" + num);
        var mreturn = (ecpm / 100) * (1/num);
        Debug.Log("返回进度条" + mreturn);
        return mreturn;
    }
}
