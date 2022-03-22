using System;
using UnityEngine.UI;

public class MissWithdrawUI : UIBase
{
    public Text txtComfort;
    public Button btnClose;
    private Action sure;
    // Start is called before the first frame update
    protected void Start()
    {
        btnClose.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        sure?.Invoke();
        UIManager.Instance.Hide<MissWithdrawUI>();
    }

    public void OnShow(int reward,Action Onsure)
    {
        sure = Onsure;
        txtComfort.text = string.Format("+{0}",reward);
    }
}
