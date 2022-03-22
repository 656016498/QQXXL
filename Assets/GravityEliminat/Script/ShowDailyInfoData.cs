using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BayatGames.SaveGamePro;
using System;

public class ShowDailyInfoData 
{
    public float dailyEpm;//每日首次ecpm 
    public int dailyVedio;//每日观看视频次数
    public DateTime lastTime;//上次时间
    public int dailyShowSignPanel;//是否显示签到
    public int dailyShowPigPanel;//是否显示存钱罐界面
    public int dailyShowSevenPanel;//是否显示七日提现界面
    public int normalShowSevenPanel;
    
   
    public ShowDailyInfoData()
    {
        dailyEpm = 0;
        dailyVedio = 0;
        dailyShowSignPanel = 0;
        dailyShowPigPanel = 0;
        dailyShowSevenPanel = 0;
        normalShowSevenPanel = 0;
        lastTime = GameTime.GameClock.NowTime;
    }
}

public class ShowDailyInfoDataControl
{
    private static ShowDailyInfoDataControl instance;
    public static ShowDailyInfoDataControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ShowDailyInfoDataControl();
            }
            return instance;
        }
    }
    public ShowDailyInfoDataControl()
    {
        if(!isInit)
        {
            LoadData();
            isInit = true;
        }
    }
    bool isInit = false;
    public ShowDailyInfoData mdata;
    public const string local_key = "ShowDailyInfoData_Key";
    private void LoadData()
    {
        mdata = SaveGame.Load<ShowDailyInfoData>(local_key);
        if (mdata == null)
        {
            mdata = new ShowDailyInfoData();
        }
        else
        {
            if (!TimeExtension.IsSameDay(mdata.lastTime, GameTime.GameClock.NowTime))
            {
                //隔天刷新
                mdata.lastTime = GameTime.GameClock.NowTime;
                mdata.dailyShowSignPanel = 0;
                mdata.dailyShowPigPanel = 0;
                mdata.dailyShowSevenPanel = 0;

            }
        }
        SaveData();
    }
    private void SaveData()
    {
        SaveGame.Save(local_key, mdata);
    }
    //设置首次首次ecpm值
    public void SetDailyFirstEcpm(float value)
    {
        if (mdata.dailyVedio == 1)
        {
            mdata.dailyEpm = value;
            Debug.Log("每日首个视频ECPM" + mdata.dailyEpm);
            SaveData();
        }
    }

    //设置每日视频次数
    public void SetDailyVedio(int vedioNum)
    {
        if (mdata.dailyVedio == 0)
        {
            mdata.dailyVedio = vedioNum;
            SaveData();
        }
    }


    //是否可以显示七日提现
    public bool IsShowSevenDayCash
    {
        get
        {
            return mdata.dailyEpm >= 200;
        }

    }



    //每日首次登陆显示界面
    public void DailyShowSignPanel()
    {
        mdata.dailyShowSignPanel += 1;
        Debug.Log("mdata.dailyShowSignPanel" + mdata.dailyShowSignPanel);
        mdata.dailyShowPigPanel += 1;
        if (SignDataControl.Instance.mdata.canSign)//显示签到
        {

            Observable.TimeInterval(System.TimeSpan.FromSeconds(.5f)).Subscribe(_ =>
            {
                UIManager.Instance.ShowPopUp<SignPanel>((SignPanel mpanel) =>
                {
                    mpanel.AddListenToBtn(
                        () => { GameADControl.Instance.ShowIntAd("daily_half"); mpanel.Hide(); DailyShowPigPanel(); },
                        () =>
                        {
                            if (SignDataControl.Instance.mdata.canSign)
                            {
                                AdControl.Instance.ShowRwAd("daily_sign_video", () =>
                                {
                                    Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(value =>
                                    {
                                        Debug.Log("mpanel.mSignType.awardType)" + mpanel.mSignType.awardType);
                                        SignDataControl.Instance.AddSignIndex(mpanel.mSignType.awardType);
                                        UmengDisMgr.Instance.CountOnPeoples("daily_suc", string.Format("{0}", SignDataControl.Instance.mdata.signDay));
                                    });

                                });

                            }
                            else
                            {
                                ShowPublicTip.Instance.Show("今天已签到~");
                            }
                        },
                        () =>
                        {
                            var nowType = SignDataControl.Instance.nowTimeType.Value;
                            if (nowType == SignDataControl.TimeType.Stop)
                            {
                                AdControl.Instance.ShowRwAd("daily_online_video", () =>
                                {
                                    Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(value =>
                                    {
                                        var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
                                        var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                                        var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                                        popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                                      {
                                          popup2.effect.SetActive(false);
                                          Debug.Log("关闭红包二级界面");
                                          if (!GameManager.Instance.isCash)
                                          {
                                              RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                                          }
                                      });
                                        SignDataControl.Instance.SetOnline();
                                        mpanel.RefrishUi();

                                    });
                                });

                            }
                            else
                            {
                                ShowPublicTip.Instance.Show("倒计时结束即可领取~");
                            }
                        });
                });
            });
        }
        else if (mdata.dailyShowPigPanel == 1)//显示金猪
        {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(.5f)).Subscribe(_ =>
            {
                var mpanel = UIManager.Instance.ShowPopUp<PiggyBankUI>();
                mpanel.OnRefresh();
            });
        }
        SaveData();
    }

    //每日首次显示存钱罐界面
    public void DailyShowPigPanel()
    {
        //mdata.dailyShowPigPanel +=1;
        if (mdata.dailyShowPigPanel == 1)
        {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(.5f)).Subscribe(_ =>
            {
                var mpanel = UIManager.Instance.ShowPopUp<PiggyBankUI>();
                mpanel.OnRefresh();
            });
               
        }
        SaveData();
    }

    //当ecpm>200后自动弹七日提现界面
    public void DailyShowSevenPanel()
    {
       
            if (IsShowSevenDayCash)
            {
                mdata.dailyShowSevenPanel += 1;
                if (mdata.dailyShowSevenPanel == 1)
                {
                Observable.TimeInterval(System.TimeSpan.FromSeconds(.5f)).Subscribe(_ =>
                {
                    UIManager.Instance.ShowPopUp<SevenWithdrawPanel>();
                });
                
} 
            }
            SaveData();
       
            
    }
    //完成七日提现后自动弹七日提现界面
    public void CompleteShowSevenPanel()
    {
        if (mdata.normalShowSevenPanel == 1)
        {
            UIManager.Instance.ShowPopUp<SevenWithdrawPanel>();
            mdata.normalShowSevenPanel = 0;
        }
        SaveData();
    }
    public void SetNormalShowSevenPanel()
    {
        mdata.normalShowSevenPanel = 1;
        SaveData();
           
    }
}