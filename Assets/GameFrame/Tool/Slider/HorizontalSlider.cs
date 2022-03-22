using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;

public class HorizontalSlider : MonoBehaviour
{
    //导入所有的组件
    //计算中央位置的显示区域,入场位置,出场位置
    [Header("停留位置")] [SerializeField] private Transform ShowPos;
    [SerializeField]private RectTransform[] mRecttrans;
    [SerializeField] private RectTransform Content;
    [SerializeField] private ScrollRect mScrollRect;
    [SerializeField] private Transform[] mItems;
    private Vector2 mCenterPos;
    private List<RectTransform> mList_allItem;
    private RectTransform mShowingRect;
    private Color mHideColor = new Color(0, 0, 0, 0);
    private bool IsAutoCheck = true;
    private bool IsMouseUp = false;
    private Vector3 mHideScale = new Vector3(0.75f, 0.75f, 0.75f);
    private Dictionary<RectTransform, Transform> mdic = new Dictionary<RectTransform, Transform>();

    private void Awake()
    {
        mList_allItem=new List<RectTransform>(mRecttrans.Length);
        for (int i = 0; i < mRecttrans.Length; i++)
        {
            mList_allItem.Add(mRecttrans[i]);
        }
        InitDic();  

        AddItemClick();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //AutoMove();
            IsMouseUp = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            IsMouseUp = false;
        }
        ChcckShowingItem();
        AutoFollow();
        if (IsMouseUp)
        {
            if (Mathf.Abs(mScrollRect.velocity.x) <100)
            {
                IsMouseUp = false;
                IsAutoCheck = true;
                AutoMove();                
            }
        }
    }

    private void ChcckShowingItem()
    {
        if (!IsAutoCheck) return;
        mCenterPos = (Vector2)ShowPos.position;
        for (int j = 0; j < mList_allItem.Count - 1; j++)
        {
            for (int i = 0; i < mList_allItem.Count - 1 - j; i++)
            {
                var a = mList_allItem[i];
                var b = mList_allItem[i + 1];
                var disA = Vector2.Distance(a.position, mCenterPos);
                var disB = Vector2.Distance(b.position, mCenterPos);
                if (disA > disB)
                {
                    mList_allItem[i] = b;
                    mList_allItem[i + 1] = a;
                }
            }
        }
        mShowingRect = mList_allItem[0];
    }

    private void AutoMove()
    {
        int index = 0;
        for (int i = 0; i < mRecttrans.Length; i++)
        {
            if (mShowingRect == mRecttrans[i])
            {
                index = i;
                break;
            }
        }
        var value = -mShowingRect.sizeDelta.x * index;
        mScrollRect.inertia = false;
        var target = new Vector2(value, Content.anchoredPosition.y);
        Content.DOAnchorPos(target, 0.3f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                mScrollRect.inertia = true;
                NowShowItem.Run(mdic[mShowingRect]);//实际上是传值给跟随的那个对象
            })
            ;
    }

    private void InitDic()
    {
        for (int i = 0; i < mRecttrans.Length; i++)
        {
            mdic.Add(mRecttrans[i], mItems[i]);
        }
    }

    private void AutoFollow()
    {
        foreach (var item in mdic)
        {
            item.Value.position = item.Key.position;
            if (item.Key == mShowingRect)
            {
                item.Value.transform.SetAsLastSibling();
                item.Value.transform.DOScale(Vector3.one, 0.2f);
                item.Value.transform.GetChild(4).transform.HideCanvasGroup();
            }
            else
            {                
                if (item.Value.transform.localScale.x>0)
                {
                    item.Value.transform.SetSiblingIndex(mList_allItem.Count - 2);
                    item.Value.transform.DOScale(mHideScale,0.2f);
                    item.Value.transform.GetChild(4).transform.ShowCanvasGroup();
                }
                else
                {
                    item.Value.transform.SetAsFirstSibling();
                }
            }
        }

    }

    private void AddItemClick()
    {
        for (int i = 0; i < mRecttrans.Length; i++)
        {
            var item= mList_allItem[i];
            if (item.GetImage().enabled==false)
            {
                item.GetImage().enabled = true;
                item.GetImage().color = mHideColor;
            }
            var mbtn = item.GetButton();
            mbtn.onClick.AddListener(() =>
            {
                IsAutoCheck = false;
                mShowingRect = item.GetComponent<RectTransform>();                
            });

        }
    }

    /// <summary>
    /// 当前正在显示的对象
    /// </summary>
    public System.Action<Transform> NowShowItem { get; set; }

    public void SetShowDays(int value)
    {
        IsMouseUp = true;
        var miten = mRecttrans[value - 1];
        miten.GetButton().onClick.Invoke();
    }

}
