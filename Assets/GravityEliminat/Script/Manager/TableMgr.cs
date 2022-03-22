using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using EasyExcel;
using EasyExcelGenerated;
public class TableMgr : Singlton<TableMgr>
{
    public readonly EEDataManager _eeDataManager = new EEDataManager();
 public   StarScore star;//每关获取一次
    // Start is called before the first frame update
    public void Init() {
        _eeDataManager.Load();

    }

    #region 分数
    /// <summary>
    /// 获取星光宝箱表格
    /// </summary>
    /// <param name="level"></param>
    public void GetScoreTable(int level) {

        star = _eeDataManager.Get<StarScore>(level);
    }
    public StarScore GetScore(int level)
    {
       return  _eeDataManager.Get<StarScore>(level);
    }
    public float GetStarPro(int nowScore) {

        //Debug.Log(nowScore + "/" + star.FullMarks  + "lll" + ((float)nowScore / (star.FullMarks ) * 0.66F));
        return Mathf.Lerp(0.1f,0.66f, ((float)nowScore / (star.FullMarks))) ;

    }


    public int GetDiamondNum(bool isPassed) {

        if (isPassed)
        {
            return star.diamond;
        }
        else {

            return star.OneDiamond;

        }

    }
    #endregion


    public List<int> InitTicketLevel() {

        List<int> temp = new List<int>();
        int N = _eeDataManager.GetList<StarScore>().Count;
        for (int i = 1; i <= N; i++)
        {
            if (_eeDataManager.Get<StarScore>(i).raffle == 1)
            {
                temp.Add(i);
            }
        }
        return temp;
    }

    public int GetTickLevel() {

        for (int i = 0; i < DataManager.Instance.data.TicketLevel.Count; i++)
        {
            if (DataManager.Instance.data.UnlockLevel< DataManager.Instance.data.TicketLevel[i])
            {
                return DataManager.Instance.data.TicketLevel[i];
            }
        }
        return 0;
    
    }

    public List<int> InitMoneyLevel() {

        List<int> temp = new List<int>();
        int N = _eeDataManager.GetList<StarScore>().Count;
        for (int i = 1; i <= N; i++)
        {
            if (_eeDataManager.Get<StarScore>(i).money == 1)
            {
                temp.Add(i);
            }
        }
        return temp;
    }

    #region 宝石

    public List<int> InitGemData() {
        List<int> temp = new List<int>();
      int N=  _eeDataManager.GetList<StarScore>().Count;
        for (int i = 1; i <= N; i++)
        {
            if (_eeDataManager.Get<StarScore>(i).gem==1)
            {
                temp.Add(i);
            } 
        }
        return temp;
    }

    public bool IsColorGem(int Level) {

        if (DataManager.Instance.data.GemLevel.Contains(Level))
        {
            return true;
        }
        return false;
    }

    public ShareRedDataManger.DiamondsType GetGemColor() {

        var H = _eeDataManager.GetList<Share>().Count;
        int sum = 0;
        for (int i = 1; i <= H; i++)
        {
            sum += _eeDataManager.Get<Share>(i).weight;
        }

        int R = Random.Range(0, sum);
        int now = 0;
        for (int i = 1; i <=H; i++)
        {
            now += _eeDataManager.Get<Share>(i).weight;
            if (R<now)
            {
                switch (_eeDataManager.Get<Share>(i).gem)
                {
                    case "青色宝石":
                        return ShareRedDataManger.DiamondsType.cyan;
                    case "紫色宝石":
                        return ShareRedDataManger.DiamondsType.purple;
                    case "黄色宝石":
                        return ShareRedDataManger.DiamondsType.yellow;
                    case "红色宝石":
                        return ShareRedDataManger.DiamondsType.red;
                    case "蓝色宝石":
                        return ShareRedDataManger.DiamondsType.blue;
                    //case "青色宝石":
                    //    return ShareRedDataManger.DiamondsType.cyan;

                    default:
                        break;
                }
            }
        }
        return ShareRedDataManger.DiamondsType.cyan;
    }

    #endregion

    #region 引导
    public bool NeedMapGuide(int level)
    {
       
        if (DataManager.Instance.data.NeedGudieMap.Contains(level))
        {
            return true;
        }
        return false;


    }

    public string GuideImg1() {

#if Easy
        return _eeDataManager.Get<Guide>(GameManager.Instance.CurrentLevel).SpriteName1;
#else
        return _eeDataManager.Get<Guide>(GameManager.Instance.CurrentLevel).SpriteName1;
#endif

    }

    public string GuideImg2()
    {

#if Easy
        return _eeDataManager.Get<Guide>(GameManager.Instance.CurrentLevel).SpriteName2;
#else
        return _eeDataManager.Get<Guide>(GameManager.Instance.CurrentLevel).SpriteName2;

#endif

    }

    public string GuideDes() {

#if Easy
        return _eeDataManager.Get<Guide>(GameManager.Instance.CurrentLevel).Des1;

#else
        return _eeDataManager.Get<Guide>(GameManager.Instance.CurrentLevel).Des1;

#endif
    }

    public List<int> GuideData() {


        List<int> temp = new List<int>();
#if Easy
        foreach (var item in _eeDataManager.GetList<Guide>())
        {
            temp.Add(item.ID);
        }
        return temp;
#else
       foreach (var item in _eeDataManager.GetList<Guide>())
        {
            temp.Add(item.ID);
        }
        return temp;
#endif


    }
    #endregion



}
