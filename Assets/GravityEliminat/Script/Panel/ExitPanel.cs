using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanel : UIBase
{
    public IButton exitBtn;
    public IButton noBtn;
    void Start()
    {
        exitBtn.onClick.AddListener(() => { Application.Quit(); });
        noBtn.onClick.AddListener(() => { Hide(); });
    }
    // Update is called once per frame
    public override void Show()
    {
        GameADControl.Instance.ExitInit();
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
    }
}
