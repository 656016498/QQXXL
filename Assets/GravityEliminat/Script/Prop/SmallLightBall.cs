//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SmallLightBall : MonoBehaviour
//{
//    public LightBall lightBall;
//    public bool isDownWall = false;

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.transform.CompareTag("Ball"))
//        {
//            Ball tempBall = collision.transform.GetComponent<Ball>();
//            tempBall.Eliminat();
//            if (tempBall.ballType != BallType.ColorBall)
//            {
//                GameManager.Instance.ClickSpecailBall.Add(tempBall);
//            }
//            else
//            {
//                WillDown willDown = new WillDown(tempBall.ballType, tempBall.sort, tempBall.isFix, 1);
//                if (lightBall.temp.ContainsKey(willDown.BallTypeString))
//                {
//                    lightBall.temp[willDown.BallTypeString].num++;
//                }
//                else
//                {
//                    lightBall.temp.Add(willDown.BallTypeString, willDown);
//                }

//                lightBall.needSpswnNum += tempBall.Eliminat();
//                //GameManager.Instance.colorBalls.Remove(tempBall.GetComponent<ColorBall>());
//                if (!isDownWall)
//                {
//                    Pool.Instance.Despawn(Pool.Prop_PoolName, transform);
//                    //lightBall.smallNum--;
//                    Debug.Log("lightBall.smallNum:::" + lightBall.smallNum);
//                    if (lightBall.smallNum == 0)
//                    {
                     
//                    }
//                }
//            }
//        }
//    }
//}
