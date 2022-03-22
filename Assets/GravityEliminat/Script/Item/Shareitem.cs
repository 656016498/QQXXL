using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shareitem : MonoBehaviour
{
    public Text numText;
    public ShareRedDataManger.DiamondsType diamondsType;
    void Start()
    {
        
    }

    //刷新UI
    public  void RefrishUi()
    {
        var mdata = ShareRedDataManger.Instance.mdata;
        numText.text = string.Format("{0}", mdata.mdiamonds[diamondsType]);
        //Debug.Log("mdata.mdiamonds" + mdata.mdiamonds[diamondsType]);
    }
   
}
