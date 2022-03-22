using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LargeCashPanel : UIBase
{
    public Text awardNum;
    public Image icon;
    public IButton openBtn;
    public IButton closeBtn;
    public Action OpenCallBack;
    public Action closeCallBack;
    string id;
    float maxNum;
    void Start()
    {
        openBtn.onClick.AddListener(() => {

            if (id == null || id == "")
            {
                OpenCallBack.Run();
                Hide();
            }
            else
            {
                AdControl.Instance.ShowRwAd(id, () => {
                    OpenCallBack.Run();
                    Hide();
                });
            }
            
        });
        closeBtn.onClick.AddListener(() => {
            closeCallBack.Run();
            Hide();
        });
    }

    public override void Show()
    {
        RefrishUi();
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    } 

    public void OnOpen(string mId,float mMaxNum,Action mOpenCallBck = null,Action mCloseCallBack = null)  
    {
        id = mId; 
        OpenCallBack = mOpenCallBck;
        closeCallBack = mCloseCallBack;
        maxNum = mMaxNum;

        Show();
    }

    //刷新UI
    void RefrishUi()
    {
        awardNum.text = string.Format("{0}元", maxNum);
    }
}
