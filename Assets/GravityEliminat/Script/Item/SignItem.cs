using EasyExcelGenerated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignItem : MonoBehaviour
{
    [Header("UI")]
    public Text dayText;//第几天
    public Image icon;//图片 红包or钞票
    public Image yes;//对号
    public Image mask;//黑色遮罩
    public Image itemBg;//item背景
    public Sprite select;//选中框
    public Sprite noselect;//未选中
    public Sprite  hb;//红包
    public Sprite pigCash;//金猪币
    public Sprite cash;//真提现
    public Image cashFont;//领现金字样
    public Transform textBg;//文本bg
    public int index;
    public Button btn;
    public  SignDataControl.AwardType awardType;
    public SignRedConfig mSignRedConfig; 
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(Sign);
    }

    //刷新ui
    public void RefrishUI()
    {
        Init();
        var mindexSign = SignDataControl.Instance.mdata.indexSign;
        if (index < mindexSign)//已签到
        {
            itemBg.sprite = noselect;
            mask.transform.ShowCanvasGroup();
            yes.transform.ShowCanvasGroup();
        }
        else if (index == mindexSign)//可签到
        {
            itemBg.sprite = SignDataControl.Instance.mdata.canSign ? select : noselect;
        }
        else if(index> mindexSign)//不可签到
        {
            itemBg.sprite = noselect;
        }
        dayText.text = string.Format("第{0}天",SignDataControl.Instance.Rel(index));
        switch (awardType)
        {
            case SignDataControl.AwardType.hb:
                icon.sprite = hb;
                break;
            case SignDataControl.AwardType.pigCash:
                icon.sprite = pigCash;
                textBg.transform.ShowCanvasGroup();
                textBg.GetChild(0).GetComponent<Text>().text = string.Format("最高{0}元", mSignRedConfig.MostGet);
                cashFont.transform.ShowCanvasGroup();
                break;
            case SignDataControl.AwardType.cash:
                textBg.transform.ShowCanvasGroup();
                textBg.GetChild(0).GetComponent<Text>().text = string.Format("最高{0}元", mSignRedConfig.MostGet);
                cashFont.transform.ShowCanvasGroup();
                icon.sprite = pigCash;
                break;
            default:
                break; 
        }
        icon.SetNativeSize();
    }

    void Sign()
    {
        if (!SignDataControl.Instance.mdata.canSign ) return;
        if (index != SignDataControl.Instance.mdata.indexSign) return;
        if (!WeChatContral.Instance.mWexinIsLogin.Value && SignDataControl.Instance.mdata.indexSign == 2)
        {
            WeChatContral.Instance.MLogin(() => {
                Debug.Log("登陆成功");
            });
            return;
        }
        //签到
        AdControl.Instance.ShowRwAd("daily_sign_video", () => {
            SignDataControl.Instance.AddSignIndex(awardType);
        });

    }

    private void Init()
    {
        Debug.Log("ignDataControl.Instance.mSignRedConfigs" + SignDataControl.Instance.mSignRedConfigs.Count);
        mSignRedConfig = SignDataControl.Instance.mSignRedConfigs[index-1];
        mask.transform.HideCanvasGroup();
        yes.transform.HideCanvasGroup();
        cashFont.transform.HideCanvasGroup();
        textBg.transform.HideCanvasGroup();
        awardType = (SignDataControl.AwardType)mSignRedConfig.AwardType;
        //Debug.Log("awardType"+awardType);
        
    }
}
