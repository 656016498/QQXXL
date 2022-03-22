using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;
using System;
/// <summary>
/// 游戏广告控制
/// </summary>
public class GameADControl : Singlton<GameADControl>
{
    public class Data
    {
        public bool IsFristEnterGame = false;
    }

    private Data GetData;
    private const string localkey = "GameADControl";
    public GameADControl()
    {
        GetData = SaveGame.Load<Data>(localkey);
        if (GetData==null)
        {
            GetData = new Data();
            SaveData();
        }
    }

    private void SaveData()
    {
        SaveGame.Save(localkey, GetData);
    }

    public const string EnableGameADKEY = "game_awaken";
    public const string EnableGameVideoKEY = "game_awaken_video";
    public const string ExitInitKEY = "exit_game";
    public const string BannerKEY = "banner";
    public const string MSGKEY = "msg";

    //开屏
    public void ShowSplash()
    {
        var isfrist = GetData.IsFristEnterGame;
        if (isfrist)
        {
            GetData.IsFristEnterGame = false;
            SaveData();
        }
        else
        {
            AdControl.Instance.ShowSplash();
        }
    }
    //移除插屏
    public void ExitInit()
    {
        AdControl.Instance.ShowIntAd(ExitInitKEY);
    }
    

    /// <summary>
    /// 退出页面插屏
    /// </summary>
    public void Exit()
    {
        AdControl.Instance.ShowIntAd(ExitInitKEY);
    }


    public void Banner(bool isShow)
    {
        if (GameManager.Instance.CurrentLevel <= 1) return;
        if (isShow)
        {
            AdControl.Instance.ShowBanner();
            Debug.Log("显示banner");
        }
        else
        {
            AdControl.Instance.HideBanner();
            Debug.Log("隐藏banner");
        }
    }

    //信息流
    public void ShowMsg(bool isShow)
    {
        if (isShow)
        {
            AdControl.Instance.ShowMessageAd(MSGKEY,-1,-1);
            Debug.Log("展示信息流");
        }
        else
        {
            AdControl.Instance.HideMessageAd();
            Debug.Log("隐藏信息流");

        }
    }

    //插屏--参数控制
    public void ShowIntAd(string id)
    {
        AdControl.Instance.Parameter(id);
    }
}
