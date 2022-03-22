using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimeMgr : Singlton<TimeMgr>
{

    public int LoveCountdown;
    public string LoveString;

    public void Init()
    {
        TimeClock.Init();
        NowTimeSenderSub();
    }



    public void JoinInitData() {

        //初始化爱心
        if (DataManager.Instance.data.Love<10)
        {
             GameManager.Instance.LoveStar.Value += (int)(( TimeClock.NowTime- DataManager.Instance.data.UseLoveTime).TotalSeconds / 600);
            if (GameManager.Instance.LoveStar.Value > 10)
            {
                GameManager.Instance.LoveStar.Value = 10;
            }
        }


    }




    //时间监听
    public void NowTimeSenderSub() {


        TimeClock.NowTimeListening.Subscribe(_ => {

            if (GameManager.Instance.LoveStar.Value < 10)
            {
                LoveCountdown = 600-(int)(_- DataManager.Instance.data.UseLoveTime).TotalSeconds;

                if (LoveCountdown <0)
                {
                    GameManager.Instance.LoveStar.Value++;
                    DataManager.Instance.data.UseLoveTime =_;
                }

                LoveString = LoveCountdown.Second_TransFrom_Math(); 
            }
            else {
                
                LoveString = null;

            }
            
        });
    }




}
