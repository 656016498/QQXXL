using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Furnace : MonoBehaviour
{

    public int NowStep;
    /// <summary>
    /// 对应位置
    /// </summary>
    public List<Transform> chain = new List<Transform>();
    /// <summary>
    /// 消除链子对应的球类型
    /// </summary>
    public SortType clearType;
    /// <summary>
    /// 消除对应数量
    /// </summary>
    public int num;

    public TextMesh texture3D;

    private void Start()
    {
        EventManager.Instance.AddEvent(MEventType.SortType ,RefreshMySelf);
        texture3D.text = clearType.ToString()+ num.ToString();
    }

    public void RefreshMySelf(object[] args) {
        if (NowStep==GameManager.Instance.LevelStep)
        {
            SortType pairs = (SortType)args[0];
            if (pairs==clearType)
            {
                //Debug.Log("Eliminat");
                num--;
                if (num > 0)
                {
                    texture3D.text =clearType.ToString()+ num.ToString();
                }
                else if (num == 0)
                {
                    foreach (var item in chain)
                    {
                        item.gameObject.SetActive(false);
                    }
                    transform.gameObject.SetActive(false);
                    GameManager.Instance.LevelStep++;
                    EventManager.Instance.ExecuteEvent(MEventType.LevelNextStep, GameManager.Instance.LevelStep);
                    texture3D.text = "0";
                }
                else { 
                    texture3D.text = "0";

                }
            }
        }
    }

    public void BreakChain() {

         
        foreach (var item in chain)
        {
            item.gameObject.SetActive(false);
        }
    
    }
}
