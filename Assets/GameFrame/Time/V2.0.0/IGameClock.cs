using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTime
{
    using System;
    using UniRx;

    public enum GameUseTime
    {
        /// <summary>
        /// 系统时间
        /// </summary>
        SystemTime,
        /// <summary>
        /// 服务器时间
        /// </summary>
        ServerTime
    }


    public enum GameClockServerLoadState 
    {
        None,
        Success,
        Faild
    }

    public interface IGameClock
    {
        /// <summary>
        /// 游戏使用时间类型
        /// </summary>
        GameUseTime GameUseTimeType { get; set; }
        /// <summary>
        /// 进入游戏的时间
        /// </summary>
        DateTime EnterGameTime { get; set; }
        /// <summary>
        /// 现在的时间
        /// </summary>
        DateTime NowTime { get; }

        /// <summary>
        /// 服务器时间URL
        /// </summary>
        string ServerTimeUrl { get; }


        /// <summary>
        /// 服务器连接状态
        /// </summary>
        ReactiveProperty<GameClockServerLoadState> ServerLoadTimeState { get; set; }
        
        /// <summary>
        /// 当前时间发送者
        /// </summary>
        ReactiveProperty<DateTime> NowTimeSender { get; set; }

    }
}
