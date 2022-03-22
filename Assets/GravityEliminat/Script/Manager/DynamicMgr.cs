using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;
public class DynamicMgr : Singlton<DynamicMgr>
{
    float effectTime=1;
    public void FlyEffect(Vector3 orgin, Transform taget, string effectName, Action callBack = null)
    {
        effectTime = 0.5F;
        Transform effect = Pool.Instance.Spawn(Pool.Effect_PoolName, effectName);
        effect.position = orgin;
        float midpointX = 0;
        float midpointY = 0;

        if (Math.Abs(taget.position.x - orgin.x) >= Math.Abs(taget.position.y - orgin.y))
        {
            midpointX = (taget.position.x + orgin.x) / 2;

        }
        else
        {

            midpointX = orgin.x - Mathf.Abs(orgin.x - taget.position.x) / 2;

        }
        if (taget.position.y >= orgin.y)
        {
            midpointY = taget.position.y + Mathf.Abs(taget.position.y - orgin.y) / 2;
        }
        else
        {
            midpointY = orgin.y - Mathf.Abs(orgin.y - taget.position.y) / 2;
        }
        effect.SetParent(taget);
        Debug.Log("起点: " + orgin + "中点:" + new Vector3(midpointX, midpointY, 0) + "终点：" + taget);
        Vector3[] vectors = new Vector3[3] { orgin, new Vector3(midpointX, midpointY, 0), Vector3.zero };
        effect.transform.DOLocalPath(vectors, effectTime, PathType.CatmullRom).SetDelay(1).OnComplete(() =>
        {
            Pool.Instance.Despawn(Pool.Effect_PoolName, effect);
            //if (callBack != null)
            //{
            callBack();
            //}
        });
    }


