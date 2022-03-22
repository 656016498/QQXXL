using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
public class PassPop : UIBase
{
    public Sprite[] passImg;
    public IButton closeBtn;
    public IButton continueBtn;
    public Button starBtn;
    public Button cstarBtn;
    public Transform starParent;
    public Transform newInterface;
    public Transform starFull;
    public Transform challengeFull;

    public Transform underTran;
    public Text levelText;
    public Text scoreText;
    public Text diamondText;
    public Text starText;
    public Text challengeText;



    public Image rewardImg;
    public Image starImg;
    public Image challengeImg;
    public Vector2 orginVct2;
    public Vector2 endVct2;
    public Transform[] allShowStar;
    public Text sore2Text;
    public Text sore3Text;
    public Text bxText;

    private void Start()
    {
        continueBtn.onClick.AddListener(() => {

           
            //UIManager.Instance.Show<MainPanel>(UIType.Normal);
            var can = target - GameManager.Instance.StarShineStarSub.Value;
            if (can == 0)
            {
                
                UIManager.Instance.Show<TreasurePop>(UIType.PopUp,TreasureType.Starlight);
                UIManager.Instance.GetBase<TreasurePop>().RefrehData(0,addText);
                UIManager.Instance.GetBase<TreasurePop>().OpenFunTow();
                Hide();
            }
            else
            {
                CloseFun();
            }

        });
        closeBtn.onClick.AddListener(CloseFun);
        

        starBtn.onClick.AddListener(() => {
            if (DataManager.Instance.data.StarshineStar >= 10)
            {
                //TODO 打开宝箱
                UIManager.Instance.Show<TreasurePop>(UIType.PopUp, TreasureType.Starlight);
            }
            else {
                //TODO 点击弹出提示1"条件不满足，请继续闯关开启宝箱"

            }

        });
        cstarBtn.onClick.AddListener(() => {
            if (DataManager.Instance.data.ChallengeStar >= 10)
            {
                //TODO 打开宝箱
                UIManager.Instance.Show<TreasurePop>(UIType.PopUp, TreasureType.Challenge);

            }
            else
            {
                //TODO 点击弹出提示1"条件不满足，请继续闯关开启宝箱"

            }



        });
    }

    public override void Show()
    {
        base.Show();
       
        GamePassAddData();
        ReduceUI();
        InitPos();
        InitPage();
        //显示banner
        GameADControl.Instance.Banner(true);
    }

    /// <summary>
    /// 游戏通关数据添加
    /// </summary>
    public void GamePassAddData() {

        if (DataManager.Instance.data.NeedGudieMap.Contains(GameManager.Instance.CurrentLevel))
        {
            DataManager.Instance.data.NeedGudieMap.Remove(GameManager.Instance.CurrentLevel);
        }

        var nowlevleStar = UIManager.Instance.GetBase<GamePanel>().nowLevelGetStar;
        //GameManager.Instance.StarShineStarSub.Value += nowlevleStar > 3 ? 3 : nowlevleStar;
        GameManager.Instance.StarShineStarSub.Value += 1;
        DataManager.Instance.data.levelStar[GameManager.Instance.CurrentLevel - 1]+= nowlevleStar > 3 ? 3 : nowlevleStar;
        //if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
        //{
            GameManager.Instance.SendLevel(2);
            //dadian
            UmengDisMgr.Instance.CountOnPeoples("level_win", DataManager.Instance.data.UnlockLevel.ToString());
            UmengDisMgr.Instance.CountOnPeoples("level_win_jsy", DataManager.Instance.data.UnlockLevel.ToString());

            if (nowlevleStar == 3)
            {
                UmengDisMgr.Instance.CountOnPeoples("level_win_mx", DataManager.Instance.data.UnlockLevel.ToString());
            }

    
            SevenWithdrawDataMgr.Instance.AddTaskData(1,1);
            DataManager.Instance.data.ChallengeStar++;
            DataManager.Instance.data.UnlockLevel++;
            DataManager.Instance.data.levelStar.Add(0);
            OrderSystemControl.Instance.AddOrderLevel(1);

        //}
        DataManager.Instance.SaveGameData();
    }


    public void CloseFun() {

        GameManager.Instance.DestoryLevel();
        Hide();

#if BB108

        UIManager.Instance.Hide<GamePanel>();
        UIManager.Instance.Show<MainPanel>(UIType.Normal);
        InfiniteScrollView.Instance.RefreshLevelMap();
        InfiniteScrollView.Instance.SetShowOut();

        if (!GuideMgr.Instance.GuideIsComplete(10))
        {
            GuideMgr.Instance.ShowGuide_10(() =>
            {
                XDebug.Log("展示第十步");
            });
        }
#else
        GameManager.Instance.LoadLevel();
#endif

    }
    public override void Hide()
    {
        //关闭banner
        GameADControl.Instance.Banner(false);
        base.Hide();
    }

    /// <summary>
    /// 初始化位置 
    /// </summary>
    public void InitPos() {

        underTran.transform.GetComponent<RectTransform>().anchoredPosition = orginVct2;
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition=new Vector2 (0,Screen.height);
        AudioMgr.Instance.PlaySFX("转场");
    }

