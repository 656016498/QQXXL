using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class TimeExtension
{
    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>
    /// <returns></returns>
    public static long GetTimeStamp(bool bflag = true)
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        long ret;
        if (bflag)
            ret = Convert.ToInt64(ts.TotalSeconds);
        else
            ret = Convert.ToInt64(ts.TotalMilliseconds);
        return ret;
    }


    /// <summary>
    /// 时钟式倒计时
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetSecondString(int second)
    {
        return string.Format("{0:D2}", second / 3600) + string.Format("{0:D2}", second % 3600 / 60) + ":" + string.Format("{0:D2}", second % 60);
    }


    /// 将Unix时间戳转换为DateTime类型时间 --最小单位:秒
    /// </summary>
    /// <param name="d">double 型数字</param>
    /// <returns>DateTime</returns>
    public static System.DateTime ConvertIntDateTime(this double d)
    {      
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(mDefaultTime);       
        var time = startTime.AddSeconds(d);
        return time;
    }

    private static DateTime mDefaultTime = new System.DateTime(1970, 1, 1, 0, 0, 0);
    ///<summary>
    /// 将毫秒级时间戳 -> DateTime类型时间
    /// </summary>
    /// <param name="d">double 型数字</param>
    /// <returns>DateTime</returns>
    public static System.DateTime TimestampToDateTimeOnMillisecons(this double d)
    {        
        var startTime = TimeZone.CurrentTimeZone.ToLocalTime(mDefaultTime);         
        var time = startTime.AddMilliseconds(d);
        return time;
    }

    /// <summary>
    /// DateTime->秒 级 时间戳
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>double</returns>
    public static double DateTimeToTimestampOnSeconds(this System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(mDefaultTime);
        intResult = (time - startTime).TotalSeconds;
        return intResult;
    }

    /// <summary>
    /// DateTime-> 毫秒 时间戳
    /// </summary>
    /// <param name="mdatetime"></param>
    /// <returns></returns>
    public static double DateTimeToTimeStampOnMillisecons(this DateTime mdatetime) 
    {
        var startime = TimeZone.CurrentTimeZone.ToLocalTime(mDefaultTime);
        return (mdatetime - startime).TotalMilliseconds;
    }




    /// <summary>
    /// 日期转换成unix时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
        return Convert.ToInt64((dateTime - start).TotalSeconds);
    }


    /// <summary>
    /// 获取秒数
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static int GetCountSeconds(this TimeSpan timeSpan)
    {
        return ((timeSpan.Days * 24 + timeSpan.Hours) * 60 + timeSpan.Minutes) * 60 + timeSpan.Seconds;       
    }

    public static float Milliseconds(this TimeSpan timeSpan)
    {
        return timeSpan.GetCountSeconds() + timeSpan.Milliseconds * 0.001f;
    }



    public static DateTime StringToDateTime(this string mStr)
    {
        return Convert.ToDateTime(mStr);
    }

    public static string DateTimeToString(this DateTime mtime)
    {
        return mtime.ToString();
    }


    private static StringBuilder mSB = new StringBuilder(20);
    /// <summary>
    /// 秒转化为其他的时间
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>    
    public static string SecondTransfrom(this int seconds)
    {
        mSB.Clear();
        var day = seconds / (24 * 60 * 60);
        var hours = (seconds - day * (24 * 60 * 60)) / (60 * 60);
        var minutes = (seconds - (day * (24 * 60 * 60)) - hours * 60 * 60) / 60;
        var msecond = seconds % 60;
        if (day > 0)
        { 
            mSB.Append(day);           
            mSB.Append("天");
        }
        if (hours > 0)
        {
            mSB.Append(hours);          
            mSB.Append("小时");
        }
        if (minutes > 0)
        {
            mSB.Append(minutes);
            mSB.Append("分钟");
        }
        if (msecond > 0)
        {
            mSB.Append(msecond);
            mSB.Append("秒");
        }
        return mSB.ToString();
    }

    public static string Second_TransFrom_Math(this int seconds)
    {
        mSB.Clear();
        var day = seconds / (24 * 60 * 60);
        var hours = (seconds - day * (24 * 60 * 60)) / (60 * 60);
        var minutes = (seconds - (day * (24 * 60 * 60)) - hours * 60 * 60) / 60;
        var msecond = seconds % 60;
        if (day > 0)
        {
            if(day<10)
            {
                mSB.Append("0");
            }

            mSB.Append(day);
            mSB.Append(":");
        }
        if (hours > 0)
        {
            if(hours<10)
            {
                mSB.Append("0");
            }

            mSB.Append(hours);
            mSB.Append(":");
        }
        if (minutes > 0)
        {
            if(minutes<10)
            {
                mSB.Append("0");
            }
            mSB.Append(minutes);
            mSB.Append(":");
        }
        else
        {
            mSB.Append("00:");
        }
        if (msecond > 0)
        {
            if(msecond<10)
            {
                mSB.Append("0");
            }
            mSB.Append(msecond);           
        }
        else
        {
            mSB.Append("00");
        }
        return mSB.ToString();
    }


    

    /// <summary>
    /// 秒转为时间  时分秒
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string Second_Trasnfrom_HMS(this int seconds)
    {
        mSB.Clear();
        //var day = seconds / (24 * 60 * 60);
        //var hours = (seconds - day * (24 * 60 * 60)) / (60 * 60);
        var hours = seconds / 3600;
        var minutes = (seconds - hours * 60 * 60) / 60;
        var msecond = seconds % 60;
        if (hours > 0)
        {
            if (hours < 10)
            {
                mSB.Append("0");
            }

            mSB.Append(hours);
            //mSB.Append(":");
            mSB.Append("小时");
        }
        if (minutes > 0)
        {
            if (minutes < 10)
            {
                mSB.Append("0");
            }
            mSB.Append(minutes);
            //mSB.Append(":");
            mSB.Append("分钟");
        }
        else
        {
            //mSB.Append("00:");
            mSB.Append("00分钟");
        }
        if (msecond > 0)
        {
            if (msecond < 10)
            {
                mSB.Append("0");
            }
            mSB.Append(msecond);
            mSB.Append("秒");
        }
        else
        {
            //mSB.Append("00");
            mSB.Append("00秒");
        }
        return mSB.ToString();
    }


    public static int DayTime(DateTime lastTime,DateTime nowTime)
    {
        var counTime = nowTime.ToString("yyyy-MM-dd").StringToDateTime() - lastTime.ToString("yyyy-MM-dd").StringToDateTime();        
        var days = counTime.Days;
        return days % 7;
    }


    /// <summary>
    /// 获取当前时间的年月日
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static DateTime GetTimeSpan_day(this DateTime time)
    {
        return time.ToString("yyyy-MM-dd").StringToDateTime();
    }



    /// <summary>
    /// 获取当月的总天数
    /// </summary>
    /// <param name="mdateTime"></param>
    /// <returns></returns>
    public static int GetMonthDays(DateTime mdateTime)
    {
        return DateTime.DaysInMonth(mdateTime.Year, mdateTime.Month);
    }



    #region 读写离线时间          
    public static void SetTime(string key)
    {
        var mkey = string.Format("EXTENSION_TIME_{0}", key);
        mkey.SetPlayerPrefsString(GameTime.GameClock.NowTime.ToString());
    }

    public static int GetTimeInfo(string key)
    {
        var mkey = string.Format("EXTENSION_TIME_{0}", key);
        var value = mkey.GetPlayerPrefsString();
        if(value.IsNullOrEmpty())
        {
            XDebug.LogError("在此之前没有设置该时间节点");
            return 0;
        }
        else
        {
            var lastime = value.StringToDateTime();
            var nowTime =  GameTime.GameClock.NowTime;
            var mtime = (nowTime - lastime).GetCountSeconds();
            return mtime;
        }
    }

    /// <summary>
    /// 获取剩余时间
    /// </summary>
    /// <param name="key"></param>
    /// <param name="countSeconds"></param>
    /// <returns></returns>
    /// < 0 : 还差多少秒
    /// =0  : 不差或者刚好达到该时间
    public static int GetTime(string key, int countSeconds)
    {
        var mkey = string.Format("EXTENSION_TIME_{0}", key);
        var value = mkey.GetPlayerPrefsString();
        if(value.IsNullOrEmpty())
        {
            mkey.SetPlayerPrefsString( GameTime.GameClock.NowTime.ToString());
            return -countSeconds;
        }

        var lastTime = value.StringToDateTime();
        var nowTime =  GameTime.GameClock.NowTime;
        var mTime = (nowTime - lastTime).GetCountSeconds() - countSeconds;
        if(mTime>0)
        {           
            return 0;//已经超过设定时间
        }
        else
        {

            return mTime;//还差多少时间才能达到指定时间
        }
    }

    public static int GetTime1(string key, int countSeconds)
    {
        var mkey = string.Format("EXTENSION_TIME_{0}", key);
        var value = mkey.GetPlayerPrefsString();
        if (value.IsNullOrEmpty())
        {
            mkey.SetPlayerPrefsString( GameTime.GameClock.NowTime.ToString());
            return 1;
        }

        var lastTime = value.StringToDateTime();
        var nowTime =  GameTime.GameClock.NowTime;
        var mTime = (nowTime - lastTime).GetCountSeconds() - countSeconds;
        if (mTime > 0)
        {
            return 0;//已经超过设定时间
        }
        else
        {

            return mTime;//还差多少时间才能达到指定时间
        }
    }

    public static void DelateTimeKey(string key)
    {
        var mkey = string.Format("EXTENSION_TIME_{0}", key);
        mkey.DelateKey();
    }



    /// <summary>
    /// 判断是否是当天
    /// </summary>
    /// <param name="key"></param>
    /// <param name="isSet">是否立即储存当前时间</param>
    /// <returns></returns>
    public static bool IsToday(string key, bool isSet = true)
    {
        bool mBool = false;
        var mkey= string.Format("EXTENSION_TIME_{0}", key);
        var lastTimeStr = mkey.GetPlayerPrefsString();
        if (lastTimeStr.IsNullOrEmpty())
        {
            mBool= false;
        }
        else
        {
            var nowTime =  GameTime.GameClock.NowTime;            
            var lastTime = lastTimeStr.StringToDateTime();
            if(nowTime.Day!=lastTime.Day)
            {
                mBool= false;
            }
            else
            {
                mBool= true;
            }
        }
        if(!mBool&&isSet)
        {
            mkey.SetPlayerPrefsString( GameTime.GameClock.NowTime.DateTimeToString());
        }

        return mBool;
    }




    #endregion


    /// <summary>
    /// 判断是否处于同一天
    /// </summary>
    /// <param name="time1"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static bool IsSameDay(DateTime time1,DateTime time2)
    {
        if(time1.Year!=time2.Year||time1.Month!=time2.Month||time1.Day!=time2.Day)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
