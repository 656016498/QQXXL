using UniRx;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
/// <summary>
/// 抽奖扩展
/// </summary>
public static class LotteryExtension
{
    /// <summary>
    /// 抽奖动画扩展
    /// </summary>
    public static class LotteryAni
    {
        public enum Type
        {
            /// <summary>
            /// 顺序
            /// </summary>
            Random,
            /// <summary>
            /// 随机
            /// </summary>
            Order,
        }
        /// <summary>
        /// 正在轮播的对象
        /// </summary>
        public static ReactiveProperty<Transform> mLunboingTrans = new ReactiveProperty<Transform>();

        /// <summary>
        /// 播放随机跳动动画
        /// </summary>
        /// <param name="mlist"></param>
        /// <param name="targetTrans"></param>
        /// <param name="lunboTimes"></param>
        /// <param name="durTime"></param>
        /// <param name="OnComplete"></param>
        /// <returns></returns>
        public static ReactiveProperty<Transform> PlayRandomSkip(List<Transform> mlist, Transform targetTrans, int lunboTimes = 10, float durTime = 3, System.Action OnComplete = null)
        {
            var lunboTimeDis = durTime / (lunboTimes * 1.0f);//轮播时间间隔 
            int index = -1;
            int lastIndex = -1;
            for (int i = 0; i < lunboTimes; i++)
            {
                var delayTime = lunboTimeDis * i;
                index = Random.Range(0, mlist.Count);
                if(index==lastIndex)
                {
                    if(index==mlist.Count-1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index += 1;
                    }
                }
                lastIndex = index;
                Transform changingTrans = mlist[index];
                Observable.TimeInterval(System.TimeSpan.FromSeconds(delayTime)).Subscribe(_ => mLunboingTrans.Value = changingTrans);
            }
            var delayTime2 = lunboTimeDis * lunboTimes;
            Observable.TimeInterval(System.TimeSpan.FromSeconds(delayTime2)).Subscribe(_ => 
            {
                mLunboingTrans.Value = targetTrans;
                if (OnComplete != null) OnComplete();
            });
            return mLunboingTrans;
        }

        /// <summary>
        /// 播放随机跳动动画
        /// </summary>
        /// <param name="transs"></param>
        /// <param name="targetTrans"></param>
        /// <param name="lunboTimes"></param>
        /// <param name="durTime"></param>
        /// <param name="OnComplete"></param>
        /// <returns></returns>
        public static ReactiveProperty<Transform> PlayRandomSkip(Transform[] transs, Transform targetTrans, int lunboTimes = 10, float durTime = 3, System.Action OnComplete = null)
        {
            var lunboTimeDis = durTime / (lunboTimes * 1.0f);//轮播时间间隔 
            int index = -1;
            int lastIndex = -1;
            for (int i = 0; i < lunboTimes; i++)
            {
                var delayTime = lunboTimeDis * i;
                index = Random.Range(0, transs.Length);
                if (index == lastIndex)
                {
                    if (index == transs.Length - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index += 1;
                    }
                }
                lastIndex = index;
                Transform changingTrans = transs[index];
                Observable.TimeInterval(System.TimeSpan.FromSeconds(delayTime)).Subscribe(_ => mLunboingTrans.Value = changingTrans);
            }
            var delayTime2 = lunboTimeDis * lunboTimes;
            Observable.TimeInterval(System.TimeSpan.FromSeconds(delayTime2)).Subscribe(_ =>
            {
                mLunboingTrans.Value = targetTrans;
                if (OnComplete != null) OnComplete();
            });
            return mLunboingTrans;
        }

        /// <summary>
        /// 播放顺序循环动画
        /// </summary>
        /// <param name="mlist"></param>
        /// <param name="targetTrans"></param>
        /// <param name="Loops"></param>
        /// <param name="durtime"></param>
        /// <param name="OnComplete"></param>
        /// <returns></returns>
        public static void PlayOrderSkipAni(List<Transform> mlist, Transform targetTrans, int Loops = 5, float durtime = 2, System.Action OnComplete = null)
        {          
            var lunboTimeDis = durtime / (Loops * mlist.Count).IntToFloat();
            int lunboIndex = 0;
            for (int i = 0; i < Loops; i++)
            {
                for (int j = 0; j < mlist.Count; j++)
                {
                    Transform mtrans = mlist[j];
                    lunboIndex += 1;
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(lunboTimeDis * lunboIndex))
                        .Subscribe(_ =>
                        {
                            mLunboingTrans.Value = mtrans;
                        });
                }
            }

            var hadDelayTime = lunboTimeDis * lunboIndex;            
            //补充到目标
            for (int i = 0; i < mlist.Count; i++)
            {
                Transform mtrans = mlist[i];
                lunboIndex += 1;

                var delayTime = lunboTimeDis * lunboIndex;
                Observable.TimeInterval(System.TimeSpan.FromSeconds(delayTime))
                    .Subscribe(_ =>
                    {
                        mLunboingTrans.Value = mtrans;
                    });
                if(mtrans==targetTrans)
                {
                    break;
                }
            }
            Observable.TimeInterval(System.TimeSpan.FromSeconds(lunboTimeDis * lunboIndex))
                .Subscribe(_ =>
                {
                    if (OnComplete != null)
                    {
                        OnComplete();
                    }

                });           
        }


        public static void PlayOrderSkipAni(List<Transform> mlist, int targetIndex, Ease type, int Loops = 5, float durtime = 2, System.Action OnComplete = null)
        {
            var lunboTimeDis = durtime / (float)(Loops * mlist.Count + targetIndex);
            int lunboIndex = 0;
            float t = 0;
            float lastT = 0;
            float currt = 0;
            DOTween.To(() => t, x => t = x, durtime + 0.01f, durtime).SetEase(type).OnUpdate(() => {
                currt += t - lastT;
                if (currt >= lunboTimeDis)
                {
                    currt = currt - lunboTimeDis;

                    mLunboingTrans.Value = mlist[lunboIndex];
                    lunboIndex++;
                    if (lunboIndex >= mlist.Count)
                    {
                        lunboIndex = 0;
                    }
                }
                lastT = t;
            }).OnComplete(() => {
                OnComplete();
            });

          
        }
    }
}

