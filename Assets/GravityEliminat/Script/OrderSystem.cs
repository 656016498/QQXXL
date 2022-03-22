using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BayatGames.SaveGamePro;

public class OrderSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Image awardNum;//奖励金额
    public Text info;
    //public SliderControl mSliderControl;
    public Button cashBtn;
    public List<Sprite> mList;
    public Image awardIcon;
    public Image fillPro;
    public Text fillText;

    private void Start()
    {
        cashBtn.onClick.AddListener(() => {

            UIManager.Instance.ShowPopUp<LotteryPanel>();
        });
        //刷新金额icon
        RefrishAwardNum();
        //刷新UI
      
        RefrishUi();

        OrderSystemControl.Instance.awardType.Subscribe(value => {

            RefrishAwardNum();
        });
    }

    public void RefrishAwardNum()
    {
        var cashNum = LotteryDataManger.Instance.mdata.cashNum; 
        if (cashNum == 0)
        {
            awardIcon.sprite = mList[0];
        }
        else if (cashNum ==0.3f)
        {
            awardIcon.sprite = mList[1]; 
        }
        else if (cashNum==0.5f)
        {
            awardIcon.sprite = mList[2];
        }
        else if (cashNum==1f)
        {
            awardIcon.sprite = mList[3];
        }
        awardIcon.SetNativeSize();
    }

    public void RefrishUi()
    {
        
        OrderSystemControl.Instance.unLockLevel.Value = OrderSystemControl.Instance.mdata.Orderlevel;
        OrderSystemControl.Instance.unLockLevel.Subscribe(value => {
            //TableMgr.Instance.GetTickLevel()
            var targetLevel = OrderSystemControl.Instance.mdata.targetlevlel;//目标抽奖券弹窗
            var nowLevel = value;
            var mlevel = targetLevel - nowLevel;
            if (mlevel > 0)
            {
                info.text = string.Format("再过<color=#ffea00>{0}关</color>可抽取现金", mlevel);
            }
            else
            {
                info.text = string.Format("可抽取现金");
            }

            var mpro = (float)nowLevel / targetLevel;
            fillPro.fillAmount = mpro;
            if (nowLevel <= targetLevel)
            {
                fillText.text = string.Format("{0}/{1}", nowLevel, targetLevel);
            }
            else
            {
                fillText.text = string.Format("{0}/{1}", targetLevel, targetLevel);
            }
        });

    }


}
public class OrderSystemControl
{
    private static OrderSystemControl instance;
    public static OrderSystemControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new OrderSystemControl();
            }
            return instance;
        }
        
    }

    public ReactiveProperty<int> unLockLevel = new ReactiveProperty<int>();

    public ReactiveProperty<float> awardType = new ReactiveProperty<float>();

    public OrderSystemData mdata;
    public const string local_Key = "Order_key";
    bool isInit = false;
    public OrderSystemControl()
    {
        if (!isInit)
        {
            Debug.Log("架子啊数据");
            LoadData();
            isInit = true;
        }
    }
    private void LoadData()
    {
        mdata = SaveGame.Load<OrderSystemData>(local_Key);
        if (mdata == null)
        {
            mdata = new OrderSystemData();
        }
        SaveData();
    }
    private void SaveData()
    {
        SaveGame.Save(local_Key,mdata);
    }

    public void AddOrderLevel(int num) 
    {
        mdata.Orderlevel += num;
        Debug.Log("mdata.Orderlevel" + mdata.Orderlevel);
        Debug.Log("mdata.targetlevlel" + mdata.targetlevlel);
        if (mdata.Orderlevel >=mdata.targetlevlel)
        {
            mdata.Orderlevel =0;
            SetTargetLevel();
            
        }
        unLockLevel.Value = mdata.Orderlevel;
        SaveData();
    }

    //设置目标关卡
    public void SetTargetLevel()
    {
        mdata.targetlevlel =/* TableMgr.Instance.GetTickLevel()*/GetTickLevel()- LastGetTickLevel();
        Debug.Log("上次关卡目标" + LastGetTickLevel());
        Debug.Log("当前最新目标关卡" + mdata.targetlevlel);
        SaveData();
    }
    //获取上个目标关卡
    public int LastGetTickLevel() 
    {

        for (int i = 0; i <mdata. mListLevel.Count; i++)
        {
            if (i!= 0)
            {
                if (DataManager.Instance.data.UnlockLevel < mdata.mListLevel[i])
                {
                    return mdata.mListLevel[i-1];
                }
            }
        }
        return 0;

    }

    public int GetTickLevel()
    {
        for (int i = 0; i < mdata.mListLevel.Count; i++)
        {
                if (DataManager.Instance.data.UnlockLevel < mdata.mListLevel[i])
                {
                    return mdata.mListLevel[i];
                }
        }
        return 0;
    }
}
//订单系统数据类
public class OrderSystemData
{
    public int Orderlevel;
    public int targetlevlel;//实时最新关卡
    public List<int> mListLevel;
    public OrderSystemData()
    {
        Orderlevel = 0;
        targetlevlel = TableMgr.Instance.GetTickLevel();
        mListLevel = TableMgr.Instance.InitTicketLevel();
    }
}