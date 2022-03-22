using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class BlueProp : Prop
{
    public List<Ball> allBall;
    int lightNum = 0;
    private void Awake()
    {
        allBall = new List<Ball>();
    }
    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        if (obj != null)
        {
            allBall.Clear();
               SizeType = (Porp_Size)obj[0];
            Gear = (int)obj[1];
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PropSprite/" + PropType.ToString() + Gear);
        }
    }
    bool dir;
    public override void OnClick()
    {
        base.OnClick();
        //PropManger.Instance.togetherBall.Clear();
        if (!canTiggle) return;
        canTiggle = false;
        Eliminat();
     
            Observable.TimeInterval(System.TimeSpan.FromSeconds(1F)).Subscribe(_ =>
            {
                if (!GameManager.Instance.OverGame)
                {
                    UIRoot.Instance.HideMask();
                }
            });

        lightNum = PropManger.Instance.GetCubeNum(PropType.ToString(),Gear);
        dir = PropManger.Instance.GetDir(transform.position);
        for (int i = 0; i < lightNum; i++)
        {
            Transform t= Pool.Instance.Spawn(Pool.Prop_PoolName,Pool.LightBall);
            t.transform.position = transform.position;
            t.transform.localScale = Vector3.one;
            t.transform.GetComponent<LightBall>().Init(new object[2] {SizeType,Gear });
            if (i % 2 == 0)
            {
                t.transform.GetComponent<LightBall>().direction = !dir;
            }
            else
            {
                t.transform.GetComponent<LightBall>().direction = dir;
            }
            t.transform.GetComponent<Prop>().OnClick();
            if (i>=2)
            {
                t.transform.GetComponent<LightBall>().UP = true;
            }
            t.transform.GetComponent<LightBall>().desBall = this;
        }
        //Pool.Instance.Despawn(Pool.Prop_PoolName,transform);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Prop>() != null)
        {
            Fusion(this, collision.transform.GetComponent<Prop>());
        }
    }
}