    /// <summary>
    /// 球曲线飞目标动画
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="orgin"></param>
    /// <param name="taget"></param>
    /// <param name="callBack"></param>
    public void  WordPosFlyUI(string typeName,Vector3 orgin, Vector2 taget , Action callBack = null) {
        orgin = WorldToUGUI(orgin);
        taget = uiWorldToUGUI(taget);
        Transform t = Pool.Instance.Spawn(Pool.PoolName_UI, Pool.UI_Ball);
        

        t.transform.SetParent(UIManager.Instance.GetBase<GamePanel>().transform);
        t.GetComponent<Image>().sprite = Resources.Load<Sprite>("BallSprite/" + typeName);
        t.localScale = Vector3.one*0.8F;
    
        //UIManager.Instance.
        //Debug.Log("eee"+orgin);
        t.localPosition = orgin;
        Vector2 point = (taget - new Vector2 (orgin.x,orgin.y))/4;
        int i = 1;
        if (orgin.x >0)
        {
            i = 1;
        }
        else { i =-1; }
        Vector2 vector= PointRotate(point, point,i*135);
        if (vector.y < orgin.y)
        {
            vector.y = orgin.y + point.y;
        }
        Vector3[] vectors = new Vector3[3] { orgin, new Vector3(vector.x, vector.y, 0), taget };

        //foreach (var item in vectors)
        //{
        //Debug.Log("路线" + vector2 + "  "+ vectors[1]+"   "+ vectors[2]);
        //}
        //t.DOScale(Vector3.one * 1.2F, 0.2F).SetEase(Ease.K).OnComplete(() => {

        //    t.transform.DOScale(Vector3.one * 1.5F, 0.2F).SetEase(Ease.InOutBack).OnComplete(() => {
        //        t.transform.DOLocalPath(vectors, effectTime, PathType.CatmullRom).OnUpdate(() => {
        //        }).OnComplete(() =>
        //        {
        //            if (callBack != null)
        //            {
        //                callBack();
        //            }
        //            t.localScale = Vector3.one;
        //            Pool.Instance.Despawn(Pool.PoolName_UI, t);

        //        });
        //    });


        //});
        //t.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 45), 0.2f, RotateMode.FastBeyond360).SetLoops(1);
        float time = 0;
        t.transform.DOScale(Vector3.one * 1.5F, 0.2F).SetEase(Ease.InOutBack).OnComplete(() => {
            t.transform.DOLocalPath(vectors, effectTime+0.2f, PathType.CatmullRom).SetDelay(0).OnUpdate(()=> {
            }).OnComplete(() =>
            {
                if (callBack != null)
                {
                    callBack();
                }
                Pool.Instance.Despawn(Pool.PoolName_UI, t);
            });
            t.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 360), effectTime + 0.1F, RotateMode.FastBeyond360);
        });

        t.transform.DOScale(Vector3.one*0.8F, effectTime).SetDelay(0.2f).OnComplete(()=> {
            t.transform.DOScale(Vector3.one * 1.4f, 0.2F).OnComplete(() => {
                t.transform.DOScale(Vector3.one, 0.2F);
            });
        });
        

    }

    public void WordPosFlyUIEffect(string typeName, string effectName,Vector3 orgin, Vector2 taget,float size=1 ,Action callBack = null)
    {
        //Vector2 vector2 = Vector3.zero;
        orgin = WorldToUGUI(orgin);
        taget = uiWorldToUGUI(taget);
        Transform t = Pool.Instance.Spawn(Pool.Effect_PoolName, effectName);
        t.transform.SetParent(UIRoot.Instance.transform);

        t.localScale = Vector3.one* size;

        //UIManager.Instance.
        //Debug.Log("eee"+orgin);
        t.localPosition = orgin;
        Vector2 point = (taget - new Vector2(orgin.x, orgin.y)) / 4;
        int i = 1;
        if (orgin.x > 0)
        {
            i = 1;
        }
        else { i = -1; }
        Vector2 vector = PointRotate(point, point, i * 135);
        if (vector.y < orgin.y)
        {
            vector.y = orgin.y + point.y;
        }
        Vector3[] vectors = new Vector3[3] { orgin, new Vector3(vector.x, vector.y, 0), taget };

        //foreach (var item in vectors)
        //{
        //Debug.Log("路线" + vector2 + "  "+ vectors[1]+"   "+ vectors[2]);
        //}
        //t.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360).SetLoops(1);
        //t.transform.DOScale(Vector3.one * 1.5F, 0.2F).SetEase(Ease.InOutBack).OnComplete(() => {
            t.transform.DOLocalPath(vectors, effectTime, PathType.CatmullRom).OnUpdate(() => {
            }).OnComplete(() =>
            {
                if (callBack != null)
                {
                    callBack();
                }
                t.localScale = Vector3.one;
                Pool.Instance.Despawn(Pool.Effect_PoolName, t);

            });
        //});
        //t.transform.DOScale(Vector3.one * 0.5F, effectTime).SetDelay(0.2F).OnComplete(() => {
        //    t.localScale = Vector3.one;


        //});
        //t.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360).SetLoops(1);





    }

    public Vector2 WorldToUGUI(Vector3 pos)
    {
        //if (Camera.main != null && UIManager.Instance .uiCamera == null)
        //    UIManager.Instance.uiCamera = ;
        Vector2 uisize = UIManager.Instance.uiCamera.transform.parent.GetComponent<RectTransform>().sizeDelta;//得到画布的尺寸
        Vector2 screenpos = Camera.main.WorldToScreenPoint(pos);//将世界坐标转换为屏幕坐标
        Vector2 screenpos2;
        screenpos2.x = screenpos.x - (Screen.width / 2);//转换为以屏幕中心为原点的屏幕坐标
        screenpos2.y = screenpos.y - (Screen.height / 2);
        Vector2 uipos;
        uipos.x = (screenpos2.x / Screen.width) * uisize.x;
        uipos.y = (screenpos2.y / Screen.height) * uisize.y;//
        return uipos;
    }


    public Vector2 uiWorldToUGUI(Vector3 pos)
    {
        //if (Camera.main != null && UIManager.Instance .uiCamera == null)
        //    UIManager.Instance.uiCamera = ;
        Vector2 uisize = UIManager.Instance.uiCamera.transform.parent.GetComponent<RectTransform>().sizeDelta;//得到画布的尺寸
        Vector2 screenpos = UIManager.Instance.uiCamera.WorldToScreenPoint(pos);//将世界坐标转换为屏幕坐标
        Vector2 screenpos2;
        screenpos2.x = screenpos.x - (Screen.width / 2);//转换为以屏幕中心为原点的屏幕坐标
        screenpos2.y = screenpos.y - (Screen.height / 2);
        Vector2 uipos;
        uipos.x = (screenpos2.x / Screen.width) * uisize.x;
        uipos.y = (screenpos2.y / Screen.height) * uisize.y;//
        return uipos;
    }

    public Vector2 UIToWord(Vector3 pos) {

        //Vector2 uisize = UIManager.Instance.uiCamera.transform.parent.GetComponent<RectTransform>().sizeDelta;//得到画布的尺寸
        Vector2 uipos;
        //uipos.x = (Screen.width - uisize.x) / uisize.x;
        //uipos.y=(Screen.height - uisize.y) / uisize.y;

        uipos = Camera.main.ScreenToWorldPoint(pos);
        return uipos;
    }

    public Vector2 PointRotate(Vector2 center, Vector2 p1, double angle)
    {
        //Debug.Log("P1:"+p1);
        Vector2 tmp = new Vector2();
        double angleHude = angle * Math.PI / 180;/*角度变成弧度*/
        double x1 = (p1.x - center.x) * Math.Cos(angleHude) + (p1.y - center.y) * Math.Sin(angleHude) + center.x;
        double y1 = -(p1.x - center.x) * Math.Sin(angleHude) + (p1.y - center.y) * Math.Cos(angleHude) + center.y;

        tmp.x = Convert.ToInt32(x1);
        tmp.y = Convert.ToInt32(y1);

        return tmp;
    }


    /// <summary>
    /// 飞分数Text
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="soreBase"></param>
    public void FlyText(Vector3 pos,int soreBase) {

        Transform soreText = Pool.Instance.Spawn(Pool.PoolName_UI, Pool.UI_ScoreText);
        soreText.SetParent(UIManager.Instance.GetBase<GamePanel>().transform);
        soreText.localScale = Vector3.one;
        soreText.transform.position = pos;
        soreText.GetComponent<Text>().text = string.Format("+{0}", soreBase);
        Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ => {

            Pool.Instance.Despawn(Pool.PoolName_UI, soreText);
        });

    }


    /// <summary>
    /// 星星旋转动画
    /// </summary>
    /// <param name="transform"></param>
    public void StarAnim(Transform transform) {

     
        //Debug.Log("EEE"+GameManager.Instance.StarShineStarSub.Value);
        transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.InOutBack).OnComplete(()=> {

            transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 360), 1,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(0).OnComplete(() => {

                transform.DOScale(Vector3.one, 0.5F).SetEase(Ease.InOutBack).OnComplete(()=> {

                    Pool.Instance.SpawnEffectByParent(Pool.Effect_PoolName,Pool.StarDown,transform,Vector3.zero,80);
                });
            });
        });
    }


    /// <summary>
    /// 生成特效直线飞行
    /// </summary>
    /// <param name="orgin"></param>
    /// <param name="target"></param>
    /// <param name="effectName"></param>
    /// <param name="action"></param>
    public void FlyEffectLine(Vector3 orgin,Vector3 target,string effectName,float times=1 ,Action action=null,float size=1) {


        Transform transform = Pool.Instance.Spawn(Pool.Effect_PoolName, effectName);
        orgin = WorldToUGUI(orgin);
        transform.SetParent(UIManager.Instance.GetBase<GamePanel>().transform);
        transform.localScale = Vector3.one* size;
        transform.localPosition = orgin;
        transform.DOMove(target, times).SetEase(Ease.Linear).OnComplete(()=> {

            if (action!=null)
            {
                action();
            }
            Pool.Instance.Despawn(Pool.Effect_PoolName, transform);
        });
    }

    public void FlyEffectLineUI(Vector3 orgin, Vector3 target, string effectName, float times = 1, Action action = null, float size = 1)
    {


        Transform transform = Pool.Instance.Spawn(Pool.PoolName_UI, effectName);
        orgin = WorldToUGUI(orgin);
        //target = uiWorldToUGUI(target);
        transform.SetParent(UIManager.Instance.GetBase<GamePanel>().transform);
        transform.localScale = Vector3.one * size;
        transform.localPosition = orgin;
        transform.DOMove(target, times).SetEase(Ease.Linear).OnComplete(() => {

            if (action != null)
            {
                action();
            }
            Pool.Instance.Despawn(Pool.PoolName_UI, transform);
        });
    }


    private Vector3 GetBetweenPoint(Vector3 start, Vector3 end, float percent = 0.5f)
    {
        Vector3 normal = (end - start).normalized;
        
        float distance = Vector3.Distance(start, end);
        //XDebug.Log("起点：" + start + "  终点：" + end + "  中间点：" + (normal * (distance * percent) + start) + "  两点距离:" + distance+"   方向："+ normal);
        return normal * (distance * percent) + start;
    }
    private Vector2 GetBetweenPoint2D(Vector2 start, Vector2 end, float percent = 0.5f)
    {
        Vector2 normal = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        return normal * (distance * percent) + start;
    }


    /// <summary>
    /// 步数流星飞
    /// </summary>
    /// <param name="orgin"></param>
    /// <param name="target"></param>
    /// <param name="effectName"></param>
    /// <param name="times"></param>
    /// <param name="percent"></param>
    /// <param name="ager"></param>
    /// <param name="action"></param>
    public void FlyEffectCurve(Vector3 orgin, Vector3 target, string effectName, float times = 1,float percent = 0.5f, float ager =90, Action action = null)
    {

        orgin = uiWorldToUGUI(orgin);
        target = WorldToUGUI(target);
        //orgin =orgin;
        //target = WorldToUGUI(target);
        Vector3 vector = GetBetweenPoint(orgin, target, percent);
        vector = PointRotate(orgin, vector, ager);
        Transform transform = Pool.Instance.Spawn(Pool.Effect_PoolName, effectName);
        transform.SetParent(UIManager.Instance.GetBase<GamePanel>().transform);
        transform.localPosition = orgin;
        transform.localScale = Vector3.one;
        //transform.SetParent(UIManager.Instance.GetBase<GamePanel>().transform);
        //Debug.Log("步数坐标" + orgin + "  " + vector + "   "+ target);
        transform.DOLocalPath(new Vector3[3] { orgin, vector, target }, times,PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => {
            if (action != null)
            {
                action();
            }
           //transform.position = orgin;

            //transform.localScale = Vector3.zero;
            Pool.Instance.Despawn(Pool.Effect_PoolName, transform);
        });
    }

    /// <summary>
    /// 飞预设体曲线飞行
    /// </summary>
    /// <param name="orgin">起点</param>
    /// <param name="target">终点</param>
    /// <param name="transform">预设</param>
    /// <param name="roat">是否要要在飞行时旋转</param>
    /// <param name="times">时间</param>
    /// <param name="percent">百分比</param>
    /// <param name="ager">旋转角度</param>
    /// <param name="action"></param>
     public void FlyEffectCurveByGG(Vector3 orgin, Vector3 target, Transform transform,int roat ,float times = 1,float percent = 0.5f, float ager =90, Action action = null)
    {
        float a = 0;
        Vector3 vector = GetBetweenPoint(orgin, target, percent);   
        vector = PointRotate(orgin, vector, ager);
        //Transform transform = Pool.Instance.Spawn(Pool.Effect_PoolName, effectName);
        //transform.localScale = Vector3.one*0.7F;
        transform.position = orgin;
       
        transform.DOPath (new Vector3[3] { orgin, vector, target }, times,PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => {
            if (action != null)
            {
                action();
            }
        }).OnUpdate(()=> {
            transform.eulerAngles = new Vector3(0, 0, a+= roat);
        });
    }

    public void FlyEffectCurveSetParent(Vector3 orgin, Vector3 target, Transform transform, int roat, float times = 1, float percent = 0.5f, float ager = 90, Action action = null)
    {
        float a = 0;

        //orgin = uiWorldToUGUI(orgin);
        target = WorldToUGUI(target);
        transform.position = orgin;
        transform.SetParent(UIManager.Instance.GetBase<GamePanel>().transform);
        transform.localScale = Vector3.one;
        Vector3 vector = GetBetweenPoint(orgin, target, percent);
        vector = PointRotate(orgin, vector, ager);
        //Transform transform = Pool.Instance.Spawn(Pool.Effect_PoolName, effectName);
        //transform.localScale = Vector3.one*0.7F;
        transform.position = orgin;

        transform.DOLocalPath(new Vector3[2] { orgin,/* vector,*/ target }, times, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => {
            if (action != null)
            {
                action();
            }

        }).OnUpdate(() => {
            transform.eulerAngles = new Vector3(0, 0,  a+= roat);
        });
    }



    public float StarFlyBoom(Vector3 orgin, Transform target, string effectName, Action action = null)
    {
        float TIME = 0.8F; 
        Transform effect = Pool.Instance.Spawn(Pool.Effect_PoolName, effectName);
        effect.localScale = Vector3.one;
        effect.position = orgin;
        //effect.SetParent(target);
        Vector3 vector = GetBetweenPoint(orgin, target.transform.position, 0.5f);
        vector = PointRotate(orgin, vector, 90);
        //Debug.Log("旋转角度"+ vector);
        effect.DOPath(new Vector3[3] { orgin, vector, target.transform.position }, TIME, PathType.CatmullRom)/*.SetEase(Ease.InOutFlash)*/.OnComplete(() =>
        {
            if (action != null)
            {
                action();
            }
            Pool.Instance.Despawn(Pool.Effect_PoolName, effect);
            Pool.Instance.SpawnMarkEffect(Pool.Effect_PoolName, "effect_yaoshuibaozha02", target, 1, 0.5f);
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.5F)).Subscribe(_ => {
                Pool.Instance.SpawnEffect(Pool.Effect_PoolName, "effect_yaoshuibaozha02", target.transform.position, 1, 0.5f);
            });
            Observable.TimeInterval(System.TimeSpan.FromSeconds(0.2f)).Subscribe(_ =>
            {
                Pool.Instance.SpawnMarkEffect(Pool.Effect_PoolName, Pool.StarMark, target, 1.2f, 0.5f);

            });
            /*  Transform star *//*= */


        });
        return TIME + 0.5F;
    }


    public void FlyImage(Sprite sprite,Vector3 orgin,Vector3 target,Transform parent,Action action=null) {


        Transform img = Pool.Instance.Spawn(Pool.PoolName_UI, Pool.FlyImg);
        img.position = orgin;
        img.SetParent(parent);

        img.transform.GetComponent<Image>().sprite = sprite;
        img.transform.GetComponent<Image>().SetNativeSize();
        img.localScale = Vector3.one;
        img.DOMove(target,1).SetEase(Ease.Linear).OnComplete(()=> {

            if (action!=null)
            {
                action();
            }
            Pool.Instance.Despawn(Pool.PoolName_UI,img);

        });

    }

    public void FlyUIPrefab(string prefabName, Vector3 orgin, Vector3 target, float size=1,Action action = null)
    {
        Transform img = Pool.Instance.Spawn(Pool.PoolName_UI, prefabName);
        img.position = orgin;
        img.SetParent(UIRoot.Instance.transform);
        img.localScale = Vector3.one* size;
        img.DOMove(target, 0.5f).SetEase(Ease.Linear).OnComplete(() => {

            if (action != null)
            {
                action();
            }
            Pool.Instance.Despawn(Pool.PoolName_UI, img);

        });

    }

    public void LineFly(string PoolName,string pafabSting,Vector3 orgin,Vector3 target,Action action=null) { 
        Transform parfab = Pool.Instance.Spawn(PoolName, pafabSting);
        parfab.position = orgin; parfab.localScale = Vector3.one;
        parfab.DOMove(target,0.5f).SetEase(Ease.InOutBack);
    }
}
