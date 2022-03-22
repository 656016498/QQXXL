using System;
using UnityEngine.UI;

public class ReplaceUI : UIBase
{

    public Button btnSure, btnNo;
    public Text txtShow;
    private Action Sure,No;
    // Start is called before the first frame update
    protected void Start()
    {
        btnNo.onClick.AddListener(OnNO);
        btnSure.onClick.AddListener(OnSure);
    }

    private void OnSure()
    {
        Sure?.Invoke();
        OnNO();
    }

    private void OnNO()
    {
        No?.Invoke();
        UIManager.Instance.Hide<ReplaceUI>();
    }

    public void OnShwo(float reward,Action action ,Action no)
    {
        Sure = action;
        No = no;
        txtShow.text =string.Format("恭喜抽中<color=#ffbc1c>{0}元</color>限时提现！\n是否需要<color=#ffbc1c>替换</color>当前限时提现？",reward);
    }
}
