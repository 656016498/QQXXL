using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILevelBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => {

            transform.GetComponent<UILevelItem>().beforeJoinStar = DataManager.Instance.data.levelStar[transform.GetComponent<UILevelItem>().level - 1];
            Debug.Log("UILevelBtn" + transform.GetComponent<UILevelItem>().level);
            UIManager.Instance.Show<JoinPop>(UIType.PopUp, transform.GetComponent<UILevelItem>().level);
            InfiniteScrollView.Instance.JoinLevel = transform.GetComponent<UILevelItem>().level;
            Debug.LogError("进入关卡" + InfiniteScrollView.Instance.JoinLevel);
            //InfiniteScrollView.Instance.levelItem = transform.GetComponent<UILevelItem>();
            //GameManager.Instance.LoveStar.Value--;
            //DataManager.Instance.data.CurrentLevel = transform.GetComponent<UILevelItem>().level;
            //GameManager.Instance.LoadLevel();
            //UIManager.Instance.Hide<MainPanel>();
        });
    }

    // Update is called once per frame
   
}
