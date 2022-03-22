using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEditor;

public static class StringExtension
{
    
    public static void CheckNullStr(string s)
    {       
        if (s == "\n")
        {
            XDebug.Log("n");
        }
        else if (s == "\f")
        {
            XDebug.Log("f");
        }
        else if (s == "\r")
        {
            XDebug.Log("r");
        }
        else if (s == "\t")
        {
            XDebug.Log("t");
        }
        else if (s == "\v")
        {
            XDebug.Log("v");
        }
        else if(string.IsNullOrEmpty(s))
        {
            XDebug.Log("null");
        }
    }
    public static bool IsNullStr(string s)
    {
        bool mS = true;
        if(string.IsNullOrEmpty(s))
        {
            mS = true;
        }
        else
        {
            switch (s)
            {
                case "\n":
                case "\f":
                case "\r":
                case "\t":
                case "\v":
                    break;
                default:
                    mS = false;
                    break;
            }
        }        
        return mS;
    }

    /// <summary>
    /// 检查字符串
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsEffectStr(this string s)
    {
        bool b = false;
        if (s == "\n")
        {
            XDebug.Log("n");
        }
        else if (s == "\f")
        {
            XDebug.Log("f");
        }
        else if (s == "\r")
        {
            XDebug.Log("r");
        }
        else if (s == "\t")
        {
            XDebug.Log("t");
        }
        else if (s == "\v")
        {
            XDebug.Log("v");
        }
        else if (string.IsNullOrEmpty(s))
        {
            XDebug.Log("null");
            b = true;
        }        
        switch (s)
        {
            case "\n":
            case "\f":
            case "\r":
            case "\t":
            case "\v":            
                b = true;
                break;
            default:
                break;
        }
        return b;

    }

    public static bool IsNullOrEmpty(this string s)
    {
        bool isTrue = false;
        if(string.IsNullOrEmpty(s))
        {
            isTrue = true;
        }
        else
        {
            isTrue = false;
        }
        return isTrue;
    }    


    #region StringBuilder 待测试
   
    private static StringBuilder GetStringBuilder(int capacity)
    {
        return new StringBuilder(capacity);
    }   
    public static void AddString(this StringBuilder s,string addStr)
    {
        s.Append(addStr);           
    }
    public static void AddStringFormat(this StringBuilder s,string str1,string str2)
    {
        s.AppendFormat(str1, str2);        
    }    
    public static void UpdateString(this StringBuilder s,string str)
    {
        s.Clear();
        s.Append(str);
    }

    public static void ResetString(this StringBuilder s)
    {
        s.Clear();
    }


    #endregion


    //a b c d e f g h i j k m n o p q r s t u v w x y z
    // ab ac ad ae af ag ah ai aj ak am an ....
    private static string mUnitStr = "KMABCDEFGHIJLNOPQRSTUVWXYZ";
    private static int UnitEvolve = 3;//三位一跳
    private static List<string> mUnitList;
    private static List<string> UnitList
    {
        get
        {
            if(mUnitList==null)
            {
                mUnitList = new List<string>(mUnitStr.Length);
                for (int i = 0; i < mUnitStr.Length; i++)
                {                   
                    mUnitList.Add(mUnitStr[i].ToString());
                }

                for (int i = 0; i < mUnitList.Count; i++)
                {
                    mDic_Unit.Add(mUnitList[i], (i + 1) * UnitEvolve);
                }
            }
            return mUnitList;
        }
    }

    private static Dictionary<string, int> mDic_Unit = new Dictionary<string, int>(mUnitStr.Length);


    #region Doub计算
    public static string GetDoubleStr(this double d)
    {
        //XDebug.Log(d);

        string mstr = d.ToString();        
        if (!mstr.Contains("+"))
        {
            //没有使用科学计数法
            var lenth = mstr.Length;           
            for (int i = lenth-1; i >0; i--)
            {
                if(i% UnitEvolve == 0)
                {
                    GetStr(i, d);
                    break;
                }
            }            
        }
        else
        {            
            var strs = mstr.Split('+');
            var one = strs[0].TryParseFloat();
            var index = strs[1].TryParseInt();
            for (int i = index - 1; i > 0; i--)
            {
                if (i % UnitEvolve == 0)
                {
                    GetStr(i, d);
                    break;
                }
            }
        }
        
        return mstr;        
    }   

    private static string GetStr(int lenth,double d)
    {
        var str1 = (d / Mathf.Pow(10, lenth));
        var index = (int)Mathf.Round((lenth / UnitEvolve) - 1);
        string str2 = null;
        if(index<UnitList.Count)
        {
            str2 = UnitList[index];
        }
        else
        {
            var fullIndex = index - UnitList.Count;
            str2 = UnitList[index] + UnitList[fullIndex];
        }
        XDebug.Log(lenth + "/" + d + "->" + str1 + str2);
        return str1 + str2;
    }
    #endregion

