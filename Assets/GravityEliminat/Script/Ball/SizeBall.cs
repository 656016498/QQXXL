using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SizeBall : Ball
{
    public int ReduceTimes;
    public Sprite splitSprite;
    public  Sprite mySelf;
    public bool NextStep;
    private void Start()
    {
        Init(sort,isFix);
    }
    //public override void Init(SortType color, bool ISFix)
    //{
    //    base.Init(color, ISFix);
    //}
    public override int Eliminat(int Numbase=1,bool S=false)
    {
        AudioMgr.Instance.PlaySFX("特殊方块--硬糖");
        transform.DOBlendableRotateBy(new Vector3(0, 0, 2.5f), 0.05f, RotateMode.FastBeyond360).SetLoops(4, LoopType.Yoyo);
        ReduceTimes--;
        if (ReduceTimes==2)
        {
            ReduceTimes = 0;
        }
      Transform effect=Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.SizeBallBomb,transform.position);
        ParticleSystem.ShapeModule shape = effect.GetComponent<ParticleSystem>().shape;
        shape.radius = 0.26F * (ReduceTimes / 2);

        if (ReduceTimes > 0)
        {
            if (ReduceTimes % 2 == 1)
            {
                transform.GetComponent<SpriteRenderer>().sprite = splitSprite;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().sprite = mySelf;
                transform.DOScale(Vector3.one*(ReduceTimes/2*0.107F),0.3F).SetEase(Ease.InOutBack);
            }
        } 
        else {
            string name = typeName;
            if (GameManager.Instance.IsCondition(typeName))
            {

                if (GameManager.Instance.nowConditionR[typeName] > 0)
                {
                    //GameManager.Instance.nowConditionR[typeName]--;
                    GameManager.Instance.ReduceTarget(name);

                    Fly(GameManager.Instance.GetCondition(typeName), () =>
                    {
                        EventManager.Instance.ExecuteEvent(MEventType.PassConditon, GameManager.Instance.nowConditionR);

                        //{
                        //GameManager.Instance.PassGame();

                    });
                }
            }
            if (NextStep)
            {

                GameManager.Instance.LevelStep++;
                EventManager.Instance.ExecuteEvent(MEventType.LevelNextStep, GameManager.Instance.LevelStep);

            }
            Destroy(gameObject);
        }
        return 0;
    }

}
