using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TreasurePop : UIBase
{
    TreasureType Ttype;
    public IButton openBtn;
    public IButton doubleBtn;
    public Button happyBtn;
    public Text addText;

    public Transform starClose;
    public Transform starOpen;
    public Transform challengeClose;
    public Transform challengeOpen;

    public System.Action callBack;

    private void Start()
    {
        openBtn.onClick.AddListener(OpenFunTow); 
        happyBtn.onClick.AddListener(()=> { Hide();
            AudioMgr.Instance.PlaySFX("onClick");
        });
        doubleBtn.onClick.AddListener(()=> {
            RewardEunm rewardEunm = RewardEunm.Null;
            AdControl.Instance.ShowRwAd(doublePoint, () => {
                switch (propR)
                {
                    case 0:
                        rewardEunm = RewardEunm.Step;
                        DataManager.Instance.data.addStepN++; imgName = "iibw_rfce_hiov_icon"; break;
                  
                    case 1:
                        rewardEunm = RewardEunm.Metor;
                        DataManager.Instance.data.addBombN++; imgName = "iibw_rfce_wvod_icon"; break;
                    case 2: 
                        rewardEunm = RewardEunm.Refresh;
                        DataManager.Instance.data.addRefreshN++; imgName = "iibw_rfce_ysif_icon"; break;
                    default:
                        break;
                }
                Hide();
               
                RewardData rewardData = new RewardData(rewardEunm, 2, false);
                UIManager.Instance.Show<RewardPop>(UIType.PopUp, rewardData);
                UmengDisMgr.Instance.CountOnNumber("starbox_double_get");
            });
        });
           
    }

    public void RefrehData
        (int m,float n)
    {
        addNum = n;
       
    }

    int propR;
    string imgName;
    public void OpenFun() {

        propR = Random.Range(0,3);
        AudioMgr.Instance.PlaySFX("宝箱弹出物品");
        switch (propR)
        {
            case 0: DataManager.Instance.data.addStepN++; imgName = "iibw_rfce_hiov_icon";  break;
            case 1: DataManager.Instance.data.addBombN++; imgName = "iibw_rfce_wvod_icon"; break;
            case 2: DataManager.Instance.data.addRefreshN++; imgName = "iibw_rfce_ysif_icon"; break;
            default:
                break;
        }
       

        switch (Ttype)
        {
            case TreasureType.Starlight:
                UmengDisMgr.Instance.CountOnNumber("starbox_get");
                starOpen.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Texture/" + imgName);
                SevenWithdrawDataMgr.Instance.AddTaskData(1,2);
           
               
                starClose.gameObject.SetActive(false);
                starOpen.gameObject.SetActive(true);
                starOpen.GetChild(0).DOLocalMoveY(180, 1);
                starOpen.GetChild(0).DOScale(Vector3.one, 1);
                break;
            case TreasureType.Challenge:

                challengeOpen.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Texture/" + imgName);

                SevenWithdrawDataMgr.Instance.AddTaskData(1,3);
                DataManager.Instance.data.ChallengeStar -= 10;
                UIManager.Instance.Refresh<MainPanel>();
                challengeClose.gameObject.SetActive(false);
                challengeOpen.gameObject.SetActive(true);
                challengeOpen.GetChild(0).DOLocalMoveY(180, 1);
                challengeOpen.GetChild(0).DOScale(Vector3.one, 1);

                break;
            default:
                break;
        }
        openBtn.gameObject.SetActive(false);
        doubleBtn.gameObject.SetActive(true);
        happyBtn.gameObject.SetActive(true);
        DataManager.Instance.SaveGameData();
    }

    public float addNum;
    public int taregtNum;
    public void OpenFunTow() {

        UmengDisMgr.Instance.CountOnPeoples("leve_box_get",GameManager.Instance.CurrentLevel.ToString());

        addText.text = addNum.ToString("f2")+"元";

        switch (Ttype)
        {
            case TreasureType.Starlight:
                UmengDisMgr.Instance.CountOnNumber("starbox_get");
                //starOpen.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Texture/" + imgName);
                SevenWithdrawDataMgr.Instance.AddTaskData(1, 2);
               
                GameManager.Instance.StarShineStarSub.Value = 0;
                starClose.gameObject.SetActive(false);
                starOpen.gameObject.SetActive(true);
                starOpen.GetChild(0).DOLocalMoveY(200, 1);
                starOpen.GetChild(0).DOScale(Vector3.one*1.2f, 1);
                break;
            case TreasureType.Challenge:

                //challengeOpen.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Texture/" + imgName);

                SevenWithdrawDataMgr.Instance.AddTaskData(1, 3);
                DataManager.Instance.data.ChallengeStar -= 10;
                UIManager.Instance.Refresh<MainPanel>();
                challengeClose.gameObject.SetActive(false);
                challengeOpen.gameObject.SetActive(true);
                challengeOpen.GetChild(0).DOLocalMoveY(180, 1);
                challengeOpen.GetChild(0).DOScale(Vector3.one, 1);

                break;
            default:
                break;
        }
        openBtn.gameObject.SetActive(false);
        //doubleBtn.gameObject.SetActive(true);
        happyBtn.gameObject.SetActive(true);
    }

    /// <summary>
    /// 恢复UI
    /// </summary>
    public void ReduceUI() {

        happyBtn.gameObject.SetActive(false);
        doubleBtn.gameObject.SetActive(false);
        openBtn.gameObject.SetActive(true);
        starClose.transform.gameObject.SetActive(false);
        starOpen.transform.gameObject.SetActive(false);
        challengeClose.transform.gameObject.SetActive(false);
        challengeOpen.transform.gameObject.SetActive(false);
        starOpen.GetChild(0).localPosition = Vector2.zero;
        starOpen.GetChild(0).localScale = Vector2.zero;
        challengeOpen.GetChild(0).localPosition = Vector2.zero;
        challengeOpen.GetChild(0).localScale = Vector2.zero;


    }
    string doublePoint = null;
    public override void Show(object data)
    {

        base.Show();
        ReduceUI();
        Ttype = (TreasureType)data;
        switch (Ttype)
        {
            case TreasureType.Starlight:
                doublePoint = "star_box_video";
                starClose.gameObject.SetActive(true);
                starClose.GetComponent<Animator>().enabled = true;
                break;
            case TreasureType.Challenge:
                doublePoint = "level_box_video";
                challengeClose.transform.gameObject.SetActive(true);
                challengeClose.GetComponent<Animator>().enabled = true;
                break;
            default:
                break;
        }

        //UIManager.Instance.Hide<WithdrawUI>();
        //显示banner
        GameADControl.Instance.Banner(true);
    }
    public override void Hide()
    {
        base.Hide();


        UIManager.Instance.Refresh<GamePanel>();
        starClose.GetComponent<Animator>().enabled = false;
        challengeClose.GetComponent<Animator>().enabled = false;
        UIManager.Instance.Refresh<PassPop>();

        //隐藏banner
        GameADControl.Instance.Banner(false);
        if (UIManager.Instance.GetBase<GamePanel>() != null)
        {
            UIManager.Instance.GetBase<GamePanel>().RefreshBottomUI();
            var mGamePanel = UIManager.Instance.GetBase<GamePanel>();
            var mPos1 = mGamePanel.largeCashBtn.transform.GetChild(2).position;
            MoneyFly.instance.Play(3, Vector3.zero, mPos1, () => {
                XDebug.Log("飞达目的地");
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HbEffect2, UIManager.Instance.GetBase<GamePanel>().largeCashBtn.transform.GetChild(2).position);

            });

        }
        GameManager.Instance.DestoryLevel();
        GameManager.Instance.LoadLevel();
    }

}