    /// <summary>
    /// 还原界面
    /// </summary>
    public void ReduceUI() {

        newInterface.gameObject.SetActive(false);
        newInterface.localScale = Vector3.one * 2;
        for (int i = 0; i < starParent.childCount; i++)
        {
            starParent.GetChild(i).GetChild(2).localEulerAngles = new Vector3(0, 0, 180);
            starParent.GetChild(i).GetComponent<Image>().enabled = true;
            starParent.GetChild(i).GetChild(0).localScale = Vector3.zero;
            starParent.GetChild(i).GetChild(1).localScale = Vector3.zero;
            starParent.GetChild(i).GetChild(2).localScale = Vector3.zero;
            starParent.GetChild(i).Find("shade").gameObject.SetActive(false);
        }

        foreach (var item in allShowStar)
        {
            item.gameObject.SetActive(false);
        }
    }

    int target;
    float addText;
        
    bool CanShowTreasure = false;
    public void InitPage()
    {

      
        sore2Text.text = string.Format("{0}分以上:" , TableMgr.Instance.GetScore(GameManager.Instance.CurrentLevel).twostar);
        sore3Text.text = string.Format("{0}分以上:" ,TableMgr.Instance.GetScore(GameManager.Instance.CurrentLevel).threestar);
        levelText.text = string.Format("第{0}关", GameManager.Instance.CurrentLevel);
        scoreText.text = GameManager.Instance.LevelScore.ToString();
        RefreshPage();
        //starText.text = DataManager.Instance.data.StarshineStar >= 10 ? "点击开启" : DataManager.Instance.data.StarshineStar + "/" + 10;
        //challengeText.text = DataManager.Instance.data.ChallengeStar >= 10 ? "点击开启" : DataManager.Instance.data.ChallengeStar + "/" + 10;
        //starImg.fillAmount = (float)DataManager.Instance.data.StarshineStar / 10;
        //challengeImg.fillAmount = (float)DataManager.Instance.data.ChallengeStar / 10;
        //if (challengeImg.fillAmount >= 1)
        //{
        //    challengeFull.eulerAngles = new Vector3(0, 0, -5);
        //    challengeFull.GetComponent<DOTweenAnimation>().DOPlay();
        //    challengeFull.GetChild(0).gameObject.SetActive(true);
        //}
        //else {
        //    challengeFull.eulerAngles = new Vector3(0, 0, 0);
        //    challengeFull.GetComponent<DOTweenAnimation>().DOPause();
        //    challengeFull.GetChild(0).gameObject.SetActive(false);
        //}

        //if (starImg.fillAmount >= 1)
        //{
        //    starFull.eulerAngles = new Vector3(0, 0, -5);
        //    starFull.GetComponent<DOTweenAnimation>().DOPlay();
        //    starFull.GetChild(0).gameObject.SetActive(true);
        //}
        //else {

        //    starFull.eulerAngles = new Vector3(0, 0, 0);
        //    starFull.GetComponent<DOTweenAnimation>().DOPause();
        //    starFull.GetChild(0).gameObject.SetActive(true);
        //}
                        
        //钻石显示
        //diamondText.text = "x" + TableMgr.Instance.GetDiamondNum(GameManager.Instance.CurrentLevel < DataManager.Instance.data.UnlockLevel);
        //DataManager.Instance.data.Diamond +=TableMgr.Instance.GetDiamondNum(GameManager.Instance.CurrentLevel < DataManager.Instance.data.UnlockLevel);

        //随机获取道具
        int rangIndex = RandomByWeight.RomdIndex(new int[3] {5,3,1});
        XDebug.LogError("获取道具下标"+ rangIndex);
        string propImgName = null;
        string propAddNum = "+1";
        switch (rangIndex)
        {
            case 0:
                //propAddNum = "+5";
                DataManager.Instance.data.BombProp[0]+=1; propImgName = "Red1";
                break;
            case 1:
                //propAddNum = "+3";
                DataManager.Instance.data.BombProp[1] += 1; propImgName = "Red2"; break;
            case 2:
                //propAddNum = "+1";
                DataManager.Instance.data.BombProp[2] += 1; propImgName = "Red3"; break;
            default:
                break;
        }
        diamondText.text = propAddNum;
        rewardImg.SetImage(Resources.Load<Sprite>("PropSprite/" + propImgName));


        //for (int i = 0; i < allShowStar.Length; i++)
        //{
        //    if (DataManager.Instance.data.levelStar[DataManager.Instance.data.UnlockLevel-1] == i)
        //    {
        //Debug.LogError(DataManager.Instance.data.levelStar[GameManager.Instance.CurrentLevel - 1]+"//");
        //if (DataManager.Instance.data.levelStar[GameManager.Instance.CurrentLevel - 1] - 1>0)
        //{
            allShowStar[DataManager.Instance.data.levelStar[GameManager.Instance.CurrentLevel - 1] - 1].gameObject.SetActive(true);
        //}
        //    }
        //}



        transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(0, 1).SetEase(Ease.InOutBack).OnComplete(() => {
        underTran.GetComponent<RectTransform>().DOAnchorPos(endVct2, 0).SetEase(Ease.InOutBack).OnComplete(()=> {
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0)).Subscribe(_ =>
            {
                for (int i = 0; i < 3; i++)
                {
                    int P = i;
                    if (DataManager.Instance.data.levelStar[GameManager.Instance.CurrentLevel - 1] > i)
                    {
                        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5f * P)).Subscribe(j =>
                        {
                            starParent.GetChild(P).GetChild(2).localScale = Vector3.one * 3f;
                            starParent.GetChild(P).GetChild(2).GetComponent<Image>().DOFade(0.1F,0);
                            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.7f)).Subscribe(_1 => {
                                Pool.Instance.SpawnEffectByParent(Pool.Effect_PoolName, Pool.StarDown, starParent.GetChild(P).transform, Vector3.zero, 180);
                                if (canvasGroup.alpha == 1)
                                {
                                    AudioMgr.Instance.PlaySFX("星星到位");
                                }
                            });
                            starParent.GetChild(P).GetChild(2).GetComponent<Image>().DOFade(1, 0.75f);
                            starParent.GetChild(P).GetChild(2).DOLocalRotate(Vector3.zero,0.75F,RotateMode.FastBeyond360).SetDelay(0.2F).SetEase(Ease.Linear);
                            starParent.GetChild(P).GetChild(2).DOScale(Vector3.one, 1).SetEase(Ease.InOutBack).OnComplete(()=> {
                                starParent.GetChild(P).GetChild(0).localScale = Vector3.one * 180;
                                starParent.GetChild(P).GetChild(1).localScale = Vector3.one * 133;
                                starParent.GetChild(P).Find("shade").gameObject.SetActive(true);
                            });
                        });
                    }
                    //新纪录
                    //if (GameManager.Instance.LevelScore.Value > DataManager.Instance.data.bestScore)
                    //{
                    //    AudioMgr.Instance.PlaySFX("新纪录");
                    //    newInterface.gameObject.SetActive(true);
                    //    DataManager.Instance.data.bestScore = GameManager.Instance.LevelScore.Value;
                    //    newInterface.DOScale(Vector3.one, 1).SetEase(Ease.InOutBack);
                    //}
                }
            });
        });
        });
        //bxText.text = string.Format("再过{0}关即可打开宝箱！", DataManager.Instance.GetTargetBox() - GameManager.Instance.StarShineStarSub.Value);


        var can = DataManager.Instance.GetTargetBox() - GameManager.Instance.StarShineStarSub.Value;
        if (can == 0)
        {
            addText = LargeCashDataControl.Instance.GetLargeCoin();
            target = DataManager.Instance.GetTargetBox();
            if (DataManager.Instance.data.BXLevek.Count > 0)
            {
                DataManager.Instance.data.BXLevek.RemoveAt(0);
            }
            DataManager.Instance.data.StarshineStar = 0;
            continueBtn.transform.GetChild(0).GetComponent<Image>().sprite = passImg[1];
            continueBtn.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
            DataManager.Instance.SaveGameData();
        }
        else
        {
            target = DataManager.Instance.GetTargetBox();
            CanShowTreasure = false;
            continueBtn.transform.GetChild(0).GetComponent<Image>().sprite = passImg[0];
            continueBtn.transform.GetChild(0).GetComponent<Image>().SetNativeSize();

        }
    }

    public void RefreshPage() {

        //starText.text = DataManager.Instance.data.StarshineStar >= 10 ? "点击开启" : DataManager.Instance.data.StarshineStar + "/" + 10;
        //starImg.fillAmount = (float)DataManager.Instance.data.StarshineStar / 10;
        //if (starImg.fillAmount >= 1)
        //{
        //    starFull.eulerAngles = new Vector3(0, 0, -5);
        //    starFull.GetComponent<DOTweenAnimation>().DOPlay();
        //    starFull.GetChild(0).gameObject.SetActive(true);
        //}
        //else
        //{

        //    starFull.eulerAngles = new Vector3(0, 0, 0);
        //    starFull.GetComponent<DOTweenAnimation>().DOPause();
        //    starFull.GetChild(0).gameObject.SetActive(true);
        //}

        //挑战宝箱
        //challengeText.text = DataManager.Instance.data.ChallengeStar >= 10 ? "点击开启" : DataManager.Instance.data.ChallengeStar + "/" + 10;
        //challengeImg.fillAmount = (float)DataManager.Instance.data.ChallengeStar / 10;
        //if (challengeImg.fillAmount >= 1)
        //{
        //    challengeFull.eulerAngles = new Vector3(0, 0, -5);
        //    challengeFull.GetComponent<DOTweenAnimation>().DOPlay();
        //    challengeFull.GetChild(0).gameObject.SetActive(true);
        //}
        //else
        //{
        //    challengeFull.eulerAngles = new Vector3(0, 0, 0);
        //    challengeFull.GetComponent<DOTweenAnimation>().DOPause();
        //    challengeFull.GetChild(0).gameObject.SetActive(false);
        //}


    }
    public override void Refresh()
    {
        base.Refresh();
        RefreshPage();
    }
}
