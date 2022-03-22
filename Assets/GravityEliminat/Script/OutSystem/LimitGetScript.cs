using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class LimitGetScript : MonoBehaviour
{
    public Text lTF;

    public Button limitBt;

    //public GameObject getImg;
    //public GameObject daoImg;
    //public GameObject part;

    private int backTime;



    // Start is called before the first frame update
    void Start()
    {

        //limitBt.onClick.AddListener(LimitClick);
        //InitView();
        //backTime = 0;
        limitBt.onClick.AddListener(() => {

            if (OnLineCutTimeControl.Instance.canGet.Value)
            {
                //在线红包icon点击次数打点
                UmengDisMgr.Instance.CountOnNumber("online_icon");
                ////可领取
                ///



                var pop1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
                pop1.OnOpen("online_hb_video", () => {
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(0)).Subscribe(_ => {
                        UmengDisMgr.Instance.CountOnNumber("online_hb_get");
                        var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
                        var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                        //打开回调
                        var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                        popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                        {
                            if (!GameManager.Instance.isCash)
                            {
                                RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                            }
                            popup2.effect.SetActive(false);
                            OnLineCutTimeControl.Instance.ToBackTime();
                        });
                        pop1.defult.SetActive(false);

                    });
                   
                }, () => { });
                //AdControl.Instance.ShowRwAd("online_hb_video", () => {
                    // Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ =>
                    // {
                    //  //在线红包领取次数打点

                    // });
                //});
            }
            else
            {
               ShowPublicTip.Instance.Show("请在倒计时结束后领取");
            }
        });

        OnLineCutTimeControl.Instance.cutTime.Subscribe(value => {

            lTF.text = string.Format("{0}", value);
        });
        OnLineCutTimeControl.Instance.canGet.Subscribe(value => {
            if (value)
            {
                lTF.transform.HideCanvasGroup();
                gameObject.GetComponent<Animator>().enabled = true;
            }
            else
            {
                lTF.transform.ShowCanvasGroup();
                transform.localScale = Vector3.one;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                gameObject.GetComponent<Animator>().enabled = false;
               
            }

        });
    }

    void InitView()
    {
        lTF.transform.ShowCanvasGroup();
        backTime = 60;
        lTF.text = "0" + backTime / 60 + ":" + String.Format("{0:D2}", (backTime % 60));
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        gameObject.GetComponent<Animator>().enabled = false;
        if (!IsInvoking("ToBackTime"))
        {
            InvokeRepeating("ToBackTime", 0, 1);
        }
    }


    void ToBackTime()
    {
        backTime--;

        if (backTime <= 0)
        {

            lTF.transform.HideCanvasGroup();
            CancelInvoke("ToBackTime");
            gameObject.GetComponent<Animator>().enabled = true;
            return;
        }
        lTF.text = "0" + backTime / 60 + ":" + String.Format("{0:D2}", (backTime % 60));
    }


    /// <summary>
    /// 点击逻辑
    /// </summary>
    void LimitClick()
    {
       
        if (IsInvoking("ToBackTime"))
        {
          ShowPublicTip .Instance.Show("请在倒计时结束后领取");
        }
        else
        {
#if UNITY_EDITOR

            #region
            //var mpanel = UIManager.Instance.ShowPopUp<LargeCashPanel>();
            //var mConfig = LargeCashDataControl.Instance.largeCashConfig;
            //mpanel.OnOpen("", mConfig.FM,
            //() => {
            //    LargeCashDataControl.Instance.AddtoTotal(mConfig.AD);
            //    //在线红包领取次数打点
            //    UmengDisMgr.Instance.CountOnNumber("online_hb_get");
            //    var mControl = LargeCashDataControl.Instance;
            //    var mPanel2 = UIManager.Instance.ShowPopUp<LargeCashTwoPanel>();
            //    mPanel2.RefrishUi(mConfig.AD, mControl.mData.totalNum, mControl.LastMoney);
            //    mPanel2.OnOpen(LargeHbType.Nomal,
            //        () => {
            //            InitView();
            //        },
            //        () => {
            //            ShowPublicTip.Instance.Show("红包满200元才可以提现哦");
            //        },
            //        () => {
            //            InitView();
            //        });
            //},
            //() => {

            //}
            //);
            #endregion
           
            var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
            popup1.OnOpen("online_hb_video", () => {
                //打开回调
                var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
                var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                {
                    popup2.effect.SetActive(false);
                    InitView();
                });
                popup1.defult.SetActive(false);
                },
            () =>
                {
                //关闭回调
                Debug.Log("关闭红包一级界面");
                popup1.defult.SetActive(false);
                InitView();
            });
#else

            #region
            //var mpanel = UIManager.Instance.ShowPopUp<LargeCashPanel>();
            //var mConfig = LargeCashDataControl.Instance.largeCashConfig;
            //mpanel.OnOpen("online_hb_video", mConfig.FM,
            //() => {
            //    LargeCashDataControl.Instance.AddtoTotal(mConfig.AD);
            //    //在线红包领取次数打点
            //    UmengDisMgr.Instance.CountOnNumber("online_hb_get");
            //    var mControl = LargeCashDataControl.Instance;
            //    var mPanel2 = UIManager.Instance.ShowPopUp<LargeCashTwoPanel>();
            //    mPanel2.RefrishUi(mConfig.AD, mControl.mData.totalNum, mControl.LastMoney);
            //    mPanel2.OnOpen(LargeHbType.Nomal,
            //        () => {
            //          InitView();
            //        },
            //        () => {
            //            ShowPublicTip.Instance.Show("红包满200元才可以提现哦");
            //        },
            //        () => {
            //          InitView();
            //        });
            //},
            //() => {

            //}
            //);
            #endregion
            var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
            var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
            var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
            popup1.OnOpen("online_hb_video", rewardRedIcon,() => {
                //打开回调
                var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                {
                    popup2.effect.SetActive(false);
                    InitView();
                });
                popup1.defult.SetActive(false);
                },
            () =>
                {
                //关闭回调
                Debug.Log("关闭红包一级界面");
                popup1.defult.SetActive(false);
                InitView();
            });
#endif
            ////在线红包icon点击次数打点
            //UmengDisMgr.Instance.CountOnNumber("online_icon");
        }
    }

}


public class OnLineCutTimeControl
{
    private static OnLineCutTimeControl instance;
    public static  OnLineCutTimeControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new OnLineCutTimeControl();
            }
            return instance;
        }
    }
    public int backTime = 60;
    public ReactiveProperty<string> cutTime = new ReactiveProperty<string>();
    public ReactiveProperty<bool> canGet = new ReactiveProperty<bool>(true);
    public IDisposable mdispose;
    //开始倒计时
    public  void ToBackTime()
    {
        backTime = 60;
        cutTime.Value = backTime.Second_TransFrom_Math();
        canGet.Value = false;
        mdispose= Observable.Interval(TimeSpan.FromSeconds(1))
         .Subscribe(_ => {
             if (backTime > 0)
             {
                 backTime--;
                 cutTime.Value = backTime.Second_TransFrom_Math();

             }
             if (backTime <= 0)
             {
                 //可领取奖励
                 canGet.Value = true;
                 mdispose.Dispose();
             }
         });

    }

}