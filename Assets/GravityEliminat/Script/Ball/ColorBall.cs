using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using ClockStone;
using UniRx;
public class ColorBall : Ball,CanClick
{
    public bool IsIce;
    CircleCollider2D circle;
    Vector2 circleOff;
   public SortType willColorBall= SortType.Default;
    private void Awake()
    {
        circle = transform.GetComponent<CircleCollider2D>();
    }
    public override void Start()
    {
        base.Start();
        if (isPut)
        {
            GameManager.Instance.colorBalls.Add(this);
        }
    }
    float RA = 0;
    public override void Init(SortType color, bool ISFix,int i=0)
    {
        NeedInit = false;
        base.Init(color, ISFix);
        if (circle==null)
        {
            circle = transform.GetComponent<CircleCollider2D>();
        }
        circleOff = Vector2.zero;
        switch (color)
        {
          
            case SortType.Default:
                break;
            case SortType.Red:
                circleOff = new Vector2(0, 0.03F);
                RA = 0.72F;
                break;
            case SortType.Yellow:
                RA = 0.7F;
                break;
            case SortType.Blue:
                RA = 0.75F;
                break;
            case SortType.Green:
                RA = 0.695F;
                circleOff = new Vector2(0, 0.005F);
                break;
            case SortType.Coat:
                break;
            case SortType.Orange:
                RA = 0.7F;
                break;
            case SortType.Cyan:
                RA = 0.7F;
                break;
            case SortType.Pink:
                RA = 0.7F;

                break;
            case SortType.Purple:
                RA = 0.72F;
                break;
            default:
                break;
        }
        circle.offset = circleOff;
        circle.radius = RA;
        transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/" + typeName);
        //transform.localScale = Vector3.one * GameManager.Instance.ballSize;
        //willChangeColor = false;
    }
       
    public override int Eliminat(int Numbase=1,bool P=false)
    {

        if (IsIce)
        {
            GameManager.Instance.IceNum--;
            IsIce = false;
        }
        EventManager.Instance.ExecuteEvent(MEventType.SortType, sort);

        //if (GameManager.Instance.colorBalls.Contains(this))
        //{
        //    GameManager.Instance.colorBalls.Remove(this);
        //}
        //if (isPut)
        //{
            GameManager.Instance.colorBalls.Remove(this);
        //}


        /*  AudioObject soundObj =*/
        //AudioMgr.Instance.PlaySFX("消除方块");

        //soundObj.
        //Debug.Log("染色瓶"+typeName);
        //SetHight();
        return base.Eliminat(Numbase,P);
    }

    void OnAudioCompleteleyPlayed( AudioObject audioObj )
    {
        Debug.Log( "Finished playing " + audioObj.audioID + " with clip " + audioObj.primaryAudioSource.clip.name );
    }

    public void SetSubger()
    {

        //if (GameManager.Instance.IsCondition(typeName))
        //{
        //    Eliminat();
        //}
        //else
        //{
        //Transform t = Pool.Instance.Spawn(Pool.Effect_PoolName, Pool.IceState);
        //t.transform.SetParent(transform);
        //t.localPosition = Vector3.zero;
        //t.localScale = Vector3.one;
        //GameManager.Instance.colorBalls.Add(t.GetComponent<ColorBall>());

        IsIce = true;
        GameManager.Instance.IceNum++;
        typeName = "SugarCube_Default";
        transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BallSprite/Ice_" + sort.ToString());

    }

    string lsEffect=null;
    /// <summary>
    /// 换色功能
    /// </summary>
    /// <param name="tempSort">染色类型</param>
    /// <param name="i">1 技能染色 2 染色瓶染色</param>
    public void ChangeColor(SortType tempSort,int i) {

        lsEffect = null;

        switch (i)
        {
            case 1:
                switch (tempSort)
                {
                    case SortType.Red:
                        lsEffect = Pool.PeopLsRed;
                        break;
                    case SortType.Yellow:
                        lsEffect = Pool.PeopLsYellow;
                        break;
                    case SortType.Blue:
                        lsEffect = Pool.PeopLsBule;
                        break;
                    case SortType.Green:
                        lsEffect = Pool.PeopLsGreen;
                        break;
                    case SortType.Orange:
                        lsEffect = Pool.PeopLsOrgin;
                        break;
                    case SortType.Purple:
                        lsEffect = Pool.PeopLsPuple;
                        break;
                    default:
                        break;
                }
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, lsEffect, transform.position);
                break;
            case 2:

                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, Pool.DyeBall+ tempSort, transform.position);

                break;
            default:
                break;
        }
        if (tempSort == sort)
        {

            if (GameManager.Instance.nowConditionR[typeName] > 0)
            {
                //GameManager.Instance.nowConditionR[typeName]--;
                GameManager.Instance.ReduceTarget(typeName);
                //GameManager.Instance.ReduceTarget(name);
                EventManager.Instance.ExecuteEvent(MEventType.PassConditon, GameManager.Instance.nowConditionR);


            }
            /*if (tempSort == sort) */
            return;
            //GameManager.Instance.ReduceTarget(typeName);
        }
       
        Init(tempSort, isFix);
        
        transform.DOBlendableRotateBy(new Vector3(0, 0, 20), 0.15f,RotateMode.FastBeyond360).SetLoops(2,LoopType.Yoyo);
        Debug.Log("染色瓶"+typeName);
        willColorBall = SortType.Default;
        //{
        //    GameManager.Instance.PassGame();
        //}
       
    }

    public void OnClick()
    {
        throw new System.NotImplementedException();
    }
    //public void AddFore(object[] all)
    //{
    //    //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5) * 1F*(float)all[0]);
    //    //transform.GetComponent<Rigidbody2D>().mass = 0;
    //    //transform.


    //}

    public override void Fly(int index, Action action)
    {
        //string name = typeName;
        if (typeName == "SugarCube_Default")
        {
            typeName = "Ice_" + sort.ToString();
        }
       
        DynamicMgr.Instance.WordPosFlyUI(typeName, transform.position, UIManager.Instance.GetBase<GamePanel>().PassParent.GetChild(index).transform.position, action);
    }
 
}
