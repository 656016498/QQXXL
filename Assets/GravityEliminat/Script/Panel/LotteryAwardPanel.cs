using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public enum AawardPanelType 
{
    Lottery,//转盘
    Sign,//签到
}
public class LotteryAwardPanel : UIBase
{
    public IButton exitBtn;
    public IButton okBtn;
    public Image icon;
    public Action exit_Click;
    public Action okClick;
    [Header("对应奖励类型图片")]
    public List<Sprite> maward;
    public Text addValue;
    [Header("金猪")]
    public Transform pig;
    public AawardPanelType mtype;
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
    public void SetIcon(int index) 
    {
        switch (mtype)
        {
            case AawardPanelType.Lottery:
                icon.transform.ShowCanvasGroup();
                Debug.Log("index" + index);
                icon.sprite = maward[index - 1];
                if (index == 2)
                {
                    icon.transform.localRotation = Quaternion.Euler(0, 0, 100);
                    icon.transform.localPosition = new Vector3(0, 10.6f, 0);
                }
                else if (index == 5)
                {
                    icon.transform.localRotation = Quaternion.Euler(0, 0, -135f);
                    icon.transform.localPosition = new Vector3(0, 0f, 0);
                }
                else
                {
                    icon.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    icon.transform.localPosition = new Vector3(0, 0f, 0);
                }

                icon.SetNativeSize();
                break;
            case AawardPanelType.Sign:
                pig.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        
    }

    public override void Hide()
    {
        base.Hide();
    }
    public override void Show()
    {
        icon.transform.HideCanvasGroup();
        pig.gameObject.SetActive(false);
        exitBtn.transform.HideCanvasGroup();
        //Observable.Timer(System.TimeSpan.FromSeconds(2f)).Subscribe(_ => {
        //    exitBtn.transform.ShowCanvasGroup();
        //});
        base.Show();
    }
    public void SetDir(int value)
    {
        addValue.text = string.Format("+{0}", value);
    }
}
