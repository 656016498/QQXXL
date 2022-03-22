using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

/// <summary>
/// 滚动信息
/// </summary>
public class RollInfo : MonoBehaviour
{
    public Color ShowColor = Color.black;
    public Color DefaultColor = new Color(114 / (float)255, 114 / (float)255, 114 / (float)255);

    public Vector3 TopScale = Vector3.one;
    public Vector3 CenterScale = Vector3.one;
    public Vector3 BottonScale = Vector3.one;

    public float LoopTimeDistance = 1.5f;
    public float MoveTime = 0.5f;
    public IDisposable mdispos;

    private string mInsetStr = "*";

    private void Start()
    {

        
    }
    private void OnEnable()
    {
        InitPaoMa();
    }

    public void InitPaoMa()
    {
        if (mdispos != null)
        {
            mdispos.Dispose();
        }
        //设置名字
        Debug.Log("刚进入跑马灯————");
        for (int i = 0; i < this.transform.childCount; i++)
        {
            SetName(this.transform.GetChild(i).GetChild(1).GetComponent<Text>());
        }

        //设置初始状态
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var index = i;
            var mtrans = this.transform.GetChild(index);
            var mtransText = this.transform.GetChild(index).GetChild(1);
            var mclor = index == this.transform.childCount - 2 ? ShowColor : DefaultColor;
            var mscale = index == this.transform.childCount - 2 ? CenterScale : TopScale;
            mtransText.GetComponent<Text>().color = mclor;
            mtrans.DOScale(mscale, 0);
        }

        //循环滚动
        mdispos= Observable.Interval(System.TimeSpan.FromSeconds(LoopTimeDistance))
            .Subscribe(_ =>
            {
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    var index = i;
                    var mtrans = this.transform.GetChild(index);
                    var mtransText = this.transform.GetChild(index).GetChild(1);
                    if (index == 0)
                    {
                        var targetpos = this.transform.GetChild(this.transform.childCount - 1).position;
                        mtrans.HideCanvasGroup();

                        SetName(mtransText.GetComponent<Text>());
                        mtrans.DOMove(targetpos, MoveTime).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            mtrans.SetAsLastSibling();
                            mtrans.ShowCanvasGroup();
                        });
                    }
                    else
                    {
                        var targetpos = this.transform.GetChild(index - 1).position;
                        var mcolor = index == this.transform.childCount - 1 ? ShowColor : DefaultColor;
                        mtransText.GetComponent<Text>().color = mcolor;
                        var mScale = index == this.transform.childCount - 1 ? CenterScale : TopScale;
                        mtrans.DOScale(mScale, MoveTime);
                        mtrans.DOMove(targetpos, MoveTime).SetEase(Ease.Linear);
                    };
                }
            }).AddTo(this);
    }

    private void SetName(Text text)
    {
       
        text.text = string.Format("{0}玩游戏<color=#ff6735>{1}天</color>已提现<color=#ff6735>{2}元</color>", WithdrawSucManger.Instance.RandomName(), RandomDay(), GetJine());
    }


    private float[] mJinE = new float[]
    {
       200
    };
    /// <summary>
    /// 获得金额
    /// </summary>
    /// <returns></returns>
    private string GetJine()
    {
        //var value = Random.Range(0, mJinE.Length);
        var value = UnityEngine.Random.Range(0, 100);
        if(value<60)
        {
            value = 0;
        }
        else
        {
            value = 0;
        }
        return mJinE[value].ToString();
    }

    private int  RandomDay()
    {
        return
             UnityEngine.Random.Range(10, 20);
    }
}
