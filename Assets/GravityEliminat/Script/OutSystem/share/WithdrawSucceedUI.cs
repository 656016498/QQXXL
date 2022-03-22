using UnityEngine.UI;

public class WithdrawSucceedUI : UIBase
{
    //关闭，提示
    public Button btnClose1, btnClose2, btnIcon;
    //提现金额
    public Text txtReward,txtState;
    protected void Start()
    {
        btnClose1.onClick.AddListener(OnClose);
        btnClose2.onClick.AddListener(OnClose);
        btnIcon.onClick.AddListener(OnHint);
    }

    private void OnHint()
    {
        //弹出提示
        //ShowText("该界面由第三方平台提供，本游戏无法操作");
    }

    private void OnClose()
    {
        UIManager.Instance.Hide<WithdrawSucceedUI>();
    }



    public void OnShow(string str)
    {
        
        txtReward.text = str;
        //if (rmb >= 1)
        //{
        //    txtState.text = "商家转账审核中";
        //    txtReward.text = str + "(审核中)";
        //}
    }
    public void OnShow2(string str)
    {
        txtState.text = "商家转账审核中";
        txtReward.text = str + "(审核中)";
    }

}
