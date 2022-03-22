using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class UIExtension
{
    public static void SetDefalut(this Transform mtrnas)
    {
        mtrnas.localPosition = Vector3.zero;
        mtrnas.localRotation = Quaternion.identity;
        mtrnas.localScale = Vector3.one;
    }


    /// <summary>
    /// 隐藏
    /// </summary>
    /// <param name="cg"></param>
    public static void HideCanvasGroup(this CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public static void HideCanvasGroup(this GameObject obj)
    {
        if (obj == null) return;
        var cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = obj.AddComponent<CanvasGroup>();
        }
        cg.HideCanvasGroup();
    }

    public static void HideCanvasGroup(this Transform tra)
    {
        if (tra == null) return;
        var cg = tra.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = tra.gameObject.AddComponent<CanvasGroup>();
        }
        cg.HideCanvasGroup();
    }

    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="cg"></param>
    public static void ShowCanvasGroup(this CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public static void ShowCanvasGroup(this GameObject obj)
    {
        if (obj == null) return;
        var cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = obj.AddComponent<CanvasGroup>();
        }
        cg.ShowCanvasGroup();
    }


    public static CanvasGroup GetCanvasGroup(this Transform mTrans)
    {
        CanvasGroup mcg = mTrans.GetComponent<CanvasGroup>();
        if (mcg == null)
        {
            mcg = mTrans.gameObject.AddComponent<CanvasGroup>();
        }
        return mcg;
    }

    public static void ShowCanvasGroup(this Transform tra)
    {
        if (tra == null) return;
        var cg = tra.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = tra.gameObject.AddComponent<CanvasGroup>();
        }
        cg.ShowCanvasGroup();
    }

    public static void ShowCanvasGroup(this Transform mtrans, bool blocksRaycasts)
    {
        if (mtrans == null) return;
        var cg = mtrans.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = mtrans.gameObject.AddComponent<CanvasGroup>();
        }
        if (blocksRaycasts)
        {
            cg.ShowCanvasGroup();
        }
        else
        {
            cg.alpha = 1;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }

    public static bool IsShowCanvasGroup(this CanvasGroup cg)
    {
        return cg.alpha > 0 ? true : false;
    }

    public static bool IsShowCanvasGroup(this Transform tra)
    {
        bool isShow = false;
        var cg = tra.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            XDebug.LogError("该对象没有该物体");
        }
        else
        {
            isShow = IsShowCanvasGroup(cg);
        }
        return isShow;
    }

    public static bool IsShowCanvasGroup(this GameObject obj)
    {
        return IsShowCanvasGroup(obj.transform);
    }



    /// <summary>
    /// 获取按钮
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Button GetButton(this GameObject obj)
    {
        var mBtn = obj.GetComponent<Button>();
        if (mBtn == null)
        {
            mBtn = obj.AddComponent<Button>();
        }
        return mBtn;
    }

    /// <summary>
    /// 获取按钮
    /// </summary>
    /// <param name="tra"></param>
    /// <returns></returns>
    public static Button GetButton(this Transform tra)
    {
        var btn = tra.GetComponent<Button>();
        if (btn == null)
        {
            btn = tra.gameObject.AddComponent<Button>();
        }
        //AddListen(btn);
        return btn;
    }

    private static List<Button> mList = new List<Button>(20);

    private static void AddListen(Button btn)
    {
        if (mList.Contains(btn)) return;
        mList.Add(btn);
        btn.onClick.AddListener(() =>
        {

        });
    }

    /// <summary>
    /// 改变Button Instantiate
    /// </summary>
    /// <param name="mBtn"></param>
    public static void InsButton(this Button mBtn, bool b)
    {
        mBtn.interactable = b;
    }
    /// <summary>
    /// 检查按钮是否正常
    /// </summary>
    /// 待定，看后期是否需要添加上去
    private static void CheckButton(this Button mBtn)
    {
        var mImag = mBtn.transform.GetComponent<Image>();
        if (mImag != null)
        {
            mImag.raycastTarget = true;
        }

        var mText = mBtn.transform.GetComponent<Text>();
        if (mText != null)
        {
            mText.raycastTarget = true;
        }
    }

    /// <summary>
    /// 禁止Button按钮
    /// </summary>
    /// <param name="trans"></param>
    public static void BanButton(this Transform trans)
    {
        //Button.Instantiate
        var btn = trans.GetComponent<Button>();
        if (btn == null) return;
        btn.enabled = false;
    }
    /// <summary>
    /// 激活Button按钮
    /// </summary>
    /// <param name="trans"></param>
    public static void ActiveButton(this Transform trans)
    {
        var btn = trans.GetComponent<Button>();
        if (btn == null) return;
        btn.enabled = true;
    }


    public static void ActiveAllClick(this Transform trans)
    {
        var cg = trans.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = trans.gameObject.AddComponent<CanvasGroup>();
        }
        cg.interactable = true;
    }

    public static void BanAllClick(this Transform trans)
    {
        var cg = trans.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = trans.gameObject.AddComponent<CanvasGroup>();
        }
        cg.interactable = false;
    }



    /// <summary>
    /// 将自身设置为最底部
    /// </summary>
    /// <param name="tra"></param>
    public static void SettingLast(this Transform tra)
    {
        tra.SetAsLastSibling();
    }

    /// <summary>
    /// 获取文本
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static Text GetText(this Transform trans)
    {
        return trans.GetComponent<Text>();
    }

    public static Text GetChiText(this Transform trans)
    {
        return trans.GetComponentInChildren<Text>();
    }

    public static Text[] GetChisText(this Transform trans)
    {
        return trans.GetComponentsInChildren<Text>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <param name="var"></param>
    public static void SetText<T>(this Text text, T var, string s = null)
    {
        text.text = s + (var.ToString().Numdispose());
    }

    static string[] symbol = { "K", "M", "G", "T", "P", "E", "Z", "Y", "B", "N", "D", "KK", "KM", "KG", "KT", "KP", "KE", "KZ", "KY", "KB", "KN", "KD" };
    static char char_0 = '0';
    static string endStr = ".00";

    static string StrType = "0.00";
    public static string Numdispose(this string num)
    {
        num = System.Decimal.Parse(num, System.Globalization.NumberStyles.Float).ToString();
        if (num.Length > 3)
        {
            int a = (num.Length - 4) / 3;
            string str1 = num.Substring(0, num.Length - 3 * (a + 1));
            string str2 = num.Substring(num.Length - 3 * (a + 1), 2);
            int zeroNum = 0;
            foreach (char item in str2)
            {
                if (item == char_0)
                {
                    zeroNum++;
                }
            }
            if (zeroNum >= 2)
            {
                return str1 + endStr + symbol[a];
            }
            return str1 + "." + str2 + symbol[a];
        }
        return num;
    }

    /// <summary>
    /// 获取Image
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static Image GetImage(this Transform trans)
    {
        var mImage = trans.GetComponent<Image>();
        if (mImage == null)
        {
            mImage = trans.gameObject.AddComponent<Image>();
        }
        return mImage;
    }


    /// <summary>
    /// 刷新HorizontalLayoutGroup
    /// </summary>
    /// <param name="mtrans"></param>
    /// <returns></returns>
    /// 有时该组件不能自适配
    public static void Refresh_HorizontalLayoutGroup(this Transform mtrans)
    {
        var mH = mtrans.GetComponent<HorizontalLayoutGroup>();
        mH.enabled = false;
        mH.enabled = true;
    }




    #region Color   
    public static void SettingColor(this Transform trans, Color color)
    {
        if (trans.GetText() != null)
        {
            trans.GetText().color = color;
        }
        else if (trans.GetImage() != null)
        {
            trans.GetImage().color = color;
        }
    }

    /// <summary>
    /// 单独更改透明度
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="aValue"></param>
    public static void SettingColor_A(this Transform trans, float aValue)
    {
        var mText = trans.GetText();
        if (mText != null)
        {
            var mColor = mText.color;
            mText.color = new Color(mColor.r, mColor.g, mColor.b, aValue);
        }
        else
        {
            var mImage = trans.GetImage();
            if (mImage != null)
            {
                var mColor = mImage.color;
                mImage.color = new Color(mColor.r, mColor.g, mColor.b, aValue);
            }
        }
    }
    #endregion

    #region Image

    public static void SetImage(this Image mImage, Sprite sprite)
    {
        mImage.sprite = sprite;
        mImage.SetNativeSize();
    }

    public static void SetTransfromImage(this Transform trans, Sprite sprite)
    {
        SetImage(trans.GetImage(), sprite);
    }
    #endregion


#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/unity拓展/UI/AddAutoFitAndLayout_V", false, 9)]
    private static void Menu()
    {
        var changeObj = UnityEditor.Selection.objects[0] as GameObject;
        var autoFit = changeObj.GetComponent<ContentSizeFitter>();
        if (autoFit == null)
        {
            autoFit = changeObj.AddComponent<ContentSizeFitter>();
        }
        autoFit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        autoFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var mV = changeObj.GetComponent<VerticalLayoutGroup>();
        if (mV == null)
        {
            mV = changeObj.AddComponent<VerticalLayoutGroup>();
        }
        mV.childAlignment = TextAnchor.UpperCenter;
    }


    [UnityEditor.MenuItem("GameObject/unity拓展/UI/AddAutoFitAndLayout_H", false, 9)]
    private static void Menu1()
    {
        var changeObj = UnityEditor.Selection.objects[0] as GameObject;
        var autoFit = changeObj.GetComponent<ContentSizeFitter>();
        if (autoFit == null)
        {
            autoFit = changeObj.AddComponent<ContentSizeFitter>();
        }
        autoFit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        autoFit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var mV = changeObj.GetComponent<HorizontalLayoutGroup>();
        if (mV == null)
        {
            mV = changeObj.AddComponent<HorizontalLayoutGroup>();
        }
        mV.childAlignment = TextAnchor.UpperCenter;
    }

    [UnityEditor.MenuItem("GameObject/unity拓展/UI/RemoveAutoFitAndLayout", false, 9)]
    private static void Menu1_1()
    {
        var changeObj = UnityEditor.Selection.objects[0] as GameObject;
        var autoFit = changeObj.GetComponent<ContentSizeFitter>();
        if (autoFit != null)
        {
            UnityEngine.Object.Destroy(autoFit);
        }
        var mV = changeObj.GetComponent<VerticalLayoutGroup>();
        if (mV != null)
        {
            UnityEngine.Object.Destroy(mV);
        }

        var mh = changeObj.GetComponent<HorizontalLayoutGroup>();
        if (mh != null)
        {
            UnityEngine.Object.Destroy(mh);
        }
    }

    [UnityEditor.MenuItem("GameObject/unity拓展/UI/Text_AutoFit", false, 9)]
    public static void Menu_2_1()
    {
        var chageobj = UnityEditor.Selection.objects[0] as GameObject;
        if (chageobj.GetComponent<Text>() == null)
        {
            return;
        }

        var auto = chageobj.GetComponent<ContentSizeFitter>();
        if (auto == null)
        {
            auto = chageobj.AddComponent<ContentSizeFitter>();
        }
        auto.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        auto.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    }


    [UnityEditor.MenuItem("GameObject/unity拓展/UI/Text/Creat", false, 9)]
    public static void Menu3_1()
    {
        GameObject obj = new GameObject("Text");
        UnityEditor.Undo.RegisterCreatedObjectUndo(obj, "");
        var mcanvas = GameObject.FindObjectOfType<Canvas>();
        if (mcanvas != null)
        {
            obj.transform.SetParent(mcanvas.transform);
        }
        var mfit = obj.AddComponent<ContentSizeFitter>(); ;
        mfit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        mfit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        obj.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        var mText = obj.AddComponent<Text>();
        mText.text = "请输入";
    }

    private static RectTransform mRect;
    private static Text mText;
    [UnityEditor.MenuItem("GameObject/unity拓展/UI/Text/CopyAll", false, 9)]
    public static void Menu3_2()
    {
        var mchange = UnityEditor.Selection.objects[0] as GameObject;
        mRect = mchange.GetComponent<RectTransform>();
        mText = mchange.GetComponent<Text>();
    }

    [UnityEditor.MenuItem("GameObject/unity拓展/UI/Text/PasteCopyll", false, 9)]
    public static void Menu3_3()
    {
        var mchange = UnityEditor.Selection.objects[0] as GameObject;
        var rect = mchange.GetComponent<RectTransform>();
        if (mRect != null && rect != null)
        {
            rect.anchoredPosition = mRect.anchoredPosition;
            rect.sizeDelta = mRect.sizeDelta;
            rect.pivot = mRect.pivot;
            rect.rotation = mRect.rotation;
            rect.localScale = mRect.localScale;
        }

        var text = mchange.GetComponent<Text>();
        if (mText != null && text != null)
        {
            text.font = mText.font;
            text.fontSize = mText.fontSize;
            text.lineSpacing = mText.lineSpacing;
            text.supportRichText = mText.supportRichText;
            text.alignment = mText.alignment;
            text.alignByGeometry = mText.alignByGeometry;
            text.horizontalOverflow = mText.horizontalOverflow;
            text.verticalOverflow = mText.verticalOverflow;
            text.resizeTextForBestFit = mText.resizeTextForBestFit;
            text.color = mText.color;
            text.material = mText.material;
            text.raycastTarget = mText.raycastTarget;
            text.maskable = mText.maskable;
        }
    }


#endif



}
