using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcpmPanel : UIBase
{
    public static bool ShowPanel = false;
    public Button exitBtn;//退出按钮
    [Header("普通红包币")]
    public Text hb;
    [Header("金猪币")]
    public Text pigText;
    [Header("转盘")]
    public Text lotfillPro;//fillPro
    

    void Start()
    {
        exitBtn.onClick.AddListener(() => { Hide(); });
    }

    public override void Show()
    {
        
        base.Show();
    }

    public override void Hide()
    {
       
        base.Hide();
    }

    
   
  

    public void RefrishUi(float vedioXi,float mhbXi,float mecpm,float mawardXi,float ecpmAwardc,float mhbCoin,float mlotFill,float hbccc) 
    {
        //(int)(RandomVedioModulus() * RandomHbCoinModulus() * ecpm * (RandomRewardModulus() + ReturnConstant()))
        //普通红包币
        hb.text = string.Format("小额红包币：{0}*{1}*{2}*({3}+{4})*1.5f={5}-红包系数管控：{6}", vedioXi, mhbXi, mecpm, mawardXi, ecpmAwardc, Convert.ToInt16(mhbCoin * 1.5F), hbccc);

        pigText.text = string.Format("金猪币：{0}*{1}*{2}*({3}+{4})*0.3f={5}", vedioXi, mhbXi, mecpm, mawardXi, ecpmAwardc, Convert.ToInt16(mhbCoin * 0.3F));
        //ECPM值 / 100 * 1 /？元
        lotfillPro.text = string.Format("转盘进度：{0}/100*1/{1}={2}", mecpm, LotteryDataManger.Instance.mdata.cashNum, mlotFill);
        Show();
    }


}
