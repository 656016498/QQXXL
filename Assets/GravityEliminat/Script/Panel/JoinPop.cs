using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class JoinPop : UIBase
{
    Porp_Size porp;
    public bool[] PropIsChoose = new bool[3] {false,false,false };
    public Text titleText;
    public Transform[] needImgP;
    public IButton closeBtn;
    public Transform propParent;
    public IButton BeginBtn;
    public Text talkText;
    public int[] unLockLevel=new int[3] { 9,16,28};

    public IButton addDiamond;
    public Text diamondText;

    private void Start()
    {
        //addDiamond.onClick.AddListener(() =>
        //{
        //    UIManager.Instance.Show<AddPop>(UIType.PopUp, AddEumn.Diamond);
        //});
        BeginBtn.onClick.AddListener(() => {

            if (GameManager.Instance.LoveStar.Value > 0)
            {
                UIRoot.Instance.ShowMask();
                AudioMgr.Instance.PlaySFX("闯关爱心");
                DynamicMgr.Instance.FlyUIPrefab(Pool.LoveImg, UIManager.Instance.GetBase<MainPanel>().loveBtn.transform.parent.GetChild(0).position, BeginBtn.transform.position,0.5f,()=> {

                    Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.LoveArrive, BeginBtn.transform.position,2);
 
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_=> {


                        if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
                        {
                            UmengDisMgr.Instance.CountOnPeoples("level_startp", DataManager.Instance.data.UnlockLevel.ToString());
                            UmengDisMgr.Instance.CountOnNumber("level_startu", DataManager.Instance.data.UnlockLevel.ToString());
                        }

                        AudioMgr.Instance.PlayMusic("战斗界面BGM");



                        GameManager.Instance.LoveStar.Value--;
                        GameManager.Instance.CurrentLevel = showLevel;
                        GameManager.Instance.LoadLevel();
                        GameManager.Instance.BegInCreateProp(PropIsChoose);
                        UIManager.Instance.Hide<MainPanel>();
                        Hide();
                        //UIRoot.Instance.HideMask();
                    });
                    

                });
            }
            else {

                AddPopMgr.Instance.isPassivity = true;
                UIManager.Instance.Show<AddPop>(UIType.PopUp,AddEumn.Love);

              

            }

            //点击开始打点
            UmengDisMgr.Instance.CountOnPeoples("level_start_tc_go", GameManager.Instance.CurrentLevel.ToString());
        });
        closeBtn.onClick.AddListener(() => {

            Hide();
            //AudioMgr.Instance.PlaySFX("onClick");

        });
        for (int i = 0; i < propParent.childCount; i++)
        {
            int index = i;
            propParent.GetChild(index).GetComponent<IButton>().onClick.AddListener(() => {
                if (DataManager.Instance.data.BombProp[index] <= 0)
                {
                    //展示补充界面
                    BuyData buyData = new BuyData((BuyType)index, BuyWay.Video, "zs_add_video");
                    UIManager.Instance.Show<BuyPop>(UIType.PopUp, buyData);

                }
                else
                {
                    PropIsChoose[index] = !PropIsChoose[index];
                    RefreshProp(index);
                }
            });
        }

    }


    public void RefreshProp(int index) {

        if (!PropIsChoose[index])
        {
            if (DataManager.Instance.data.BombProp[index] > 0)
            {
                propParent.GetChild(index).Find("add").gameObject.SetActive(false);
                propParent.GetChild(index).Find("NUM").gameObject.SetActive(true);
            }
            else
            {
                propParent.GetChild(index).Find("add").gameObject.SetActive(true);
                propParent.GetChild(index).Find("NUM").gameObject.SetActive(false);

            }
            propParent.GetChild(index).Find("gou").gameObject.SetActive(false);
        }
        else
        {
            propParent.GetChild(index).Find("gou").gameObject.SetActive(true);
        }
        propParent.GetChild(index).Find("NUM").GetComponent<Text>().text=DataManager.Instance.data.BombProp[index].ToString();
    }
    int showLevel;
    public override void Show(object data)
    {
        base.Show();

        PropIsChoose = new bool[3] { false, false, false };
        showLevel = (int)data;
        InitTargetImg();

        for (int i = 0; i < 3; i++)
        {
            RefreshProp(i);
        }
        titleText.text = string.Format("第 {0} 关", showLevel);
        talkText.text = string.Format("再过{0}抽现金","<color=#ff5400>" + (TableMgr.Instance.GetTickLevel()+1- showLevel) + "关</color>");

        //展示信息流
        //GameADControl.Instance.ShowMsg(true);
        //打点
        UmengDisMgr.Instance.CountOnPeoples("level_start_tc_show", GameManager.Instance.CurrentLevel.ToString());
    }




   LevelSetting level;
    int realLevel;
    public void InitTargetImg() {

        realLevel = GameManager.Instance.GetRealLevel(showLevel);
        if (DataManager.Instance.data.useWho)
        {
            level = Resources.Load<LevelSetting>("AllLevel/Level" + realLevel);
        }
        else { 
             level = Resources.Load<LevelSetting>("Level/Level" + realLevel);
        }
        for (int i = 0; i < needImgP.Length; i++)
        {
            if (i > level.cc.Count - 1)
            {
                needImgP[i].gameObject.SetActive(false);
            }
            else
            {
                needImgP[i].gameObject.SetActive(true);
                needImgP[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("BallSprite/" + level.cc[i].ballType.ToString() + "_" + level.cc[i].colorType.ToString());
                needImgP[i].GetComponentInChildren<Text>().text= string.Format("{0}", level.cc[i].num.ToString());
            }
        }
    

    }

    public override void Refresh()
    {
        base.Refresh();
        for (int i = 0; i < 3; i++)
        {
            RefreshProp(i);
        }
    }

    public override void Hide()
    {
        //隐藏信息流
        //GameADControl.Instance.ShowMsg(false);
        base.Hide();
    }


}
