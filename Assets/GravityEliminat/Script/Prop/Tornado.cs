using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class Tornado : Prop
{

    public bool CanMove=false;
    public int hitNum = 0;
    public bool change=false;
    public Dictionary<string, WillDown> temp = new Dictionary<string, WillDown>();
    int needSpswnNum;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            Ball tempBall = collision.transform.GetComponent<Ball>();

            if (tempBall.ballType != BallType.ColorBall)
            {
                GameManager.Instance.ClickSpecailBall.Add(tempBall);
            }
            else
            {
                WillDown willDown = new WillDown(tempBall.ballType, tempBall.sort, tempBall.isFix, 1);
                if (temp.ContainsKey(willDown.BallTypeString))
                {
                    temp[willDown.BallTypeString].num++;
                }
                else
                {
                    temp.Add(willDown.BallTypeString, willDown);
                }
                //Debug.LogError(transform + "   ");
                needSpswnNum += tempBall.Eliminat();
                //Debug.LogError(transform + "   " + needSpswnNum);
                //GameManager.Instance.colorBalls.Remove(tempBall.GetComponent<ColorBall>());
            }
        }
    }
    int num = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            if (hitNum == 0)
            {
                Debug.LogError("撞到墙HIT0");
            
                AddBatch(temp, needSpswnNum);
                GameManager.Instance.ElimintNoColorlBall();
                Observable.TimeInterval(System.TimeSpan.FromSeconds(1F)).Subscribe(_ => {

                    CanMove = false;
                    Pool.Instance.Despawn(Pool.Prop_PoolName,transform);

                }); temp.Clear();
                 needSpswnNum =0;
            }
            else
            {
                change = !change;
                AddBatch(temp, needSpswnNum);
                GameManager.Instance.ElimintNoColorlBall();
                temp.Clear();
                needSpswnNum = 0;
                hitNum--;
            }
            
        }
    }

    public override void Init(object[] obj = null)
    {
        base.Init(obj);
        needSpswnNum = 0;
        temp.Clear();
        hitNum = (int)obj[0];
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            change = true;
        }
        else {
            change = false;
        }
    }

    public void Update()
    {

        if (CanMove)
        {
            transform.Translate(new Vector3 (change?-10:10,0,0)*Time.deltaTime*0.25f,Space.Self);
        }

    }

    

}
