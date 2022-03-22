using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UniRx;
/// <summary>
/// 无限滑动列表
/// </summary>
public class InfiniteScrollView : SinglMonoBehaviour<InfiniteScrollView>
{
    public int OneLevelNum;
    public UILevelItem levelItem;
    private ScrollRect scrollRect;//滑动框组件
    private RectTransform content;//滑动框的Content
    private VerticalLayoutGroup layout;//布局组件
    public RectTransform topLevel;
    [Header("滑动类型")]
    public ScrollType scrollType;
    [Header("固定的Item数量")]
    public int fixedCount;
    [Header("Item的预制体")]
    public GameObject itemPrefab;
    public float cellSizeY;
    private int totalCount;//总的数据数量
    private List<RectTransform> dataList = new List<RectTransform>();//数据实体列表
    private int headIndex;//头下标
    public int tailIndex;//尾下标
    private Vector2 firstItemAnchoredPos;//第一个Item的锚点坐标
    public int JoinLevel=-1;//进入的管卡
    public Transform unLockTran;
    public Transform avatar;//头像框
    public Animator avatarAtor;
    public UILevelItem[] uILevelItems;
    Vector2 mapPos;
    MainPanel startPanel;
    int moveDownIndex = 4;
    #region Init

    /// <summary>
    /// 实例化Item
    /// </summary>
    private void InitItem()
    {
        for (int i = 0; i < fixedCount; i++)
        {
            GameObject tempItem = Instantiate(itemPrefab, content);
            dataList.Add(tempItem.GetComponent<RectTransform>());
            SetShow(tempItem.GetComponent<RectTransform>(), nowIndex+i);
            SetPos(tempItem.GetComponent<RectTransform>(), nowIndex+i);
            //Debug.Log("SetShow" + nowIndex);
        }
    }


    public void RefreshLevelMap() {
        //nowIndex = ((DataManager.Instance.data.UnlockLevel - topLevel.transform.childCount) / OneLevelNum);.
        InitTopLevel();
        SetShow(dataList[0].GetComponent<RectTransform>(),headIndex);
        SetShow(dataList[1].GetComponent<RectTransform>(), tailIndex);
        SetUnlockPos();
        SetContentSize();
        //if (DataManager.Instance.data.UnlockLevel <=4)
        //{
        //    avatar.transform.SetParent(FindLevelItem(DataManager.Instance.data.UnlockLevel));
        //    avatar.transform.DOLocalMove(AvatarDefaultPos, 1).OnComplete(() => {
        //        avatarAtor.SetBool("play", true);
        //        UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);

        //    });                                                                                                                                                                                                                                               
        //}
        //else {
      
        //}
        if (DataManager.Instance.data.UnlockLevel>=5)
        {
            AutomaticPos();
        }
        SetAvatar();

        //for (int i = 0; i < dataList.Count; i++)
        //{
        //    SetShow(dataList[i].GetComponent<RectTransform>(), GameManager.Instance.CurrentLevel / OneLevelNum+i);
        //}

    }

    public void ChangePos() {

        for (int i = 0; i < dataList.Count; i++)
        {
            SetShow(dataList[i].GetComponent<RectTransform>(),nowIndex);

            Debug.Log("当前数据下标"+nowIndex);
        }
    }

    public float cloudDis = 182;
    /// <summary>
    /// 设置Content大小
    /// </summary>
    private void SetContentSize()
    {
        content.sizeDelta = new Vector2
            (
             /*   layout.padding.left + layout.padding.right + totalCount * (layout.cellSize.x + layout.spacing.x) - layout.spacing.x - content.rect.width*/0,
                //layout.padding.top + layout.padding.bottom + totalCount * (cellSizeY + layout.spacing) - layout.spacing+ topLevel.sizeDelta.y
                unLockTran.GetComponent<RectTransform>().anchoredPosition.y + unLockTran.GetComponent<RectTransform>().sizeDelta.y - cloudDis
            ) ;

        Debug.Log("默认大小"+(layout.padding.top + layout.padding.bottom + totalCount * (cellSizeY + layout.spacing) - layout.spacing));
    }

    /// <summary>
    /// 设置布局
    /// </summary>
    private void SetLayout()
    {
        //layout.childAlignment = VerticalLayoutGroupLowerLeft;
        //layout.startAxis = GridLayoutGroup.Axis.Vertical;
        //layout.childAlignment = TextAnchor.LowerLeft;
        //layout.constraintCount = 1;
        //if (scrollType == ScrollType.Horizontal)
        //{
        //    scrollRect.horizontal = true;
        //    scrollRect.vertical = false;
        //    layout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        //}
        //else if (scrollType == ScrollType.Vertical)
        //{
        //    scrollRect.horizontal = false;
        //    scrollRect.vertical = true;
        //    layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //}
    }

