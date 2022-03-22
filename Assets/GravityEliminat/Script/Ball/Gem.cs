using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public bool canGet=true;
    public ShareRedDataManger.DiamondsType GemType;

    public void Init(ShareRedDataManger.DiamondsType type) {

        GemType = type;
        transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Texture/Gem/"+ GetGemName(type));

    }
    public string GetGemName(ShareRedDataManger.DiamondsType type) {

        switch (type)
        {
            case ShareRedDataManger.DiamondsType.cyan:
                return "img_baoshi_1";
                break;
            case ShareRedDataManger.DiamondsType.purple:
                return "img_baoshi_2";
                break;
            case ShareRedDataManger.DiamondsType.yellow:
                return "img_baoshi_3";
                break;
            case ShareRedDataManger.DiamondsType.red:
                return "img_baoshi_4";
                break;
            case ShareRedDataManger.DiamondsType.blue:
                return "img_baoshi_5";
                break;
            default:
                break;
        }
        return null;
    }

    public void Elimnt() {
        if (canGet)
        {
            Destroy(this.gameObject);
            ShareRedDataManger.Instance.AddDiamonds(GemType, 1);
            canGet = false;
            RewardData data = new RewardData(RewardEunm.Null,1,false, transform.GetComponent<SpriteRenderer>().sprite);
            UIManager.Instance.Show<RewardPop>(UIType.PopUp,data);
        }
      
    }
}
