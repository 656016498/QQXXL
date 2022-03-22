using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace GameTime
{
    public class MGameClock : IGameClock
    {
        public GameUseTime GameUseTimeType { get; set; }
        public DateTime EnterGameTime { get; set; }
        public DateTime NowTime
        {
            get
            {
                switch (GameUseTimeType)
                {
                    case GameUseTime.SystemTime:
                        return DateTime.Now;
                    case GameUseTime.ServerTime:
                        var runTime = Time.realtimeSinceStartup;//运行时间：秒
                        return EnterGameTime.AddSeconds(runTime);
                    default:
                        return DateTime.Now;
                }
            }
        }

        public ReactiveProperty<GameClockServerLoadState> ServerLoadTimeState { get; set; } = new ReactiveProperty<GameClockServerLoadState>(GameClockServerLoadState.None);

        public string ServerTimeUrl => "http://game.mis.77hd.com/api/apps/game/timestamp.action";//填入服务器Url

        public ReactiveProperty<DateTime> NowTimeSender { get; set; } = new ReactiveProperty<DateTime>();

        //时间戳转换需要一个基础值
        private static DateTime DefaultTime = new System.DateTime(1970, 1, 1, 0, 0, 0);

        public MGameClock()
        {
            Init();
        }

        public MGameClock(GameUseTime timeType)
        {
            Debug.Log("游戏时钟初始化");            
            GameUseTimeType = timeType;
            Init();
        }

        private void Init()
        {
            if (GameUseTimeType == GameUseTime.SystemTime)
            {
                EnterGameTime = DateTime.Now;
                ServerLoadTimeState.Value = GameClockServerLoadState.Success;

                NowTimeSenderInit();
                return;
            }

            //加载网络
            UnityWebRequestAPI.Instance.GetRequest(ServerTimeUrl, (bool isSucc, string info) =>
            {
                if (!isSucc)
                {
                    //获取失败，输出失败信息
                    Debug.LogError("时钟失败"+info);
                    ServerLoadTimeState.Value = GameClockServerLoadState.Faild;
                }
                else
                {
                    //获取数据成功                   
                    Debug.LogError("时钟成功" + info);

                    //自行提取服务器数据中的“时间戳”                    
                    info = info.Replace("\"", null);

                    double.TryParse(info, out double mTimeStamp);
                    //var mTimeStamp = 1611822111;         //这里随便定义的一个时间戳

                    //判断时间戳的位数,如果时间戳的位数>10的话，说明该时间戳是毫秒级，反之则是秒级
                    DateTime mTime;
                    if (mTimeStamp.ToString().Length > 10)
                    {
                        mTime = TimeZone.CurrentTimeZone.ToLocalTime(DefaultTime).AddMilliseconds(mTimeStamp);
                    }
                    else
                    {
                        mTime = TimeZone.CurrentTimeZone.ToLocalTime(DefaultTime).AddSeconds(mTimeStamp);
                    }
                    EnterGameTime = mTime;

                    ServerLoadTimeState.Value = GameClockServerLoadState.Success;

                    NowTimeSenderInit();
                }
            });
        }


        private void NowTimeSenderInit()
        {
            var timedis = 0.5f;
            Observable.Interval(System.TimeSpan.FromSeconds(timedis))
                .Subscribe(_ => 
                {
                    NowTimeSender.Value = NowTime;
                });
        }
    }
}