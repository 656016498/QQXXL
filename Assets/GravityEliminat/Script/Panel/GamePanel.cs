using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using Spine.Unity;
using TMPro;

public class GamePanel : UIBase
{
    public Transform HF;
    public Button CatBtn;
    public SkeletonGraphic skeleton;
    public Text pigText;
    public Button qaBtn;
    public MagicFly magicFly;
    public Image EnergImg;
    public IButton pauseBtn;
    public IButton addDiamond;
    public Transform EnergyPro;//蓄能进度条
    public Transform propDirTran;
    public int inputPos;
    public Transform topParent;
    public Transform bottomParent;
    public Transform PassParent;
    public Transform startPop;
    public Transform startTraget;
    public Text RemainingStepsText;
    public Text levelText;
    public IButton stepBtn;//添加步数
    public IButton bombBtn;//流行爆炸
    public IButton refreshBtn;//刷新位置
    public Button energybtn;//蓄能按钮
    public Transform mask;
    public Text cashText;
    public Text diamondText;
    public IButton cashBtn;
    public IButton diamondBtn;
    public Treasure treasureUI;
    public Image SorePro;
    public Image SorePro2;
    public Transform scoreFull;
    public Transform[] threeStar;
    public bool[] canStar=new bool[3] { false,false,false};
    public Text stepText;
    public Text bombText;
    public Text refreshText;
    public Vector2 topAnimPos;
    public Vector2 bottomAnimPos;
    public Button maskBtn;
    public Button nextBtn;
    public Button RewardSkip;
    public Transform funTime;
    [Header("在线红包")]
    public Button onLineHb;
    [Header("存钱罐")]
    public Button pigBtn;
    [Header("飞行红包")]
    bool SHOW = false;
    [Header("顶部通关条件")]
    public Transform topTip;
    public IButton largeCashBtn;//大额提现
    //public Button NextBtn;
    //public 
    // Start is called before the first frame update
    public bool StartJoin = false;
    float ScreenFitPos =0;
    public int nowLevelGetStar;

    Transform choosePorpEffect;
    private void Awake()
    {
        base.Awake();
        if ((Screen.height / Screen.width) >= 2)
        {
            ScreenFitPos = 50;
        }
        else
        {
            ScreenFitPos = 0;
        }

        pigBtn.transform.localPosition = new Vector2(pigBtn.transform.localPosition.x, pigBtn.transform.localPosition.y- ScreenFitPos);
        CatBtn.transform.localPosition = new Vector2(CatBtn.transform.localPosition.x, CatBtn.transform.localPosition.y - ScreenFitPos);
        UmengDisMgr.Instance.CountOnPeoples("FirstEnterLoadingsuccess");

    }

    public void IsShowCatTip(bool can) {


        CatBtn.transform.Find("tip").gameObject.SetActive(can);


    }
  
    void Start()
    {

        CatBtn.onClick.AddListener(() => {
            UIManager.Instance.ShowPopUp<CatPanel>();
        });
#if !BB108
        if (DataManager.Instance.data.FristJoinGame)
        {
            DataManager.Instance.data.FristJoinGame = false;
            StartPopJoin();
        }
        else
        {
            UIRoot.Instance.HideMask();
            var pigPanel = UIManager.Instance.ShowPopUp<PiggyBankUI>();
            pigPanel.OnRefresh();
            pigPanel.callBack = () => {
                UIRoot.Instance.ShowMask();
                StartPopJoin();
            };
        }
#endif
        GameManager.Instance.EliminatPower.Value = 0;
        qaBtn.onClick.AddListener(() => {
            UIManager.Instance.ShowPopUp<GameQAPanel>();
        });
        AudioMgr.Instance.PlayMusic("战斗界面BGM");
        if (!GuideMgr.Instance.GuideIsComplete(1))
        {
            GuideMgr.Instance.ShowGuide_1(() =>
            {
                XDebug.Log("开始第三步~");
                //GuideMgr.Instance.ShowGuide_3(() =>
                //{
                //    XDebug.Log("开始第四步~");
                //});
            });
        }

        choosePorpEffect = Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.ChoosePorpBtn,Vector3.zero);
        choosePorpEffect.gameObject.SetActive(false);
        RewardSkip.onClick.AddListener(() => {
            RewardSkip.gameObject.SetActive(false);
            GameManager.Instance.SkipRewardTime();
        });
       
        addDiamond.onClick.AddListener(() =>
        {
            UIManager.Instance.Show<AddPop>(UIType.PopUp, AddEumn.Diamond);
        });
        GameManager.Instance.DiamondSub.Subscribe(_ =>
        {
            diamondText.text = _.ToString();

        });
        pauseBtn.onClick.AddListener(() =>
        {

            UIManager.Instance.Show<InGamePause>(UIType.PopUp);

        });
        EventManager.Instance.AddEvent(MEventType.PassConditon, RefrehPass);

