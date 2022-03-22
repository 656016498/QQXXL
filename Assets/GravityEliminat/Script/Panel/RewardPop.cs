using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum RewardEunm { 
    Null=-1,
    Diamond=0,
    Step,
    Metor,
    Refresh,
    Ticket,
}

public class RewardData
{
    public RewardEunm rewardEunm;
   
    public Sprite sprite;
    public int RewdNum;
    public bool isMultiple;
    public RewardData(RewardEunm rewardEunm,int RewardNum,bool IsMultiple) {
      this.rewardEunm = rewardEunm;
        RewdNum = RewardNum;
        isMultiple = IsMultiple;
    }
    public RewardData(RewardEunm rewardEunm, int RewardNum, bool IsMultiple,Sprite sprite)
    {
        this.rewardEunm = rewardEunm;
        this.sprite = sprite;
        RewdNum = RewardNum;
        isMultiple = IsMultiple;
    }
}
public class RewardPop : UIBase
{
    public Image rewardImg;
    public Text rewardNumText;
    public IButton closeBtn;
    public IButton MultipelBtn;
    public IButton GetBtn;
    public Sprite[] allImg;
    RewardData data;
    int joinNum;
    string multiplePoint;
    void Start()
    {
        GetBtn.onClick.AddListener(Hide);
        closeBtn.onClick.AddListener(Hide);
        MultipelBtn.onClick.AddListener(() => {
            AdControl.Instance.ShowRwAd(multiplePoint, () => {
                int r = Random.Range(1, 5);
                data.RewdNum = r * data.RewdNum;
                data.isMultiple = false;
                switch (data.rewardEunm)
                {
                    case RewardEunm.Diamond:
                        GameManager.Instance.DiamondSub.Value += data.RewdNum;
                        break;
                    default:
                        break;
                }
                data.RewdNum += joinNum;
                Show(data);

            });
            
        });
    }
    public override void Show(object obj)
    {
        multiplePoint = "";
        base.Show();
        data = obj as RewardData;
        IntiPage();
    }

    public void IntiPage() {
        joinNum = data.RewdNum;
        MultipelBtn.gameObject.SetActive(data.isMultiple);
        closeBtn.gameObject.SetActive(data.isMultiple);
        if (data.sprite == null)
        {
            rewardImg.sprite = allImg[(int)data.rewardEunm];
        }
        else {

            rewardImg.sprite = data.sprite;

        }
        var imgScale = 1f;
        switch (data.rewardEunm)
        {
            case RewardEunm.Null:
                break;
            case RewardEunm.Diamond:
                multiplePoint = "zs_multiple_video";
                break;
            case RewardEunm.Step:
                //imgScale = 2;
                break;
            case RewardEunm.Metor:
                //imgScale = 2;

                break;
            case RewardEunm.Refresh:
                //imgScale = 2;

                break;
            case RewardEunm.Ticket:
                imgScale = 1.5f; 
                break;
            default:
                break;
        }

        rewardImg.transform.localScale = Vector3.one * imgScale;

        rewardImg.SetNativeSize();
        rewardNumText.text ="+"+ (joinNum).ToString();
    }

}
