using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineItem : MonoBehaviour
{

    public Image yes;
    public Text timeText;
    public int index;
    void Start()
    {
        
    }

    public void RefeishUi()
    {
        Init();
        var mIndex = SignDataControl.Instance.CanGetIndex();
       
        if (index < mIndex || mIndex==0)
        {
            yes.transform.ShowCanvasGroup();
        }
    }

    private void Init() 
    {
        yes.transform.HideCanvasGroup();
        
    }
}
