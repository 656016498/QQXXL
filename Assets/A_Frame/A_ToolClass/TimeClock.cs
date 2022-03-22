using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class TimeClock { 
    public static DateTime NowTime { get { return System.DateTime.Now; } }
    public static ReactiveProperty<DateTime> NowTimeListening = new ReactiveProperty<DateTime>();


    public static void Init() {
        NowTimeSenderInit();
        if ( !IsSameDay( CatManager.Instance.catData.Day,System.DateTime.Now))
        {
            CatManager.Instance.catData.Day = System.DateTime.Now;
            CatManager.Instance.catData.now = new int[2] { 0, 0 };
            if (CatManager.Instance.catData.taskGets.Count>0)
            {
                Debug.LogError(" CatManager.Instance.catData.GetTimes" + CatManager.Instance.catData.GetTimes);
                for (int i = 0; i < CatManager.Instance.catData.GetTimes; i++)
                {
                    CatManager.Instance.catData.taskGets.RemoveAt(0);
                }
            }
            CatManager.Instance.catData.GetTimes = 0;
            CatManager.Instance.catData.IsGet = new bool[2] { false, false };
           
            CatManager.Instance.SaveData();
        }
    }

    

    private static void NowTimeSenderInit()
    {
        var timedis = 0.5f;
        Observable.Interval(System.TimeSpan.FromSeconds(timedis))
            .Subscribe(_ =>
            {
                NowTimeListening.Value = NowTime;
            });
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

    /// <summary>
    /// 判断是否是连续签到
    /// </summary>
    /// <param name="lastTime"></param>
    /// <param name="nowData"></param>
    /// <returns></returns>
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

}