        GameManager.Instance.EliminatPower.Subscribe(_ =>
        {
            if (DataManager.Instance.data.UnlockLevel < 6) return;
            if (SHOW||GameManager.Instance.OverGame) return;
            EnergyPro.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Lerp(-54.3F, 65, (float)_ / 300), 1).SetEase(Ease.InOutBack).SetDelay(0.8f);
            EnergImg.DOFillAmount((float)_ / 300, 1).SetEase(Ease.InOutBack).SetDelay(0.5f).OnComplete(()=>{

                EnergImg.transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
                //EnergImg.transform.parent.DOBlendableScaleBy(Vector3.one* 0.2F,0.5F).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBack);
                //EnergImg.GetComponent<Animator>().
                //EnergImg.transform.parent.GetComponent<Animator>().SetTrigger("pop");
                EnergImg.transform.parent.DOScale(Vector3.one * 0.85F, 0.1F).SetEase(Ease.Linear).OnComplete(() =>
                {
                    EnergImg.transform.parent.DOScale(Vector3.one * 1.1f, 0.2F).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        EnergImg.transform.parent.DOScale(Vector3.one*0.9F, 0.2F).SetEase(Ease.Linear).OnComplete(()=> {

                            EnergImg.transform.parent.DOScale(Vector3.one, 0.07F).SetEase(Ease.Linear);

                        });
                    });
                });

            });
            if (_ >= 300)
            {
                GameManager.Instance.EliminatPower.Value = 0;
                SHOW = true;
                EnergImg.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 200);
                //Pool.Instance.SpawnEffectByPos(Pool.Effect_PoolName, Pool.BottleBomb, energybtn.transform.position, 10);
                //Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.YaoSuiBoom, energybtn.transform.position, 2);
                Transform MagicBottle = Pool.Instance.Spawn(Pool.PoolName_UI, "MagicUI");
                //MagicBottle.localScale = Vector3.one;
                //MagicBottle.position = EnergImg.transform.position;
                //MagicBottle.GetComponent<MagicBottle>().Init();
                //MagicBottle.SetParent(GameManager.Instance.allBallParent);
                Vector3 target = GameManager.Instance.RomdRangBallPos();
                DynamicMgr.Instance.FlyEffectCurveSetParent(energybtn.transform.position, target, MagicBottle, 0, 1, 0.5F, -20, 
                    () =>{

                        //Debug.LogError("魔法瓶回调");
                        //MagicBottle.gameObject.layer =      LayerMask.NameToLayer("Default");
                        //MagicBottle.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
                        //MagicBottle.GetComponent<CircleCollider2D>().enabled = true;
                        //MagicBottle.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Ball");
                        //MagicBottle.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        //MagicBottle.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Ball");
                        //MagicBottle.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                        Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.PropSpawn, MagicBottle.transform.position);
                        Pool.Instance.Despawn(Pool.PoolName_UI, MagicBottle);
                        Transform Magic = Pool.Instance.Spawn(Pool.Prop_PoolName, Pool.MagicBottle);
                        Magic.localScale = Vector3.one*1.2F;
                        Magic.transform.position = target;
                        Magic.GetComponent<MagicBottle>().Init();
                        Magic.SetParent(GameManager.Instance.allBallParent);
                        GameManager.Instance.magicBottles.Add(Magic.transform.GetComponent<MagicBottle>());
                    });


                //Observable.TimeInterval(System.TimeSpan.FromSeconds(1.01F)).Subscribe(_1 =>
                //{
                //    MagicBottle.GetComponent<CircleCollider2D>().enabled = true;
                //    MagicBottle.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Ball");
                //    MagicBottle.GetComponent<SpriteRenderer>().sortingOrder = 2;
                //    MagicBottle.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Ball");
                //    MagicBottle.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                //    Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.PropSpawn, MagicBottle.transform.position);
                //});

                Observable.TimeInterval(System.TimeSpan.FromSeconds(1.2F)).Subscribe(_1 =>
                {
                    EnergImg.transform.parent.gameObject.SetActive(true);
                    SHOW = false;
                    AudioMgr.Instance.PlaySFX("瓶子飞");
                });
                EnergImg.transform.parent.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 1).SetDelay(1.2F).SetEase(Ease.InCirc).SetDelay(0.5F);


                //AudioMgr.Instance.PlaySFX("蓄力技能满时");
                //EnergyPro.transform.GetChild(0).gameObject.SetActive(false);
                //EnergImg.transform.parent.GetComponent<Animator>().SetBool("isFull", true);
                //Pool.Instance.SpawnEffectByPos(Pool.Effect_PoolName, Pool.EnergyStorage, energybtn.transform.position);
                //EnergImg.transform.parent.Find("effect").gameObject.SetActive(true);
            }
            if (_ == 0)
            {
                //EnergyPro.transform.GetChild(0).gameObject.SetActive(true);
                //EnergImg.transform.parent.GetComponent<Animator>().SetBool("isFull", false);
                //EnergImg.transform.parent.Find("effect").gameObject.SetActive(false);
            }
            
        });
        GameManager.Instance.LevelScore.Subscribe(vaule =>
        {
            if (vaule == 0)
            {
                SorePro.fillAmount = 0;
            }
            else
            {
                SorePro.fillAmount = TableMgr.Instance.GetStarPro((int)(vaule));
            }
            if (vaule >= TableMgr.Instance.star.onestar && !canStar[0])
            {
                AddNowLevelStar(0);
            }
            if (vaule >= TableMgr.Instance.star.twostar && !canStar[1])
            {
                AddNowLevelStar(1);
            }
            if (vaule >= TableMgr.Instance.star.threestar && !canStar[2])
            {
                AddNowLevelStar(2);
            }
            SorePro2.fillAmount = SorePro.fillAmount;
            if (SorePro.fillAmount >= 0.66f)
            {
                scoreFull.gameObject.SetActive(true);
            }
        });
        cashBtn.onClick.AddListener(() =>
        {
            //TODO 提现界面
            var mpanel = UIManager.Instance.ShowPopUp<WithdrawUI>();
            mpanel.UpdateUI();

        });
        diamondBtn.onClick.AddListener(() =>
        {
            //TODO 添加钻石界面
            UIManager.Instance.Show<AddPop>(UIType.PopUp, AddEumn.Diamond);

        });
        //energybtn.onClick.AddListener(() =>
        //{

            //if (GameManager.Instance.EliminatPower.Value >= 100)
            //{
            //    //SHOW = false;

            //    GameManager.Instance.EliminatPower.Value = 0;
            //    EnergImg.transform.parent.GetComponent<Animator>().enabled = false;
            //    EnergImg.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 200);
            //    Pool.Instance.SpawnEffectByPos(Pool.Effect_PoolName, Pool.BottleBomb, energybtn.transform.position, 10);
            //    Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.YaoSuiBoom, energybtn.transform.position, 2);

            //    Observable.Timer(System.TimeSpan.FromSeconds(1.2F)).Subscribe(_ =>
            //    {
            //        EnergImg.transform.parent.gameObject.SetActive(true);
            //        AudioMgr.Instance.PlaySFX("瓶子飞");
            //    });


            //    EnergImg.transform.parent.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 1).SetEase(Ease.InCirc).OnComplete(() =>
            //    {

            //        EnergImg.transform.parent.GetComponent<Animator>().enabled = true;

            //    });
            //    if (GameManager.Instance.level.propModel)
            //    {

            //    }
            //    else
            //    {
            //        GameManager.Instance.EMostOfSameColor();
            //    }
            //}
        //});
        nextBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.DestoryLevel();
            //DataManager.Instance.data.CurrentLevel++;
            if (GameManager.Instance.CurrentLevel > 4)
            {
                GameManager.Instance.CurrentLevel = 1;
            }
            //GameManager.Instance.LoadLevel();
            Hide();
            UIManager.Instance.Show<MainPanel>(UIType.Normal);
            nextBtn.gameObject.SetActive(false);
        });
        stepBtn.onClick.AddListener(AddStepUI);
        bombBtn.onClick.AddListener(LiuxingBombUI);
        refreshBtn.onClick.AddListener(RefrehAllColor);
        GameManager.Instance.RemainingSteps.Subscribe(_ =>
        {
            if (_ <= 0)
            {
                if (!GameManager.Instance.OverGame)
                {
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(1.2f)).Subscribe(X =>
                {
                    if (!GameManager.Instance.OverGame)
                    {
                        UIRoot.Instance.HideMask();
                        UIManager.Instance.Show<NoStepPop>(UIType.PopUp);
                    }
                });
                }
            }
           
            RemainingStepsText.text = _.ToString();
        });

        //在线红包点击
