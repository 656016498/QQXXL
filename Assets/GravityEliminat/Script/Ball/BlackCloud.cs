using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class BlackCloud : Ball
{
    //string typeName = "BlackCloud_Default";
    new private void Start()
    {
        base.Start();
        transform.GetComponent<SpriteRenderer>().sprite = null;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.LogError(collision.name);
        Ball ball = collision.GetComponent<Ball>();
        if (ball != null)
        {
            //Debug.LogError(collision.name+"DD");

            if (ball.isEliminat)
            {

                //Debug.LogError(ball.name+"EEEEEE"+ball.isEliminat);
                if (isEliminat) return;
                //Debug.LogError(collision.name + "ee");
                isEliminat = true;
                AudioMgr.Instance.PlaySFX("特殊方块--黑云");
                //Debug.Log("黑云" + ball.name);
                //ball.isEliminat = false;
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, "effect_heiyun02", transform.position);
                if (GameManager.Instance.IsCondition(typeName))
                {
                     if (GameManager.Instance.nowConditionR[typeName] >= 0)
                    {
                        //GameManager.Instance.nowConditionR[typeName]--;
                        DynamicMgr.Instance.WordPosFlyUIEffect(typeName, "effect_heiyun03",transform.position, UIManager.Instance.GetBase<GamePanel>().PassParent.GetChild(GameManager.Instance.GetCondition(typeName)).transform.position,50,()=> {

                            GameManager.Instance.ReduceTarget(typeName);
                            EventManager.Instance.ExecuteEvent(MEventType.PassConditon, GameManager.Instance.nowConditionR);
                            //Debug.LogError(typeName+ GameManager.Instance.nowConditionR[typeName]);
                       
                        });
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }
    public override int Eliminat(int soreBase = 1,bool NEED=false)
    {

        return base.Eliminat(soreBase, NEED);
    }


}
