using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using DG.Tweening;
using UnityEditor;


class GameManager : SinglMonoBehaviour<GameManager>
{
    #region 参数定义
    public bool CanPop = true;
    public bool CanReward=true;
    public float ballSize;
    public int LevelStep;
    [HideInInspector]
    public int CurrentLevel = 0;
    [Header("每个球检测半径")]
    public float radius;
    [HideInInspector]
    public LevelSetting level;
    [Header("每批掉落x轴")]
    public float batchx;
    [Header("每批掉落y轴")]
    public float batchy;
    List<Ball> allballs = new List<Ball>();
    [Header("未点击时间")]
    public float willClickTimes;
    public bool IsMeteorBomb = false; //是否是流行轰炸
    bool isRewardTime = false;//奖励时间
    public bool OverGame = true;
    //当前通关条件
    //public Dictionary<string, int> nowCondition = new Dictionary<string, int>();
    public Dictionary<string, int> nowConditionR = new Dictionary<string, int>();

    //收集同类型球
    List<Ball> ClickSameBall = new List<Ball>();
    public List<Ball> ClickSpecailBall = new List<Ball>();
    /// <summary>
    /// 特殊掉落
    /// </summary>
    public Dictionary<string, BallWeights> SpecailBallInfo = new Dictionary<string, BallWeights>();
    public Dictionary<string, int> SpecailBallNum = new Dictionary<string, int>();
    //public Dictionary<string,>

    public List<ColorBall> colorBalls = new List<ColorBall>();//所有彩球
    public List<Prop> propWillBomb = new List<Prop>();//将要爆炸的道具
    #endregion
    public ReactiveProperty<int> RemainingSteps = new ReactiveProperty<int>();
    public ReactiveProperty<int> EliminatPower = new ReactiveProperty<int>();
    public ReactiveProperty<int> LevelScore = new ReactiveProperty<int>();//关卡分数
    public ReactiveProperty<int> StarShineStarSub = new ReactiveProperty<int>();
    public ReactiveProperty<int> LoveStar = new ReactiveProperty<int>();
    public ReactiveProperty<int> DiamondSub = new ReactiveProperty<int>();
   public GamePanel gamePanel;
    public Transform allBallParent;
    public Transform allPlaneParent;
    public new Collider2D collider;
    
    public List<MagicBottle> magicBottles = new List<MagicBottle>();
    //临时变量
    public bool isCash = false;
    public void Init()
    {
        CanReward = true;
        StarShineStarSub.Value = DataManager.Instance.data.StarshineStar;
        LoveStar.Value = DataManager.Instance.data.Love;
        StarShineStarSub.Subscribe(_ =>
        {
            DataManager.Instance.data.StarshineStar = _;
        });
        DiamondSub.Value = DataManager.Instance.data.Diamond;
        DiamondSub.Subscribe(_ =>
        {

            //Debug.Log("钻石数量" + _);
            DataManager.Instance.data.Diamond = _;

        });
        LoveStar.Subscribe(_ =>
        {
            //Debug.Log("爱心数量:" + _);
            if (DataManager.Instance.data.Love == 10)
            {
                DataManager.Instance.data.UseLoveTime = System.DateTime.Now;
            }
            if (_%10==0&&_!=0)
            {
                UmengDisMgr.Instance.CountOnNumber("starbox_arrive");
            }
            DataManager.Instance.data.Love = _;

        });

        RemainingSteps.Subscribe(_ => {

            nowStep++;
            if (nowStep>=30)
            {
                nowStep = 0;
                if (DataManager.Instance.data.UnlockLevel > 5&&!OverGame)
                {
                    DownRedStart();
                }
            }
        });
    }

    //重力消失
    public void GravityDisappear() {

        if (!OverGame)
        {
            Physics2D.gravity = new Vector2(0, 0);
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.25f)).Subscribe(_X =>
            {
                Physics2D.gravity = new Vector2(0, -9.81f);
            });
        }

    }


    public void GetTicket() {

        if (DataManager.Instance.data.TicketLevel.Contains(GameManager.Instance.CurrentLevel))
        {
            LotteryDataManger.Instance.AddLotteryPaper(1);
            RewardData data = new RewardData(RewardEunm.Ticket, 1, false, Resources.Load<Sprite>("UI/Texture/img_choujiangjuan_rmuq"));
            UIManager.Instance.Show<RewardPop>(UIType.PopUp, data);
            DataManager.Instance.data.TicketLevel.Remove(GameManager.Instance.CurrentLevel);
        }
    }

    /// <summary>
    /// 游戏结束初始化数据 
    /// </summary>
    public void InitLevelByOverGame()
    {



    }
    float levelTime = 0;
    public void SendLevel(int F)
    {
        AdControl.Instance.MySendEvent(CurrentLevel.ToString(), F, (int)levelTime);
    }

    int nowStep = 0;

    public bool canShowCat = true;
    public void LoadLevel()
    {
        CanPop = true;
        CatManager.Instance.InitLevel();
        UmengDisMgr.Instance.CountOnPeoples("level_startp", DataManager.Instance.data.UnlockLevel.ToString());
        UmengDisMgr.Instance.CountOnPeoples("level_startu", DataManager.Instance.data.UnlockLevel.ToString());
        GameManager.Instance.SendLevel(1);
#if !BB108
        CurrentLevel = DataManager.Instance.data.UnlockLevel;
#endif
        nowStep = 0;
        CameraManager.Instance.RecoverPos();
        PropManger.Instance.allProp.Clear();
        LevelScore.Value = 0;//关卡分数
        Observable.TimeInterval(System.TimeSpan.FromSeconds(1.5F)).Subscribe(_ =>
        {
            OverGame = false;
            noClickTime = 0;
            levelTime = 0;

        });
        isRewardTime = false;
        fixNumBallList.Clear();
        batchDownList.Clear();
        colorBalls.Clear();
        SpecailBallNum.Clear();
        TableMgr.Instance.GetScoreTable(GetRealLevel(CurrentLevel));
        nowIndex = 0;
        NeedSpwanIce = 0;//冰数量
        LevelStep = 0;//关卡步骤
        level = Instantiate(Resources.Load<LevelSetting>("Level/Level" + GetRealLevel(CurrentLevel)), transform);
        collider = level.transform.Find("Terrain/DX").GetComponent<Collider2D>();
        CameraManager.Instance.tow = false;
        allPlaneParent = level.transform.Find("DefualGrid/Tilemap");
        allBallParent = level.transform.Find("AllBall");
        RemainingSteps.Value = level.RemainingSteps;
        //EliminatPower.Value = 0;
        //nowCondition.Clear();
        nowConditionR.Clear();
        foreach (var item in level.cc)
        {
            nowConditionR.Add(item.ballType.ToString() + "_" + item.colorType.ToString(), item.num);
            //nowCondition.Add(item.ballType.ToString() + "_" + item.colorType.ToString(), item.num);
        }
        allballs.Clear();
        InitWeights();
        if (level.startDownBall)
        {
            StartLevelDownBall();
            DownSpecailBall(true);
        }
        UIManager.Instance.Show<GamePanel>(UIType.Fixed);
        gamePanel = UIManager.Instance.GetBase<GamePanel>();
        EventManager.Instance.ExecuteEvent(MEventType.LevelNextStep, LevelStep);
        CameraManager.Instance.StartLevelMove();
        IsRedDown();
    }

    /// <summary>
    /// 获取真实关卡
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetRealLevel(int level) {


        if (level > DataManager.Instance.LevelFoldNum)
        {
            return  level % DataManager.Instance.LevelFoldNum + 66;
        }
        else
        {
            return  level;
        }
    }


    public void IsRedDown()
    {

        //if (TableMgr.Instance.IsColorGem(CurrentLevel))
        //{
        //    DataManager.Instance.data.GemLevel.Remove(CurrentLevel);
        //    GameObject gemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Ball/Gem"));
        //    gemObj.transform.SetParent(allBallParent);
        //    gemObj.GetComponent<Gem>().Init(TableMgr.Instance.GetGemColor());
        //    gemObj.transform.position = RomdDownPos();
        //}
        if (DataManager.Instance.data.TicketLevel.Contains(CurrentLevel))
        {
            //Transform T=   Pool.Instance.Spawn(Pool.Ball_PoolName,Pool.TicketBall);
            //    T.SetParent(allBallParent);
            //    T.position = RomdDownPos();
        }
        //if (DataManager.Instance.data.MoneyLevel.Contains(CurrentLevel))
        //{

        //}
        if (DataManager.Instance.data.UnlockLevel > 1)
        {

            //if (DataManager.Instance.data.UnlockLevel >= 20)
            //{
                DownRedStart();
            //}

            //DownRedStart();

        }
    }
    public void DownRedStart()
    {

        Transform T = Pool.Instance.Spawn(Pool.Ball_PoolName, Pool.MoneyBall);
        T.GetComponent<MoneyBall>().can = true;
        T.SetParent(allBallParent);
        T.position = RomdDownPos();
    }

    /// <summary>
    /// 开始游戏创建道具
    /// </summary>
    public void BegInCreateProp(bool[] isUse)
    {

        for (int i = 0; i < isUse.Length; i++)
        {
            if (isUse[i])
            {
                DataManager.Instance.data.BombProp[i]--;
                CreateProp(Porp_Size.小, RomdDownPos(), SortType.Red, i + 1);
            }
        }
        if (DataManager.Instance.data.UnlockLevel<5)
        {
            PropManger.Instance.allProp.Clear();
        }
      

    }

    public void DestoryLevel()
    {
        magicBottles.Clear();
        OverGame = true;
        noClickTime = 0;
        InitLevelByOverGame();
        CameraManager.Instance.CanMoveCamera = false;
        Destroy(level.gameObject);

    }

    private void Start()
    {
        EventManager.Instance.AddEvent(MEventType.WaterMove, MoveDonPos);
    }
    public void MoveDonPos(object[] agr)
    {

        //foreach (var item in level.downPos)
        //{
        //    foreach (var M in item.nowPos)
        //    {
        //        M.transform.position += new Vector3(0, (float)agr[0], 0);
        //    }
        //}

    }

    float noClickTime = 0;
    public List<Ball> tempMaxNumberBall = new List<Ball>();

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CameraManager.Instance.SetShake(3, 0.1f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10);
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
            int touchCount = Input.touchCount;
            if (touchCount == 1)
            {
                Touch t = Input.GetTouch(0);
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.FingerPoint, t.position);
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(t.fingerId))
                    return;
            }
