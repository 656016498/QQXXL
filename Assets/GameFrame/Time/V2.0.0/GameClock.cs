using System;
using UniRx;

namespace GameTime
{
    public class GameClock
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="timeType"></param>
        public static void Init(GameUseTime timeType)
        {
            mGameClock = new MGameClock(timeType);
            mGameClock.ServerLoadTimeState.Subscribe(value => ServerLoadTimeState.Value = value);
            mGameClock.NowTimeSender.Subscribe(value => NowTimeListening.Value = value);
        }

        /// <summary>
        /// 本次进入游戏时间
        /// </summary>
        public static DateTime EnterGameTime
        {
            get
            {
                return MGameClock.EnterGameTime;
            }
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        public static DateTime NowTime
        {
            get
            {
                return MGameClock.NowTime;
            }
        }

        /// <summary>
        /// 服务器加载状态
        /// </summary>
        public static ReactiveProperty<GameClockServerLoadState> ServerLoadTimeState = new ReactiveProperty<GameClockServerLoadState>(GameClockServerLoadState.None);

        /// <summary>
        /// 当前时间监听
        /// </summary>
        public static ReactiveProperty<DateTime> NowTimeListening = new ReactiveProperty<DateTime>();


        #region 

        private static MGameClock mGameClock;
        private static MGameClock MGameClock
        {
            get
            {
                if (mGameClock == null)
                {
                    mGameClock = new MGameClock();
                }
                return mGameClock;
            }
        }

        #endregion
    }
}