using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class WithdrawRecordUI : UIBase
{
    public Button btnClose,btnOther,btnRefresh;
    //用户id,无记录
    public Text txtID,txtNo;
    
    public Transform content;
    public GameObject item;

    //提现记录
    private List<RecordsData> records;

    private List<GameObject> items = new List<GameObject>();

    // Start is called before the first frame update
    protected void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnOther.onClick.AddListener(OnClickOther);
        btnRefresh.onClick.AddListener(OnRefresh);
        AdControl.Instance.GetAndroidID((inf) =>
        {
            txtID.text = string.Format("用户ID：{0}", inf);
        });
    }

    private void OnRefresh()
    {
        btnRefresh.interactable = false;
        //WeChatContral.Instance.GetHistory(()=> {
            records = RedWithdrawData.Instance.redData.records;
            UpdateUI();
        //});
        Observable.TimeInterval(TimeSpan.FromSeconds(5)).Subscribe(_ => {
            btnRefresh.interactable = true;
        }).AddTo(this);
    }

    private void OnClose()
    {
        UIManager.Instance.Hide<WithdrawRecordUI>();
    }

  

    public void OnOpen(List<RecordsData> rec)
    {
        if (rec==null)
        {
            return;
        }
        records = rec;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int id = 0;
        if (records.Count > 0)
        {
            txtNo.gameObject.SetActive(false);
            foreach (var item in records)
            {

                AddOneRecord(id, item.time, item.rmb, item.state);
                id++;

            }
        }
        else
        {
            txtNo.gameObject.SetActive(true);
        }
    }
    private void AddOneRecord(int id,string time,float rmb,int state)
    {
        GameObject nowitem;
        if (items.Count>id)
        {
            nowitem = items[id];
        }
        else
        {
            nowitem = Instantiate(item, content);
            items.Add(nowitem);
        }
        nowitem.transform.Find("txtReward").GetComponent<Text>().text = rmb.ToString();
        nowitem.transform.Find("txtTime").GetComponent<Text>().text = time;
        string st = "商家转账入账通知";
        if (state == 4) st = "商家转账审核中";
        if (state == 7) st = "商家转账审核失败";
        if (state == 10) st = "商家转账入账中";
        nowitem.transform.Find("txtState").GetComponent<Text>().text = st;
        nowitem.transform.GetComponent<Button>().onClick.AddListener(OnClickOther);
    }
    private void OnClickOther()
    {
        //弹出提示
        //ShowText("该界面由第三方平台提供，本游戏无法操作");
    }
}
