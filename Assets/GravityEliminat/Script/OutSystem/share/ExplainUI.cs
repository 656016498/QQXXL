using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainUI : UIBase
{
    public IButton btnSure;
    public Transform Content;

    // Start is called before the first frame update
    protected void Start()
    {
        btnSure.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        UIManager.Instance.Hide<ExplainUI>();
    }


    public void EnterPanelUpdate(List<string> mlist)
    {
        var contengChicount = Content.childCount;
        for (int i = 0; i < contengChicount; i++)
        {
            Content.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < mlist.Count; i++)
        {
            if (i < contengChicount)
            {
                Content.GetChild(i).gameObject.SetActive(true);
                Content.GetChild(i).GetComponent<Text>().text = mlist[i];
            }
            else
            {
                //需要实例化
                var newdir = Instantiate(Content.GetChild(0), Content, false);
                newdir.GetComponent<Text>().text = mlist[i];
            }
        }
    }
}
