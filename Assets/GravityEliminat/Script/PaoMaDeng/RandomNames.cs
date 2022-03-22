﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 随机名字
/// </summary>
public static class RandomNames
{
    #region 百家姓
    private readonly static string mFamilNameData =
        "赵钱孙李，周吴郑王。冯陈褚卫，蒋沈韩杨。" +
        "朱秦尤许，何吕施张。孔曹严华，金魏陶姜。" +
        "戚谢邹喻，柏水窦章。云苏潘葛，奚范彭郎。" +
        "鲁韦昌马，苗凤花方。俞任袁柳，酆鲍史唐。" +
        "费廉岑薛，雷贺倪汤。滕殷罗毕，郝邬安常。" +
        "乐于时傅，皮卞齐康。伍余元卜，顾孟平黄。" +
        "和穆萧尹，姚邵湛汪。祁毛禹狄，米贝明臧。" +
        "计伏成戴，谈宋茅庞。熊纪舒屈，项祝董梁。" +
        "杜阮蓝闵，席季麻强。贾路娄危，江童颜郭。" +
        "梅盛林刁，钟徐邱骆。高夏蔡田，樊胡凌霍。" +
        "虞万支柯，昝管卢莫。经房裘缪，干解应宗。" +
        "丁宣贲邓，郁单杭洪。包诸左石，崔吉钮龚。" +
        "程嵇邢滑，裴陆荣翁。荀羊於惠，甄曲家封。" +
        "芮羿储靳，汲邴糜松。井段富巫，乌焦巴弓。" +
        "牧隗山谷，车侯宓蓬。全郗班仰，秋仲伊宫。" +
        "宁仇栾暴，甘钭厉戎。祖武符刘，景詹束龙。" +
        "叶幸司韶，郜黎蓟薄。印宿白怀，蒲邰从鄂。" +
        "索咸籍赖，卓蔺屠蒙。池乔阴鬱，胥能苍双。" +
        "闻莘党翟，谭贡劳逄。姬申扶堵，冉宰郦雍。" +
        "卻璩桑桂，濮牛寿通。边扈燕冀，郏浦尚农。" +
        "温别庄晏，柴瞿阎充。慕连茹习，宦艾鱼容。" +
        "向古易慎，戈廖庾终。暨居衡步，都耿满弘。" +
        "匡国文寇，广禄阙东。欧殳沃利，蔚越夔隆。" +
        "师巩厍聂，晁勾敖融。冷訾辛阚，那简饶空。" +
        "曾毋沙乜，养鞠须丰。巢关蒯相，查后荆红。" +
        "游竺权逯，盖益桓公。万俟司马，上官欧阳。" +
        "夏侯诸葛，闻人东方。澹台公冶，宗政濮阳。" +
        "淳于单于，太叔申屠。公孙仲孙，轩辕令狐。" +
        "钟离宇文，长孙慕容。鲜于闾丘，司徒司空。" +
        "丌官司寇，仉督子车。颛孙端木，巫马公西。" +
        "漆雕乐正，壤驷公良。拓跋夹谷，宰父谷梁。" +
        "晋楚闫法，汝鄢涂钦。段干百里，东郭南门。" +
        "呼延归海，羊舌微生。岳帅缑亢，况郈有琴。" +
        "梁丘左丘，东门西门。商牟佘佴，伯赏南宫。" +
        "墨哈谯笪，年爱阳佟。第五言福，百家姓终。";
    #endregion

