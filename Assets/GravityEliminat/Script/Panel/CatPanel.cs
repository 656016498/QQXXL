using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using BayatGames.SaveGamePro;
using UniRx;
using EasyExcel;
using EasyExcelGenerated;
public class CatManager {

     static CatManager Ins;
    public CatData catData;
    public static CatManager Instance {

       get  {
            if (Ins==null)
            {
                Ins = new CatManager();
            }
            return Ins;
        }
    }
    public EEDataManager EE = new EEDataManager();
    public int NowCatNum;
    public int LevelCat = 10;
    public CatManager() {
        EE.Load();
        catData = SaveGame.Load<CatData>("CatDATA");
        if (catData==null)
        {
            catData = new CatData();
            catData.taskGets = EE.GetList<TaskGet>();
            SaveData();
        }
    }


    public void CanGet(bool ShowpAGE=true) {

        if (GameManager.Instance.OverGame) return;
        var can = false;
        if (catData.now[0]>=10&&!catData.IsGet[0])
        {
            Debug.LogError("EEE00" + catData.IsGet[0]);

            can = true;
        }
        if (catData.now[1] >= 100 && !catData.IsGet[1])
        {
            can = true;
            Debug.LogError("EEE11" + catData.IsGet[1]);
        }

        if (can&&GameManager.Instance.canShowCat)
        {
            if (ShowpAGE)
            {
                GameManager.Instance.canShowCat = false;
                UIManager.Instance.ShowPopUp<CatPanel>();
            }
            
        }
        GameManager.Instance.gamePanel.IsShowCatTip(can);

    }
    public void SaveData() {

        SaveGame.Save("CatDATA", catData);

    }

    
    public void InitLevel() {

        LevelCat = 10;
        NowCatNum = 0;
    }

    public void CreatCat() {

        if (DataManager.Instance.data.UnlockLevel>1&& LevelCat>0&&catData.now[1]<=100)
        {
            for (int i = 0; i < 2; i++)
            {
                if (NowCatNum > 4) break;
                Transform temp = Pool.Instance.Spawn(Pool.Ball_PoolName, Pool.Cat);
                temp.GetComponent<CatBall>().isEliminat = false;
                temp.SetParent(GameManager.Instance.level.AllBallParent);
                temp.gameObject.SetActive(true);
                temp.position=GameManager.Instance.RomdDownPos();
                NowCatNum++;
                LevelCat--;
            }
        }
    }


    
}


public class CatData {
    public int TaskTimes;
    public int AllCat;
    public int CatMoney;
    public int CatNum;
    public System.DateTime Day;
    public bool[] IsGet;
    public int[] now;
    public int GetTimes;
    public List<TaskGet> taskGets = new List<TaskGet>();
    public CatData() {

        TaskTimes = 1;
        AllCat = 0;
        CatMoney = 0;
        CatNum = 0;
        Day = System.DateTime.Now;
        IsGet = new bool[] { false, false };
        GetTimes = 0;
        now = new int[2] { 0, 0 };
    }

}

public class CatPanel : UIBase
{
    public Button BX;
    
    public EEDataManager EE = new EEDataManager();
    public List<TaskGet> taskGets = new List<TaskGet>();
    public Button closeBtn,task1Btn,task2Btn;
    public Text moneyText,task1,task2,nowTask1,nowTask2;
    public Image pro;
    public Transform hb1, hb2, CatM1, CatM2;
    public int lastMoney;
 public  override void Awake()
    {
        base.Awake();
        EE.Load();
        taskGets = EE.GetList<TaskGet>();
        lastMoney = CatManager.Instance.catData.CatMoney;
    }