#region
        //onLineHb.onClick.AddListener(() => {
        //    var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
        //    var rewardVoucher = RedWithdrawData.Instance.GetVoucherPro();
        //    var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
        //    popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
        //    {
        //        popup2.effect.SetActive(false);
        //        Debug.Log("关闭红包二级界面");
        //        RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
        //    });
        //});
#endregion
        pigBtn.onClick.AddListener(() => {
            var mpanel = UIManager.Instance.ShowPopUp<PiggyBankUI>();
            mpanel.OnRefresh();
        });

        RedWithdrawData.Instance.RedCoin.Subscribe(value => {
            cashText.text = string.Format("{0}<color=23>元</color>", (value / 10000).ToString("f2"));
        });

        largeCashBtn.onClick.AddListener(() => {
            BigRedTest();
        });

        LargeCashDataControl.Instance.largeCash.Subscribe(value => {

            largeCashBtn.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}<size=20>元</size>", value.ToString("f2"));
        });
    }

    public void HFFly() {

        HF.GetComponentInChildren<TMP_Text>().text = string.Format("<sprite={0}>", DataManager.Instance.GetTargetBox()-GameManager.Instance.StarShineStarSub.Value);
        HF.transform.localPosition = new Vector3(Screen.width/2 +300, 349, 0);
        AudioMgr.Instance.PlaySFX("咻");
        Observable.TimeInterval(System.TimeSpan.FromSeconds(2.5f)).Subscribe(_ => {

            AudioMgr.Instance.PlaySFX("咻");

        });
        Sequence ransequence = DOTween.Sequence();
        ransequence.Append(HF.DOLocalMoveX(0, 0.5f)).AppendInterval(2).Append(HF.DOLocalMoveX(-Screen.width / 2 - 300, 0.5f));
    }

    //展示大额红包
    public void BigRedTest()
    {
        var mConfig = LargeCashDataControl.Instance.largeCashConfig;
        var mAddValue = mConfig.AD;
        //LargeCashDataControl.Instance.AddtoTotal(mAddValue);
        var mControl = LargeCashDataControl.Instance;
        var mPanel2 = UIManager.Instance.ShowPopUp<LargeCashTwoPanel>();
        mPanel2.RefrishUi(mAddValue, mControl.mData.totalNum, mControl.LastMoney);
        mPanel2.OnOpen(LargeHbType.Weichat,
            () => {

            },
            () => {
                ShowPublicTip.Instance.Show("红包满200元才可以提现哦");
            },
            () => {

            });
     
    }
    public void AddNowLevelStar(int index) {

        canStar[index] = true;
        //GameManager.Instance.StarShineStarSub.Value++;
        nowLevelGetStar++;
        //if (DataManager.Instance.data.levelStar[DataManager.Instance.data.CurrentLevel - 1] < 3)
        //{
        //    DataManager.Instance.data.levelStar[DataManager.Instance.data.CurrentLevel - 1]++;
        //}
        threeStar[0].parent.GetComponent<Image>().enabled = false;
        DynamicMgr.Instance.StarAnim(threeStar[index]);
    }


    public void AddStepUI()
    {
       
        if (DataManager.Instance.data.addStepN == 0)
        {
            //TODO 展示补充步数界面
            BuyData buyData = new BuyData(BuyType.Step,BuyWay.Video, "dj_bs_video", stepBtn.transform.GetChild(0).GetComponent<Image>().sprite);
            UIManager.Instance.Show<BuyPop>(UIType.PopUp,buyData);
        }
        else
        {

            if (inputPos == 1 || inputPos == 2)
            {
                ExitPropDir();
            }
            stepBtn.transform.GetChild(0).localScale = Vector3.zero;
            UIRoot.Instance.ShowMask();
            DynamicMgr.Instance.FlyImage(stepBtn.transform.GetChild(0).GetComponent<Image>().sprite, stepBtn.transform.GetChild(0).position, RemainingStepsText.transform.position, transform, () =>
            {

                for (int i = 0; i < 10; i++)
                {
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(i * 0.2f)).Subscribe(_ =>
                    {
                        GameManager.Instance.RemainingSteps.Value += 1;
                    });
                }
                DataManager.Instance.data.addStepN--;
                RefreshBottomUI();
                AudioMgr.Instance.PlaySFX("步数+5到达");
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.StepEffect2, RemainingStepsText.transform.position);
                stepBtn.transform.GetChild(0).DOScale(Vector3.one, 0.5F).SetEase(Ease.InOutBack).OnComplete(() =>
                {

                    UIRoot.Instance.HideMask();

                });

            });


            //DynamicMgr.Instance.FlyEffectLine(stepBtn.transform.position, RemainingStepsText.transform.position,Pool.AddStepEffect,()=> {



            //});

        }

    }

    /// <summary>
    /// 流行轰炸
    /// </summary>
    public void LiuxingBombUI() {

        
        if (DataManager.Instance.data.addBombN == 0)
            {
            //TODO 展示补充界面
            BuyData buyData = new BuyData(BuyType.Meteor, BuyWay.Video, "dj_bz_video", bombBtn.transform.GetChild(0).GetComponent<Image>().sprite);
            UIManager.Instance.Show<BuyPop>(UIType.PopUp, buyData);
        }
            else
        {
            if (inputPos == 1 && maskBtn.gameObject.activeSelf)
            {
                GameManager.Instance.IsMeteorBomb = false;
                ExitPropDir();
            }
            else
            {

                inputPos = 1;
                TopExitAnim();
                //Pool.Instance.SpawnEffect();
                //GameManager.Instance.IsMeteorBomb = true;
                ShowChoosePropEffect(bombBtn.transform);
                ShowPropDir("流星雨", "点击一个物体,召唤流星轰炸区域", bombBtn.transform.GetChild(0).GetComponent<Image>().sprite, () =>
                {
                    if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
                    {
                        UmengDisMgr.Instance.CountOnNumber("dj_xx_use", DataManager.Instance.data.UnlockLevel.ToString());
                    }
                    choosePorpEffect.SetParent(bombBtn.transform);

                    //if (IsMeteorBomb && gamePanel.inputPos == 1)
                    AudioMgr.Instance.PlaySFX("流行轰炸");
                    UIRoot.Instance.ShowMask();
                    //AudioMgr.Instance.PlaySFX("步数流星");
                    Debug.Log("流行轰炸");
                    Vector3 orginNow = Input.mousePosition;
                    Vector3 orgin = UIManager.Instance.uiCamera.ScreenToWorldPoint(Input.mousePosition);

                    for (int i = 0; i < 3; i++)
                    {
                        int P = i;
                        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2F * P)).Subscribe(_ => {
                            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.LiuXinBomb, orgin /*+ new Vector3(0, 0.4F, 0)*/);
                        });
                    }

                    Observable.TimeInterval(System.TimeSpan.FromSeconds(1.7F)).Subscribe(_ => {
                        CameraManager.Instance.isshakeCamera = false;
                        DataManager.Instance.data.addBombN--;
                        orgin=   Camera.main.ScreenToWorldPoint(orginNow);
                        GameManager.Instance.MeteorBombing(orgin);
                        ExitPropDir();
                        RefreshBottomUI();
                        PropManger.Instance.BeginOnCilck();
                        UIRoot.Instance.HideMask();
                    });
                    //return;
                    //}
                    //DataManager.Instance.data.addRefreshN--;
                    //GameManager.Instance.AdjacentPos();
                    ExitPropDir();

                });

            }

        }



    }


    /// <summary>
    /// 刷新的方法
    /// </summary>
    public void RefrehAllColor() {
       
#if !GameTest
        if (DataManager.Instance.data.addRefreshN == 0)
            {
            //TODO 展示补充揭密胺
            BuyData buyData = new BuyData(BuyType.Refresh, BuyWay.Video, "dj_fl_video", refreshBtn.transform.GetChild(0).GetComponent<Image>().sprite);
            UIManager.Instance.Show<BuyPop>(UIType.PopUp, buyData);
        }
            else
            {
#endif
            if (inputPos == 2 && maskBtn.gameObject.activeSelf)
            {
                ExitPropDir();
            }
            else
            {
                inputPos = 2;
                TopExitAnim();
                ShowChoosePropEffect(refreshBtn.transform);
                ShowPropDir("一网打尽", "点击任意位置,将糖果按颜色分类", refreshBtn.transform.GetChild(0).GetComponent<Image>().sprite, () =>
            {
                if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
                {
                    UmengDisMgr.Instance.CountOnNumber("dj_fl_use", DataManager.Instance.data.UnlockLevel.ToString());
                }
                AudioMgr.Instance.PlaySFX("道具分类");
                UIRoot.Instance.ShowMask();
                //AudioMgr.Instance.PlaySFX("分类道具");
                DataManager.Instance.data.addRefreshN--;
                //Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.BroomEffect, Vector3.zero, 2, 5);
                GameManager.Instance.AdjacentPos();
                //GameManager.Instance.FlyZeroPos();
                //Observable.Timer(System.TimeSpan.FromSeconds(1)).Subscribe(_ => { 

                // //GameManager.Instance.AdjacentPos();

                //});

                Observable.TimeInterval(System.TimeSpan.FromSeconds(1)).Subscribe(_ =>
                {
                    UIRoot.Instance.HideMask();
                    ExitPropDir();
                    RefreshBottomUI();

                });

            });
            }

#if !GameTest
        }
#endif


    }


    public float animTime;
    public void TopJoinAmin() {
        topParent.GetComponent<RectTransform>().DOAnchorPos(-new Vector2( topAnimPos.x, topAnimPos.y+ScreenFitPos), animTime).SetEase(Ease.InOutBack);

    }


    public void TopExitAnim() {

        topParent.GetComponent<RectTransform>().DOAnchorPos(topAnimPos, animTime).SetEase(Ease.InOutBack);
     

    }


    public void BottomDeftAim() {

        for (int i = 0; i < bottomParent.childCount; i++)
        {
             Transform T = bottomParent.GetChild(i);
             T.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(T.transform.GetComponent<RectTransform>().anchoredPosition.x, -T.transform.GetComponent<RectTransform>().sizeDelta.y-20);
        }

    }


    public void BottomJoinAnim() {
        //var posY = 0;
        for (int i = 0; i < bottomParent.childCount; i++)
        {
            //if (i < 3)
            //{
            //    posY = 23;
            //}
            //else {
            //    posY = 0;
            //}
            Transform T = bottomParent.GetChild(i);
            T.transform.GetComponent<RectTransform>().DOAnchorPosY(0, animTime).SetDelay(i *0.2f).SetEase(Ease.InOutBack);
        }

    }

    int passIndex = 0;
    public void RefrehPass(object[] args)
    {
        //Debug.Log("刷新界面目标数");
        passIndex = 0;
        Dictionary<string, int> pairs = args[0] as Dictionary<string, int>;
        foreach (var item in pairs.Keys)
        {
            Transform img = PassParent.GetChild(passIndex);
            if (img.GetComponentInChildren<Text>().text!= pairs[item].ToString())
            {
                int index = passIndex;
                int xnum = int.Parse(PassParent.GetChild(index).GetComponentInChildren<Text>().text);
                DOTween.To(() => xnum, x => xnum = x, pairs[item], 0.5F).OnUpdate(()=> {
                    PassParent.GetChild(index).GetComponentInChildren<Text>().text = string.Format("{0}", xnum);
                });
                img.GetChild(0).DOScale(Vector3.one * 1.2F, 0.5F).SetEase(Ease.InOutBack).OnComplete(() => {
                    img.GetChild(0).DOScale(Vector3.one, 0.5F).SetEase(Ease.InOutBack);
                });
            }
            if (pairs[item] <= 0)
            {
                PassParent.GetChild(passIndex).Find("complete").GetComponent<Image>().DOFillAmount(1, 0.5f);
            
            }
            passIndex++;
        }
    }


    /// <summary>
    /// 奖励时间
    /// </summary>
    /// 
    public void RewardTime() {
        BottomDeftAim();
        StartCoroutine("Reward");
    }


   IEnumerator Reward() {

        int Propcount = PropManger.Instance.allProp.Count;
        float waitTime = 0;
        if (Propcount == 0 && GameManager.Instance.RemainingSteps.Value == 0)
        {
            Debug.Log("EWQEQWEQWEQWE +" + GameManager.Instance.RemainingSteps.Value);

            CameraManager.Instance.SetDownLinePos(() => {
                GameManager.Instance.ShowGamePassStep1();
            });
        }
        else
        {
            //引爆当前场景所有炸弹
            
            funTime.GetComponent<Image>().enabled = true;
            funTime.localScale = Vector3.one;
            funTime.gameObject.SetActive(true);
#if Animal
            skeleton.transform.localPosition = new Vector3(0, -35, 0);
            skeleton.gameObject.SetActive(true);
           var KK= skeleton.AnimationState.SetAnimation(0, "LevelEndStart", false).Animation.Duration;
            AudioMgr.Instance.PlaySFX("技能生成");

            Observable.TimeInterval(System.TimeSpan.FromSeconds(KK+0.1F)).Subscribe(_=> {
                skeleton.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                float time = skeleton.AnimationState.SetAnimation(0, "LevelEndEnd", false).Animation.Duration;
                Observable.TimeInterval(System.TimeSpan.FromSeconds(time)).Subscribe(_1 => {
                    skeleton.AnimationState.SetAnimation(0, "LevelEndEnd_Idle", true);
                });

            });
           
#endif
            int now = 0;
            DOTween.To(() => now, x => now = x, 1, 3).OnUpdate(() => {

                var pop = UIManager.Instance.GetBase<LargeCashPanel>();
                if (pop != null)
                {
                    pop.Hide();
                }
                var pop2 = UIManager.Instance.GetBase<LargeCashTwoPanel>();
                if (pop2 != null)
                {
                    pop2.Hide();
                }
            });

            AudioMgr.Instance.PlaySFX("奖励时间弹出");
            yield return new WaitForSeconds(0.7F);

            Pool.Instance.SpawnEffect(Pool.Effect_PoolName, "effect_bushujiangli01", funTime.transform.position, 1, 1.5F);
            funTime.GetChild(0).gameObject.SetActive(true);
            //奖励动画
            yield return new WaitForSeconds(1.3F);

            
            funTime.GetComponent<Image>().enabled = false;
            funTime.GetComponent<Animator>().enabled = false;
            funTime.DOScale(Vector3.zero, 0.25F).SetEase(Ease.InBack);
            yield return new WaitForSeconds(0.2F);

            if (GameManager.Instance.magicBottles.Count != 0)
            {

                for (int i = 0; i < GameManager.Instance.magicBottles.Count; i++)
                {

                    GameManager.Instance.magicBottles[i].OnClick();
                    yield return new WaitForSeconds(1.7F);
                }
                GameManager.Instance.magicBottles.Clear();
                yield return new WaitForSeconds(1f);
            }

            funTime.GetComponent<Image>().enabled = false;
            funTime.GetComponent<Animator>().enabled = true;
            funTime.gameObject.SetActive(false);

            PropManger.Instance.GuideExplosion();
            if (Propcount == 0)
            {
                waitTime = 2.4f;
            }
            else
            {
                waitTime = 2.4f + Propcount * PropManger.Instance.times;
            }
            yield return new WaitForSeconds(Propcount * 0.7f + 0.3F);
          
            
            GameManager.Instance.RewardTime();
            RewardSkip.gameObject.SetActive(true);
            funTime.GetChild(0).gameObject.SetActive(false);

        }
    }


    public override void Show()
    {
        base.Show();
        InitPassIocn();
       
        
        levelText.text = string.Format("关卡 {0}", GameManager.Instance.CurrentLevel);
        nowLevelGetStar = 0;
        SHOW = false;
     
        UIRoot.Instance.ShowMask();
        InitStartPop();
#if !BB108
        if (StartJoin)
        {
            StartPopJoin();
        }
#endif
        StartPopJoin();
        BottomDeftAim();
        TopJoinAmin();
        BottomJoinAnim();
   
        RefreshBottomUI();
        RefresUI();
        InitStar();
        FlyHBFun();


        if (DataManager.Instance.data.UnlockLevel < 6)
        {
            energybtn.gameObject.SetActive(false);
        }
        else { 
            energybtn.gameObject.SetActive(true);

        }

        RewardSkip.gameObject.SetActive(false);
        //RewardTime();

        //关闭banner
        GameADControl.Instance.Banner(false);
    }
    //Tween flyTween;


   
    public void FlyHBFun() {

        
        if (DataManager.Instance.data.UnlockLevel >= 2)
        {
            magicFly.Pause();
            Observable.TimeInterval(System.TimeSpan.FromSeconds(3)).Subscribe(_ =>
            {
                magicFly.BegainFly("Game") ;

            });
        }

    }

    //展示引导
    private void ShowGuide()
    {
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ => {

            if (GameManager.Instance.CurrentLevel == 1)
            {
                if (!GuideMgr.Instance.GuideIsComplete(4))
                {
                    GuideMgr.Instance.ShowGuide_4(() =>
                    {
                        XDebug.Log("完成第四步");
                        GuideMgr.Instance.ShowGuide_5(() => { Debug.Log("完成第五步"); });
                    });
                }
                else if (!GuideMgr.Instance.GuideIsComplete(5))
                {
                    GuideMgr.Instance.ShowGuide_5(() => { Debug.Log("完成第五步"); });
                }
            }
            else if (GameManager.Instance.CurrentLevel == 2)
            {
                Debug.Log("开始第二关引导");
                if (!GuideMgr.Instance.GuideIsComplete(6))
                {
                    GuideMgr.Instance.ShowGuide_6(() =>
                    {
                        GuideMgr.Instance.ShowGuide_16(() =>
                        {
                            XDebug.Log("完成第六部后续");
                        });

                    });
                }
            }
            else if (GameManager.Instance.CurrentLevel == 3)
            {
                if (!GuideMgr.Instance.GuideIsComplete(7))
                {
                    GuideMgr.Instance.ShowGuide_7(() =>
                    {
                        XDebug.Log("完成第七步");

                        GuideMgr.Instance.ShowGuide_11(() =>
                        {
                            XDebug.Log("开始第11步");
                        });
                    });
                }
                //else if (!GuideMgr.Instance.GuideIsComplete(11))
                //{
                //    GuideMgr.Instance.ShowGuide_11(() => {
                //        XDebug.Log("开始第11步");
                //    });
                //}


            }
            else if (GameManager.Instance.CurrentLevel == 4)
            {
                if (!GuideMgr.Instance.GuideIsComplete(12))
                {

                    GuideMgr.Instance.ShowGuide_12(() =>
                    {
                        XDebug.Log("完成第12步");
                        GuideMgr.Instance.ShowGuide_13(() =>
                        {
                            XDebug.Log("完成第13步");
                        });
                    });
                }
            }
            else if (GameManager.Instance.CurrentLevel == 5)
            {
                if (!GuideMgr.Instance.GuideIsComplete(14))
                {
                    GuideMgr.Instance.ShowGuide_14(() =>
                    {

                        XDebug.Log("展示第14步~");
                        GuideMgr.Instance.ShowGuide_15(() =>
                        {
                            XDebug.Log("展示第15步~");
                        });
                    });
                }
            }
          
                
        });
    }

    public override void Refresh() {

        RefresUI();
    }
    /// <summary>
    /// 添加关卡星星
    /// </summary>
    //public void AddLevelStar() {



    //    for (int i = 0; i <  (DataManager.Instance.data.CurrentLevel)-DataManager.Instance.data.levelStar.Count; i++)
    //    {
    //        DataManager.Instance.data.levelStar.Add(0);
    //    }


    //}

    public void ShowChoosePropEffect(Transform parent) {

        choosePorpEffect.SetParent(parent);
        choosePorpEffect.localPosition = Vector3.zero;
        choosePorpEffect.localScale = Vector3.one * 220;
        choosePorpEffect.gameObject.SetActive(true);
    }
        

    public void InitStartPop() {

        startPop.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPop.GetComponent<RectTransform>().sizeDelta.x,0);

    }

    [SerializeField]
     Vector2 orginStartPos;
    [SerializeField]
    Vector2 tartgerStartPos;

    [SerializeField]
    Vector2 moNStartPos;
    [SerializeField]
    Vector2 targetPos;


    public void StartPopJoin() {
        StartJoin = true;
        startPop.gameObject.SetActive(true);
        //startPop.transform.Find("Witch").localPosition = moNStartPos;
        for (int i = 0; i < GameManager.Instance.level.cc.Count; i++)
        {
            startTraget.GetChild(i).GetChild(0).localPosition = Vector3.zero;
        }
        startPop.GetComponent<RectTransform>().anchoredPosition = orginStartPos;
        Image[] startAllImg = startPop.GetComponentsInChildren<Image>();
        Text[] startAllText = startPop.GetComponentsInChildren<Text>();
        foreach (var item in startAllText)
        {
            item.DOFade(0, 0);
        }
        startPop.Find("Witch/role").GetComponent<SkeletonGraphic>().DOFade(0, 0);
        foreach (var item in startAllImg)
        {
            item.DOFade(0, 0);
        }
        foreach (var item in startAllText)
        {
            item.DOFade(1f, 0.5f);
        }
        foreach (var item in startAllImg)
        {
            item.DOFade(1f, 0.5f);
        }
        startPop.Find("Witch/role").GetComponent<SkeletonGraphic>().DOFade(1, 0.5f);
        //startPop.transform.Find("Witch").DOLocalMove(targetPos, 1).SetEase(Ease.InOutCirc);
        startPop.GetComponent<RectTransform>().DOAnchorPos(tartgerStartPos, 0.5f).SetEase(Ease.InOutBack).OnComplete(()=> {

            startPop.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
           {
                //(()=> {
                for (int i = 0; i < GameManager.Instance.level.cc.Count; i++)
               {
                   int p = i;
                   Observable.TimeInterval(System.TimeSpan.FromSeconds(p * 0.5f)).Subscribe(_ => {
                       startTraget.GetChild(p).GetChild(0).DOScale(Vector3.one * 1.2F, 0.25F).SetEase(Ease.InOutBack).OnComplete(() => {
                           startTraget.GetChild(p).GetChild(0).DOScale(Vector3.one, 0.25F).SetEase(Ease.InOutBack).OnComplete(() => {
                           //if ()
                           //{
                               startTraget.GetChild(p).GetChild(0).DOMove(PassParent.GetChild(p).transform.position, 0.6F).SetEase(Ease.InOutBack).OnComplete(()=> {
                                   if (p == GameManager.Instance.level.cc.Count - 1)
                                   {
                                       ShoMapGuide();
                                       startPop.GetComponent<RectTransform>().DOAnchorPos(tartgerStartPos, 0.5f).SetEase(Ease.Linear);
                                       foreach (var item in startAllImg)
                                       {
                                           item.DOFade(0, 0.5F);
                                       }
                                       foreach (var item in startAllText)
                                       {
                                           item.DOFade(0, 0.5F);
                                       }
                                       startPop.Find("Witch/role").GetComponent<SkeletonGraphic>().DOFade(0, 0.5f).OnComplete(() => {

                                           startPop.gameObject.SetActive(false);
                                         

                                       });
                                       //startPop.transform.Find("Witch").DOLocalMove(new Vector2(-startPop.GetComponent<RectTransform>().sizeDelta.x - 100, 0), 0.5f).OnComplete(()=> {
                                       //    startPop.gameObject.SetActive(false);
                                       //});
                                       //startPop.GetComponent<RectTransform>()
                                       UIRoot.Instance.HideMask();
                                       ShowGuide();
                                   }


                               });
                               //}
                           });
                   });
                   });
               }
               //});

               
           });
            //Observable.TimeInterval(System.TimeSpan.FromSeconds(3f)).Subscribe(_ => { 
            
            //});
           

        });



        //startPop.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 2).SetEase(Ease.InOutBack).OnComplete(()=> {


        //    for (int i = 0; i <GameManager.Instance. level.cc.Count; i++)
        //    {
        //        int p = i;
        //        Observable.Timer(System.TimeSpan.FromSeconds(p * 1)).Subscribe(_ => {
        //            startTraget.GetChild(p).GetChild(0).DOScale(Vector3.one * 1.2F, 0.5F).SetEase(Ease.InOutBack).OnComplete(() => {
        //                startTraget.GetChild(p).GetChild(0).DOScale(Vector3.one, 0.5F).SetEase(Ease.InOutBack);
        //            });
        //        });
        //    }
        //});

        //Observable.TimeInterval(System.TimeSpan.FromSeconds(4 + (GameManager.Instance.level.cc.Count - 1)*0.5F)).Subscribe(_ => {



        //});
       
    }

    public void ShoMapGuide() {

        if (TableMgr.Instance.NeedMapGuide(GameManager.Instance.CurrentLevel))
        {

            UIManager.Instance.Show<GuideMapPanel>(UIType.PopUp);
            if (GameManager.Instance.CurrentLevel == 6)
            {
                GameManager.Instance.EliminatPower.Value = 290;
            }

        }
        else
        {
            ShowHF();


        }
    }


    public void ShowHF() {
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.25f)).Subscribe(_ => {


            //if (DataManager.Instance.GetTargetBox() <= GameManager.Instance.StarShineStarSub.Value)
            //{
            //    UIManager.Instance.Show<TreasurePop>(UIType.PopUp, TreasureType.Starlight);
            //}
            //else
            //{
                HFFly();
            //}
        });

    }

    /// <summary>
    /// 展示技能描述
    /// </summary>
    public void ShowPropDir(string PropName, string PropDir, Sprite sprite, System.Action action)
    {
      
        CameraManager.Instance.ShowPropMask();
        maskBtn.interactable = true;
        propDirTran.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
        propDirTran.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
        propDirTran.Find("title").GetComponent<Text>().text = PropName;
        propDirTran.Find("dir").GetComponent<Text>().text = PropDir;
        propDirTran.GetComponent<RectTransform>().DOAnchorPosY(-topAnimPos.y, animTime);
        maskBtn.gameObject.SetActive(true);
        maskBtn.onClick.RemoveAllListeners();
        maskBtn.onClick.AddListener(() =>
        {
            maskBtn.interactable = false;
            if (action != null)
            {
                action();
            }

        });
    }


    public void ExitPropDir() {
        choosePorpEffect.gameObject.SetActive(false);
        inputPos = 0;
        TopJoinAmin();
        maskBtn.gameObject.SetActive(false);
        CameraManager.Instance.HidePropMask();
        propDirTran.GetComponent<RectTransform>().DOAnchorPosY(topAnimPos.y, animTime);
      
    }

    public override void Hide()
    {
        magicFly.Pause();
        SHOW = false;
        //GameManager.Instance.EliminatPower.Value = 0;
        magicFly.Pause();
        EnergImg.transform.parent.Find("effect").gameObject.SetActive(false);
        EnergImg.transform.parent.GetComponent<Animator>().SetBool("isFull", false);
        base.Hide();
        TopExitAnim();
        //GameManager.Instance.EliminatPower.Value = 0;
        //EnergImg.transform.parent.Find("effect").gameObject.SetActive(false);
        //EnergyPro.transform.GetChild(0).gameObject.SetActive(true);
        //EnergImg.transform.parent.GetComponent<Animator>().SetBool("isFull", false);
        //EnergImg.transform.parent.Find("effect").gameObject.SetActive(false);
        //GameManager.Instance.EliminatPower.Value = 0;
    }


    void InitStar() {

        for (int i = 0; i < threeStar.Length; i++)
        {
            threeStar[i].parent.GetComponent<Image>().enabled = true;
            canStar[i] = false;
            threeStar[i].localScale = Vector3.zero;
        }
        scoreFull.gameObject.SetActive(false);
    }

    public void RefresUI()
    {

        pigText.text = PiggyBankData.Instance.pigData.deposit.ToString();
        RemainingStepsText.text = GameManager.Instance.RemainingSteps.Value.ToString();
        treasureUI.Init();
        //cashText.text = string.Format("{0}<size=23>元</size>", (DataManager.Instance.data.HBCoin / 10000).ToString("f2"));
        diamondText.text = DataManager.Instance.data.Diamond.ToString();
    }


    public void RefreshBottomUI() {


        stepText.text = DataManager.Instance.data.addStepN == 0 ? "+" : DataManager.Instance.data.addStepN.ToString();
        bombText.text = DataManager.Instance.data.addBombN == 0 ? "+" : DataManager.Instance.data.addBombN.ToString();
        refreshText.text = DataManager.Instance.data.addRefreshN == 0 ? "+" : DataManager.Instance.data.addRefreshN.ToString();


    }

    public void InitPassIocn() {
        for (int i = 0; i < PassParent.childCount; i++)
        {
            PassParent.GetChild(i).gameObject.SetActive(false);
            startTraget.GetChild(i).gameObject.SetActive(false);
            //startTraget.GetChild(i).GetChild(2).gameObject.SetActive(false);
            PassParent.GetChild(i).GetChild(2).gameObject.GetComponent<Image>().DOFillAmount(0, 0);

        }
        if (GameManager.Instance.level != null)
        {
            LevelSetting level = GameManager.Instance.level;

            for (int i = 0; i < level.cc.Count; i++)
            {
                PassParent.GetChild(i).gameObject.SetActive(true);
                startTraget.GetChild(i).gameObject.SetActive(true);

                //if (level.cc[i].colorType != SortType.Default)
                //{
                //Debug.Log("游戏界面" + level.cc.colorType);
                startTraget.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("BallSprite/" + level.cc[i].ballType.ToString() + "_" + level.cc[i].colorType.ToString());

                //XDebug.Log("通关条件图片名"+level.cc[i].ballType.ToString() + "_" + level.cc[i].colorType.ToString());
                PassParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("BallSprite/" + level.cc[i].ballType.ToString()+"_"+ level.cc[i].colorType.ToString());
                //}
                //else
                //{
                //    PassParent.GetChild(i).transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("BallSprite/" + level.cc[i].ballType.ToString());
                //}
                PassParent.GetChild(i).transform.GetComponentInChildren<Text>().text = string.Format("{0}", level.cc[i].num.ToString());
                startTraget.GetChild(i).transform.GetComponentInChildren<Text>().text = string.Format("{0}", level.cc[i].num.ToString());

            }
        }
        else {

            Debug.LogError("场景未加载");

        }
    }

   

}
