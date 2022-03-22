using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketBall : MonoBehaviour,CanClick
{
    public void Elimnt() {
        if (DataManager.Instance.data.TicketLevel.Contains(GameManager.Instance.CurrentLevel))
        {
            DataManager.Instance.data.TicketLevel.Remove(GameManager.Instance.CurrentLevel);
        }
        if (GameManager.Instance.OverGame) return;
      
        LotteryDataManger.Instance.AddLotteryPaper(1);
        Pool.Instance.Despawn(Pool.Ball_PoolName, transform);
        RewardData data = new RewardData(RewardEunm.Ticket,1,false,transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
        UIManager.Instance.Show<RewardPop>(UIType.PopUp, data);

    }


}
