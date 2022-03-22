using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class AgainPop : UIBase
{
    public Text titleTexr;
    //public Text timeText;
    public Text LoveNum;
    public IButton closeBtn;
    public IButton again;
    public IButton AdAgin;
    //public IButton closeBtn;

    void Start()
    {
        
        closeBtn.onClick.AddListener(()=> {
            //InfiniteScrollView.Instance.JoinLevel = -1;
            GameManager.Instance.DestoryLevel();
            //DataManager.Instance.data.CurrentLevel++;

            //if (DataManager.Instance.data.CurrentLevel > 4)
            //{
            //    DataManager.Instance.data.CurrentLevel = 1;
            //}
            GameManager.Instance.LoadLevel();
            Hide();
            //UIManager.Instance.Hide<GamePanel>();
            //UIManager.Instance.Show<MainPanel>(UIType.Normal);

            //InfiniteScrollView.Instance.RefreshLevelMap();

        } );

        //TimeClock.NowTimeListening.Subscribe(_ => {
        //    //Debug.Log(timeText.text);
        //    timeText.text = TimeMgr.Instance.LoveString;
        //});

        //GameManager.Instance.LoveStar.Subscribe(_ => {
        //    LoveNum .text= _.ToString();
        //});

        again.onClick.AddListener(() => {

            //if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
            //{
            //    UmengDisMgr.Instance.CountOnNumber("level_restart", DataManager.Instance.data.UnlockLevel.ToString());
            //}
            //if (GameManager.Instance.LoveStar.Value > 0)
            //{
              

            //    GameManager.Instance.LoveStar.Value--;
                    GameManager.Instance.DestoryLevel();
                    GameManager.Instance.LoadLevel();
                    Hide();
              
                
            //}
            //else {
            //    AddPopMgr.Instance.isPassivity = true;
            //    UIManager.Instance.Show<AddPop>(UIType.PopUp, AddEumn.Love);
 

            //}

        });


        AdAgin.onClick.AddListener(() => {

     AdControl.Instance.ShowIntAd("fail_restart_int", () => {
         if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
         {
             UmengDisMgr.Instance.CountOnNumber("level_restart", DataManager.Instance.data.UnlockLevel.ToString());
         }
         Hide();
            GameManager.Instance.DestoryLevel();
            //var real
            GameManager.Instance.LoadLevel();

            Observable.TimeInterval(System.TimeSpan.FromSeconds(2)).Subscribe(_ => {

                //Debug.Log("??"+ GameManager.Instance.colorBalls.Count);
                //for (int i = 0; i < 3; i++)
                //{
                    //int R = UnityEngine.Random.Range(0,GameManager.Instance.colorBalls.Count);
                GameManager.Instance.    CreateProp(Porp_Size.大,GameManager.Instance.colorBalls[UnityEngine.Random.Range(0, GameManager.Instance.colorBalls.Count)].transform.position, SortType.Red,3);


                //}
                //DynamicMgr.Instance.FlyEffectCurve(gamePanel.RemainingStepsText.transform.position, colorBalls[R].transform.position, Pool.StepLiuXin, 0.5f, 0.5f, 5, () => {
                //Pool.Instance.Despawn(Pool.Ball_PoolName, colorBalls[R].transform);

                //if (!colorBalls.Contains(colorBalls[R])) return;

                //if (colorBalls.Contains(colorBalls[R]))
                //{
                //    colorBalls.RemoveAt(R);
                //}


                //});

            });
     });
        });

    }

    public override void Show()
    {
        base.Show();
        GameManager.Instance.SendLevel(3);
        //if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
        //{
            UmengDisMgr.Instance.CountOnPeoples("level_failp", DataManager.Instance.data.UnlockLevel.ToString());
            UmengDisMgr.Instance.CountOnNumber("level_failu", DataManager.Instance.data.UnlockLevel.ToString());
    //}
        titleTexr.text = string.Format("关卡{0}", GameManager.Instance.CurrentLevel);
}
}
