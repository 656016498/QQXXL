using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class DelayedBomb : Prop
{
    public int DelayeNum;
    public float Radius;
    TextMesh textMesh;
    private void Start()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMesh>();
        GameManager.Instance.RemainingSteps.Subscribe(_ => {
            DelayeNum--;
            textMesh.text = DelayeNum.ToString();
            if (DelayeNum==0)
            {

                Observable.TimeInterval(System.TimeSpan.FromSeconds(0.1F)).Subscribe(W => {
                    PropCollectBall();
                    GameManager.Instance.ElimintNoColorlBall();
                });
            }
        }).AddTo(this);

    }
    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        textMesh.text = DelayeNum.ToString();
    }
    public override Collider2D[] DetectionRange()
    {
        string prfabsName = null;
        switch (Gear)
        {
            case 1:

                prfabsName = Pool.OneStepBomb;
                break;
            case 2:
                prfabsName = Pool.TwoStepBomb;
                break;
            case 3:
                prfabsName = Pool.ThreeBomb;
                break;
            default:
                break;
        }
        Pool.Instance.SpawnEffect(Pool.Effect_PoolName,prfabsName,transform.position,2);

        return Physics2D.OverlapCircleAll(transform.position, Radius);
    }
    public override void Eliminat()
    {
        base.Eliminat();
    }

}
