using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public enum AddEumn { 

    Love,
    Diamond,
   
}

public class AddPopMgr : Singlton<AddPopMgr> {

    public bool isPassivity;//是否被动
    AddPopMgr() {
        isPassivity = false;

    }
}

public class AddPop : UIBase
{
    //public bool initiativ;
    AddEumn addEumn;
    public Transform DiamondImg;
    public Text DiamondAddText;
    public Transform LoveImg;
    public Text NowLoveText;
    public Text CuntDownText;
    public IButton LoveBtn;
    public IButton DiamondBtn;
    public Text TitleText;
    public IButton closeBtn;
    public Text PropDir;
    public Sprite[] loveImg;
    //public IButton adBtn;
    //public Image propImg;
    void Start()
    {
        TimeClock.NowTimeListening.Subscribe(_ => {

            CuntDownText.text = TimeMgr.Instance.LoveString;

        });


        closeBtn.onClick.AddListener(() => {

            Hide();

        });

        DiamondBtn.onClick.AddListener(()=> {
            AdControl.Instance.ShowRwAd("zs_add_video", () => {
                UmengDisMgr.Instance.CountOnNumber("zs_add_get");
                GameManager.Instance.DiamondSub.Value += 50;
                Hide();
                RewardData reward = new RewardData(RewardEunm.Diamond, 50, true);
                UIManager.Instance.Show<RewardPop>(UIType.PopUp, reward);

            });

        });

        LoveBtn.onClick.AddListener(() => {
            AdControl.Instance.ShowRwAd("tl_add_video", () =>
            {

                if (AddPopMgr.Instance.isPassivity)
                {
                    if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
                    {
                        UmengDisMgr.Instance.CountOnNumber("tl_lack_bd_get", DataManager.Instance.data.UnlockLevel.ToString());
                    }
                  
                }
                else
                {
                    UmengDisMgr.Instance.CountOnNumber("tl_lack_zd_get");

                }

                GameManager.Instance.LoveStar.Value += 10;
                LoveFly.instance.Play(5, transform.position, UIManager.Instance.GetBase<MainPanel>().loveBtn.transform.parent.GetChild(0).position, () => {

                 Transform t=   Pool.Instance.SpawnEffect(Pool.Effect_PoolName,Pool.TiLiArrive ,UIManager.Instance.GetBase<MainPanel>().loveBtn.transform.parent.GetChild(0).position) ;
                    t.SetParent(UIManager.Instance.GetBase<MainPanel>().transform);
                    AudioMgr.Instance.PlaySFX("点击按钮");
                });
                Hide();
            });
        });


       
    }

    public override void Show(object obj)
    {
        base.Show();
        addEumn = (AddEumn)obj;
        InitPage();
        //显示信息流
        GameADControl.Instance.ShowMsg(true);
    }

    string titleStr = null;
    public void InitPage() {

        LoveImg.gameObject.SetActive(false);
        DiamondImg.gameObject.SetActive(false);
        LoveBtn.gameObject.SetActive(false);
        DiamondBtn.gameObject.SetActive(false);
        //adBtn.gameObject.SetActive(false);
        //propImg.gameObject.SetActive(false);
        switch (addEumn)
        {
            case AddEumn.Love:
              
                    if (AddPopMgr.Instance.isPassivity)
                    {
                    if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
                    {
                        UmengDisMgr.Instance.CountOnNumber("tl_lack_bd", DataManager.Instance.data.UnlockLevel.ToString());
                    }
                    }
                    else
                    {
                    UmengDisMgr.Instance.CountOnNumber("tl_lack_zd");
                }
                LoveImg.gameObject.SetActive(true);
                var index = GameManager.Instance.LoveStar.Value > 0 ? 0 : 1;
                LoveImg.GetComponent<Image>().sprite= loveImg[index];
                LoveBtn.gameObject.SetActive(true);
                PropDir.text = "倒计时结束体力+1";
                NowLoveText.text = GameManager.Instance.LoveStar.Value.ToString();
                titleStr = "补充体力";
                break;

            case AddEumn.Diamond:

                UmengDisMgr.Instance.CountOnNumber("zs_add_show");

                titleStr = "补充钻石";
                DiamondImg.gameObject.SetActive(true);
                DiamondBtn.gameObject.SetActive(true);
                PropDir.text = null;
                break;

            //case AddEumn.Step:
            //    titleStr = "补充道具";
            //    propImg.gameObject.SetActive(true);
            //    propImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Texture/iibw_rfce_hiov_icon");
            //    adBtn.gameObject.SetActive(true);

            //    break;

            //case AddEumn.Meteor:
            //    titleStr = "补充道具";
            //    propImg.gameObject.SetActive(true);
            //    propImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Texture/iibw_rfce_wvod_icon");
            //    adBtn.gameObject.SetActive(true);

            //    break;
            //case AddEumn.refresh:
            //    titleStr = "补充道具";
            //    propImg.gameObject.SetActive(true);
            //    propImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Texture/iibw_rfce_ysif_icon");
            //    adBtn.gameObject.SetActive(true);
            //    break;
        }
        TitleText.text = titleStr;
    }
    public override void Hide()
    {
        base.Hide();
        //switch (addEumn)
        //{
        //    case AddEumn.Love:
                
        //        break;
        //    case AddEumn.Diamond:
        //        break;
        //    default:
        //        break;
        //}
        //显示信息流
        GameADControl.Instance.ShowMsg(false);
    }
}
