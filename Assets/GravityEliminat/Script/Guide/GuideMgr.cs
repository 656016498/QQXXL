using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;
using UniRx;
using UnityEngine.UI;
using System;
//using Assets.Script.Main.FitmentMgr;
//using GameSchdule;

public class GuideMgr
{
    #region 数据层
    private static GuideMgr instance;
    public static GuideMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GuideMgr();
            }
            return instance;
        }
    }
    private const string key = "GuideMgr_NoMain";
    public GuideData GetGuideData { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init() { }

    /// <summary>
    /// 完成该引导
    /// </summary>
    /// <param name="guideID"></param>
    public void CompleteGuide(int guideID)
    {
        if (GetGuideData.mDic_guideinfo[guideID].IsComplete)
        {
            return;
        }
        GetGuideData.mDic_guideinfo[guideID].IsComplete = true;
        SaveData();
        //新手引导打点
        UmengDisMgr.Instance.CountOnPeoples("newer_guide", string.Format("Step_{0}", guideID));
    }

    /// <summary>
    /// 判断该引导是否完成
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool GuideIsComplete(int id)
    {
        if (!GetGuideData.mDic_guideinfo.ContainsKey(id))
        {
            return true;        //如果引导中没有改步,则默认完成该步,后期如果想删除某一步,则直接移除该id即可
        }
        return GetGuideData.mDic_guideinfo[id].IsComplete;
    }

    //打点每一步信息
    public string ReturnGuideInfo(int id)
    {
        if (id == 1)
        {
            return "第一关:欢迎来到游戏";
        }
        else if (id == 3)
        {
            return "第一关:开始闯关赚钱";
        }
        else if (id == 4)
        {
            return "第一关:步数为0之前消除关卡目标";
        }
        else if (id==5)
        {
            return "第一关:点击匹配两个球";
        }
        else if (id == 6)
        {
            return "第二关：点击匹配四个球";
        }
        else if (id == 7)
        {
            return "第三关：相同技能合成";
        }
        else if (id == 8)
        {
            return "红包一级界面引导";
        }
        else if (id == 9)
        {
            return "红包二级界面引导";
        }
        else if (id == 10)
        {
            return "第二关：点击开始闯关赚钱";
        }
        else if (id == 11)
        {
            return "第三关:十字炸弹引导";
        }
        else if (id == 12)
        {
            return "第四关:组合两枚十字炸弹";
        }
        else if (id == 13)
        {
            return "第四关：点击区域炸弹";
        }
        else if (id == 14)
        {
            return "第五关：漩涡球引导";
        }
        else if (id == 15)
        {
            return "第五关：消除漩涡球引导";
        }
        else if (id == 16)
        {
            return "第二关：点击横向炸弹消除";
        }
        return null;
    }

    public GuideMgr()
    {
        LoadData();

        InitGuideInfo();
    }

    private void InitGuideInfo()
    {
        GetGuideData.AddInfo(1);
        GetGuideData.AddInfo(2);
        GetGuideData.AddInfo(3);
        GetGuideData.AddInfo(4);
        //GetGuideData.AddInfo(5);
        //GetGuideData.AddInfo(6);
        //GetGuideData.AddInfo(7);
        GetGuideData.AddInfo(8);
        GetGuideData.AddInfo(9);
        //GetGuideData.AddInfo(10);
        //GetGuideData.AddInfo(11);
        //GetGuideData.AddInfo(12);
        //GetGuideData.AddInfo(13);
        //GetGuideData.AddInfo(14);
        ////GetGuideData.AddInfo(15);
        //GetGuideData.AddInfo(16);
        //GetGuideData.AddInfo(17);
        //GetGuideData.AddInfo(18);
        //GetGuideData.AddInfo(19);
        //GetGuideData.AddInfo(20);

        #region




        ////消除场景内引导id
        //GetGuideData.AddInfo(21);
        //GetGuideData.AddInfo(22);
        //GetGuideData.AddInfo(23);
        //GetGuideData.AddInfo(24);
        //GetGuideData.AddInfo(25);
        //GetGuideData.AddInfo(26);
        //GetGuideData.AddInfo(27);
        //GetGuideData.AddInfo(28);

        //GetGuideData.AddInfo(101);
        //GetGuideData.AddInfo(102);
        //GetGuideData.AddInfo(103);
        //GetGuideData.AddInfo(104);
        //GetGuideData.AddInfo(105);
        //GetGuideData.AddInfo(106);
        //GetGuideData.AddInfo(107);
        //GetGuideData.AddInfo(108);
        //GetGuideData.AddInfo(109);
        //GetGuideData.AddInfo(110);
        //GetGuideData.AddInfo(111);
        //GetGuideData.AddInfo(112);
        //GetGuideData.AddInfo(113);
        //GetGuideData.AddInfo(114);
        //GetGuideData.AddInfo(115);
        //GetGuideData.AddInfo(116);


        //GetGuideData.AddInfo(303);
        //GetGuideData.AddInfo(304);
        //GetGuideData.AddInfo(305);
        #endregion
    }

    public bool IsComtetAllGuide()
    {
        foreach (var item in GetGuideData.mDic_guideinfo)
        {
            if (!item.Value.IsComplete)
            {
                return false;
            }
        }

        return true;
    }

    public void CloseGuide()
    {
        for (int i = 1; i <=15; i++)
        {
            if (GetGuideData.mDic_guideinfo.ContainsKey(i))
            {
                CompleteGuide(i);
            }              
        }
    }

    private void LoadData()
    {
        GetGuideData = SaveGame.Load<GuideData>(key);
        if (GetGuideData == null)
        {
            GetGuideData = new GuideData();
            SaveData();
        }
    }

    private void SaveData()
    {
        SaveGame.Save(key, GetGuideData);
    }
    #endregion

    private void PlayGuideAudio(string name) 
    {
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0f)).Subscribe(_ => {
            AudioMgr.Instance.PlaySFX(name);
        });
    }
    //关闭当前音效
    private void StopGuideAudio(string name)
    {
        if (AudioController.IsPlaying(name))
        {
            AudioController.Stop(name);
        }
    }
    #region 逻辑层
    //第一步展示欢迎来到--游戏
    public void ShowGuide_1(Action onCompleteCallback)
    {
        var guide_1 = GuideMgr.Instance.GuideIsComplete(1);
        if (guide_1)
        {
            onCompleteCallback.Run();
            return;
        }
        Observable.TimeInterval(System.TimeSpan.FromSeconds(1))
            .Subscribe(_ =>
            {                
                GuideMgr.instance.CompleteGuide(1);
                UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
                {
                    PlayGuideAudio("语音1");
                    panel.SetInfo("欢迎来到<color=#A11C23>《球球消消乐》</color>!", "");
                    panel.HanderControl.Hide();
                    bool isClick = false;
                    panel.ClickArea.onClick.AddListener(() =>
                    {
                        if (isClick) return;
                        #region
                        ////第二步
                        //var guide_2 = GuideMgr.Instance.GuideIsComplete(2);
                        //if (!guide_2)
                        //{
                        //    PlayGuideAudio("语音2");
                        //    panel.SetInfo("点击这里开始闯关赚钱吧~", "");
                        //    GuideMgr.instance.CompleteGuide(2);
                        //    return;
                        //}
                        #endregion
                        isClick = true;
                        panel.Hide();
                        onCompleteCallback.Run();
                        //移除點擊事件
                        //panel.ClickArea.onClick.RemoveAllListeners();
                    });
                });
            });
    }

    //展示第三步
    public void ShowGuide_3(Action onCompleteCallback)
    {
      UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);
      Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F))
     .Subscribe(_ =>
     {
        var mPanel = UIManager.Instance.GetBase<JoinPop>();
        if (mPanel == null)
        {
            return;
        }
        var guide3 = Instance.GuideIsComplete(3);
        if (guide3) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            StopGuideAudio("语音1");
            PlayGuideAudio("语音2");
            panel.SetInfo("点击这里<color=#A11C23>开始闯关赚钱</color>吧~", "", mPanel.BeginBtn.transform, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                panel.HanderControl.SetPos(mPanel.BeginBtn.transform.position);
                panel.HanderControl.Show();
            });
            bool isClick = false;
            mPanel.BeginBtn.onClick.AddListener(() =>
            {
                if (isClick) return;
                CompleteGuide(3);
                GuideMgr.instance.SaveData();
                isClick = true;
                panel.HanderControl.Hide();
                panel.Hide();
                panel.TempControl.RecoverAllTemp();
                StopGuideAudio("语音2");
                onCompleteCallback.Run();
            });
          });
    });
  }
    //展示第四步
    public void ShowGuide_4(Action onCompleteCallback)
    {
        var mPanel = UIManager.Instance.GetBase<GamePanel>();
        if (mPanel == null)
        {
            return;
        }
        var guide4 = Instance.GuideIsComplete(4);
        if (guide4) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            panel.SetBombUpShow();
            //StopGuideAudio("语音2");
            PlayGuideAudio("语音3");
            panel.SetInfo("在<color=#A11C23>步数为0之前消除关卡目标</color>即可胜利通关!", "", mPanel.topTip.transform, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                panel.HanderControl.SetPos(mPanel.topTip.transform.position);
                panel.HanderControl.Hide();
            });
            bool isClick = false;
            panel.ClickArea.onClick.AddListener(() =>
            {
                if (isClick) return;
                CompleteGuide(4);
                GuideMgr.instance.SaveData();
                isClick = true;
                panel.HanderControl.Hide();
                panel.Hide();
                panel.TempControl.RecoverAllTemp();
                onCompleteCallback.Run();
                StopGuideAudio("语音3");
                ////移除點擊事件
                //panel.ClickArea.onClick.RemoveAllListeners();
            });
        });

    }
    //获取当前球
    public List<GuideObj> GetGuideObj(int stepIndex=0) {
        List<GuideObj> temp = new List<GuideObj>();
        GuideObj[] all= GameManager.Instance.allPlaneParent.GetComponentsInChildren<GuideObj>();
        if (all.Length!=0)
        {
            foreach (var item in all)
            {
                if (item.step==stepIndex)
                {
                    temp.Add(item);
                }
            }
        }
        return temp;
    }
    public List<Transform> mList = new List<Transform>();
    //获取第三关炸弹
    public GameObject GetBomb()
    {
        //foreach (Prop item in GameManager.Instance.allBallParent.GetComponentInChildrens<Prop>())
        //{

        //}
        return PropManger.Instance.allProp[0].gameObject;
    }
    //获取旋涡球
    public GameObject GetArriveBall()
    {
#if Easy
        return GameManager.Instance.allBallParent.Find("Arrive").gameObject;
#else
        return GameManager.Instance.allBallParent.Find("Arrive (1)").gameObject;
#endif

    }
    //获取旋涡线
    public GameObject GetArriveLine()
    {

        return GameManager.Instance.level.transform.Find("Terrain/ArriveLine (2)").gameObject;

    }

    //展示第五步--(第一关匹配两个球)
    public void ShowGuide_5(Action onCompleteCallback)
    {
        if (mList.Count > 0)
        {
            mList.Clear();
        }
        var mball = GetGuideObj();
        foreach (var item in mball)
        {
            if (item.GetComponent<Image>() == null)
            {
                var mImage=item.gameObject.AddComponent<Image>();
                mImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                var mBtn  = item.gameObject.AddComponent<Button>();
                mList.Add(item.transform);
            }
        }
        var guide5 = Instance.GuideIsComplete(5);
        if (guide5) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            StopGuideAudio("语音3");
            PlayGuideAudio("语音4");
            panel.HanderControl.SetPos(mList[0].transform.position);
            panel.SetInfo2("匹配<color=#A11C23>至少2个球</color>来收集它们!", "", mList, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                //panel.HanderControl.SetPos(mList[0].transform.position);
                panel.HanderControl.Show();
            });
            
            bool isClick = false;
            foreach (var item in mList)
            {
                item.GetComponent<Button>().onClick.AddListener(() => {

                    if (isClick) return;
                    GameManager.Instance.CollectSameRangBall(mList[0]);
                    GameManager.Instance.ElimintBall(mList[0].GetComponent<Ball>());
                    CompleteGuide(5);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    //panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();

                    ///第五步后续
                    StopGuideAudio("语音4");
                    PlayGuideAudio("语音5");
                    panel.SetInfo("继续匹配并完成关卡目标吧。", "");
                });
            }

            panel.ClickArea.onClick.AddListener(() => {

                if (!Instance.GuideIsComplete(5)) return;
                panel.Hide();
                panel.ClickArea.onClick.RemoveAllListeners();
            });
        });
    }
   
    //展示第六步--(第二关匹配四个球)
    public void ShowGuide_6(Action onCompleteCallback)
    {
        if (mList.Count > 0)
        {
            mList.Clear();
        }
        var mball = GetGuideObj();
        foreach (var item in mball)
        {
            if (item.GetComponent<Image>() == null)
            {
                var mImage = item.gameObject.AddComponent<Image>();
                mImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                var mBtn = item.gameObject.AddComponent<Button>();
                mList.Add(item.transform);
            }
        }
        var guide6 = Instance.GuideIsComplete(6);
        if (guide6) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            StopGuideAudio("语音10");
            PlayGuideAudio("语音11");
            panel.HanderControl.SetPos(mList[0].transform.position);
            panel.SetInfo2("匹配4个或以上数量的红球来制造<color=#A11C23>直线炸弹</color>!", "", mList, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                //panel.HanderControl.SetPos(mList[0].transform.position); 
                panel.HanderControl.Show();
            });

            bool isClick = false;
            foreach (var item in mList)
            {
                item.GetComponent<Button>().onClick.AddListener(() => {

                    if (isClick) return;
                    GameManager.Instance.CollectSameRangBall(mList[0]);
                    GameManager.Instance.ElimintBall(mList[0].GetComponent<Ball>());
                    CompleteGuide(6);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    //panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();
                    ///下一步后续
                    StopGuideAudio("语音11");
                    //PlayGuideAudio("语音5");
                    //panel.SetInfo("继续匹配并完成关卡目标吧。", "");
                });
            }
            //panel.ClickArea.onClick.AddListener(() => {

            //    if (!instance.GuideIsComplete(6)) return;
            //    panel.Hide();
            //    panel.ClickArea.onClick.RemoveAllListeners();
            //});
        });
    }
    //展示第六步后续
    public void ShowGuide_16(Action onCompleteCallback)
    {
        var mpanel = UIManager.Instance.ShowPopUp<GuidePanel>();
        mpanel.Mask.ShowCanvasGroup();
        Observable.TimeInterval(System.TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            var mball1 = GetBomb(); 
            Debug.Log("GetBomb" + mball1.name);
            //var mball1 = GameObject.Instantiate(mball);

            if (mball1.GetComponent<Image>() == null)
            {
                var sprite = mball1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                var mImage = mball1.gameObject.AddComponent<Image>();
                mImage.sprite = sprite;
                mball1.gameObject.AddComponent<Button>();
                mball1.GetComponent<RectTransform>().sizeDelta = new Vector2(0.8f, 0.5f);
            }

            var guide16 = Instance.GuideIsComplete(16);
            if (guide16) return;
            UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
            {
                var pos = panel.mAreaPanelDefalutPos;
                pos.y -= 100;
                panel.SetDirAreaPos(pos);
                panel.SetBomb2Pos(mball1.transform.position);
                StopGuideAudio("语音11");
                PlayGuideAudio("语音18");
                panel.HanderControl.SetPos(mball1.transform.position);
                panel.SetInfo("真棒！点击它来清除<color=#A11C23>左右两个方向</color>的球球吧！", "", mball1.transform, () =>
                {
                    //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                    //panel.HanderControl.SetPos(mball1.transform.position);
                    panel.HanderControl.Show();
                });

                bool isClick = false;
                //panel.ClickArea.onClick.AddListener(() => {
                mball1.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (isClick) return;
                    panel.HideBomb2();
                    //销毁组件
                    UnityEngine.Object.Destroy(mball1);
                    //模拟爆炸
                    mball1.transform.GetComponent<EliminatBomb>().des = true;
                    mball1.transform.GetComponent<EliminatBomb>().OnClick();

                    CompleteGuide(16);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    //panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();

                    //后续
                    StopGuideAudio("语音18");
                    PlayGuideAudio("语音5");
                    panel.SetInfo("继续匹配并完成关卡目标吧。", "");
                });

                panel.ClickArea.onClick.AddListener(() =>
                {
                    if (!instance.GuideIsComplete(16)) return;
                    panel.Hide();
                    panel.ClickArea.onClick.RemoveAllListeners();
                });
            });
        });
    }
    //展示第七步--(第三关 相同技能合成)
    public void ShowGuide_7(Action onCompleteCallback)
    {
        if (mList.Count > 0)
        {
            mList.Clear();
        }
        var mball = GetGuideObj();
        foreach (var item in mball)
        {
            if (item.GetComponent<Image>() == null)
            {
                var mImage = item.gameObject.AddComponent<Image>();
                mImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                var mBtn = item.gameObject.AddComponent<Button>();
                mList.Add(item.transform);
            }
        }
        var guide7 = Instance.GuideIsComplete(7);
        if (guide7) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            StopGuideAudio("语音5");
            PlayGuideAudio("语音12");
            panel.HanderControl.SetPos(mList[0].transform.position);
            panel.SetInfo2("组合2枚直线炸弹。", "", mList, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                //panel.HanderControl.SetPos(mList[0].transform.position);
                panel.HanderControl.Show();
            });

            bool isClick = false;
            foreach (var item in mList)
            {
                item.GetComponent<Button>().onClick.AddListener(() => {

                    if (isClick) return;
                    GameManager.Instance.CollectSameRangBall(mList[0]);
                    GameManager.Instance.ElimintBall(mList[0].GetComponent<Ball>());
                    CompleteGuide(7);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();
                    
                });
            }


            //panel.ClickArea.onClick.AddListener(() => {
            //    if (!instance.GuideIsComplete(7)) return;
            //     panel.Hide();
            //    panel.ClickArea.onClick.RemoveAllListeners();

            //});
        });
    }
    /// <summary>
    /// 红包开启引导
    /// </summary>
    public void ShowGuide_8(Action onCompleteCallback)
    {
        var gudie = Instance.GuideIsComplete(8);
        if (gudie) return;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                var redPanel = UIManager.Instance.GetBase<OpenRedPopup3>();
                //var handerpos = redPanel.btnOpen.transform.position;
                if (redPanel == null) return;

                UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
                {
                    var pos = panel.mAreaPanelDefalutPos;
                    pos.y -= 100;
                    panel.SetDirAreaPos(pos);
                    StopGuideAudio("语音5");
                    PlayGuideAudio("语音7");
                    panel.SetInfo("打开红包可获得<color=#A11C23>大量红包币</color>！", "", redPanel.parent, () =>
                    {
                        //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                        panel.HanderControl.SetPos(redPanel.btnOpen.transform.position);
                        panel.HanderControl.Show();
                    });
                    bool isClick = false;
                    redPanel.btnOpen.onClick.AddListener(() =>
                    {
                        if (isClick) return;
                        CompleteGuide(8);
                        GuideMgr.instance.SaveData();
                        isClick = true;
                        panel.HanderControl.Hide();
                        panel.Hide();
                        panel.TempControl.RecoverAllTemp();
                        onCompleteCallback.Run();
                    });

                   
                });

            });
    }
    ///红包二级引导
    public void ShowGuide_9(Action onCompleteCallback)
    {
        var gudie = Instance.GuideIsComplete(9);
        if (gudie) return;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2f))
            .Subscribe(_ =>
            {
                var redPanel = UIManager.Instance.GetBase<RedTwoPopup>();
                //var handerpos = redPanel.btnOpen.transform.position;
                if (redPanel == null) return;

                UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
                {
                    redPanel.btnWD.interactable = false;
                    var pos = panel.mAreaPanelDefalutPos;
                    pos.y -= 100;
                    panel.SetDirAreaPos(pos);
                    StopGuideAudio("语音7");
                    PlayGuideAudio("语音8");
                    panel.SetInfo("红包进度达到<color=#A11C23>100%</color>即可<color=#A11C23>立即提现</color>!", "", redPanel.downPro, () =>
                    {
                        //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                        panel.HanderControl.SetPos(redPanel.downPro.position);
                        panel.HanderControl.Hide();
                    });
                    bool isClick = false;
                    bool isShowNext = false;
                    panel.ClickArea.onClick.AddListener(() =>
                    {
                        redPanel.btnWD.interactable = true;
                        if (isClick) return;
                        CompleteGuide(9);
                        GuideMgr.instance.SaveData();
                        panel.TempControl.RecoverAllTemp();
                        if (!isShowNext)
                        {
                            StopGuideAudio("语音8");
                            PlayGuideAudio("语音9");
                            panel.SetInfo("红包币累计到达指定额度后,既可领取<color=#A11C23>现金大奖</color>！","", redPanel.up);
                            isShowNext = true;
                            return;
                        }
                        isClick = true;
                        //panel.HanderControl.Hide();
                        StopGuideAudio("语音9");
                        panel.Hide();
                        panel.TempControl.RecoverAllTemp();
                        onCompleteCallback.Run();
                        panel.ClickArea.onClick.RemoveAllListeners();
                    });
                    redPanel.btnClose.onClick.AddListener(() =>
                    {
                        //panel.HanderControl.Hide();
                        StopGuideAudio("语音9");
                        panel.Hide();
                        panel.TempControl.RecoverAllTemp();
                        onCompleteCallback.Run();
                        panel.ClickArea.onClick.RemoveAllListeners();
                    });
                    redPanel.btnClose2.onClick.AddListener(() =>
                    {
                        //panel.HanderControl.Hide();
                        StopGuideAudio("语音9");
                        panel.Hide();
                        panel.TempControl.RecoverAllTemp();
                        onCompleteCallback.Run();
                        panel.ClickArea.onClick.RemoveAllListeners();
                    });
                });

            });

    }

    //返回主界面再次强弹闯关界面
    public void ShowGuide_10(Action onCompleteCallback)
    {
        UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F))
       .Subscribe(_ =>
       {
           var mPanel = UIManager.Instance.GetBase<JoinPop>();
           if (mPanel == null)
           {
               return;
           }
           var guide10 = Instance.GuideIsComplete(10);
           if (guide10) return;
           UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
           {
               var pos = panel.mAreaPanelDefalutPos;
               pos.y -= 100;
               panel.SetDirAreaPos(pos);
               StopGuideAudio("语音9");
               PlayGuideAudio("语音10");
               panel.SetInfo("继续闯关赚钱吧!", "", mPanel.BeginBtn.transform, () =>
               {
                 //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                 panel.HanderControl.SetPos(mPanel.BeginBtn.transform.position);
                   panel.HanderControl.Show();
               });
               bool isClick = false;
               mPanel.BeginBtn.onClick.AddListener(() =>
               {
                   if (isClick) return;
                   CompleteGuide(10);
                   GuideMgr.instance.SaveData();
                   isClick = true;
                   panel.HanderControl.Hide();
                   panel.Hide();
                   panel.TempControl.RecoverAllTemp();
                   onCompleteCallback.Run();
               });
           });
       });
    }
    //第三关炸弹引导
    public void ShowGuide_11(Action onCompleteCallback)
    {
        var mpanel = UIManager.Instance.ShowPopUp<GuidePanel>();
        mpanel.Mask.ShowCanvasGroup();


        UIRoot.Instance.ShowMask();


        Observable.TimeInterval(System.TimeSpan.FromSeconds(1.2f)).Subscribe(_ =>
        {
           
            var mball = GetBomb();
            Debug.Log("GetBomb" + mball.name);
            if (mball.GetComponent<Image>() == null)
            {
                var sprite= mball.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                var mImage = mball.gameObject.AddComponent<Image>();
                mball.gameObject.AddComponent<Button>();
                mImage.sprite = sprite;
               
            }
        
        var guide11 = Instance.GuideIsComplete(11);
        if (guide11) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            StopGuideAudio("语音12");
            PlayGuideAudio("语音13");
            panel.SetBomb4Pos(mball.transform.position);
            panel.HanderControl.SetPos(mball.transform.position);
            panel.SetInfo("真棒！这样就变成<color=#A11C23>十字炸弹</color>了。点击它来清除四个方向的球球吧!", "", mball.transform, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                panel.HanderControl.Show();
            });
          
            
            UIRoot.Instance.HideMask();

            bool isClick = false;
            mball.GetComponent<Button>().onClick.AddListener(() => {

                    if (isClick) return;
                    panel.HideBomb4();
                  //模拟爆炸
                   mball.transform.GetComponent<EliminatBomb>().des = true;
                   mball.transform.GetComponent<Prop>().OnClick();

                    CompleteGuide(11);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    //panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();
                    //提示后续
                     StopGuideAudio("语音13");
                    PlayGuideAudio("语音5");
                    panel.SetInfo("继续匹配并完成关卡目标吧。", "");
                });


            panel.ClickArea.onClick.AddListener(() =>
            {
                if (!instance.GuideIsComplete(11)) return;
                panel.Hide();
                panel.ClickArea.onClick.RemoveAllListeners();

            });
        });
      });
    }
    //第四关组合两枚十字炸弹
    public void ShowGuide_12(Action onCompleteCallback)
    {
       
        if (mList.Count > 0)
        {
            mList.Clear();
        }
        var mball = GetGuideObj();
        foreach (var item in mball)
        {
            if (item.GetComponent<Image>() == null)
            {
                var mImage = item.gameObject.AddComponent<Image>();
                mImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                var mBtn = item.gameObject.AddComponent<Button>();
                mList.Add(item.transform);
            }
        }
        var guide12 = Instance.GuideIsComplete(12);
        if (guide12) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            
            StopGuideAudio("语音5");
            PlayGuideAudio("语音14");
            panel.HanderControl.SetPos(mList[0].transform.position);
            panel.SetInfo2("组合2枚十字炸弹。", "", mList, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                //panel.HanderControl.SetPos(mList[0].transform.position);
                panel.HanderControl.Show();
            });

            bool isClick = false;
            foreach (var item in mList)
            {

                item.GetComponent<Button>().onClick.AddListener(() => {

                    if (isClick) return;
                    //模拟爆炸
                    GameManager.Instance.CollectSameRangBall(mList[0]);
                    GameManager.Instance.ElimintBall(mList[0].GetComponent<Ball>());
                    CompleteGuide(12);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    //panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();
                    
                });
            }
          
        });
    }
    //第四关点击合成炸弹
    public void ShowGuide_13(Action onCompleteCallback)
    {
        UIRoot.Instance.ShowMask();
        //var mpanel = UIManager.Instance.ShowPopUp<GuidePanel>();
        //mpanel.Mask.ShowCanvasGroup();
        Observable.TimeInterval(System.TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
        {
            var mball = GetBomb();
            Debug.Log("GetBomb" + mball.name);
            if (mball.GetComponent<Image>() == null)
            {
                var sprite = mball.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                var mImage = mball.gameObject.AddComponent<Image>();
                mball.gameObject.AddComponent<Button>();
                mImage.sprite = sprite;
            }
            var guide13 = Instance.GuideIsComplete(13);
            if (guide13) return;
            UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
            {
                var pos = panel.mAreaPanelDefalutPos;
                pos.y -= 100;
                panel.SetDirAreaPos(pos);
                panel.SetBomb6Pos(mball.transform.position);
                StopGuideAudio("语音14");
                PlayGuideAudio("语音15");
                panel.HanderControl.SetPos(mball.transform.position);
                panel.SetInfo("棒极了！这样就变成<color=#A11C23>区域炸弹</color>了。点击它来清除范围内的大量球球吧。", "", mball.transform, () =>
                {
                    //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                    //panel.HanderControl.SetPos(mball.transform.position);
                    panel.HanderControl.Show();
                });
                UIRoot.Instance.HideMask();
                bool isClick = false;
                mball.GetComponent<Button>().onClick.AddListener(() => {

                    if (isClick) return;
                    panel.HideBomb6();
                    //模拟爆炸
                    mball.transform.GetComponent<EliminatBomb>().des = true;
                    mball.transform.GetComponent<Prop>().OnClick();
                    //移除图片和按钮
                    CompleteGuide(13);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();
                    //提示后续
                    StopGuideAudio("语音15");
                    PlayGuideAudio("语音5");
                    panel.SetInfo("继续匹配并完成关卡目标吧。", "");
                });

                panel.ClickArea.onClick.AddListener(() =>
                {
                    if (!instance.GuideIsComplete(13)) return;
                    panel.Hide();
                    panel.ClickArea.onClick.RemoveAllListeners();

                });
            });
        });
    }
    //第五关漩涡球引导
    public void ShowGuide_14(Action onCompleteCallback)
    {
        if(mList.Count > 0)
        {
            mList.Clear();
        }
        var mXuanwoBall = GetArriveBall();
        if (mXuanwoBall.GetComponent<Image>() == null)
        {
          var sp = mXuanwoBall.GetComponent<SpriteRenderer>().sprite;
          var mImage= mXuanwoBall.gameObject.AddComponent<Image>();
          mImage.sprite = sp;
          mList.Add(mImage.transform);
        }
        XDebug.Log("mXuanwoBall" + mXuanwoBall.name);
        //var mXuanwoLine = GetArriveLine();
        //if (mXuanwoLine.GetComponent<Image>() == null)
        //{
        //    var sp = mXuanwoLine.GetComponent<SpriteRenderer>().sprite;
        //    var mImage= mXuanwoLine.gameObject.AddComponent<Image>();
        //    mImage.sprite = sp;
        //    mList.Add(mImage.transform);
        //}
        //XDebug.Log("mXuanwoLine" + mXuanwoLine.name);

       

        var guide14 = Instance.GuideIsComplete(14);
        if (guide14) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            StopGuideAudio("语音5");
            PlayGuideAudio("语音16");
            panel.SetInfo2("引导<color=#A11C23>旋涡球</color>到达底部即可获胜!", "", mList, () =>
            {
              
                //panel.HanderControl.SetPos(mList[0].transform.position);
                //panel.HanderControl.Hide();
            });

            bool isClick = false;
           
                panel.ClickArea.onClick.AddListener(() => {

                    if (isClick) return;
                    CompleteGuide(14);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();

                    panel.ClickArea.onClick.RemoveAllListeners();
                });
           
        });

    }
    //第五关消除漩涡球引导
    public void ShowGuide_15(Action onCompleteCallback)
    {
        if (mList.Count > 0)
        {
            mList.Clear();
        }
        var mball = GetGuideObj();
        foreach (var item in mball)
        {
            if (item.GetComponent<Image>() == null)
            {
                var mImage = item.gameObject.AddComponent<Image>();
                mImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
                var mBtn = item.gameObject.AddComponent<Button>();
                mList.Add(item.transform);
            }
        }
        var guide15 = Instance.GuideIsComplete(15);
        if (guide15) return;
        UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
        {
            var pos = panel.mAreaPanelDefalutPos;
            pos.y -= 100;
            panel.SetDirAreaPos(pos);
            StopGuideAudio("语音16");
            PlayGuideAudio("语音17");
            panel.HanderControl.SetPos(mList[0].transform.position);
            panel.SetInfo2("点击消除即可引导漩涡球!", "", mList, () =>
            {
                //UmengDisMgr.Instance.CountOnPeoples("newer_guide", "v1_2");
                //panel.HanderControl.SetPos(mList[0].transform.position);
                panel.HanderControl.Show();
            });

            bool isClick = false;
            foreach (var item in mList)
            {
                item.GetComponent<Button>().onClick.AddListener(() => {

                    if (isClick) return;
                    GameManager.Instance.RemainingSteps.Value--;
                    GameManager.Instance.CollectSameRangBall(mList[0]);
                    GameManager.Instance.ElimintBall(mList[0].GetComponent<Ball>());
                    CompleteGuide(15);
                    GuideMgr.instance.SaveData();
                    isClick = true;
                    panel.HanderControl.Hide();
                    //panel.Hide();
                    panel.TempControl.RecoverAllTemp();
                    onCompleteCallback.Run();

                    ///第五步后续
                    StopGuideAudio("语音17");
                    PlayGuideAudio("语音5");
                    panel.SetInfo("继续匹配并完成关卡目标吧。", "");

                    panel.ClickArea.onClick.AddListener(() => {

                        Debug.Log("关闭第五步");
                        if (!Instance.GuideIsComplete(15)) return;
                        Debug.Log("关闭第五步");
                        panel.Hide();
                        panel.ClickArea.onClick.RemoveAllListeners();
                    });
                });
            }

            
        });
    }

    /// <summary>
    /// 只显示手指引导===附带引导
    /// </summary>
    /// <param name="id"></param>
    /// <param name="temp"></param>
    /// <param name="clickObj"></param>
    /// <param name="oncomplete"></param>
    public void onlyShowGuideHander(string title, Transform temp, Button clickObj, Action oncomplete = null)
    {
        Debug.LogError("Guide:"+ title);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {                
                UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
                {
                    panel.SetInfo("", "", temp, () =>
                    {
                        panel.HanderControl.SetPos(temp.position);
                        panel.HanderControl.Show();
                    });
                    panel.DirArea.HideCanvasGroup();
                    bool isClick = false;
                    clickObj.onClick.AddListener(() =>
                    {
                        if (isClick) return;
                        isClick = true;
                        panel.HanderControl.Hide();
                        panel.Hide();
                        panel.DirArea.ShowCanvasGroup();
                        panel.TempControl.RecoverAllTemp();
                        oncomplete.Run();
                    });
                });
            });
    }

    public void onlyShowGuideHander(string title, Transform temp,Vector3 handerpos, Button clickObj,float bgMaskAlpha, Action oncomplete = null)
    {
        Debug.LogError("Guide:" + title);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                GuidePanel.MaskAlpha = bgMaskAlpha;
                UIManager.Instance.ShowPopUp<GuidePanel>((GuidePanel panel) =>
                {                    
                    panel.SetInfo("胜利通关后，会获得红包奖励", "", temp, () =>
                    {
                        panel.HanderControl.SetPos(handerpos);
                        panel.HanderControl.Show();
                    });
                    panel.DirArea.HideCanvasGroup();
                    bool isClick = false;
                    clickObj.onClick.AddListener(() =>
                    {
                        if (isClick) return;
                        isClick = true;
                        panel.HanderControl.Hide();
                        panel.Hide();
                        panel.DirArea.ShowCanvasGroup();
                        panel.TempControl.RecoverAllTemp();
                        oncomplete.Run();
                    });
                });
            });
    }

    #endregion
}

#if UNITY_EDITOR
public class GuideEidotr
{

    [UnityEditor.MenuItem("GameEditor/Guide/101")]
    private static void Menu()
    {
        //GuideMgr.Instance.ShowGuide_103();
    }
    [UnityEditor.MenuItem("GameEditor/Guide/108")]
    private static void Menu_108()
    {
        //GuideMgr.Instance.ShowGuide_108();
    }

    [UnityEditor.MenuItem("GameEditor/Guide/111")]
    private static void Menu_111()
    {
        //GuideMgr.Instance.ShowGuide_111();
    }

}
#endif

public class GuideData
{
  

    public Dictionary<int, GuideInfo> mDic_guideinfo;
    public GuideData()
    {
        mDic_guideinfo = new Dictionary<int, GuideInfo>();
    }    
    public void AddInfo(int id,string dir=null)
    {
        if (!mDic_guideinfo.ContainsKey(id))
        {
            mDic_guideinfo.Add(id,new GuideInfo(id));
        }
    }
    
}

public class GuideInfo
{
    /// <summary>
    /// 引导ID
    /// </summary>
    public int GuideID;
    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsComplete;

    public string Dir;

    public GuideInfo(int id)
    {
        GuideID = id;
        IsComplete = false;
    }
}