    /// <summary>
    /// 得到第一个数据的锚点位置
    /// </summary>
    private void GetFirstItemAnchoredPos()
    {
        firstItemAnchoredPos = new Vector2
            (
               /* layout.padding.left + cellSizeY / 2*/0,
                //-layout.padding.top - cellSizeY / 2 + topLevel.sizeDelta.y
                topLevel.sizeDelta.y + topLevel.anchoredPosition.y-5
            ) ;
    }

    #endregion

    #region Main

    /// <summary>
    /// 滑动中
    /// </summary>
    private void OnScroll(Vector2 v)
    {

        //Debug.Log("当前滑动位置"+ content.anchoredPosition.y+ "对应位置下限：-content.anchoredPosition.y" + ( mapPos.y - Screen.height / 2)+ "对应位置下限 "+ (mapPos.y + Screen.height / 2));
        if (content.anchoredPosition.y > (mapPos.y + Screen.height/2) || content.anchoredPosition.y <( mapPos.y - Screen.height/2))
        {
            //Debug.Log("显示");
            startPanel.SetAutoPosBtn(true);
            avatar.gameObject.SetActive(false);
        }
        else {
            //Debug.Log("隐藏");

            startPanel.SetAutoPosBtn(false);
            avatar.gameObject.SetActive(true);
            avatarAtor.SetBool("play", true);


        }
        //if (-content.anchoredPosition.y <= firstItemAnchoredPos.y)
        //{
        //    SetShow(topLevel, -1);
        //}
        if (dataList.Count == 0)
        {
            Debug.LogWarning("先调用SetTotalCount方法设置数据总数量再调用Init方法进行初始化");
            return;
        }
        if (scrollType == ScrollType.Vertical)
        {
            //向上滑
           
            while (-content.anchoredPosition.y >=  (headIndex+1) * (cellSizeY)+firstItemAnchoredPos.y
            /*&& tailIndex != totalCount - 1*/)
            {
                Debug.LogError("当前位置Headindex上移" + headIndex);
                //将数据列表中的第一个元素移动到最后一个
                RectTransform item = dataList[0];
                dataList.Remove(item);
                dataList.Add(item);

                //设置位置
                SetPos(item, tailIndex + 1);
                //设置显示
                SetShow(item, tailIndex + 1);

                headIndex++;
                tailIndex++;
            }
            //向下滑
            while (-content.anchoredPosition.y <= headIndex * (cellSizeY)+firstItemAnchoredPos.y
                && headIndex != 0)
            {
                //Debug.LogError("当前位置Headindex下移"+ headIndex);
                //将数据列表中的最后一个元素移动到第一个
                RectTransform item = dataList.Last();
                dataList.Remove(item);
                dataList.Insert(0, item);

                //设置位置
                SetPos(item, headIndex - 1);
                //设置显示
                SetShow(item, headIndex - 1);

                headIndex--;
                tailIndex--;
            }
        }
        //else if (scrollType == ScrollType.Horizontal)
        //{
        //    //向左滑
        //    while (content.anchoredPosition.x <= -layout.padding.left - (headIndex + 1) * (cellSizeY + layout.spacing)
        //    && tailIndex != totalCount - 1)
        //    {
        //        //将数据列表中的第一个元素移动到最后一个
        //        RectTransform item = dataList[0];
        //        dataList.Remove(item);
        //        dataList.Add(item);

        //        //设置位置
        //        SetPos(item, tailIndex + 1);
        //        //设置显示
        //        //SetShow(item, allIndex - (DataManager.Instance.data.CurrentLevel / OneLevelNum));

        //        headIndex++;
        //        tailIndex++;
        //    }
        //    //向右滑
        //    while (content.anchoredPosition.x >= -layout.padding.left - headIndex * (cellSizeY + layout.spacing)
        //    && headIndex != 0)
        //    {
        //        //将数据列表中的最后一个元素移动到第一个
        //        RectTransform item = dataList.Last();
        //        dataList.Remove(item);
        //        dataList.Insert(0, item);

        //        //设置位置
        //        SetPos(item, headIndex - 1);
        //        //设置显示
        //        SetShow(item, headIndex - 1);

        //        headIndex--;
        //        tailIndex--;
        //    }
        //}
    }

    #endregion


    #region Tool