    #region String计算

    private static void Init2()
    {        
        //GetStringStr("10000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    public static string GetStringStr(this string mstr)
    {               
        //没有使用科学计数法
        var lenth = mstr.Length;
        for (int i = lenth - 1; i > 0; i--)
        {
            if (i % UnitEvolve == 0)
            {
                GetStr(i, mstr);
                break;
            }
        }
        return mstr;
    }
    private static string GetStr(int lenth, string d)
    {        
        var str1=d.Remove(d.Length-lenth,lenth);
        var index = (int)Mathf.Round((lenth / UnitEvolve) - 1);
        string str2 = null;
        if (index < UnitList.Count)
        {
            str2 = UnitList[index];
        }
        else
        {
            var fullIndex = index - UnitList.Count;
            str2 = UnitList[index] + UnitList[fullIndex];
        }
        XDebug.Log(lenth + "/" + d + "->" + str1 + str2);
        return str1 + str2;
    }
    #endregion



    #region mTest
    static string mStr = "开始游戏第关抽奖领取超级红包小时金币收倍收益加速收益惊喜再抽次可领取" +
        "已经接受到个红包直接领取玩游戏就能不断得获得现金红包红包雨红包元余额立即提现签到任务" +
        "设置登陆签到连续签到天可获得超级红包第一天二三四五六七八九十拼手气获得大额红包,最高元" +
        "已领取每日任务成就勤劳的老司机合成5辆车领取" +
        "赛车拥有者获得过最少辆级车" +
        "赛车爱好者赛车收藏者赛车大亨车王声音音效" +
        "加速快来体验加速收益的快感吧获得小时加速时间免费商店" +
        "蓝黄货防护车押运车复古道奇维多利亚皇冠高尔夫大卡尔启达库奇林加林肯加长林加敞篷车皮卡" +
        "商务车货拉拉救护车超-红包车光越野悍马改装牧马人金币福袋水泥大货车矿救护车冬克沃特" +
        "压路车推土车柯尼塞格金福车" +
        "商店加速" +
        "提现说明非实名用户账户无法支持提现，请务必将提现的微信号进行实名认证当玩家发起提现后，提现的金额会在次日点之前打入玩家的账户本次活动最解释权由版权方所有" +
        "常规提现当前现金金额提现恭喜通关获得红包不了谢谢秒" +
        "解锁新车红包放弃" +
        "选择赛车" +
        "赛车等级越高过弯越容易操控完成一次合成后解锁" +
        "开始游戏第关加速秒红包雨可开启幸运转盘提现元领取大量金币商店天降红包导航栏签到任务设置声音音效登录签到连续签到天可获得超级红包第一二三四五六七天拼手气获得大额红包最高元每日任务成就赛车拥有者获得过至少辆级车赛车爱好者收藏者大亨车王车神勤劳的老司机合成车已领取快来体验加速收益的快感吧获取小时加速时间免费小蓝车小黄车运货车防护押运车复古道奇维多利亚皇冠高尔夫大卡尔启达库奇林加林肯加长敞篷车皮卡商务车救护车货拉拉超光红包车越野车悍马改牧马人福袋车水泥车大货车矿车救护车冬克沃特压路车推土车柯尼塞格金福车天降红包等级越高过弯容易操控倒计时已经接到个一大波红包雨即将到来幸运大转盘抽奖收益加速惊喜超级再抽次可领奖当前现金金额常规提现新人专享元立即提现去赚红包提现说明非实名用户账户无法支持提现请务必将提现的微信号进行实名认证当玩家发起提现后提现的金额会在次日点之前打入玩家的账户本次活动最终解释权由版权方所有恭喜通关获得开不了谢谢过关红包余额双倍领取玩游戏就能不断获得现金红包直接领取立即提现余额不足请继续游戏赚取余额点击复活挑战失败赛车等级越高过弯越容易返回主界面安慰红包升级为超级红包";

#if UNITY_EDITOR
    [MenuItem("GameEditor/获得非重复文字")]
    public static void GetData()
    {
        List<string> mlis = new List<string>(100);
        var count = mStr.Length;
        string eamp = null;

        for (int i = 0; i < count; i++)
        {
            eamp = mStr[i].ToString();
            if(mlis.Contains(eamp)==false)
            {
                mlis.Add(eamp);
            }
        }

        string eampStr = null;
        foreach (var item in mlis)
        {
            eampStr += item;
        }
        XDebug.LogError(eampStr);
    }
#endif
#endregion

}
