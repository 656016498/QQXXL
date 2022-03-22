using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LotteryTipPanel : UIBase 
{
    public Button exitBtn;
    public Button okBtn;
    public Text dir;
    public Action exit_Click;
    public Action okClick;
    void Start()
    {
        ButtonSet();
    }

    public void ButtonSet()
    {
        exitBtn.onClick.AddListener(ExitBtnClick);
        okBtn.onClick.AddListener(OkBtnClick);
    }
    private void ExitBtnClick()
    {
        exit_Click.Run();
    }
    private void OkBtnClick()
    {
        okClick.Run();
    }

    public void AddListenToBtn(Action clickClose, Action clickOk)
    {
        exit_Click = clickClose;
        okClick = clickOk;
    }
    public void SetDir(string str)
    {
        dir.text = str;
    }
    

}
