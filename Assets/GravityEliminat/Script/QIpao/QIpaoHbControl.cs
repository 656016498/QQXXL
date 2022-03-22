using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QIpaoHbControl : SinglMonoBehaviour<QIpaoHbControl>
{
    public List<QIpaoHb> hbList = new List<QIpaoHb>();


    private void Start()
    {
        Init();
        //默认解锁右边气泡红包
        //hbList[0].transform.ShowCanvasGroup();
    }

    //初始
    void Init()
    {
        foreach (var item in hbList)
        {
            gameObject.SetActive(true);
            //item.transform.ShowCanvasGroup();
            //item.mBtn.onClick.AddListener(() => {
            // InitHb(item.hbIndex == 0 ? 1: 0);
            //});
        }
    }

    public  void InitHb(int index)
    {
       hbList[index].transform.ShowCanvasGroup();
    }
}
