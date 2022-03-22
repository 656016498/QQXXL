using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class Pool : SinglMonoBehaviour<Pool>
{

    public static string PoolName_UI => "UI";
    public static string UI_Ball => "BallFly";
    public static string UI_ScoreText => "ScoreText";

    public static string LoveImg => "LoveImg";
    public static string FlyImg => "FlyImg";
    public static string IceFly => "SugarFly";

    public static string Cat => "CatBall";
    #region 球

    public static string Ball_PoolName => "BallPool";
    public static string Ball_Color => "Ball";
    public static string Ball_Arrive => "Arrive";
    //public static string Ball_Red => "Red";
    //public static string Ball_Greeb => "Green";
    public static string Ball_Soda => "Soda";

    public static string Ball_Chocolate => "Chocolate";
    public static string Ball_SizeBall => "SizeBall";

    public static string Ball_CandyCoat => "CandyCoat";

    public static string Ball_CornKernel => "CornKernel";

    public static string Ball_PopCorn => "PopCorn";
    public static string Ball_FreezeBall => "FreezeBall";

    public static string Ball_SuberCube => "SugarCube";

    public static string Ball_Dye => "Dye_Blue";

    public static string MoneyBall => "MoneyBall";
    public static string TicketBall => "Ticket";
    #endregion

    #region 道具
    public static string MagicBottle => "MagicBottle";

    public static string UprihtProp = "UprightProp";
    public static string TornadoProp = "TornadoBtn";
    public static string Tornado = "tornado";
    public static string XProp = "XProp";
    public static string BoxProp= "BoxProp";
    public static string GreenBomb = "GreenBomb";

    public static string BlueProp => "BlueProp";
    public static string LightBall => "LightBall";
    public static string SmallLightBall => "SmallLightBall";
    public static string PurpProp => "PurpProp";
    public static string Prop_PoolName => "PropBall";
    public static string HorizontalProp => "HorizontalProp";
    public static string HengProp => "HengProp";
    public static string ShiziProp => "ShiziProp";
    public static string QQProp => "CircleProp";


    public static string CrossProp => "CrossProp";
    public static string BombProp => "BombProp";

    public static string PropGreen => "PropGreen";
    public static string EliminateOrgne => "EliminateOrgne";

    public static string DelayedProp => "DelayedProp";

    public static string DelayedBomb => "DelayedBomb";

    #endregion


    #region 特效
    public static string BuShu5 => "effect_shengyubushu_5";
    public static string BuShu10 => "effect_shengyubushu_10";

    public static string ChoosePorpBtn => "effect_daojutishi01";
    public static string HitBall => "effect_dianjite01";
    public static string LoveArrive => "effect_tilixiaohao01";
    public static string ElecriBomb => "effect_dianqiu03";
    public static string ElecriBall => "effect_dianqiu01";
    public static string ElectricWire => "effect_dianqiu02";
    public static string Yellow_LD => "effect_leidianquan01";
    public static string Effect_PoolName => "AllEffect";
    public static string effect_heiyunFly => "effect_heiyun03";

    public static string SizeBallBomb => "effect_yingtangxc01";

    public static string ChocolatePoLie => "effect_qiaokelihe02";

    public static string ChocolateCodyPoL => "effect_qiaokelihe01";

    public static string CandyCoat => "effect_tangguoxc01";

    public static string SodaBomb => "effect_qishuibaopo01";

    public static string LiheBomb => "effect_lihebaozha01";

    public static string BingKuai => "effect_bingkuaibaozha01";

    //public static string DyeBottleBome => "effect_ranseping01_1";
    //public static string DyeFly => "effect_ranseping01_2";
    //public static string DoDye => "effect_ranseping01_3";

    public static string CornKernelBomb => "effect_yumizha01";

    public static string CornBomb => "effect_baomihua01";

    public static string HiveBomb => "effect_fengmiiaochu01";

    public static string SugarBomb => "effect_bingtangxiaochu01";

    public static string IceState => "effect_bingtangxingdian01";

    public static string LDLine => "effect_leidianbianji02";

    public static string HitWallEffect => "effect_zhuangjidian";
    public static string HorizontalBomb => "effect_hengxiangbaozha01";

    public static string CrossBomb => "effect_xxingbaozha01";

    public static string CircularBomb => "effect_yuanxingbaozha01";

    public static string UprightBomb => "effect_shuxiangbaozha01";
    public static string HengBomb => "effect_hengxiangbaozha01";

    public static string Xbomb => "effect_shizibaozha02";

    public static string BoxBomb => "effect_fangxingbaozha01";

    public static string GreenBombX => "effect_fangxingbaozha02";

    public static string LiuGuang => "effect_liuguangtuowei";

    public static string GreenBombEffect => "effect_yuandibaozha01";

    public static string OneStepBomb = "effect_chongjibaozha01";
    public static string TwoStepBomb = "effect_fangxingmiaobianbaozha01";
    public static string ThreeBomb = "effect_yuanbaozhayun01";

    public static string BallEnimlit = "effect_baozha01";

    public static string AddStepEffect = "effect_bushu01";
    public static string StepEffect2 = "effect_bushu02";

    public static string EnergyStorage = "effect_yaoshuimanzai01";

    public static string BottleBomb => "effect_yaoshuizha01";

    public static string StarMark => "effect_yaoshuibiaojidian01";

    public static string StarFly => "effect_yaoshuituowei01";


    public static string PropSpawn => "effect_baoxing01";

    public static string YellowProp => "effect_qukuairanseping01";

    public static string LiuXinBomb => "effect_liuxingxiaochu01";


    public static string BroomEffect => "effect_fenleidasao01";

    public static string TiLiArrive => "effect_tiliaixing01";
    public static string StarDown => "effect_jinduxingxing01";

    public static string YaoSuiBoom => "effect_yaoshuibaozha01";

    public static string ShiZhiBoom => "effect_shizibaozha01";

    public static string ShuBoom=> "effect_shuxiangbaozha01";
    public static string StepLiuXin => "effect_bushu03";
    public static string StarFM => "effect_xingmo01";
    public static string XXFM => "ximmo";
    
    public static string PeopLsBule => "effect_ransequyu01";
    public static string PeopLsGreen => "effect_ransequyu02";
    public static string PeopLsOrgin => "effect_ransequyu03";

    public static string PeopLsPuple => "effect_ransequyu04";

    public static string PeopLsRed => "effect_ransequyu05";

    public static string PeopLsYellow => "effect_ransequyu06";

    public static string HightL => "effect_tishixiaochu01";

    public static string FingerPoint => "effect_dianjite01";

    public static string DyeBoom => "effect_ranseping_1";

    public static string DyeFlyEffect => "effect_ranseping_2";
    public static string DyeBall => "effect_ranseping_3";

    public static string LevelStarza => "effect_guanqiaxing01";

    public static string NewLevel => "effect_xinguanqia01";

    public static string HbEffect => "effect_tixiansanqian01";
    public static string HbEffect2 => "effect_money2";


    //public static string GreenFen = "effect_fangxingbaozha02";

    #endregion
    // Start is called before the first frame update
    public SpawnPool sp;
    public Transform Spawn(string poolName,string pefrb) {

        return PoolManager.Pools[poolName].Spawn(pefrb);
    }

    public Transform SpawnEffect(string poolName, string pefrb, Vector3 orgin, float scale=1,float times=3)
    {
        Transform effect = PoolManager.Pools[poolName].Spawn(pefrb);
        effect.position = orgin;
        effect.localScale = Vector3.one * scale;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(times)).Subscribe(_ => {
            Despawn(poolName, effect);
        });
        return effect;
    }

    public Transform SpawnEffectByParent(string poolName, string pefrb,Transform parent ,Vector3 orgin, float scale = 1, float times = 3)
    {
        Transform effect = PoolManager.Pools[poolName].Spawn(pefrb);
        effect.SetParent(parent);
        effect.localPosition = orgin;
        effect.localEulerAngles = Vector3.zero;
        effect.localScale = Vector3.one * scale;
        Observable.TimeInterval(System.TimeSpan.FromSeconds(times)).Subscribe(_ => {
            Despawn(poolName, effect);
        });
        return effect;
    }

    /// <summary>
    /// 生成标记特效
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="pefrb"></param>
    /// <param name="Mparent"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public Transform SpawnMarkEffect(string poolName, string pefrb, Transform Mparent, float scale=1,float time = 1)
    {
        Transform transform = PoolManager.Pools[poolName].Spawn(pefrb);
        transform.position = Mparent.position;
        transform.localScale = Vector3.one* scale;
        transform.SetParent(Mparent);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(time)).Subscribe(_ =>
        {
            Despawn(Effect_PoolName, transform);
        });
        return transform;
    }

    public void Despawn(string poolName, Transform self) {

        if (self == null) return;
        PoolManager.Pools[poolName].Despawn(self,0,Pool.Instance.transform);
    }
    public Transform SpawnEffectByPos(string poolName, string pefrb, Vector3 Mparent, float bs = 1, float AD = 0)
    {
        Transform effect = PoolManager.Pools[poolName].Spawn(pefrb);
        effect.position = Mparent;
        effect.localScale = Vector3.one * (bs + AD);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(3)).Subscribe(_ => {
            Despawn(Effect_PoolName, effect);
        });
        return effect;
    }

}