#else
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
#endif
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI")) return;
                if (RemainingSteps.Value - 1 < 0) return;
                if (hit.transform.GetComponent<CanClick>() == null) return;
                if (GameManager.Instance.OverGame) return;
                if (hit.collider.transform.GetComponent<MoneyBall>() != null)
                {
                    hit.collider.transform.GetComponent<MoneyBall>().Eliminat();
                    return;
                }
                if (hit.collider.transform.GetComponent<TicketBall>() != null)
                {
                    hit.collider.transform.GetComponent<TicketBall>().Elimnt();
                    return;
                }
                else if (hit.collider.gameObject.CompareTag("Ball"))
                {
                    noClickTime = 0;
                    //if (hit.collider.gameObject.transform.GetComponent<MoneyBall>() != null)
                    //{
                    //    hit.collider.gameObject.transform.GetComponent<MoneyBall>().Eliminat();
                    //}
                    Ball nowBall = hit.collider.gameObject.transform.GetComponent<Ball>();
                    //if (nowBall.isEliminat) return;
                    CollectSameRangBall(hit.collider.transform);
                    if (ClickSameBall.Count == 1 ||nowBall.isEliminat)
                    {
                        ClickSameBall.Clear();
                        ClickSpecailBall.Clear();
                        return;
                    }
                    
                    //点击特效
                    //Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.HitBall, nowBall.transform.position);
                    ElimintBall(nowBall);
                    DownSpecailBall();
                    Observable.TimeInterval(System.TimeSpan.FromSeconds(0.25f)).Subscribe(_ => {
                        ElimintNoColorlBall();
                    });
     
                    RemainingSteps.Value--;
                    if (RemainingSteps.Value == 5)
                    {
                        Pool.Instance.SpawnEffectByParent(Pool.Effect_PoolName, Pool.BuShu5, gamePanel.transform, Vector3.zero);
                    }
                    if (RemainingSteps.Value == 10)
                    {
                        Pool.Instance.SpawnEffectByParent(Pool.Effect_PoolName, Pool.BuShu10, gamePanel.transform, Vector3.zero);
                    }
                }
                else if (hit.collider.gameObject.CompareTag("Prop"))
                {
                    noClickTime = 0;
                    PropManger.Instance.BeginOnCilck();
                    hit.collider.GetComponent<Prop>().OnClick();
                    RemainingSteps.Value--;
                }
            }
        }
        if (!OverGame) {
            levelTime += Time.deltaTime;
            if (noClickTime <= willClickTimes + 1)
            {
                noClickTime += Time.deltaTime;
            }
            else
            {
                CanClickSameBall.Clear();
                noClickTime = 0;
                tempMaxNumberBall.Clear();
                for (int i = 0; i < colorBalls.Count; i++)
                {
                    if (tempMaxNumberBall.Contains(colorBalls[i]))
                    {
                        continue;
                    }
                    if (OverGame) return;
                    CanClickCollectSameRangBall(colorBalls[i].transform);
                    if (CanClickSameBall.Count > tempMaxNumberBall.Count)
                    {
                        tempMaxNumberBall.Clear();
                        foreach (var item in ClickSameBall)
                        {
                            tempMaxNumberBall.Add(item);
                        }
                    }
                    CanClickSameBall.Clear();
                }
                foreach (var item in tempMaxNumberBall)
                {
                    item.SetHight();
                }
                ClickSpecailBall.Clear();
            }
        }
        //观看射线范围 TODO 删除
        if (Input.GetMouseButton(1))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Ball"))
            {
                Debug.DrawLine(hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.position + Vector3.one * (hit.collider.transform.GetComponent<CircleCollider2D>().radius / 2) + Vector3.one * radius, Color.black);
            }
        }
    }

    /// <summary>
    /// 消除掉落特殊球
    /// </summary>
    /// <param name="isFrist"></param>

    //public void 
    public void DownSpecailBall(bool isFrist = false)
    {
        if (OverGame && !isFrist) return;
        string ballname = null;
        //Debug.Log("SpecailBallInfo" + SpecailBallInfo.Values);
        foreach (var item in SpecailBallInfo.Values)
        {
            ballname = GetParfabName(item.ballType);
            StartCoroutine(CloneSpecail(item, ballname, isFrist));
        }
    }
    public string GetParfabName(BallType ballType)
    {
        switch (ballType)
        {
            case BallType.ColorBall:
                return Pool.Ball_Color;
            case BallType.SizeBall:
                return Pool.Ball_SizeBall;
            case BallType.Soda:
                return Pool.Ball_Soda;
            case BallType.Arrive:
                return Pool.Ball_Arrive;
            case BallType.Chocolate:
                return Pool.Ball_Chocolate;
            case BallType.CandyCoat:
                return Pool.Ball_CandyCoat;
            case BallType.CornKernel:
                return Pool.Ball_CornKernel;
            case BallType.PopCorn:
                return Pool.Ball_PopCorn;
            case BallType.SugarCube:
                return Pool.Ball_SuberCube;
            case BallType.Dye:
                return Pool.Ball_Dye;
            case BallType.FreezeBall:
                return Pool.Ball_FreezeBall;
            default:
                return null;
        }
    }


    IEnumerator CloneSpecail(BallWeights item, string ballname, bool isFrist)
    {
        var needSpecailNum = SpecailBallNum[item.ballType.ToString() + "_" + item.colorType.ToString()];
        Debug.Log(ballname + needSpecailNum);
        if (!isFrist)
        {
            needSpecailNum = item.viewNum - SpecailBallNum[item.ballType.ToString() + "_" + item.colorType.ToString()];
        }
        for (int i = 0; i < needSpecailNum; i++)
        {
            item.fixNum--;
            if (!isFrist)
            {
                SpecailBallNum[item.ballType.ToString() + "_" + item.colorType.ToString()]++;
            }
            if (item.fixNum < 0) yield break;
            yield return new WaitForSeconds(0.2F);
            Transform temp = Pool.Instance.Spawn(Pool.Ball_PoolName, ballname);
            temp.gameObject.SetActive(true);
            temp.GetComponent<Ball>().Init(item.colorType, item.isFixNum, item.Gern);
            temp.SetParent(level.AllBallParent);
            temp.localPosition = SpecailRomdDownPos(item.specialDown);
        }
    }

    /// <summary>
    /// 收集相同颜色的球
    /// </summary>
    /// <param name="origin"></param>
    public void CollectSameRangBall(Transform origin,bool can=true)
    {
        Collider2D[] hitRang = new Collider2D[] { };
        List<Ball> tempSameBall = new List<Ball>();
        Ball nowBall = origin.GetComponent<Ball>();
        hitRang = Physics2D.OverlapCircleAll(origin.position, radius + origin.GetComponent<CircleCollider2D>().radius / 2);
        for (int i = 0; i < hitRang.Length; i++)
        {
            //if (can)
            //{
                if (hitRang[i].gameObject.GetComponent<MoneyBall>() != null&& hitRang[i].gameObject.GetComponent<MoneyBall>().can)
                {
                    hitRang[i].gameObject.GetComponent<MoneyBall>().Eliminat();
                    continue;
                }
            //}
            if (hitRang[i].CompareTag("Ball"))
            {
                Ball tempBall = hitRang[i].gameObject.GetComponent<Ball>();
                if ((tempBall.ballType == nowBall.ballType || tempBall.ballType==BallType.FreezeBall) && tempBall.sort == nowBall.sort&&!tempBall.isEliminat)
                {
                    if (!ClickSameBall.Contains(tempBall))
                    {
                        if (nowBall != tempBall)
                        {
                            tempSameBall.Add(tempBall);
                        }
                        else
                        {
                            ClickSameBall.Add(nowBall);
                        }
                    }
                }
                else if (tempBall.ballType == BallType.SizeBall || (tempBall.ballType == BallType.Chocolate /*&& tempBall.sort == SortType.Default*/) || tempBall.ballType == BallType.CornKernel || tempBall.ballType == BallType.PopCorn || tempBall.ballType == BallType.Hive || tempBall.ballType == BallType.SugarCube)
                {
                    if (!ClickSpecailBall.Contains(tempBall))
                    {
                        ClickSpecailBall.Add(tempBall);
                    }
                }
            }
            else if (hitRang[i].CompareTag("Prop"))
            {
                if (hitRang[i].gameObject.GetComponent<GreenElimiat>() != null)
                {
                    hitRang[i].gameObject.GetComponent<Prop>().OnClick();
                }
            }
        }

        if (tempSameBall.Count > 0)
        {
            for (int i = 0; i < tempSameBall.Count; i++)
            {
                CollectSameRangBall(tempSameBall[i].transform);
            }
        }
    }

    public  List<Ball> CanClickSameBall = new List<Ball>();
    public void CanClickCollectSameRangBall(Transform origin)
    {
        Collider2D[] hitRang = new Collider2D[] { };
        List<Ball> tempSameBall = new List<Ball>();
        Ball nowBall = origin.GetComponent<Ball>();
        hitRang = Physics2D.OverlapCircleAll(origin.position, radius + origin.GetComponent<CircleCollider2D>().radius / 2);
        for (int i = 0; i < hitRang.Length; i++)
        {
            if (hitRang[i].CompareTag("Ball"))
            {
                Ball tempBall = hitRang[i].gameObject.GetComponent<Ball>();
                if (tempBall.ballType == nowBall.ballType && tempBall.sort == nowBall.sort)
                {

                    if (!CanClickSameBall.Contains(tempBall))
                    {
                        if (nowBall != tempBall)
                        {
                            tempSameBall.Add(tempBall);
                        }
                        else
                        {
                            CanClickSameBall.Add(nowBall);
                        }
                    }
                }
            }
        }
        if (tempSameBall.Count > 0)
        {
            for (int i = 0; i < tempSameBall.Count; i++)
            {
                CanClickCollectSameRangBall(tempSameBall[i].transform);
            }
        }
    }





    public Ball RomdBall()
    {

        int t = UnityEngine.Random.Range(0, tempAdjBall.Count);

        if (FinalPos.ContainsValue(tempAdjBall[t].transform.position))
        {
            return RomdBall();
        }
        else
        {
            return tempAdjBall[t];
        }
    }


    /// <summary>
    ///消除球
    /// </summary>
    List<WillDown> willDowns = new List<WillDown>();

    
    public void OneByOneToEliminate(Ball nowBall) {

        //List<Ball> TEMP = new List<Ball>();
        //foreach (var item in ClickSameBall)
        //{
        //    TEMP.Add(item);
        //}
        var needSpawnNum = 0;//需要重新生成得球数量
        propOrgin = nowBall.transform.position;


        //for (int i = 0; i < fenpiA.Count; i++)
        //{
        //    foreach (var item in fenpiA[i])
        //    {
        //        needSpawnNum += item.Eliminat(i + 1, true);
        //    }
        //}

        ClickSameBall.Sort((x,y)=>Vector3.Distance(x.transform.position, propOrgin).CompareTo(Vector3.Distance(y.transform.position, propOrgin)));
        //for (int i = 0; i < ClickSameBall.Count ; i++)
        //{
        //    //yield return new WaitForSeconds(waitTime1);
        //    //yield return new WaitForSeconds(waitTime2);
        //    needSpawnNum += ClickSameBall[i].Eliminat(i + 1,true);
        //}
        for (int i = 0; i < ClickSameBall.Count; i++)
        {
            //foreach (var item in fenpiA[i])
            //{
                needSpawnNum += ClickSameBall[i].Eliminat(i + 1, true);
            //}
        }

        if (ClickSameBall[0].isFix)
        {
            willDowns.Add(new WillDown(ClickSameBall[0].ballType, ClickSameBall[0].sort, ClickSameBall[0].isFix, needSpawnNum));
            ByFixAddBatch(willDowns, needSpawnNum);
            CreateBall(level.downRangs[JudgeBatch(level.fallAllBall)].downInterval);
        }
        else
        {
            RomdbyWeights(needSpawnNum, level.downRangs[JudgeBatch(level.fallAllBall)].batch);
            CreateBall(level.downRangs[JudgeBatch(ClickSameBall.Count)].downInterval);
            
        }
        if (needSpawnNum >= 4 && needSpawnNum <= 5)
        {
            //Observable.Timer(System.TimeSpan.FromSeconds(0.34F)).Subscribe(_ => {
            DelayedCreatePorp(Porp_Size.小, propOrgin, nowBall.sort,0.34f);
            //});
           
        }
        else if (needSpawnNum >= 6 && needSpawnNum <= 7)
        {
            //Observable.Timer(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ => {
            DelayedCreatePorp(Porp_Size.中, propOrgin, nowBall.sort,0.5F);
            //});
            
        }
        else if (needSpawnNum > 7)
        {
            //Observable.Timer(System.TimeSpan.FromSeconds(0.59F)).Subscribe(_ => {
            DelayedCreatePorp(Porp_Size.大, propOrgin, nowBall.sort,0.59F);
            //});
        }
        //消除能量添加
        if (DataManager.Instance.data.UnlockLevel>=6)
        {
            EliminatPower.Value +=(int)needSpawnNum;
        }
        ClickSameBall.Clear();
        //Observable.Timer(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ =>
        //{
        //if (CameraManager.Instance.pos != GetBestDownPos())
        //{

        //}
        //});
        CameraManager.Instance.BeginMove();

    }

    public void DelayedCreatePorp(Porp_Size porp_Size, Vector2 orgin, SortType sort,float time) {

        Observable.TimeInterval(System.TimeSpan.FromSeconds(time)).Subscribe(_ => {
        CreateProp(porp_Size, orgin, sort);
        });
    }

    public void ElimintBall(Ball nowBall)
    {
        willDowns.Clear();
        OneByOneToEliminate(nowBall);
    }
    public float dowuY = 0;
    int order;//消除顺序
    /// <summary>
    /// 消除特殊方块
    /// </summary>
    public void ElimintNoColorlBall()
    {

        willDowns.Clear();
        int sodaNum = 0;
        bool isSpecail = false;
        order = 0;
        //DyeChangeBall.Clear();
        Dye.Clear();
        Dictionary<string, List<Ball>> tempBall = new Dictionary<string, List<Ball>>();
        foreach (var item in ClickSpecailBall)
        {
            order++;
            if (item.ballType == BallType.Arrive) continue;
            if (item.ballType == BallType.SugarCube && item.GetComponent<SugarCube>().Gear != 1) {
                item.Eliminat();
                continue;
            }
            if (item.ballType == BallType.FreezeBall)
            {
                
                item.Eliminat();
               
                continue;
            }
            if (item.ballType == BallType.Soda)
            {
                sodaNum++;
            }
            if (SpecailBallInfo.ContainsKey(item.typeName))
            {
                if (item.typeName == "CornKernel_Default")
                {
                    item.Eliminat(order);
                }
                else
                {
                    isSpecail = true;
                    SpecailBallNum[item.typeName]--; 
                    item.Eliminat(order);
                }
            }
            else if (item.typeName == "PopCorn_Default" && SpecailBallInfo.ContainsKey("CornKernel_Default"))
            {
                isSpecail = true;
                SpecailBallNum["CornKernel_Default"]--;
                item.Eliminat(order);
            }
            else
            {
                if (item != null && item.gameObject.activeSelf)
                {
                    var needNum = item.Eliminat(order + 1);
                    if (item.isFix)
                    {
                        if (needNum != 0)
                        {
                            if (!tempBall.ContainsKey(item.typeName))
                            {
                                List<Ball> balls = new List<Ball>();
                                balls.Add(item);
                                tempBall.Add(item.typeName, balls);
                            }
                            else
                            {
                                tempBall[item.typeName].Add(item);
                            }
                        }
                    }
                }
            }
        }
        foreach (var item in tempBall.Values)
        {
            BallType TYPE = item[0].ballType;
            WillDown tempW = new WillDown(TYPE, item[0].sort, item[0].isFix, item.Count);
            if (TYPE == BallType.CornKernel && tempW.isFix)
            {
                continue;
            }

            if (TYPE == BallType.PopCorn && tempW.isFix)
            {
                tempW.type = BallType.CornKernel;
            }
            willDowns.Add(tempW);
            if (item[0].isFix)
            {
                foreach (BallWeights item1 in level.ballWeights)
                {
                    if (item[0].ballType == item1.ballType)
                    {
                        tempW.Gern = item1.Gern;
                    }
                }
                ByFixAddBatch(willDowns, item.Count);
                CreateBall(level.downRangs[JudgeBatch(level.fallAllBall)].downInterval);
            }
            else
            {
                RomdbyWeights(item.Count, level.downRangs[JudgeBatch(level.fallAllBall)].batch);
                CreateBall(level.downRangs[JudgeBatch(ClickSameBall.Count)].downInterval);
            }
        }
        if (sodaNum > 0)
        {
            EventManager.Instance.ExecuteEvent(MEventType.WaterMove, level.riseNum * sodaNum);
        }
        if (isSpecail)
        {
            DownSpecailBall();
        }
        if (DataManager.Instance.data.UnlockLevel >= 6)
        {
            EliminatPower.Value +=(int)ClickSpecailBall.Count;
        }
        ClickSpecailBall.Clear();
    }

    public int IceNum;
    public int NeedSpwanIce;
    public void RomdIce()
    {

        for (int i = 0; i < NeedSpwanIce; i++)
        {
            var R = UnityEngine.Random.Range(0, colorBalls.Count);
            colorBalls[R].SetSubger();
        }
        NeedSpwanIce = 0;
        //if (colorBalls.Count > 6)
        //{
        //    for (int i = 0; i < NeedSpwanIce; i++)
        //    {

        //    }
        //    NeedSpwanIce -= 6;
        //}
        //else
        //{
        //    NeedSpwanIce -= colorBalls.Count;
        //    for (int i = 0; i < colorBalls.Count; i++)
        //    {
        //        var R = UnityEngine.Random.Range(0, colorBalls.Count);
        //        colorBalls[R].SetSubger();
        //    }
        //    colorBalls.Clear();
        //}
    }

    //public void 


    public List<ColorBall> AllSameColor(SortType sort) {
        List<ColorBall> tempC = new List<ColorBall>();
        try
        {
            for (int i = 0; i < colorBalls.Count; i++)
            {
                if (colorBalls[i].sort != sort &&!Dye.Contains(colorBalls[i]))
                {
                    tempC.Add(colorBalls[i]);
                }
            }
            return tempC;
        }
        catch {

            Debug.LogError("出现问题");
            return null;
        }


    }

    SortType DyeNeedColor;
   public int DyeNeedColorNum;
    public ColorBall RandCanOperateBall(SortType sort) {
        List<ColorBall> C = AllSameColor(sort);
        if (C.Count == 0)
        {
            DyeNeedColor = sort;
            DyeNeedColorNum++;
            return null;
        }
        var R = UnityEngine.Random.Range(0, C.Count);
        return C[R];
    }


    List<ColorBall> Dye = new List<ColorBall>();
    public void DyeChangeColor(SortType tagetType, Vector3 orgin)
    {
        SortType temp = tagetType;
        for (int i = 0; i < 3; i++)
        {
        //Debug.LogError("染色瓶炸裂"+ temp);
            string v = temp.ToString();
           
            ColorBall colorBall = RandCanOperateBall(tagetType);
            if (colorBall == null) continue;
            Transform Flyeffect = Pool.Instance.Spawn(Pool.Effect_PoolName, Pool.DyeFlyEffect + v);
            Dye.Add(colorBall);
          
            //if (colorBall.willColorBall== tagetType)
            //{
            //    GameManager.Instance.ReduceTarget(colorBall.ballType.ToString() + "_" + tagetType.ToString());
            //    Debug.Log("将要" + colorBall.ballType.ToString() + "_" + tagetType.ToString());
            //    continue;
            //}
            colorBall.willColorBall = tagetType;
            Debug.Log("染色瓶染色");
            
            FlyDye(colorBall, tagetType, orgin, Flyeffect);
        }
    }

    public void FlyDye(ColorBall colorBall, SortType tagetType, Vector3 orgin, Transform Flyeffect) {

        DynamicMgr.Instance.FlyEffectCurveByGG(orgin, colorBall.transform.position, Flyeffect, 0, 1F, 0.5F, 90F, () =>
        {
            if (colorBall.isEliminat)
            {
                ReduceTarget("ColorBall_"+ tagetType.ToString());
            }
            
            colorBall.ChangeColor(tagetType, 2);
            
        });

        Observable.TimeInterval(System.TimeSpan.FromSeconds(1)).Subscribe(_ => {

            Pool.Instance.Despawn(Pool.Effect_PoolName, Flyeffect);

        });
    }


    /// <summary>
    /// 染色瓶随机球
    /// </summary>
    /// <param name="sort"></param>
    /// <param name="NOW"></param>
    /// <returns></returns>


    //public ColorBall RomdNoSame(SortType sort)
    //{
    //    var sortNum = 0;
    //    foreach (var item in colorBalls)
    //    {
    //        if (sort==item.sort)
    //        {
    //            sortNum++;
    //        }
    //    }
    //    if (sortNum == colorBalls.Count|| colorBalls.Count==0)
    //    {
    //            //DyeNeedColor.Add(sort);
    //        return null;
    //    }
    //    else
    //    {
    //        var R = UnityEngine.Random.Range(0, colorBalls.Count);
    //        if (colorBalls[R].sort == sort || DyeChangeBall.Contains(colorBalls[R]))
    //        {
    //            return RomdNoSame(sort);
    //        }
    //        else
    //        {
    //            return colorBalls[R];
    //        }
    //    }
    //}




    /// <summary>
    /// 游戏通关
    /// </summary>
    public void PassGame()
    {
        CanPop = false;
        //UIManager.Instance.GetBase<GamePanel>().NextBtn.gameObject.SetActive(true);
        if (RemainingSteps.Value >= 0)
        {
            OverGame = true;
            if (CanReward)
            {
                UIRoot.Instance.ShowMask();
                Observable.TimeInterval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
                {
                    gamePanel.RewardTime();
                });
            }
            //奖励时间

            //gamePanel.nextBtn.gameObject.SetActive(true);



        }
    }
    public Vector3 RomdRangBallPos(List<Ball> balls = null)
    {
        //Ball ball = null;
        Vector3 taget = Vector3.zero;
        var allNum = GameManager.Instance.allPlaneParent.childCount + GameManager.Instance.allBallParent.childCount;
        if (allNum == 0)
        {
            //Eliminat();
            Debug.LogError("场景无物体");
            return taget;
        }
        int r =UnityEngine. Random.Range(0, allNum);
        if (r < GameManager.Instance.allPlaneParent.childCount)
        {

            taget = GameManager.Instance.allPlaneParent.GetChild(r).position;

        }
        else
        {
            taget = GameManager.Instance.allBallParent.GetChild(r - GameManager.Instance.allPlaneParent.childCount).position;
        }
        if (!CameraManager.Instance.IsInScerrn(taget.y))
        {
            return RomdRangBallPos();
        }
        else
        {
            return taget;
        }
       
    }

    IEnumerator StartFlyStep()
    {

        allProp.Clear();
        times = RemainingSteps.Value;

        for (int i = 0; i < times; i++)
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.GetBase<GamePanel>().RemainingStepsText.text= (times-i-1).ToString();
            Vector3 vector = RomdRangBallPos();
            GameManager.Instance.LevelScore.Value += 500;
            AudioMgr.Instance.PlaySFX("步数流星");
            DynamicMgr.Instance.FlyEffectCurve(gamePanel.RemainingStepsText.transform.parent.position, vector, Pool.StepLiuXin, 0.38f, 0.5f, 5, () =>
            {
                //if (R < colorBalls.Count - 1)
                //{
                    //int GEAR = UnityEngine.Random.Range(1, 3);
                    //if (GEAR == 1)
                    //{
                        allProp.Add(CreateProp(Porp_Size.中, vector, SortType.Red,1,true));
                        //allProp[i].canTiggle = false;
                    //}
                    //else
                    //{
                    //    allProp.Add(CreateProp(Porp_Size.中, vector, SortType.Orange,1,true));
                    //    //allProp[i].canTiggle = false;

                    //}
                //}
            });
        }
        var n = 0;
        if (allProp.Count > 10)
        {
            n = 10;
        }
        else {
            n = allProp.Count;
        }
        float waitTime = 0.2f;
        var shakeLevel = PropManger.Instance.GetShackLevel("Red", 1, Porp_Size.小) * 0.5f;
        var shakeTime = PropManger.Instance.GetShackTime("Red", 1, Porp_Size.小);
        for (int i = 0; i < n; i++)
        {
            allProp[i].OnClick();
            CameraManager.Instance.SetShake(shakeLevel, shakeTime);
            waitTime -= 0.02F;
            yield return new WaitForSeconds(waitTime);
        }
        for (int i = 10; i < allProp.Count; i++)
        {
            allProp[i].OnClick();
            CameraManager.Instance.SetShake(shakeLevel, shakeTime);
            yield return new WaitForSeconds(0.02f);
        }
       

        //yield return new WaitForSeconds(0.2f);
        //propRang2D.Clear();
        //for (int i = 10; i < allProp.Count; i++)
        //{
        //    allProp[i].Eliminat();
        //    foreach (var c in allProp[i].DetectionRange())
        //    {
        //        if (!propRang2D.Contains(c))
        //        {
        //            propRang2D.Add(c);
        //        }
        //    }
        //}
        //RangeElimination(propRang2D.ToArray());
        if (times != 0)
        {
            CameraManager.Instance.SetShake(3, 0.5F);
        }
        gamePanel.RewardSkip.gameObject.SetActive(false);
        //展示通关界面
        yield return new WaitForSeconds(0.7f);
        CameraManager.Instance.SetDownLinePos(() => {
            ShowGamePassStep1();
        });
        //ShowGamePassStep1();
    }

    int times = 0;
    /// <summary>
    /// 奖励时间
    /// </summary>
    List<Collider2D> propRang2D = new List<Collider2D>();
    List<Prop> allProp = new List<Prop>();

    public void RewardTime()
    {
        isRewardTime = true;
        StartCoroutine("StartFlyStep");
    }

    public void SkipRewardTime() {
        propRang2D.Clear();
        GameManager.Instance.LevelScore.Value += 1000* RemainingSteps.Value;
        StopCoroutine("StartFlyStep");
        CameraManager.Instance.SetDownLinePos(()=> { 
        ShowGamePassStep1();
        });

    }


    public void ShowGamePassStep1() {
#if Animal
             gamePanel.skeleton.transform.DOLocalMoveY(-1000,1).OnComplete(()=> {
            gamePanel.skeleton.gameObject.SetActive(false);
            gamePanel.skeleton.transform.localPosition = new Vector3(0, 0, 0);
                 gamePanel. skeleton.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
             });
#endif

        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.25F)).Subscribe(_ =>
        {
            UIRoot.Instance.HideMask();
        });
        AdControl.Instance. JoinLevelNum ++;
        InitLevelByOverGame();
        //var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
        //var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
        var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
       
        if (GameManager.Instance.CurrentLevel==DataManager.Instance.data.UnlockLevel)
        {
            UmengDisMgr.Instance.CountOnPeoples("win_hb_show", string.Format("{0}", GameManager.Instance.CurrentLevel));
        }

        popup1.OnOpen("win_hb_video", 0, () => {
            //打开回调
          Observable.TimeInterval(System.TimeSpan.FromSeconds(0.0f)).Subscribe(_ =>
          {

            var  rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
            var  rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
            var popup2 = UIManager.Instance.ShowPopUp<RedTwoPopup>();
            popup2.OnOpen(rewardVoucher, rewardRedIcon, "双倍key", () =>
            {
                ////设置新人红包
                //RedWithdrawData.Instance.SetFirstBag();

                if (!GameManager.Instance.isCash)
                {
                    RedWithdrawData.Instance.UpdateRedIcon(rewardRedIcon);
                }
                UmengDisMgr.Instance.CountOnPeoples("win_hb_get", string.Format("{0}", GameManager.Instance.CurrentLevel));
                popup2.effect.SetActive(false);

               
                UIManager.Instance.Show<PassPop>(UIType.PopUp);

                Debug.Log("关闭红包二级界面");
            });
            popup1.defult.SetActive(false);
          });
         },
        () =>

        {
            //关闭回调
            Debug.Log("关闭红包一级界面");
            popup1.defult.SetActive(false);
           
                UIManager.Instance.Show<PassPop>(UIType.PopUp);

        });


    }


    /// <summary>
    /// 是否通关
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>'
    public void ReduceTarget(string name)
    {
        if (nowConditionR.ContainsKey(name))
        {
            if (nowConditionR[name] > 0)
            {
                GameManager.Instance.nowConditionR[name]--;
                if (nowConditionR[name] == 0)
                {
                    if (IsPass())
                    {
                       
                        PassGame();
                    }
                }
            }
        }
    }


    public bool IsPass() {

        foreach (var item in nowConditionR.Values)
        {
            if (item != 0)
            {
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// 判断是否是通关条件
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public bool IsCondition(string s)
    {
        return nowConditionR.ContainsKey(s);
    }

    /// <summary>
    /// 获取通关下标
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public int GetCondition(string s)
    {
        int x = 0;
        foreach (var item in nowConditionR.Keys)
        {
            //Debug.Log("通关条件 " + item);
            if (item == s)
            {
                return x;
            }
            x++;
        }
        return x;
    }


    // 道具生成点
    Vector2 propOrgin;
    /// <summary>
    /// 创建代码
    /// </summary>
    /// <param name="porp_Size"></param>
    /// <param name="orgin"></param>
    public Prop CreateProp(Porp_Size porp_Size, Vector2 orgin, SortType sort, int Gear = 1,bool reward=false)
    {
        porp_Size = Porp_Size.大;
        string prefabName = null;
        //switch (sort)
        //{
        //    case SortType.Red:
        //        prefabName = Pool.HorizontalProp;
        //        break;
        //    case SortType.Yellow:
        //        prefabName = Pool.EliminateOrgne;
        //        break;
        //    case SortType.Blue:
        //        prefabName = Pool.BlueProp;
        //        break;
        //    case SortType.Green:
        //        prefabName = Pool.PropGreen;
        //        break;
        //    case SortType.Coat:
        //        break;
        //    case SortType.Orange:
        //        prefabName = Pool.UprihtProp;
        //        break;
        //    case SortType.Cyan:
        //        prefabName = Pool.LightBall;
        //        break;
        //    case SortType.Pink:
        //        prefabName = Pool.DelayedProp;
        //        break;
        //    case SortType.Purple:
        //        prefabName = Pool.PurpProp;
        //        break;
        //    default:
        //        break;
        //}
#if Animal
        prefabName = Pool.HengProp;
#else
        prefabName = Pool.HorizontalProp;
        if (reward)
        {
            int GEAR = UnityEngine.Random.Range(1, 3);
            if (GEAR == 1)
                prefabName = Pool.UprihtProp;
        }
#endif
        Transform t = Pool.Instance.Spawn(Pool.Prop_PoolName, prefabName);
        t.SetParent(level.AllBallParent);
        t.transform.position = orgin;
        Prop ElimiProp = t.GetComponent<Prop>();
        ElimiProp.SizeType = porp_Size;
        object[] all = new object[2] { porp_Size, Gear };
        ElimiProp.Init(all);
        switch (ElimiProp.SizeType)
        {
            case Porp_Size.小:
                ElimiProp.transform.localScale = Vector3.one * 1.17F;
                break;
            case Porp_Size.中:
                ElimiProp.transform.localScale = Vector3.one * 1.4f;
                break;
            case Porp_Size.大:
                ElimiProp.transform.localScale = Vector3.one * 1.6f;
                break;
            default:
                break;
        }
        ElimiProp.transform.position = orgin;
        Pool.Instance.SpawnEffectByParent(Pool.Effect_PoolName, Pool.PropSpawn, ElimiProp.transform, Vector3.zero);
        AudioMgr.Instance.PlaySFX("技能生成");
        //if (DataManager.Instance.data.UnlockLevel<5)
        //{
        //    t.gameObject.AddComponent<GuideObj>();
        //}
        return ElimiProp;
    }

    /// <summary>
    /// 每批掉落的球
    /// </summary>
    public List<WillDown> batchDownList = new List<WillDown>();
    /// <summary>
    /// 开始游戏掉落球
    /// </summary>
    public void StartLevelDownBall()
    {
        var allNum = 0;
        DownRang downRang = level.downRangs[JudgeBatch(level.fallAllBall)];
        if (fixNumBallList.Count > 0)
        {
            foreach (var item in fixNumBallList)
            {
                if (item.isFixNum)
                {
                    if (item.fixNum >= downRang.batch)
                    {
                        for (int i = 0; i < item.fixNum / downRang.batch; i++)
                        {
                            WillDown willDown = new WillDown(item.ballType, item.colorType, item.isFixNum, downRang.batch);
                            if (item.ballType == BallType.SugarCube)
                            {
                                willDown.Gern = item.Gern;
                            }
                            batchDownList.Add(willDown);
                            allNum += downRang.batch;
                        }
                        if (item.fixNum % downRang.batch!= 0)
                        {
                            WillDown willDown = new WillDown(item.ballType, item.colorType, item.isFixNum, downRang.batch);
                            if (item.ballType == BallType.SugarCube)
                            {
                                willDown.Gern = item.Gern;
                            }
                            batchDownList.Add(willDown);
                            allNum += item.fixNum % downRang.batch;
                        }
                    }
                    else
                    {
                        WillDown willDown = new WillDown(item.ballType, item.colorType, item.isFixNum, item.fixNum);
                        if (item.ballType == BallType.SugarCube)
                        {
                            willDown.Gern = item.Gern;
                        }
                        batchDownList.Add(willDown);
                        allNum += item.fixNum;
                    }
                }
            }
        }
        RomdbyWeights(level.fallAllBall - allNum, downRang.batch);
        batchDownList = ListRandom(batchDownList);
        CreateBall(downRang.downInterval, false);
        CatManager.Instance.CreatCat();
    }



    /// <summary>
    /// 通过固定数添加批数
    /// </summary>
    public int ByFixAddBatch(List<WillDown> wills, int needDownCount)
    {
        int allNum = 0;

        DownRang downRang = level.downRangs[JudgeBatch(level.fallAllBall- colorBalls.Count)];
        if (wills.Count > 0)
        {
            foreach (WillDown item in wills)
            {
                Debug.Log("item.num" + item.num);
                if (item.type == BallType.Arrive || item.type == BallType.Soda)
                {
                    for (int i = 0; i < item.num; i++)
                    {
                        WillDown willDowns = new WillDown(item.type, item.color, item.isFix, 1);
                        willDowns.Gern = item.Gern;
                        batchDownList.Add(willDowns);
                        allNum++;
                    }
                    break;
                }
                if (item.num > downRang.batch)
                {
                    for (int i = 0; i < item.num / downRang.batch; i++)
                    {
                        WillDown willDowns = new WillDown(item.type, item.color, item.isFix, downRang.batch);
                        willDowns.Gern = item.Gern;
                        batchDownList.Add(willDowns);
                        allNum += downRang.batch;
                    }
                    if (item.num % downRang.batch != 0)
                    {
                        WillDown willDowns = new WillDown(item.type, item.color, item.isFix, item.num % downRang.batch);
                        willDowns.Gern = item.Gern;
                        batchDownList.Add(willDowns);
                        allNum += item.num % downRang.batch;
                    }
                }
                else
                {
                    WillDown willDowns = new WillDown(item.type, item.color, item.isFix, item.num);
                    willDowns.Gern = item.Gern;
                    batchDownList.Add(willDowns);
                    allNum += item.num;
                }
            }
        }
        return allNum;
    }

    /// <summary>
    /// 通过权重随机
    /// </summary>
    /// <param name="needNum">需要随机的个数</param>
    /// <param name="batchNum">每批个数</param>
    public void RomdbyWeights(int needNum, int batchNum)
    {
        //var x = 0;
        needNum = level.fallAllBall- colorBalls.Count;
        //Debug.Log("需要随机生成的球" + needNum);
        if (needNum < 0)
        {
            batchDownList.Clear();
        }
        for (int i = 0; i < (needNum) / batchNum; i++)
        {
            WillDown willDown = RomdWeights(batchNum);
            if (DyeNeedColorNum != 0)
            {
                willDown.color = DyeNeedColor;
                //willDown.num = DyeNeedColor.Count;
                //for (int x = 0; x < willDown.num; x++)
                //{
                //    DyeNeedColor.RemoveAt(x);
                //}

                DyeNeedColorNum -= batchNum;

            }
            if (willDown != null)
            {
                batchDownList.Add(willDown);
            }
            //x += batchNum;
        }
        if (needNum % batchNum != 0)
        {
            WillDown willDown = RomdWeights(needNum % batchNum);
            if (DyeNeedColorNum != 0)
            {
                willDown.color = DyeNeedColor;
                //willDown.num = DyeNeedColor.Count;
                //for (int x = 0; x < willDown.num; x++)
                //{
                //    DyeNeedColor.RemoveAt(x);
                //}

                DyeNeedColorNum -= needNum % batchNum;

            }
            if (willDown != null)
            {
                batchDownList.Add(willDown);
            }
            //DyeNeedColorNum -= needNum % batchNum;

            //x += batchNum;
        }


    }

    /// <summary>
    /// 流星轰炸
    /// </summary>
    public void MeteorBombing(Vector3 orgin)
    {
        RangList.Clear();
        Collider2D[] collision2s = Physics2D.OverlapCapsuleAll(orgin, new Vector2(3.5F, 4), CapsuleDirection2D.Horizontal, 0);
        RangeElimination(collision2s);
        EliminatPower.Value += collision2s.Length;
        CameraManager.Instance.BeginMove();
    }
    List<Ball> RangList = new List<Ball>();
    /// <summary>
    /// 范围消除
    /// </summary>
    public void RangeElimination(Collider2D[] collider2Ds)
    {
        propWillBomb.Clear();
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            if (collider2Ds[i] != null)
            {
                if (collider2Ds[i].GetComponent<Ball>() != null)
                {
                    Ball tempBall = collider2Ds[i].gameObject.GetComponent<Ball>();
                    if (!RangList.Contains(tempBall)&& !tempBall.isEliminat)
                    {
                        RangList.Add(tempBall);

                    }
                }

                if (collider2Ds[i].CompareTag("Prop"))
                {
                    Prop tempProp = collider2Ds[i].transform.GetComponent<Prop>();
                    if (!tempProp.isReady)
                    {
                        tempProp.SetReady(tempProp.transform);
                    }
                }
                //if (collider2Ds[i].GetComponent<TicketBall>() != null)
                //{
                //    collider2Ds[i].GetComponent<TicketBall>().Elimnt();
                //}
                //if (collider2Ds[i].GetComponent<MoneyBall>() != null)
                //{
                //    //if (collider2Ds[i].gameObject.transform.GetComponent<MoneyBall>() != null)
                //    //{
                //    collider2Ds[i].gameObject.transform.GetComponent<MoneyBall>().Eliminat();
                //    //}
                //}
            }
        }
        var needSpswnNum = 0;

        //Debug.Log(collider2Ds.Length + "E///" + needSpswnNum);

        Dictionary<string, WillDown> temp = new Dictionary<string, WillDown>();
        for (int i = 0; i < RangList.Count; i++)
        {
            if (RangList[i].isEliminat) continue;
           
            if (RangList[i].ballType != BallType.ColorBall/*|| RangList[i].ballType == BallType.SizeBall || (RangList[i].ballType == BallType.Chocolate && RangList[i].sort == SortType.Default) || RangList[i].ballType == BallType.CornKernel || RangList[i].ballType == BallType.PopCorn || RangList[i].ballType == BallType.Hive || RangList[i].ballType == BallType.SugarCube*/)
            {
                if (!ClickSpecailBall.Contains(RangList[i]))
                {
                    ClickSpecailBall.Add(RangList[i]);
                }
                //GameManager.Instance..Add();
            }
            else
            {
                //彩球情况
                WillDown willDown = new WillDown(RangList[i].ballType, RangList[i].sort, RangList[i].isFix, 1);
                if (temp.ContainsKey(willDown.BallTypeString))
                {
                    temp[willDown.BallTypeString].num++;
                }
                else
                {
                    temp.Add(willDown.BallTypeString, willDown);
                }
                needSpswnNum += RangList[i].Eliminat(1);
                //GameManager.Instance.colorBalls.Remove(PropCollectList[i].GetComponent<ColorBall>());
                
            }
        }
        GravityDisappear();
        RangList.Clear();
        ElimintNoColorlBall();
        AddBatch(temp, needSpswnNum);
        DownSpecailBall();
    }
    public void AddBatch(Dictionary<string, WillDown> temp, int needSpswnNum)
    {
        //彩球添加
        //Debug.Log("道具消除 球掉落:" + needSpswnNum);
        List<WillDown> FixWillDown = new List<WillDown>();
        foreach (var item in temp.Values)
        {
            if (item.isFix && item.type == BallType.ColorBall)
            {
                FixWillDown.Add(item);
            }
        }
        var fixNum = GameManager.Instance.ByFixAddBatch(FixWillDown, needSpswnNum);
        DownRang downRang = GameManager.Instance.level.downRangs[GameManager.Instance.JudgeBatch(needSpswnNum)];
        GameManager.Instance.RomdbyWeights(needSpswnNum - fixNum, downRang.batch);
        GameManager.Instance.CreateBall(downRang.downInterval);

        ClickSpecailBall.Clear();

    }

    public void PropBomb()
    {

        //foreach (var item in)
        //{

        //}

    }


    /// <summary>
    /// 魔法刷新 收集彩色球
    /// </summary>
    /// <param name="origin"></param>
    public void CollectColorBall(Transform origin)
    {
        if (nowNeedFP.Count == 0)
        {
            return;
        }
        if (dyBall.Count == 0 && nowNeedFP.Count == 0)
        {
            return;
        }
        Collider2D[] hitRang = new Collider2D[] { };
        List<ColorBall> tempSameBall = new List<ColorBall>();
        hitRang = Physics2D.OverlapCircleAll(origin.position, /*radius + origin.GetComponent<CircleCollider2D>().radius /*/ 10);
        List<ColorBall> tempColor = new List<ColorBall>();
        for (int i = 0; i < hitRang.Length; i++)
        {
            ColorBall tempColorBall = hitRang[i].transform.GetComponent<ColorBall>();
            if (tempColorBall != null)
            {

                if (!FinalPos.ContainsValue(tempColorBall.transform.position))
                {
                    if (tempColorBall != origin.transform.GetComponent<ColorBall>())
                    {
                        tempSameBall.Add(tempColorBall);
                    }
                    if (nowNeedFP.Count != 0)
                    {
                        FinalPos.Add(nowNeedFP[0], tempColorBall.transform.position);
                        nowNeedFP.RemoveAt(0);
                    }

                }
            }
        }
        if (dyBall.Count == 0 && nowNeedFP.Count == 0)
        {
            FenPwezhi();
            return;
        }
        if (nowNeedFP.Count == 0)
        {
            GetColorList();
            CollectColorBall(RomdBall().transform);//随机一个位置集中分配
            return;
        }
        else
        {
            for (int i = 0; i < tempSameBall.Count; i++)
            {
                if (nowNeedFP.Count == 0) return;
                CollectColorBall(tempSameBall[i].transform);
            }
        }
    }

    /// <summary>
    /// 魔法刷新 分配位置
    /// </summary>
    public void FenPwezhi()
    {

        foreach (var item in FinalPos.Keys)
        {
            // item.transform.GetComponent<Collider2D>().enabled = true;
            // //olorBalls[i].GetComponent<Collider2D>().enabled = false;
            //item.transform.GetComponent<Rigidbody2D>().mass = 1;
            item.transform.DOMove(FinalPos[item], 0.5F).SetEase(Ease.InCirc);
        }
        FinalPos.Clear();
    }
    public List<Ball> balls = new List<Ball>();

    /// <summary>
    /// 魔法刷新 获取彩色列表
    /// </summary>
    public void GetColorList()
    {
        //Debug.LogError("调用几次");
        nowNeedFP.Clear();
        SortType sortType = SortType.Default;
        if (dyBall.Count != 0)
        {
            foreach (var item in dyBall.Values)
            {
                sortType = item[0].sort;
                break;
            }
            nowNeedFP = dyBall[sortType];
            dyBall.Remove(sortType);
        }
    }

    public List<ColorBall> nowNeedFP = new List<ColorBall>();
    Dictionary<SortType, List<ColorBall>> dyBall = new Dictionary<SortType, List<ColorBall>>();
    List<ColorBall> tempAdjBall = new List<ColorBall>();
    Dictionary<Ball, Vector3> FinalPos = new Dictionary<Ball, Vector3>();

    public void FlyZeroPos()
    {

        for (int i = 0; i < colorBalls.Count; i++)
        {
            colorBalls[i].transform.DOMove(Vector3.zero, 0.5F).SetEase(Ease.Linear);
            //colorBalls[i].GetComponent<Collider2D>().enabled = false;
            //colorBalls[i].GetComponent<Rigidbody2D>().mass = 1;
        }

    }


    /// <summary>
    /// 邻近的位置
    /// </summary>
    /// <param name="change"></param>
    public void AdjacentPos(bool change = true)
    {

        dyBall.Clear();
        nowNeedFP.Clear();
        FinalPos.Clear();
        tempAdjBall = colorBalls;

        foreach (var item in tempAdjBall)
        {
            if (dyBall.ContainsKey(item.sort))
            {
                dyBall[item.sort].Add(item);
            }
            else
            {
                List<ColorBall> balls = new List<ColorBall>();
                dyBall.Add(item.sort, balls);
            }
        }
        if (change)
        {
            GetColorList();
            CollectColorBall(tempAdjBall[0].transform);
        }
    }


    /// </summary>
    public void EMostOfSameColor(Vector3 orgin)
    {
        float time = 0;
        AdjacentPos(false);
        SortType sortType = SortType.Default;
        int num = 0;
        foreach (var item in dyBall.Values)
        {
            if (item.Count >= num)
            {
                Debug.Log(item.Count);
                num = item.Count;
                sortType = item[0].sort;
            }
        }
        List<ColorBall> will = new List<ColorBall>();
        List<Collider2D> collider2Ds = new List<Collider2D>();
        foreach (var item in dyBall[sortType])
        {
            collider2Ds.Add(item.GetComponent<Collider2D>());
            EliminateGoal(item,will);
            time = DynamicMgr.Instance.StarFlyBoom(orgin, item.transform, Pool.StarFly);
        }


        Vector3 POS = Vector3.zero;
        List<ColorBall> temp = dyBall[sortType];

        Dictionary<Vector3, int> propDic = new Dictionary<Vector3, int>();
        ColorBall color= temp[0];


        temphh.Clear();
        tempzz.Clear();

        for (int i = 0; i < temp.Count; i++)
        {
            //if (i < temp.Count - 1)
            //{
            if (!tempzz.Contains(temp[i]))
            {
                Get(temp[i]);
                propDic.Add(temp[i].transform.position,temphh.Count);
                temphh.Clear();
            }
           
            

            //if (Vector3.Distance(temp[i].transform.position, color.transform.position) <= (radius + color.GetComponent<CircleCollider2D>().radius)*(i+1))
            //    {
            //    Debug.Log(i+"  距离："+ Vector3.Distance(temp[i].transform.position, color.transform.position)+" KK "+(radius + color.GetComponent<CircleCollider2D>().radius));
            //        LIANZHEci++;
            //    color = temp[i];
            //    }
            //    else
            //    {
            //        if (LIANZHEci>=4)
            //        {
            //            propDic.Add(color.transform.position, LIANZHEci);
            //        }
            //        color = temp[i];
            //        LIANZHEci = 0;
            //    }
            ////}
            //if (i== temp.Count-1)
            //{
            //    if (LIANZHEci >= 4)
            //    {
            //        propDic.Add(color.transform.position, LIANZHEci);
            //    }
            //}
        }
        


        UniRx.Observable.TimeInterval(System.TimeSpan.FromSeconds(time)).Subscribe(_ =>
        {
            foreach (var item in propDic.Keys)
            {
                var needSpawnNum = propDic[item];
                if (needSpawnNum >= 4 && needSpawnNum <= 5)
                {
                    //Observable.Timer(System.TimeSpan.FromSeconds(0.34F)).Subscribe(_ => {
                    DelayedCreatePorp(Porp_Size.小, item, SortType.Red, 0);
                    //});

                }
                else if (needSpawnNum >= 6 && needSpawnNum <= 7)
                {
                    //Observable.Timer(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ => {
                    DelayedCreatePorp(Porp_Size.中, item, SortType.Red, 0);
                    //});

                }
                else if (needSpawnNum > 7)
                {
                    //Observable.Timer(System.TimeSpan.FromSeconds(0.59F)).Subscribe(_ => {
                    DelayedCreatePorp(Porp_Size.大, item, SortType.Red, 0);
                    //});
                }
            }
            AudioMgr.Instance.PlaySFX("蓄力技能消除方块");
            RangeElimination(collider2Ds.ToArray());
            if (!GameManager.Instance.OverGame)
            {
                CameraManager.Instance.SetShake(PropManger.Instance.GetShackLevel(SortType.Red.ToString(), 3, Porp_Size.大), PropManger.Instance.GetShackTime(SortType.Red.ToString(), 3, Porp_Size.大));
            }

            ElimintNoColorlBall();
        });

    }

    List<ColorBall> temphh = new List<ColorBall>();
    List<ColorBall> tempzz = new List<ColorBall>();

    public void Get(ColorBall colorBall) {
        Collider2D[] hitRang = new Collider2D[] { };
        List<Ball> tempSameBall = new List<Ball>();
        hitRang = Physics2D.OverlapCircleAll(colorBall.transform.position, radius + colorBall.GetComponent<CircleCollider2D>().radius / 2);
        foreach (var item in hitRang)
        {
            ColorBall color = item.transform.GetComponent<ColorBall>();
            if (color!=null&&color.typeName== colorBall.typeName)
            {
                if (!temphh.Contains(color))
                {
                    if (colorBall != color)
                    {
                        tempSameBall.Add(color);
                    }
                    else
                    {
                        temphh.Add(colorBall);
                        tempzz.Add(colorBall);
                    }
                }
            }
        }
        if (tempSameBall.Count>0)
        {
            for (int i = 0; i < tempSameBall.Count; i++)
            {
                Get(tempSameBall[i].GetComponent<ColorBall>());
            }
        }
    }


    public void EliminateGoal(ColorBall origin,List<ColorBall> balls)
    {
        Collider2D[] hitRang = new Collider2D[] { };
        hitRang = Physics2D.OverlapCircleAll(origin.transform.position, radius + origin.GetComponent<CircleCollider2D>().radius / 2);
        for (int i = 0; i < hitRang.Length; i++)
        {
            if (hitRang[i].gameObject.GetComponent<MoneyBall>() != null && hitRang[i].gameObject.GetComponent<MoneyBall>().can)
            {
                hitRang[i].gameObject.GetComponent<MoneyBall>().Eliminat();
                continue;
            }
            if (hitRang[i].CompareTag("Ball"))
            {
                Ball tempBall = hitRang[i].gameObject.GetComponent<Ball>();
                if (tempBall.ballType == origin.ballType && tempBall.sort == origin.sort && !tempBall.isEliminat)
                {
                    //if (!balls.Contains())
                    //{
                    //    if (nowBall != tempBall)
                    //    {
                    //        tempSameBall.Add(tempBall);
                    //    }
                    //    else
                    //    {
                    //        ClickSameBall.Add(nowBall);
                    //    }
                    //}


                }
                 if (tempBall.ballType == BallType.SizeBall || (tempBall.ballType == BallType.Chocolate && tempBall.sort == SortType.Default) || tempBall.ballType == BallType.CornKernel || tempBall.ballType == BallType.PopCorn || tempBall.ballType == BallType.Hive || tempBall.ballType == BallType.SugarCube)
                {
                    if (!ClickSpecailBall.Contains(tempBall))
                    {
                        ClickSpecailBall.Add(tempBall);
                    }
                }
            }
        }
    }



    //创建批数下标
    int nowIndex = 0;

    int willNum;
    /// <summary>
    /// 创建球
    /// </summary>
    public void CreateBall(float downInterval, bool IsLimit = true)
    {
        CatManager.Instance.CreatCat();
        if (batchDownList.Count == 0) return;
        if (isRewardTime)
        {
            nowIndex = 0;
            batchDownList.Clear();
            return;
        }
        //}

        var downpos = RomdDownPos();
        //随机位置
        if (IsLimit)
        {
            downpos = RomdDownPos(batchx, batchy);
        }

        //通过批数掉落完毕
        if (nowIndex >= batchDownList.Count)
        {
            nowIndex = 0;
            batchDownList.Clear();
            return;
        }

        var ballname = GetParfabName(batchDownList[nowIndex].type);
        for (int i = 0; i < batchDownList[nowIndex].num; i++)
        {

            Transform temp = Pool.Instance.Spawn(Pool.Ball_PoolName, ballname);
            temp.SetParent(level.AllBallParent);
            temp.gameObject.SetActive(true);
            temp.GetComponent<Ball>().Init(batchDownList[nowIndex].color, batchDownList[nowIndex].isFix, batchDownList[nowIndex].Gern);
            if (temp.GetComponent<Ball>().ballType == BallType.ColorBall)
            {

                colorBalls.Add(temp.GetComponent<ColorBall>());

            }
            if (IsLimit)
            {
                temp.transform.position = RomdRownPosLimit(downpos);
            }
            else
            {
                temp.transform.position = RomdDownPos();
            }
        }
        if (nowIndex < batchDownList.Count - 1)
        {
            nowIndex++;
            Observable.TimeInterval(System.TimeSpan.FromSeconds(downInterval)).Subscribe(_ =>
            {
                CreateBall(downInterval, IsLimit);
            });
        }
        else
        {
            nowIndex = 0;
            batchDownList.Clear();
        }
        if (NeedSpwanIce > 0)
        {
            RomdIce();
        }

    }
    public Transform GetBestDownPos()
    {

        //if ()
        //{

        //}
        if (colorBalls.Count == 0) return null;
        GameManager.Instance.colorBalls.Sort((x, y) => x.transform.position.y.CompareTo(y.transform.position.y));
        ///*f*/oreach (var item in GameManager.Instance.level.AllBallParent.)
        //{
        //Vector3.Lerp
        //}
        //GameManager.Instance.colorBalls[0].transform= vector3
        return GameManager.Instance.colorBalls[0].transform;
    }

    public Vector2 RomdDownPos(float limintx = 0, float liminty = 0)
    {

        //Transform transform = level.downPos[LevelStep].nowPos[UnityEngine.Random.Range(0, level.downPos[LevelStep].nowPos.Count)];
        Transform transform = level.transform.Find("DownPoint/DownLine");
        var y1 = 0F;
        if (CameraManager.Instance.tow /*&& !CameraManager.Instance.CanMoveCamera*/)
        {
            y1 = CameraManager.Instance.transform.position.y + Screen.height / 100 / 2 + 0.5F;
        }
        else
        {
            y1 = transform.position.y;
        }
        float rangx =  transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;

        float rangY = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        float x = UnityEngine.Random.Range(transform.position.x+ rangx, transform.position.x - rangx);

        //if ()
        //{

        //}
        float y = /*(Screen.height/100/2)+*/UnityEngine.Random.Range(y1 - rangY, y1 + rangY)/*+0.5F*/;


        //Debug.Log((transform.position.x + transform.GetComponent<SpriteRenderer>().bounds.size.x / 2)+"das"+(transform.position.x - transform.GetComponent<SpriteRenderer>().bounds.size.x + limintx)+"??"+ transform.GetComponent<SpriteRenderer>().bounds.size.x / 2);
        //Debug.Log("dddd"+collider.bounds.size);

        Vector3 orgin = new Vector3(x, y,0);
        var hit = Physics2D.Raycast(orgin+new Vector3 (0,0,-10), Vector2.zero, 10);
        if (hit.collider!=null)
        {
            if (hit.collider.transform.name == "DX")
            {
                return RomdDownPos();

            }
            else {
                return orgin;
            }
        }
        

        
        //     XDebug.LogError("图片大小："+new Vector2(transform.GetComponent<SpriteRenderer>().bounds.size.x / 2, transform.GetComponent<SpriteRenderer>().bounds.size.y / 2) +"  生成点"+orgin);

        //Debug.Log("生成远点"+ orgin);
        //var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(orgin), Vector2.zero, 100, LayerMask.GetMask("Terrain"));
        //Debug.Log("Hit" + hit.collider.name);
        //if (hit.collider != null && hit.collider.transform.name == "DX")
        //{
        //    //return RomdDownPos(limintx, liminty);
        //}
        //else
        //{
        return orgin;
    //}

    }

    public Vector2 SpecailRomdDownPos(List<Transform> all, float limintx = 0, float liminty = 0)
    {

        var now = UnityEngine.Random.Range(0, all.Count);
        Transform transform = all[now];
        float rangx = transform.position.x + transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float rangx2 = transform.position.x - transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float rangY = transform.position.y + transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float rangY2 = transform.position.y - transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        float x = UnityEngine.Random.Range(rangx, rangx2);
        float y = UnityEngine.Random.Range(rangY, rangY2);
        //Debug.Log("eqweqw"+ new Vector2(rangx, rangY));
        return new Vector2(x, y);
    }

    /// <summary>
    /// 通过原点缩小范围随机位置
    /// </summary>
    /// <returns></returns>
    public Vector2 RomdRownPosLimit(Vector2 orgin)
    {

        Transform transform = level.downPos[LevelStep].nowPos[UnityEngine.Random.Range(0, level.downPos[LevelStep].nowPos.Count)];
        float Tx = UnityEngine.Random.Range(orgin.x + batchx, orgin.x - batchx);
        float Ty = UnityEngine.Random.Range(orgin.y + batchy, orgin.y - batchy);
        //Debug.Log(new Vector2(Tx, Ty));

        return new Vector2(Tx, Ty);

    }


    /// <summary>
    /// 打乱数组
    /// </summary>
    /// <param name="myList"></param>
    /// <returns></returns>
    private List<T> ListRandom<T>(List<T> myList)
    {
        System.Random ran = new System.Random();
        List<T> newList = new List<T>();
        int index = 0;
        T temp;
        for (int i = 0; i < myList.Count; i++)
        {
            index = ran.Next(0, myList.Count - 1);
            if (index != i)
            {
                temp = myList[i];
                myList[i] = myList[index];
                myList[index] = temp;
            }
        }
        return myList;
    }

    /// <summary>
    /// 判断批数数组
    /// </summary>
    /// <returns></returns>
    public int JudgeBatch(int needJudgeNum)
    {

        for (int i = 0; i < level.downRangs.Count; i++)
        {
            if (needJudgeNum > level.downRangs[i].minRang && needJudgeNum <= level.downRangs[i].maxRang)
            {
                return i;
            }
        }
        return 0;
    }


    int RandColrNum = 0;
    public void InitWeights()
    {
        RandColrNum = 0;
        weightsSum = 0;
        SpecailBallInfo.Clear();
        for (int i = 0; i < level.ballWeights.Count; i++)
        {
            if (!level.ballWeights[i].isFixNum)
            {

                //ballTypes.Add(level.ballWeights[i].ballType);
                weightsSum += level.ballWeights[i].weights;
            }
            else if (!level.ballWeights[i].isSpecialDown)
            {
                RandColrNum += level.ballWeights[i].fixNum;
                if (!fixNumBallList.Contains(level.ballWeights[i]))
                {
                    fixNumBallList.Add(level.ballWeights[i]);
                    //fixBall.Add(level.ballWeights[i].ballType, level.ballWeights[i].colorType);
                }
            }
            if (level.ballWeights[i].isSpecialDown)
            {
                //RandColrNum += level.ballWeights[i].fixNum;

                SpecailBallInfo.Add(level.ballWeights[i].ballType.ToString()+"_" + level.ballWeights[i].colorType.ToString(), level.ballWeights[i]);
                SpecailBallNum.Add(level.ballWeights[i].ballType.ToString() +"_"+ level.ballWeights[i].colorType.ToString(), level.ballWeights[i].viewNum);
            }
        }
        RandColrNum = level.fallAllBall - RandColrNum;
        Debug.Log("随机颜色数量" + RandColrNum);
    }

    /// <summary>
    ///   固定类型List
    /// </summary>
    /// <typeparam name="BallType"></typeparam>
    /// <typeparam name="SortType"></typeparam>
    /// <param name=""></param>
    /// <returns></returns>
    //Dictionary<BallType, SortType> fixBall = new Dictionary<BallType, SortType>();
    //固定类型权重
    List<BallWeights> fixNumBallList = new List<BallWeights>();

    //权重总和
    int weightsSum;
    /// <summary>
    /// 通过权重返回球类型
    /// </summary>
    /// <returns></returns>
    public WillDown RomdWeights(int nim)
    {

        var temp = UnityEngine.Random.Range(0, weightsSum);
        var x = 0;
        for (int i = 0; i < level.ballWeights.Count; i++)
        {
            x += level.ballWeights[i].weights;
            if (temp < x)
            {
                WillDown willDown = new WillDown(level.ballWeights[i].ballType, level.ballWeights[i].colorType, level.ballWeights[i].isFixNum, nim);
                if (level.ballWeights[i].ballType == BallType.SugarCube)
                {
                    willDown.Gern = level.ballWeights[i].Gern;
                }
                return willDown;
            }
        }
        Debug.LogError("随机生成空");
        return null;
    }

    private void OnApplicationFocus(bool focus)
    {
        //Debug.Log("focus" + focus);
        
        if (!focus)
        {
            DataManager.Instance.SaveGameData();//退出保存数据
        }
        else
        {

            if (AdControl.Instance.canAwake && DataManager.Instance.data.UnlockLevel>5 && UIManager.Instance.GetBase<MainPanel>()!=null)
            {
                if (!UIManager.Instance.GetBase<MainPanel>().gameObject.activeInHierarchy) return;
                
                AdControl.Instance.ShowIntAd("game_awaken");
                //Observable.Timer(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ =>
                //{
                //    try
                //    {
                //        ComplwteAdGameAwakeCallBack();
                //    }
                //    catch
                //    {

                //        Debug.LogError("唤醒红包出现异常");
                //    }

                //});


            }
            else
            {
                AdControl.Instance.canAwake = true;
            }
        }

    }
    //唤醒红包回调
    public void ComplwteAdGameAwakeCallBack()
    {
        //var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
        //var rewardVoucher = (float)rewardRedIcon / (RedWithdrawData.Instance.NowCanCash * 10000);
        var popup1 = UIManager.Instance.ShowPopUp<OpenRedPopup3>();
        popup1.OnOpen("game_awaken_video", 0, () => {

            Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ =>
            {
                var rewardRedIcon = RedWithdrawData.Instance.GetRedIcon();
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
                });

                popup1.defult.SetActive(false);
            });
        },
        () =>
        {
            //关闭回调
            Debug.Log("关闭红包一级界面");
            popup1.defult.SetActive(false);
        });

        Debug.Log("打开唤醒红包");
    }
}