    private void Start()
    {
        BX.onClick.AddListener(() => {

            ShowPublicTip.Instance.Show("完成每日任务，集齐188元即可提现！");
        
        });
        task1Btn.onClick.AddListener(() => {
          

            if (CatManager.Instance.catData.IsGet[0])
            {
                return;
            }
        
            else if (CatManager.Instance.catData.now[0] >= 10)
            {
                if (CatManager.Instance.catData.taskGets.Count == 0)
                {
                    CatManager.Instance.catData.IsGet[0] = true;
                    var rewardRedIcon = Random.Range(50,80);
                    var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                    //打开回调
                    var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
                    popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                    {
                        if (!GameManager.Instance.isCash)
                        {
                            RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                        }
                        popup2.effect.SetActive(false);
                        Debug.Log("关闭红包二级界面");
                    },null,false);
                  



                }
                else
                {
                    CatManager.Instance.catData.IsGet[0] = true;
                    //Debug.LogError("qqq00" + CatManager.Instance.catData.IsGet[0]);
                    UmengDisMgr.Instance.CountOnNumber("jxj188_task_get", "1");
                    CatManager.Instance.catData.CatMoney += CatManager.Instance.catData.taskGets[0].GetM;
                    CatManager.Instance.catData.GetTimes++;
                    CatManager.Instance.SaveData();
                    MoneyFly.instance.Play(3, task1Btn.transform.position, BX.transform.position, () =>
                    {
                        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HbEffect2, BX.transform.position);
                    });
                }
                RefreshUI();
            }
            else {
                Hide();
            }
            CatManager.Instance.CanGet(false);
        });

        task2Btn.onClick.AddListener(() => {

            
            if (CatManager.Instance.catData.IsGet[1])
            {
                return;
            }
            else if (CatManager.Instance.catData.now[1] >= 100)
            {
                if (CatManager.Instance.catData.taskGets.Count == 0)
                {
                    CatManager.Instance.catData.IsGet[1] = true;
                    var rewardRedIcon = Random.Range(50, 80);
                    var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
                    //打开回调
                    var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();

                    popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
                    {
                        if (!GameManager.Instance.isCash)
                        {
                            RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                        }
                        popup2.effect.SetActive(false);
                        Debug.Log("关闭红包二级界面");
                    },null,false);
                    



                }
                else
                {
                    CatManager.Instance.catData.IsGet[1] = true;
                    Debug.LogError("qqq00" + CatManager.Instance.catData.IsGet[1]);
                    UmengDisMgr.Instance.CountOnNumber("jxj188_task_get", "2");

                    CatManager.Instance.catData.CatMoney += CatManager.Instance.catData.taskGets[1].GetM;
                    CatManager.Instance.catData.GetTimes++;
                    MoneyFly.instance.Play(3, task2Btn.transform.position, BX.transform.position, () =>
                    {
                        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HbEffect2, BX.transform.position);
                    });
    
                }
                RefreshUI();
                Debug.Log("eee") ;
                CatManager.Instance.SaveData();
            }
            else
            {
                Hide();
            }
            CatManager.Instance.CanGet(false);
        });

        BX.onClick.AddListener(() => { 
        

            
        });
        closeBtn.onClick.AddListener(() => {
            Hide();
        });
    }
    public override void Show()
    {
        base.Show();
        RefreshUI();
        UmengDisMgr.Instance.CountOnPeoples("jxj188_show", DataManager.Instance.data.UnlockLevel.ToString());
    }
    public void RefreshUI() {
        
        nowTask1.text = "("+(CatManager.Instance.catData.now[0]>=10?10: CatManager.Instance.catData.now[0])+ "/10)";
        nowTask2.text = "(" + (CatManager.Instance.catData.now[1]>=100?100: CatManager.Instance.catData.now[1]) + "/100)";
        hb1.gameObject.SetActive(false);
        hb2.gameObject.SetActive(false);
        CatM1.gameObject.SetActive(false);
        CatM2.gameObject.SetActive(false);
        var p = (float)CatManager.Instance.catData.CatMoney / 188;
        moneyText.text = string.Format("{0}/{1}",CatManager.Instance.catData.CatMoney,188);
        pro.fillAmount = lastMoney / 188;
        pro.DOFillAmount(p,0.5F);
        lastMoney = CatManager.Instance.catData.CatMoney;
        if (CatManager.Instance.catData.taskGets.Count >= 2)
        {
            task1.text = CatManager.Instance.catData.taskGets[0].GetM + "元";
            task2.text = CatManager.Instance.catData.taskGets[1].GetM + "元";
            CatM1.gameObject.SetActive(true);
            CatM2.gameObject.SetActive(true);
        }
        else if (CatManager.Instance.catData.taskGets.Count == 1)
        {
            task1.text = CatManager.Instance.catData.taskGets[0].GetM + "元";
            CatM1.gameObject.SetActive(true);
            hb2.gameObject.SetActive(true);
        }
        else {
            hb1.gameObject.SetActive(true);
            hb2.gameObject.SetActive(true);
        }
        SetBtn(task1Btn.transform,0);
        SetBtn(task2Btn.transform,1);
    }


    public void SetBtn(Transform btn,int index) {

        for (int i = 0; i < btn.childCount; i++)
        {
            btn.GetChild(i).gameObject.SetActive(false);
        }
        if (CatManager.Instance.catData.IsGet[index])
        {
            btn.GetChild(2).gameObject.SetActive(true);
        }
        else if (index ==0)
        {
            if (CatManager.Instance.catData.now[0] >= 10)
            {
                btn.GetChild(0).gameObject.SetActive(true);
                UmengDisMgr.Instance.CountOnNumber("jxj188_task_arrive","1");
            }
            
            else {
                btn.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (index == 1)
        {
            if (CatManager.Instance.catData.now[1] >= 100)
            {
                btn.GetChild(0).gameObject.SetActive(true);
                UmengDisMgr.Instance.CountOnNumber("jxj188_task_arrive", "2");

            }
            else
            {
                btn.GetChild(1).gameObject.SetActive(true);
            }
        }
    }


}
