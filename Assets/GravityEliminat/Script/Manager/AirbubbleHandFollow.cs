using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirbubbleHandFollow : MonoBehaviour
{
    public Transform mFollowObj { get; private set; }

    private Transform mTrans;

    private Vector3 mTransPos;

    private Vector3 mFollowPos;

    private Vector3 mScalue_defalut = Vector3.one;
    private Vector3 mScale_2 = new Vector3(0.7f, 0.7f, 0.7f);
    private float mScaleAniTime = 1;

    private void Start()
    {
        mTrans = this.transform;
        ScaleAni();
    }

    public void Update()
    {
        if (mFollowObj==null)
        {
            return;
        }
        mTransPos = mTrans.position;
        mFollowPos = mFollowObj.position;
        mTrans.position = Vector3.Lerp(mTransPos, mFollowPos, 0.5f);
    }


    //缩放动画
    private void ScaleAni()
    {
        Sequence mseq = DOTween.Sequence();
        mseq.Append(this.transform.DOScale(mScale_2, mScaleAniTime));
        mseq.Append(this.transform.DOScale(mScalue_defalut, mScaleAniTime));        
        mseq.SetLoops(-1);
    }

    public void SetFollowObj(Transform mobj)
    {
        Debug.Log("设置引导手指跟随对象:" + mobj);
        mFollowObj = mobj;
    }


    public void Show()
    {
        Debug.Log("设置引导手显示");
        //this.transform.ShowCanvasGroup();
    }

    public void Hide()
    {
        Debug.Log("设置引导手隐藏");
        //this.transform.HideCanvasGroup();
    }
}
