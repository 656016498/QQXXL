using EasyExcel;
using EasyExcelGenerated;
using System.Numerics;
using System;
using System.Collections.Generic;

public class PropManger : Singlton<PropManger>
{
    public float times = 0.8F;
    public readonly EEDataManager _eeDataManager = new EEDataManager();
    public List<Prop> allProp = new List<Prop>();
    //public List<Ball> togetherBall = new List<Ball>();
    public float ReadyNum = 0;
    public void Init() {
        _eeDataManager.Load();
    }

    public void BeginOnCilck() {
        ReadyNum = 0;
    }

    /// <summary>
    /// 引导爆炸
    /// </summary>
    public void GuideExplosion() {
        foreach (var item in allProp)
        {
            item.SetReady(item.transform);
        }
        allProp.Clear();
    }


    public float[] GetRang(string key, int Gear, Porp_Size porp_Size)
    {
        
        PropData prop = _eeDataManager.Get<PropData>(key);
        switch (Gear)
        {
            case 1:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.Gear1Size1;
                    case Porp_Size.中:
                        return prop.Gear1Size2;
                    case Porp_Size.大:
                        return prop.Gear1Size3;
                    default:
                        break;
                }
                break;
            case 2:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.Gear2Size1;
                    case Porp_Size.中:
                        return prop.Gear2Size2;
                    case Porp_Size.大:
                        return prop.Gear2Size3;
                    default:
                        break;
                }
                break;
            case 3:

                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.Gear3Size1;
                    case Porp_Size.中:
                        return prop.Gear3Size2;
                    case Porp_Size.大:
                        return prop.Gear3Size3;
                    default:
                        break;
                }
                break;
        }
        return null;

    }

    public float GetEffectSize(string key, int Gear, Porp_Size porp_Size)
    {
        PropData prop = _eeDataManager.Get<PropData>(key);
        switch (Gear)
        {

            case 1:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.Effect11;

                    case Porp_Size.中:
                        return prop.Effect12;
                    case Porp_Size.大:
                        return prop.Effect13;
                    default:
                        break;
                }

                break;
            case 2:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.Effect21;
                    case Porp_Size.中:
                        return prop.Effect22;
                    case Porp_Size.大:
                        return prop.Effect23;
                    default:
                        break;
                }

                break;
            case 3:

                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.Effect31;
                    case Porp_Size.中:
                        return prop.Effect32;
                    case Porp_Size.大:
                        return prop.Effect33;
                    default:
                        break;
                }
                break;
        }
        return 1;

    }


    public int GetCubeNum(string key, int Gear) {

        PropData prop = _eeDataManager.Get<PropData>(key);
        switch (Gear)
        {
            case 1:
                return prop.MarsSquareGear1;
            case 2:
                return prop.MarsSquareGear2;
            case 3:
                return prop.MarsSquareGear3;
        }

        return 1;
    }


    public float[] GetChildRang(string key, int Gear,Porp_Size porp_Size) { 

        PropData prop = _eeDataManager.Get<PropData>(key);
        switch (Gear)
        {

            case 1:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.GBombGear1Size1;
                    case Porp_Size.中:
                        return prop.GBombGear1Size2;
                    case Porp_Size.大:
                        return prop.GBombGear1Size3;
                    default:
                        break;
                }

                break;
            case 2:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.GBombGear2Size1;
                    case Porp_Size.中:
                        return prop.GBombGear2Size2;
                    case Porp_Size.大:
                        return prop.GBombGear2Size3;
                    default:
                        break;
                }

                break;
            case 3:

                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.GBombGear3Size1;
                    case Porp_Size.中:
                        return prop.GBombGear3Size2;
                    case Porp_Size.大:
                        return prop.GBombGear3Size3;
                    default:
                        break;
                }
                break;
        }
        return null;
    }
    /// <summary>
    /// 返回方向
    /// </summary>
    /// <returns></returns>
    public bool GetDir(UnityEngine.Vector3 vector3) {

        if (vector3.x>0)
        {
            return true;
        }
        return false;
    }



    public float GetShackLevel(string key, int Gear, Porp_Size porp_Size) {

        PropData prop = _eeDataManager.Get<PropData>(key);
        switch (Gear)
        {

            case 1:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.ShackLevel11;

                    case Porp_Size.中:
                        return prop.ShackLevel12;
                    case Porp_Size.大:
                        return prop.ShackLevel13;
                    default:
                        break;
                }

                break;
            case 2:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.ShackLevel21;
                    case Porp_Size.中:
                        return prop.ShackLevel22;
                    case Porp_Size.大:
                        return prop.ShackLevel23;
                    default:
                        break;
                }

                break;
            case 3:

                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.ShackLevel31;
                    case Porp_Size.中:
                        return prop.ShackLevel32;
                    case Porp_Size.大:
                        return prop.ShackLevel33;
                    default:
                        break;
                }
                break;
        }
        return 1;

    }

    public float GetShackTime(string key, int Gear, Porp_Size porp_Size)
    {

        PropData prop = _eeDataManager.Get<PropData>(key);
        switch (Gear)
        {

            case 1:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.ShackTime11;

                    case Porp_Size.中:
                        return prop.ShackTime12;
                    case Porp_Size.大:
                        return prop.ShackTime13;
                    default:
                        break;
                }

                break;
            case 2:
                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.ShackTime21;
                    case Porp_Size.中:
                        return prop.ShackTime22;
                    case Porp_Size.大:
                        return prop.ShackTime23;
                    default:
                        break;
                }

                break;
            case 3:

                switch (porp_Size)
                {
                    case Porp_Size.小:
                        return prop.ShackTime31;
                    case Porp_Size.中:
                        return prop.ShackTime32;
                    case Porp_Size.大:
                        return prop.ShackTime33;
                    default:
                        break;
                }
                break;
        }
        return 1;

    }

}