    #region 千字文
    private readonly static string Qiangziwen =
"天地玄黄，宇宙洪荒。日月盈昃，辰宿列张。寒来暑往，秋收冬藏。闰余成岁，律吕调阳。云腾致雨，露结为霜。金生丽水，玉出昆冈。剑号巨阙，珠称夜光。" +
"果珍李柰，菜重芥姜。海咸河淡，鳞潜羽翔。龙师火帝，鸟官人皇始制文字，乃服衣裳。推位让国，有虞陶唐。吊民伐罪，周发殷汤。坐朝问道，垂拱平章。" +
"爱育黎首，臣伏戎羌。遐迩一体，率宾归王。鸣凤在竹，白驹食场。，赖及万方盖此身发，四大五常。恭惟鞠养，岂敢毁伤。女慕贞洁，男效才良。知过必改，得能莫忘。" +
"罔谈彼短，靡恃己长。信使可覆，器欲难量。墨悲丝染，诗赞羔羊。景行维贤，克念作圣。德建名立，形端表正。空谷传声，虚堂习听。祸因恶积，福缘善庆。" +
"尺璧非宝，寸阴是竞。资父事君，曰严与敬。孝当竭力，忠则尽命。临深履薄，夙兴温凊。似兰斯馨，如松之盛。川流不息，渊澄取映。容止若思，言辞安定。笃初诚美，慎终宜令。" +
"荣业所基，籍甚无竟。学优登仕，摄职从政。存以甘棠，去而益咏。乐殊贵贱，礼别尊卑。上和下睦，夫唱妇随。外受傅训，入奉母仪。诸姑伯叔，犹子比儿。孔怀兄弟，同气连枝。" +
"交友投分，切磨箴规。仁慈隐恻，造次弗离。节义廉退，颠沛匪亏。性静情逸，心动神疲。守真志满，逐物意移。坚持雅操，好爵自縻。都邑华夏，东西二京。背邙面洛，浮渭据泾。" +
"宫殿盘郁，楼观飞惊。图写禽兽，画彩仙灵。丙舍旁启，甲帐对楹。肆筵设席，鼓瑟吹笙。升阶纳陛，弁转疑星。右通广内，左达承明。既集坟典，亦聚群英。杜稿钟隶，漆书壁经。" +
"府罗将相，路侠槐卿。户封八县，家给千兵。高冠陪辇，驱毂振缨。世禄侈富，车驾肥轻。策功茂实，勒碑刻铭。盘溪伊尹，佐时阿衡。奄宅曲阜，微旦孰营。桓公匡合，济弱扶倾。" +
"绮回汉惠，说感武丁。俊义密勿，多士实宁。晋楚更霸，赵魏困横。假途灭虢，践土会盟。何遵约法，韩弊烦刑。起翦颇牧，用军最精。宣威沙漠，驰誉丹青。九州禹迹，百郡秦并。" +
"岳宗泰岱，禅主云亭。雁门紫塞，鸡田赤城。昆池碣石，钜野洞庭。旷远绵邈，岩岫杳冥。治本于农，务兹稼穑。俶载南亩，我艺黍稷。税熟贡新，劝赏黜陟。孟轲敦素，史鱼秉直。" +
"庶几中庸，劳谦谨敕。聆音察理，鉴貌辨色。贻厥嘉猷，勉其祗植。省躬讥诫，宠增抗极。殆辱近耻，林皋幸即。两疏见机，解组谁逼。索居闲处，沉默寂寥。求古寻论，散虑逍遥。" +
"欣奏累遣，戚谢欢招。渠荷的历，园莽抽条。枇杷晚翠，梧桐蚤凋。陈根委翳，落叶飘摇。游鹍独运，凌摩绛霄。耽读玩市，寓目囊箱。易輶攸畏，属耳垣墙。具膳餐饭，适口充肠。" +
"饱饫烹宰，饥厌糟糠。亲戚故旧，老少异粮。妾御绩纺，侍巾帷房。纨扇圆洁，银烛炜煌。昼眠夕寐，蓝笋象床。弦歌酒宴，接杯举觞。矫手顿足，悦豫且康。嫡后嗣续，祭祀烝尝。" +
"稽颡再拜，悚惧恐惶。笺牒简要，顾答审详。骸垢想浴，执热愿凉。驴骡犊特，骇跃超骧。诛斩贼盗，捕获叛亡。布射僚丸，嵇琴阮啸。恬笔伦纸，钧巧任钓。释纷利俗，并皆佳妙。" +
"毛施淑姿，工颦妍笑。年矢每催，曦晖朗曜。璇玑悬斡，晦魄环照。指薪修祜，永绥吉劭。矩步引领，俯仰廊庙。束带矜庄，徘徊瞻眺。孤陋寡闻，愚蒙等诮。谓语助者，焉哉乎也。";
    #endregion

    private static List<string> mList_familname;
    public static List<string> mGetList_familName
    {
        get
        {
            if(mList_familname==null)
            {
                mList_familname = new List<string>(mFamilNameData.Length);
                var newdata = mFamilNameData.Replace("，", "");
                newdata = newdata.Replace("。", "");
                for (int i = 0; i < newdata.Length; i++)
                {
                    mList_familname.Add(newdata[i].ToString());                   
                }                
            }
            return mList_familname;
        }
    }

    private static string mName = null;
    private static string mGetName
    {
        get
        {
            if(mName==null)
            {
                mName = Qiangziwen.Replace("，", "").Replace("。", "");
            }
            return mName;
        }
    }
    
    
    public static string GetName_2()
    {
        var mlist = mGetList_familName;
        var value = Random.Range(0, mlist.Count);
        var familname = mlist[value];
        var namevalue = Random.Range(0, mGetName.Length);
        var name = mGetName[namevalue].ToString();
        return string.Format("{0}{1}", familname, name);
    }

    public static string GetName_2(string inset)
    {
        var mlist = mGetList_familName;
        var value = Random.Range(0, mlist.Count);
        var familname = mlist[value];
        var namevalue = Random.Range(0, mGetName.Length);
        var name = mGetName[namevalue].ToString();
        return string.Format("{0}{1}{2}", familname, inset, name);
    }


    public static string GetName_3()
    {
        var name1 = GetName_2();
        var namevalue = Random.Range(0, mGetName.Length);
        var name = mGetName[namevalue].ToString();
        return name1 + name;
    }    
}