    /// <summary>
    /// 设置位置
    /// </summary>
    private void SetPos(RectTransform trans, int index)
    {
        if (scrollType == ScrollType.Horizontal)
        {
            trans.anchoredPosition = new Vector2
            (
                index == 0 ? layout.padding.left + firstItemAnchoredPos.x :
                layout.padding.left + firstItemAnchoredPos.x + index * (cellSizeY + layout.spacing),
                firstItemAnchoredPos.y
            );
        }
        else if (scrollType == ScrollType.Vertical)
        {
            trans.anchoredPosition = new Vector2
            (
                /*firstItemAnchoredPos.x*/0,
             //index == 0 ? -layout.padding.top + firstItemAnchoredPos.y :
             //index * cellSizeY + - (index * 13) + 17
             firstItemAnchoredPos.y+(cellSizeY-13)*index
            ) ;
            //Debug.Log("item当前位置"+trans.anchoredPosition);
        }
    }
 

    #endregion

    #region 外部调用
    /// <summary>
    /// 初始化
    /// </summary>
    public int allIndex;
    public int nowIndex;

    public void Init()
    {
        startPanel = UIManager.Instance.GetBase<MainPanel>();
        GameManager.Instance.CurrentLevel = DataManager.Instance.data.UnlockLevel;
        cellSizeY = itemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        OneLevelNum = itemPrefab.transform.Find("Level").childCount;
        if ((int)(DataManager.Instance.LevelFoldNum- topLevel.transform.childCount  % OneLevelNum) == 0)
        {
            allIndex = (int)((DataManager.Instance.LevelFoldNum- topLevel.transform.childCount) / OneLevelNum);
        }
        else
        {
            allIndex = (int)((DataManager.Instance.LevelFoldNum- topLevel.transform.childCount) / OneLevelNum) + 1;
        }
        nowIndex =((DataManager.Instance.data.UnlockLevel- moveDownIndex - topLevel.transform.childCount )/ OneLevelNum);

        //Debug.Log("moveDownIndex:"+ moveDownIndex);
        //if ((DataManager.Instance.data.UnlockLevel - moveDownIndex - topLevel.transform.childCount) % OneLevelNum==0)
        //{
        //    nowIndex -= 1;
        //}

        //XDebug.Log((DataManager.Instance.data.UnlockLevel - moveDownIndex - topLevel.transform.childCount)+"当前位置下标" +nowIndex);
        SetTotalCount(allIndex);
        scrollRect = GetComponent<ScrollRect>();

        content = scrollRect.content;
        layout = content.GetComponent<VerticalLayoutGroup>();
        scrollRect.onValueChanged.AddListener((Vector2 v) => OnScroll(v));
       
        //设置布局
        SetLayout();


        //设置头下标和尾下标
        headIndex = nowIndex;
        tailIndex = nowIndex + 1;

        //设置Content大小
        GetFirstItemAnchoredPos();

        //实例化Item
        InitItem();
        topLevel.SetAsLastSibling();
        unLockTran.SetAsLastSibling();
        //得到第一个Item的锚点位置
        InitTopLevel();
        InitPos();
        //Refresh();
        SetUnlockPos();
        SetContentSize();
        InitAvatar();

        //XDebug.Log("当前下表"+nowIndex);
      
    }

   

    Vector2 AvatarDefaultPos = new Vector2(0, 86.6f);



    public void InitAvatar() {
        if (AudioMgr.Instance.mdate.icon !=null)
        {
          avatar.transform.Find("tx").GetComponent<Image>().sprite = AudioMgr.Instance.mdate.icon;
        }
        uILevelItems = content.GetComponentsInChildren<UILevelItem>();
        avatar.transform.SetParent(FindLevelItem(DataManager.Instance.data.UnlockLevel));
        avatar.localPosition = AvatarDefaultPos;
        avatarAtor.SetBool("play", true);
    }
    /// <summary>
    /// 设置头像
    /// </summary>
    public void SetAvatar() {
        Debug.LogError("进入关卡" + (JoinLevel + 1));
        //if (JoinLevel+1==DataManager.Instance.data.UnlockLevel)
        //{
            avatarAtor.SetBool("play", false);
            avatar.transform.SetParent(FindLevelItem(DataManager.Instance.data.UnlockLevel));
            avatar.transform.DOLocalMove(AvatarDefaultPos, 1).OnComplete(()=> { 
            avatarAtor.SetBool("play", true);

                if (LotteryPanel.isShowPanel)
                {
                    LotteryPanel.isShowPanel = false;

                    Observable.TimeInterval(System.TimeSpan.FromSeconds(.2f)).Subscribe(_ =>
                    {
                        LotteryDataManger.Instance.CloseCallBack=(() => {
                            UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);

                        });
                        UIManager.Instance.ShowPopUp<LotteryPanel>();

                    });
                }
                else
                {
                    UIManager.Instance.Show<JoinPop>(UIType.PopUp, DataManager.Instance.data.UnlockLevel);
                }
            });
            JoinLevel = -1;
           
