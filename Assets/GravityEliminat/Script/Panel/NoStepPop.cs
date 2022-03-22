using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoStepPop : UIBase
{

    public IButton closeBtn;
    public IButton ContinueBtn;
    public Image Img;


    int propR;
    string imgName;

    private void Start()
    {
        ContinueBtn.onClick.AddListener(()=> {
            AdControl.Instance.ShowRwAd("bs_add_video", () => {

                //if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
                //{
                    UmengDisMgr.Instance.CountOnPeoples("bs_addp", DataManager.Instance.data.UnlockLevel.ToString());
                    UmengDisMgr.Instance.CountOnNumber("bs_addu", DataManager.Instance.data.UnlockLevel.ToString());
                //}
                GameManager.Instance.RemainingSteps.Value += 10;
                Hide();
            });

           
        

        });

        closeBtn.onClick.AddListener(() => {


            UIManager.Instance.ShowPopUp<AgainPop>();
            //GameManager.Instance.DestoryLevel();
            //GameManager.Instance.LoadLevel();
            Hide();

        });
    }

    public override void Show()
    {
        base.Show();
        //if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
        //{
            UmengDisMgr.Instance.CountOnPeoples("bs_lack_showp", DataManager.Instance.data.UnlockLevel.ToString());
            UmengDisMgr.Instance.CountOnNumber("bs_lack_showu", DataManager.Instance.data.UnlockLevel.ToString());
        //}

        AudioMgr.Instance.PlaySFX("游戏失败");
        propR = Random.Range(0, 3);
        switch (propR)
        {
            case 0: DataManager.Instance.data.addStepN++; imgName = "iibw_rfce_hiov_icon"; break;
            case 1: DataManager.Instance.data.addBombN++; imgName = "iibw_rfce_wvod_icon"; break;
            case 2: DataManager.Instance.data.addRefreshN++; imgName = "iibw_rfce_ysif_icon"; break;
            default:
                break;
        }
        Img.sprite = Resources.Load<Sprite>("UI/Texture/"+ imgName); Img.SetNativeSize();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
