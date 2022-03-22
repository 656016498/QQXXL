using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class QIpaoHb : MonoBehaviour
{

    public int hbIndex;
    public Button mBtn;
    public Action CallBack;
    
    void Start()
    {
        mBtn.onClick.AddListener(() => {
            //transform.HideCanvasGroup();
        
            var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
            UmengDisMgr.Instance.CountOnNumber("bubble_hb_show");
            popup1.OnOpen("bubble_hb_video", 0,()=> {

                gameObject.SetActive(false);
                var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
                var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                {
                    //打点
                    UmengDisMgr.Instance.CountOnNumber("bubble_hb_get");
                    if (!GameManager.Instance.isCash)
                    {
                        RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                    }
                    popup2.effect.SetActive(false);
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(2f)).Subscribe(value =>
                    {
                        ShowAni();
                    });
                });
                popup1.defult.SetActive(false);
            },()=> {
               
            }
            );

            //AdControl.Instance.ShowRwAd("bubble_hb_video", () => {
            //    //弹出小额一级红包界面
            //    Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ =>
            //    {
            //        #region
            //        gameObject.SetActive(false);
                   
            //        //打开回调
            //        var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
            //        popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
            //        {
            //            //打点
            //            UmengDisMgr.Instance.CountOnNumber("bubble_hb_show");
            //            UmengDisMgr.Instance.CountOnNumber("bubble_hb_get");
            //            if (!GameManager.Instance.isCash)
            //            {
            //                RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
            //            }
            //            popup2.effect.SetActive(false);
            //            Observable.TimeInterval(System.TimeSpan.FromSeconds(2f)).Subscribe(value => 
            //            {
            //                ShowAni();
            //            });
            //        });
            //        #endregion
            //    });
            //});    

        });
    }

    //展示动画
    private void ShowAni()
    {

        gameObject.SetActive(true);
        transform.GetComponent<Animator>().Play("QiPao");
        //QIpaoHbControl.Instance.InitHb(hbIndex);

    }
}