        //}
    }


    public Transform FindLevelItem(int level) {

        foreach (var item in uILevelItems)
        {
            if (item.level==level)
            {
                return item.transform;
            }
        }
        XDebug.LogError("未存在关卡");
        return null;
    
    }

    public void InitTopLevel() {
        return;
        if (-content.anchoredPosition.y <= firstItemAnchoredPos.y)
        {
            SetShow(topLevel, -1);
        }
    }

    /// <summary>
    /// 自动定位
    /// </summary>
    public void AutomaticPos(System.Action animCallBack=null) {
        var currentPos = scrollRect.content.GetComponent<RectTransform>().anchoredPosition;
        //Debug.Log("当前解锁的关卡 "+ DataManager.Instance.data.UnlockLevel+"  自动定位关卡 ");
        DOTween.To(() => currentPos, x => currentPos = x, new Vector2(0, -GetNeedPosY((DataManager.Instance.data.UnlockLevel-moveDownIndex - topLevel.transform.childCount) / OneLevelNum, DataManager.Instance.data.UnlockLevel- moveDownIndex)),0.5F).OnUpdate(()=> {
            scrollRect.content.GetComponent<RectTransform>().anchoredPosition = currentPos;
        }).SetEase(Ease.Linear).OnComplete(()=> {
            mapPos = scrollRect.content.GetComponent<RectTransform>().anchoredPosition;
            if (animCallBack!=null)
            {
                animCallBack();
            }
        });
    }


    public void InitPos() {

        if (DataManager.Instance.data.UnlockLevel <= 4)
        {
            scrollRect.content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else {
            //Debug.Log("当前解锁的关卡 " + DataManager.Instance.data.UnlockLevel + "  自动定位关卡 ");

            scrollRect.content.GetComponent<RectTransform>().anchoredPosition= new Vector2(0, -GetNeedPosY(nowIndex, DataManager.Instance.data.UnlockLevel- moveDownIndex));
        }
        mapPos = scrollRect.content.GetComponent<RectTransform>().anchoredPosition;
    }


    public void SetUnlockPos()
    {
        unLockTran.GetComponentInChildren<Text>().text=string.Format("通关第{0}关解锁",DataManager.Instance.data.UnlockLevel);
        unLockTran.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetNeedPosY((DataManager.Instance.data.UnlockLevel + 20 - 2 - topLevel.transform.childCount) / OneLevelNum, DataManager.Instance.data.UnlockLevel+20-2));

        
    }

    public float GetNeedPosY(int mapIndex,int level) {

        //Debug.Log("?????" + mapIndex + "EEE" + ((level - topLevel.childCount) % OneLevelNum)+ "///"+ (level - topLevel.childCount)+"/// "+ OneLevelNum + "  return " + (mapIndex * (cellSizeY) + firstItemAnchoredPos.y + ((cellSizeY / OneLevelNum) * ((level - topLevel.childCount) % OneLevelNum))));
       
        return (mapIndex * (cellSizeY) + firstItemAnchoredPos.y + ((cellSizeY / OneLevelNum) * ((level - topLevel.childCount) % OneLevelNum)));
    }
    //public int StepDefaulPos() {

    //    //Debug.Log("当前地图下标"+ allIndex + "KK" +((int)tailIndex - (DataManager.Instance.data.CurrentLevel / OneLevelNum)));
    //    //return (int;

    //}

    /// <summary>
    /// 设置显示
    /// </summary>
    RectTransform Xtransform;
    int Nindex;
    public void SetShow(RectTransform trans, int index)
    {
        //return;
        //=====根据需求进行编写
        trans.gameObject.SetActive(true);
        //Debug.Log(index);
        Xtransform = trans;
        Nindex = index;
        trans.GetComponent<OneMap>().Refresh(index);
        //trans.gameObject.SetActiveRecursively
        //trans.GetComponentInChildren<Text>().text = index.ToString();
        //trans.name = index.ToString();
    }

    public void SetShowOut() {

        SetShow(Xtransform, Nindex);
    }

    /// <summary>
    /// 设置总的数据数量
    /// </summary>
    public void SetTotalCount(int count)
    {
        totalCount = count;
    }

    /// <summary>
    /// 销毁所有的元素
    /// </summary>
    public void DestoryAll()
    {
        for (int i = dataList.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(dataList[i].gameObject);
        }
        dataList.Clear();
    }

    #endregion
}

/// <summary>
/// 滑动类型
/// </summary>
public enum ScrollType
{
    Horizontal,//竖直滑动
    Vertical,//水平滑动
}