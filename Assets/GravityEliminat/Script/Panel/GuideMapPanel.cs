using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuideMapPanel : UIBase
{
    public Button pageBtn;
    public Image image1;
    public Image image2;
    public Text des;
    // Start is called before the first frame update
    void Start()
    {
        pageBtn.onClick.AddListener(()=> {

            Hide();
            GameManager.Instance.gamePanel.ShowHF();
        });
    }
    public override void Show()
    {
        base.Show();
        image1.sprite = Resources.Load<Sprite>("UI/Texture/Gudie/" + TableMgr.Instance.GuideImg1());
        image2.sprite = Resources.Load<Sprite>("UI/Texture/Gudie/" + TableMgr.Instance.GuideImg2());
        des.text = TableMgr.Instance.GuideDes();

    }




}
