using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class AirBubbleControl : Singlton<AirBubbleControl>
{
    private float left = -300;
    private float right = 300;
    private float up=350;
    private float down = -350;



    public void Init()
    {
        RestartTime();
    }


    private List<GameObject> mlist_usingAirbubble = new List<GameObject>(10);
    private List<System.IDisposable> mList_UsingUnrix = new List<System.IDisposable>(10);
    /// <summary>
    /// 显示气泡
    /// </summary>
    /// <param name="numbers">生成数量</param>
    /// <param name="showTimeDis">显示间隔</param>
    /// <param name="durtionTime">持续时间</param>
    public void ShowAirBubble(int numbers,Transform parent)
    {
        if (!IsCanShowAirbubble)
        {
            return;
        }
        //XDebug.Log("气泡红包:显示红包:" + numbers);
        //判断当前已经有多少个了
        if (mlist_usingAirbubble.Count>=1)
        {
            if (GetAirbubbleHandFollow!=null&&GetAirbubbleHandFollow.mFollowObj==null)
            {
                var followObj = mlist_usingAirbubble[Random.Range(0, mlist_usingAirbubble.Count)];
                GetAirbubbleHandFollow.SetFollowObj(followObj.transform);
                GetAirbubbleHandFollow.Show();
                Observable.TimeInterval(System.TimeSpan.FromSeconds(10))
                    .Subscribe(_ =>
                    {
                        GetAirbubbleHandFollow.SetFollowObj(null);
                        GetAirbubbleHandFollow.Hide();
                    });
            }
            return;
        }

        ////判断当前的引导步数
        //if (GameDataMgr.Simple.Data.GuideNowStep<15)
        //{
        //    return;
        //}

        for (int i = 0; i < numbers; i++)
        {
            var mairbubble = GetAirBubble(parent);
            var mrect = mairbubble.GetComponent<RectTransform>();
            //随机启动位置
            mrect.anchoredPosition = new Vector2(500+(i*200), -300);
            SetRandomTargetPos(mairbubble.transform);
            mlist_usingAirbubble.Add(mairbubble);           
        }

        //随机选择一个气泡进行跟随
        if (GetAirbubbleHandFollow!=null)
        {
            GetAirbubbleHandFollow.transform.SetParent(parent);
            var followObj = mlist_usingAirbubble[Random.Range(0, mlist_usingAirbubble.Count)];
            GetAirbubbleHandFollow.SetFollowObj(followObj.transform);
            GetAirbubbleHandFollow.Show();
            Observable.TimeInterval(System.TimeSpan.FromSeconds(10))
                .Subscribe(_ =>
                {
                    GetAirbubbleHandFollow.SetFollowObj(null);
                    GetAirbubbleHandFollow.Hide();
                });
        }             
    }



    /// <summary>
    /// 回收所有的气泡
    /// </summary>
    public void RecycleAllAirbubble()
    {
        //XDebug.Log("气泡红包:回收所有的气泡红包");
        foreach (var item in mlist_usingAirbubble)
        {
            RecycleAirBubble(item);
        }
        mlist_usingAirbubble.Clear();

        foreach (var item in mList_UsingUnrix)
        {
            item.Dispose();
        }
        mList_UsingUnrix.Clear();

        if (GetAirbubbleHandFollow!=null)
        {
            GetAirbubbleHandFollow.SetFollowObj(null);
            GetAirbubbleHandFollow.Hide();
        }     
    }

    private float moveSpeed = 100;
    private void SetRandomTargetPos(Transform mtrans)
    {
        var mrect = mtrans.GetComponent<RectTransform>();
        float value_x;
        if (mrect.anchoredPosition.x>0)
        {
            value_x = left;
        }
        else
        {
            value_x = right;
        }
        var randomTarget = new Vector2(value_x, Random.Range(down, up));
        var moveTime = Vector3.Distance(randomTarget, mrect.anchoredPosition) / moveSpeed;
        mrect.DOAnchorPos(randomTarget, moveTime).SetEase(Ease.Linear);
        if (!mtrans.gameObject.activeSelf)
        {
            return;
        }
        var mObser = Observable.TimeInterval(System.TimeSpan.FromSeconds(moveTime))
             .Subscribe(_ =>
             {
                 SetRandomTargetPos(mtrans);
             });
        mList_UsingUnrix.Add(mObser);
    }

    private List<GameObject> mList_Airbubble = new List<GameObject>(5);
    private GameObject mAirbubblePrefab;
    private Transform mDynamicCanvas;

    private GameObject GetAirBubble(Transform parent=null)
    {
        if (mList_Airbubble.Count==0)
        {
            if (mAirbubblePrefab==null)
            {
                mAirbubblePrefab = Resources.Load<GameObject>("Airbubble/Airbubble");
            }
            if (mDynamicCanvas == null)
            {
                //mDynamicCanvas = GameObject.FindObjectOfType<MainCanvas>().transform.Find("AirbubbleCanvas");
            }
            if (parent==null)
            {
                parent = mDynamicCanvas;
            }
            var newAir = Object.Instantiate(mAirbubblePrefab, parent);
            newAir.transform.localScale = Vector3.one;
            mList_Airbubble.Add(newAir);
        }

        var mair = mList_Airbubble[0];
        mair.SetActive(true);
        SetAirClick(mair.transform);
        mList_Airbubble.RemoveAt(0);       
        return mair;
    }



    public void RecycleAirBubble(GameObject mair)
    {
        //XDebug.Log("气泡红包:回收气泡红包");
        mList_Airbubble.Add(mair);
        mair.SetActive(false);


        if (GetAirbubbleHandFollow != null)
        {
            GetAirbubbleHandFollow.SetFollowObj(null);
            GetAirbubbleHandFollow.Hide();
        }
           
    }


    private void SetAirClick(Transform mari)
    {
        var mbtn= mari.GetComponent<Button>(); ;
        mbtn.onClick.RemoveAllListeners();      
        mbtn.onClick.AddListener(() =>
        {
            RecycleAirBubble(mari.gameObject);           
            //var value = MainGame.Instance.GetGameRedRmb(MainGame.Instance.HaveRmb.Value);
            //RedpanelV2.Data data = new RedpanelV2.Data(RedpanelV2.PanelShowType.One, RedpanelV2.TwoPanelBtnShowType.BtnInfo2, value, (RedpanelV2 mpanel, RedpanelV2.PanelClickBtn clickBtn) =>
            //    {
            //        switch (clickBtn)
            //        {
            //            case RedpanelV2.PanelClickBtn.One_CloseBtn:
            //                UIPanelManager.Instance.PopPanel();
            //                break;
            //            case RedpanelV2.PanelClickBtn.One_OpenBtn:
            //                GameAdControl.Instance.ShowRwAd("bubble_hb_video", () => 
            //                {
            //                    mpanel.OnePanelIsShow(false);
            //                    mpanel.TwoPanelIsShow(true);
            //                    WithdrawSignDataMgr.Instance.UpdateData(WithdrawSignDataMgr.Type.AirbubbleRed);
            //                    RestartTime();
            //                });
            //                break;
            //            case RedpanelV2.PanelClickBtn.Two_CloseBtn:
            //                MainGame.Instance.UpdateGameRmb(value);
            //                UIPanelManager.Instance.PopPanel();
            //                break;
            //            case RedpanelV2.PanelClickBtn.Two_OpenAginBtn:
            //                break;
            //            case RedpanelV2.PanelClickBtn.Two_DirectGetBtn:
            //                MainGame.Instance.UpdateGameRmb(value);
            //                UIPanelManager.Instance.PopPanel();                           
            //                break;
            //            case RedpanelV2.PanelClickBtn.DoubleGetBtn:
            //                GameAdControl.Instance.ShowRwAd("bubble_hb_double_video", () =>
            //                {
            //                    mpanel.TwoPanelIsShow(true, RedpanelV2.TwoPanelBtnShowType.BtnInfo0);
            //                    mpanel.RefreshUI(value * 2, MainGame.Instance.HaveRmb.Value);
            //                    MainGame.Instance.UpdateGameRmb(value * 2);

            //                    UmengDisMgr.Instance.CountOnNumber("qp_hb_multiple");
            //                });
            //                break;
            //            case RedpanelV2.PanelClickBtn.Two_GetBtn:
            //                MainGame.Instance.UpdateGameRmb(value);
            //                UIPanelManager.Instance.PopPanel();
            //                break;
            //            default:
            //                break;
            //        }
            //    });

            //TODO 展示红包
            //UIPanelManager.Instance.PushPanel(PanelType.RedPanel,false, data);

            RecycleAllAirbubble();

            //UmengDisMgr.Instance.CountOnNumber("qp_hb_icon");
        });
    }

    //是否可以显示气泡红包
    private bool IsCanShowAirbubble = false;

    //重新计时
    private void RestartTime()
    {
        IsCanShowAirbubble = false;
        //XDebug.Log("气泡红包:是否可以显示:" + IsCanShowAirbubble);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(Random.Range(13, 18)))
            .Subscribe(_ => 
            {
                IsCanShowAirbubble = true;
                  //XDebug.Log("气泡红包:是否可以显示:" + IsCanShowAirbubble);
            });
    }

    public bool IsHaveRedShowing
    {
        get
        {
            return mlist_usingAirbubble.Count > 0;
        }
    }



    #region 获取一个手指跟随气泡红包
    private AirbubbleHandFollow mAirbubbleHandFollow;

    private AirbubbleHandFollow GetAirbubbleHandFollow
    {
        get
        {
            if (mAirbubbleHandFollow==null&&mDynamicCanvas!=null)
            {
                var mObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Airbubble/AirbubbeHandGuide"), mDynamicCanvas, false);
                mAirbubbleHandFollow = mObj.AddComponent<AirbubbleHandFollow>();
            }
            return mAirbubbleHandFollow;
        }
    }

    #endregion
}
