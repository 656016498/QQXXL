using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
public class UILevelItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform levelImg;
    public Transform currentChoose;
    public Image tx;
    public int level;
    public Transform[] star;
    public Text levelText;
    public Transform GemQP;
    public int beforeJoinStar;
    public Transform moneyImg;
    public Transform ticketImg;
    public Button ticketBtn;

    private void Start()
    {
        //ticketBtn = GemQP.Find("ticket").GetComponent<Button>();
        ticketBtn.onClick.AddListener(() => {

            UIManager.Instance.ShowPopUp<LotteryPanel>();
        });
    }

    public void Refresh(int mapIndex, int childIndex)
    {

        if (mapIndex == -1)
        {
            level = childIndex + 1;
        }
        else
        {
            level = (mapIndex) * InfiniteScrollView.Instance.OneLevelNum + childIndex + 1 + InfiniteScrollView.Instance.topLevel.childCount;
        }

        //if (DataManager.Instance.data.UnlockLevel == level)
        //{
        //    if (InfiniteScrollView.Instance.JoinLevel+1== level)
        //    {
        //        InfiniteScrollView.Instance.JoinLevel = -1;
        //        Pool.Instance.SpawnEffect(Pool.Effect_PoolName,Pool.NewLevel, levelImg.transform.position);
        //    }
        //    currentChoose.gameObject.SetActive(true);

        //}
        //else
        //{
        //    currentChoose.gameObject.SetActive(false);
        //}

        levelText.text = level.ToString();
        //未解锁关卡
        if (level > DataManager.Instance.data.UnlockLevel)
        {
            transform.GetComponent<Button>().enabled = false;
            levelImg.transform.gameObject.SetActive(false);
            CloseStar();
        } //正在解锁的管卡
        else if (level == DataManager.Instance.data.UnlockLevel)
        {
            currentChoose.gameObject.SetActive(true);
            transform.GetComponent<Button>().enabled = true;
            levelImg.transform.gameObject.SetActive(true);
            CloseStar();
        }
        //已完成这关播放星星动画
        else if (level == InfiniteScrollView.Instance.JoinLevel)
        {
            currentChoose.gameObject.SetActive(false);
            levelImg.transform.gameObject.SetActive(true);
            if (DataManager.Instance.data.levelStar[level - 1] > beforeJoinStar)
            {
                for (int i = 0; i < star.Length; i++)
                {
                    int P = i;
                    star[i].gameObject.SetActive(true);
                    if (i <= DataManager.Instance.data.levelStar[level - 1] - 1)
                    {
                        star[i].GetChild(0).gameObject.SetActive(true);
                        if (i > beforeJoinStar - 1)
                        {
                            star[i].GetChild(0).GetComponent<Image>().DOFade(0, 0);
                            star[i].GetChild(0).transform.localScale = Vector3.one * 1.5F;
                            UniRx.Observable.TimeInterval(System.TimeSpan.FromSeconds(1 + i * 0.5F)).Subscribe(_X =>
                            {
                                star[P].GetChild(0).transform.DOScale(Vector3.one * 0.9F, 1).SetEase(Ease.InOutBack).OnComplete(() =>
                                {
                                    Pool.Instance.SpawnEffectByParent(Pool.Effect_PoolName, Pool.LevelStarza, star[P].GetChild(0).transform, Vector3.zero, 200);
                                    star[P].GetChild(0).transform.DOScale(Vector3.one, 0.1F).SetEase(Ease.Linear);
                                });
                                star[P].GetChild(0).GetComponent<Image>().DOFade(1, 1);
                            });
                        }
                    }
                }
            }
        }
        else
        {
            //已解锁的关卡
            currentChoose.gameObject.SetActive(false);
            transform.GetComponent<Button>().enabled = true;
            levelImg.transform.gameObject.SetActive(true);
            for (int i = 0; i < star.Length; i++)
            {
                star[i].gameObject.SetActive(true);
                if (i <= DataManager.Instance.data.levelStar[level - 1] - 1)
                {
                    star[i].GetChild(0).gameObject.SetActive(true);
                }
            }
        }


        //if (DataManager.Instance.data.MoneyLevel.Contains(level) && level != DataManager.Instance.data.UnlockLevel)
        //{

        //    GemQP.gameObject.SetActive(true);
        //    moneyImg.gameObject.SetActive(true);
        //}
         if (DataManager.Instance.data.TicketLevel.Contains(level) && level != DataManager.Instance.data.UnlockLevel)
        {
            GemQP.gameObject.SetActive(true);
            ticketImg.gameObject.SetActive(true);
        }
        else {
            GemQP.gameObject.SetActive(false);
            ticketImg.gameObject.SetActive(false);
            //moneyImg.gameObject.SetActive(false);
        }

    }

    public void RefreshStar() {



        for (int i = 0; i < star.Length; i++)
        {
            if (DataManager.Instance.data.levelStar[level-1] - 1 >= i)
            {
                star[i].GetChild(0).gameObject.SetActive(true);
            }
            star[i].GetChild(0).gameObject.SetActive(false);
        }

    }



    //添加星星动画
    public void CloseStar() {

        for (int i = 0; i < star.Length; i++)
        {

            star[i].gameObject.SetActive(false);
            star[i].GetChild(0).gameObject.SetActive(false);

        }

    }
}
