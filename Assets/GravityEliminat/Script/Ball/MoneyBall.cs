using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
public class MoneyBall : MonoBehaviour,CanClick
{

    public bool can;
    public void Eliminat() {
        if (GameManager.Instance.OverGame) return;
        can = false;
        if (!GameManager.Instance.CanPop) return;
       
        GameManager.Instance.CanReward = false;

        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.4f)).Subscribe(_ => {
            UIRoot.Instance.HideMask();
                RewardCash();
            Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
            });
    }

    //奖励现金
    private void RewardCash()
    {
        //Debug.LogError("eqweqw");
        var mpanel = UIManager.Instance.ShowPopUp<LargeCashPanel>();
        AdControl.Instance.ShowBanner();
        var mConfig = LargeCashDataControl.Instance.largeCashConfig;

        if (GameManager.Instance.CurrentLevel==DataManager.Instance.data.UnlockLevel)
        {
            UmengDisMgr.Instance.CountOnNumber("bubble_xj_show",DataManager.Instance.data.UnlockLevel.ToString());
        }

        DataManager.Instance.data.VideoTimes++;
        if (DataManager.Instance.data.VideoTimes == 5)
            AdControl.Instance.SdkSendEvent(5);
        mpanel.OnOpen("xj_hb_video", mConfig.FM,
        () => {
            AdControl.Instance.HideBanner();
            GameManager.Instance.CanReward = true;
            if (GameManager.Instance.CurrentLevel == DataManager.Instance.data.UnlockLevel)
            {
                UmengDisMgr.Instance.CountOnNumber("bubble_xj_get",DataManager.Instance.data.UnlockLevel.ToString());
            }

            LargeCashDataControl.Instance.AddtoTotal(mConfig.AD);
            var mControl = LargeCashDataControl.Instance;
            var mPanel2 = UIManager.Instance.ShowPopUp<LargeCashTwoPanel>();
            mPanel2.RefrishUi(mConfig.AD, mControl.mData.totalNum, mControl.LastMoney);
            mPanel2.OnOpen(LargeHbType.Nomal,
                () => {
                    GameManager.Instance.CanReward = true;
                    if (GameManager.Instance.OverGame)
                    {
                        UIManager.Instance.GetBase<GamePanel>().RewardTime();
                    }
                },
                () => {

                    ShowPublicTip.Instance.Show("红包满200元才可以提现哦");

                },
                () => {
                    GameManager.Instance.CanReward = true;
                    if (GameManager.Instance.OverGame)
                    {
                        UIManager.Instance.GetBase<GamePanel>().RewardTime();
                    }
                });
        },
        () => {
            AdControl.Instance.HideBanner();

            GameManager.Instance.CanReward = true;
            if (GameManager.Instance.OverGame)
            {
                UIManager.Instance.GetBase<GamePanel>().RewardTime();
            }
        }
        );
    }
}
