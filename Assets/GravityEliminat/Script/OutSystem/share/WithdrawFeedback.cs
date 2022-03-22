using System;
using UnityEngine;

public class WithdrawFeedback  
{
    private static WithdrawFeedback mInstace;
    public static WithdrawFeedback Instance
    {
        get
        {
            if (mInstace == null)
            {
                mInstace = new WithdrawFeedback();
            }
            return mInstace;
        }
    }
   
    public void Withdraw(float currGold,string key,Action<string,float,int> action,Action<bool> callBack)
    {
        
        //if (ConfigMgr.Instance.IsRestrictWD(currGold))
        //{
        //    ShowText(string.Format("很遗憾！\n今日【{0}元】提现用户已达到1500/1500名，\n请明日再试或尝试其他提现额度。", currGold));
        //    return;
        //}
        WeChatContral.Instance.Withdraw(key, (v) => {
            if (v == "1" || v.Contains("213"))
            {
                //弹出提示

                ShowText("提现成功，请留意微信信息！");
                var ui = UIManager.Instance.ShowPopUp<WithdrawSucceedUI>();
                if (v.Contains("213")) ui.OnShow2(currGold.ToString());
                else ui.OnShow(currGold.ToString());
                

                if (v.Contains("213")) action(key, currGold, 4);
                else action(key, currGold,1);
                callBack(true);

                //显示提现成功跑马灯
                WithdrawSucManger.Instance.AddTotalMoney(currGold);
                var mpanel = UIManager.Instance.ShowPopUp<WithdrawSucPanel>();
                var mdata = AudioMgr.Instance.mdate;
                var mSucData = WithdrawSucManger.Instance.mdata;
                mpanel.RefrishUi(mdata.icon,mdata.weiChatName,currGold, mSucData.totalMoney);
            }
            else
            {
                //弹出提示
                if (v.Contains("402") || v.Contains("209"))
                {
                    action(key, currGold,1);
                    ShowText("该额度只能提现1次!");
                    callBack(true);

                }
                else if (v.Contains("214"))
                {
                    ShowText("提现过于频繁，请稍后再试！");
                    callBack(false);
                }
                else if (v == "")
                {
                    callBack(false);
                }
                else
                {
                    ShowText("提现失败！");
                    callBack(false);
                }

            }
        });
        //AdControl.Instance.ShowRwAd("tx_video", () => { });
    }
    private void ShowText(string str)
    {
        ShowPublicTip.Instance.Show(str);
        Debug.Log(str);
    }
}
