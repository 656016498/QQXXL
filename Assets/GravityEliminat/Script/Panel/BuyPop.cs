using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// 购买类型
/// </summary>
public enum BuyType {
    HorizontalProp,
    CrossProp,
    RoundProp,
    Step,
    Meteor,
    Refresh,
}

/// <summary>
/// 购买方式
/// </summary>
public enum BuyWay {

    Diamond,
    Video,

}


public class BuyData {

    public  BuyType buyType;
    public BuyWay buyWay;
    public string video;
    public Sprite sprite=null;
  
    public BuyData(BuyType type,BuyWay way,string point, Sprite sprites) {

        buyType = type;
        buyWay = way;
        video = point;
        sprite = sprites;

    }

    public BuyData(BuyType type, BuyWay way, string point)
    {

        buyType = type;
        buyWay = way;
        video = point;


    }

    public BuyData(BuyType type, BuyWay way)
    {
        buyType = type;
        buyWay = way;
    }
}
public class BuyPop : UIBase
{
    public IButton closeBtn;
    public Image buyImg;
    public IButton DiamondbuyBtn;
    public Text buyText;
    public IButton adBtn;
    public IButton addDiamond;
    public Text diamondText;
    void Start()
    {
        addDiamond.onClick.AddListener(() =>
        {
            UIManager.Instance.Show<AddPop>(UIType.PopUp, AddEumn.Diamond);
        });
        closeBtn.onClick.AddListener(Hide);
        DiamondbuyBtn.onClick.AddListener(() => {
           

            UmengDisMgr.Instance.CountOnNumber("zs_buy");

            if (DataManager.Instance.data.Diamond >= 50)
            {
                GameManager.Instance.DiamondSub.Value -= 50;
                switch (data.buyType)
                {
                    case BuyType.HorizontalProp:
                        DataManager.Instance.data.BombProp[0] += 5;
                        break;
                    case BuyType.CrossProp:
                        DataManager.Instance.data.BombProp[1] += 3;

                        break;
                    case BuyType.RoundProp:
                        DataManager.Instance.data.BombProp[2] += 1;
                        break;

                    default:
                        break;
                }
                UIManager.Instance.Refresh<JoinPop>();
            }
            else {
                UIManager.Instance.Show<AddPop>(UIType.PopUp, AddEumn.Diamond);
            }
            Hide();
            diamondText.text = DataManager.Instance.data.Diamond.ToString();

        });
        adBtn.onClick.AddListener(() => {
            AdControl.Instance.ShowRwAd(data.video,()=> {
                UmengDisMgr.Instance.CountOnNumber("zs_buy");
                RewardEunm rewardEunm = RewardEunm.Null;
                switch (data.buyType)
                {
                    case BuyType.HorizontalProp:
                        DataManager.Instance.data.BombProp[0] += 5;
                        break;
                    case BuyType.CrossProp:
                        DataManager.Instance.data.BombProp[1] += 3;

                        break;
                    case BuyType.RoundProp:
                        DataManager.Instance.data.BombProp[2] += 1;
                        break;
                    case BuyType.Step:
                        DataManager.Instance.data.addStepN++;
                        rewardEunm = RewardEunm.Step;
                        break;
                    case BuyType.Meteor:
                        DataManager.Instance.data.addBombN++;
                        rewardEunm = RewardEunm.Metor;

                        break;
                    case BuyType.Refresh:
                        DataManager.Instance.data.addRefreshN++;
                        rewardEunm = RewardEunm.Refresh;
                        break;
                    default:
                        break;
                }
                if (UIManager.Instance.GetBase<GamePanel>()!=null)
                {
                    UIManager.Instance.GetBase<GamePanel>().RefreshBottomUI();
                }
                Hide();
                UIManager.Instance.Refresh<JoinPop>();


                if (rewardEunm!=RewardEunm.Null)
                {
                    RewardData rewardData = new RewardData(rewardEunm, 1, false);
                    UIManager.Instance.Show<RewardPop>(UIType.PopUp, rewardData);
                }
            });
        
        });
    }
    string imgName = null;
    BuyData data;
    public override void Show(object obj)
    {
        base.Show();
        diamondText.text = DataManager.Instance.data.Diamond.ToString();

        DiamondbuyBtn.gameObject.SetActive(false);
        adBtn.gameObject.SetActive(false);
        data = (BuyData)obj;
        switch (data.buyWay)
        {
            case BuyWay.Diamond:

                DiamondbuyBtn.gameObject.SetActive(true);
                UmengDisMgr.Instance.CountOnNumber("zs_buy_show");
                addDiamond.transform.parent.gameObject.SetActive(true);
                break;
            case BuyWay.Video:

                adBtn.gameObject.SetActive(true);
                addDiamond.transform.parent.gameObject.SetActive(false);

                break;
            default:
                break;
        }


        switch (data.buyType)
        {
            case BuyType.HorizontalProp:
                buyText.text = "x5";
                imgName = "Red1";
                break;
            case BuyType.CrossProp:
                buyText.text = "x3";
                imgName = "Red2";

                break;
            case BuyType.RoundProp:
                buyText.text = "x1";
                imgName = "Red3";
                break;
            case BuyType.Step:
                buyText.text = "x1";
                break;
            case BuyType.Meteor:
                buyText.text = "x1";
                break;
            case BuyType.Refresh:
                buyText.text = "x1";
                break;
            default:
                break;
        }
        if (data.sprite == null)
        {
            buyImg.sprite = Resources.Load<Sprite>("PropSprite/" + imgName);
        }
        else {
            buyImg.sprite = data.sprite;
        }
        buyImg.SetNativeSize();



        //展示信息流
        GameADControl.Instance.ShowMsg(true);
    }

    public override void Hide()
    {
        //展示信息流
        GameADControl.Instance.ShowMsg(false);
        base.Hide();
    }
}
