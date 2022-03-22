using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using System;

/// <summary>
/// 公共提示功能
/// </summary>
/// 实现功能:
/// 对象池
/// 连续点击,连续生成
/// 带动画效果
public class ShowPublicTip
{
    private static ShowPublicTip mInstace;
    public static ShowPublicTip Instance
    {
        get
        {
            if(mInstace==null)
            {
                mInstace = new ShowPublicTip();
            }
            return mInstace;
        }
    }    
    private Transform mParent;

    private Canvas mCanvas;
    private Canvas mGetCanvs
    {
        get
        {
            if(mCanvas==null)
            {
                //mCanvas = GameObject.FindObjectOfType<MainCanvas>().transform.Find("Canvas").GetComponent<Canvas>();
                mCanvas = GameObject.Find("UIRoot").GetComponent<Canvas>();
                
            }
            if(mCanvas!=null&&mParent==null)
            {
                mParent = MonoBehaviour.Instantiate(Resources.Load<Transform>("Prefabs/UI/TipShowPanel"), mCanvas.transform.GetChild(3));          
            }
            return mCanvas;
        }
    }

    private List<GameObject> mTipList = new List<GameObject>();


    public void Show(string info)
    {
        if (mGetCanvs == null) return;

        mParent.SetAsLastSibling();

        var mTip = GetTip(Vector2.zero);
        mTip.transform.Find("Text").GetText().text = info;
        mTip.ShowCanvasGroup();              
    }

    private GameObject GetTip(Vector2 starpos, float targetY = 100, float autoRecycleTime = 1)
    {
        if (mTipList.Count == 0)
        {
            var obj = CreatTip();
            mTipList.Add(obj);
        }
        var mTip = mTipList[0];
        mTipList.Remove(mTip);


        mTip.transform.localPosition = starpos;
        mTip.transform.DOLocalMoveY(targetY, 0.75f)
            .OnComplete(() =>
            {
                mTip.transform.GetComponent<CanvasGroup>().DOFade(0, 0.25f);
            });
        Observable.TimeInterval(System.TimeSpan.FromSeconds(autoRecycleTime))
            .Subscribe(_ =>
            {
                mTipList.Add(mTip);
                mTip.HideCanvasGroup();
            });
        return mTip;
    }

    
    private GameObject CreatTip()
    {
        var obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/TipText"), mParent, false);     
        obj.transform.localPosition = Vector3.zero;       
        obj.HideCanvasGroup();

        var mText = obj.transform.Find("Text").GetComponent<Text>();
        //mText.color = Color.white;
        mText.fontSize = 30;
        return obj;
    }



    public enum ShowType
    {
        Center_Slider,
        Center_Static,
        Top_Static,
        Down_Static
    }



    
    public void Show(string dir, ShowType mShowType)
    {
        switch (mShowType)
        {
            case ShowType.Center_Slider:
                break;
            case ShowType.Center_Static:
                break;
            case ShowType.Top_Static:
                break;
            case ShowType.Down_Static:
                break;
            default:
                break;
        }
    }

    private List<Transform> mListTip = new List<Transform>(10);
    private Transform GetTip()
    {
        if (mListTip.Count == 0)
        {
            var mTip = CreatTip();
            mListTip.Add(mTip.transform);
        }
        var mTrans = mListTip[0];
        mListTip.RemoveAt(0);
        return mTrans;
    }

}
