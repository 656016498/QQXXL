using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;



public class UIToolEditor_Attribute
{
    public string Name;
    public bool CreatText;
    public bool CreatImage;
    public bool CreatButton;
}
public class UIToolEditor_Windows : ScriptableWizard
{
    [MenuItem("GameFrame/UIFrame/AutoCreatScriptsWindwos", true)]
    private static bool Menu0()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("GameFrame/UIFrame/AutoCreatScriptsWindwos", false, 1100)]
    private static void Menu()
    {
        if (Selection.objects.Length==1)
        {
            ShowThis();
        }
        else
        {
            Debug.LogWarning("请选中制定对象");
        }
    }



    private GameObject mSelectObj;
    private void Awake()
    {
        mSelectObj = Selection.objects[0] as GameObject;
    }

    public static void ShowThis()
    {
        ScriptableWizard.DisplayWizard<UIToolEditor_Windows>("代码生成器", "关闭", "生成");
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        //RefreshUI();
        FenleiUI();

        EditorGUILayout.Space(20);
        if (mSelectObj.GetComponent<UIBase>()!=null)
        {
            GUILayout.Button("已存在脚本:" + mSelectObj.GetComponent<UIBase>().name);
            GUILayout.Label("需要删除该脚本,才能重新生成");
        }
        else
        {
            var isclick = GUILayout.Button("生成基类脚本");
            if (isclick)
            {
                //生成脚本
                UIToolEditor.CreatScript(mDic);                
            }
            GUILayout.Label("- 会自动生成集成UIBase的类");
            GUILayout.Label("- 选中的对象会自动绑定到脚本上");
        }       
        EditorGUILayout.EndVertical();
    }

    public Dictionary<Transform, UIToolEditor_Attribute> mDic = new Dictionary<Transform, UIToolEditor_Attribute>();

    bool isFold_btn = true;
    bool isFold_Text = true;
    bool isFold_Image = true;
    List<Transform> mList_btn = new List<Transform>(100);
    List<Transform> mList_Text = new List<Transform>(100);
    List<Transform> mlist_Image = new List<Transform>(100);
   
    public void FenleiUI()
    {
        mList_btn.Clear();
        mList_Text.Clear();
        mlist_Image.Clear();

        var mSelectObj = Selection.objects[0] as GameObject;
        var mAllTrnasFroms = mSelectObj.GetComponentsInChildren<Transform>();
          
        foreach (var item in mAllTrnasFroms)
        {
            if (item.GetComponent<Button>() != null)
            {
                mList_btn.Add(item);
            }
            if (item.GetComponent<Text>() != null)
            {
                mList_Text.Add(item);
            }
            if (item.GetComponent<Image>() != null)
            {
                mlist_Image.Add(item);
            }
        }

        EditorGUILayout.BeginVertical();

        isFold_btn = EditorGUILayout.Foldout(isFold_btn,"按钮");                
        if (isFold_btn)
        {           
            foreach (var item in mList_btn)
            {
                EditorGUILayout.BeginHorizontal();
                mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                if (info==null)
                {
                    info = new UIToolEditor_Attribute();
                    mDic.Add(item, info);
                }                
                EditorGUILayout.ObjectField(item, typeof(Object), true,GUILayout.Width(200));
                EditorGUILayout.Space(10,false);
                info.CreatButton = EditorGUILayout.Toggle("", info.CreatButton);               
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            var isClick1 = GUILayout.Button("全选", GUILayout.Width(100));
            if (isClick1)
            {
                foreach (var item in mList_btn)
                {
                    mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                    if (info == null)
                    {
                        info = new UIToolEditor_Attribute();
                        mDic.Add(item, info);
                    }
                    info.CreatButton = true;
                }
            }
            var isClick2 = GUILayout.Button("全不选", GUILayout.Width(100));
            if (isClick2)
            {
                foreach (var item in mList_btn)
                {
                    mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                    if (info == null)
                    {
                        info = new UIToolEditor_Attribute();
                        mDic.Add(item, info);
                    }
                    info.CreatButton = false;
                }
            }
            EditorGUILayout.EndHorizontal();

        }

        //text
        isFold_Text = EditorGUILayout.Foldout(isFold_Text, "Text");
        if (isFold_Text)
        {
            foreach (var item in mList_Text)
            {
                EditorGUILayout.BeginHorizontal();
                mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                if (info == null)
                {
                    info = new UIToolEditor_Attribute();
                    mDic.Add(item, info);
                }
                EditorGUILayout.ObjectField(item, typeof(Object), true, GUILayout.Width(200));
                EditorGUILayout.Space(10, false);
                info.CreatText = EditorGUILayout.Toggle("", info.CreatText);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            var isClick1 = GUILayout.Button("全选", GUILayout.Width(100));
            if (isClick1)
            {
                foreach (var item in mList_Text)
                {
                    mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                    if (info == null)
                    {
                        info = new UIToolEditor_Attribute();
                        mDic.Add(item, info);
                    }
                    info.CreatText = true;
                }
            }
            var isClick2 = GUILayout.Button("全不选", GUILayout.Width(100));
            if (isClick2)
            {
                foreach (var item in mList_Text)
                {
                    mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                    if (info == null)
                    {
                        info = new UIToolEditor_Attribute();
                        mDic.Add(item, info);
                    }
                    info.CreatText = false;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        //image
        isFold_Image = EditorGUILayout.Foldout(isFold_Image, "Image");
        if (isFold_Image)
        {
            foreach (var item in mlist_Image)
            {
                EditorGUILayout.BeginHorizontal();
                mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                if (info == null)
                {
                    info = new UIToolEditor_Attribute();
                    mDic.Add(item, info);
                }
                EditorGUILayout.ObjectField(item, typeof(Object), true, GUILayout.Width(200));
                EditorGUILayout.Space(10, false);
                info.CreatImage = EditorGUILayout.Toggle("", info.CreatImage);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            var isClick1 = GUILayout.Button("全选", GUILayout.Width(100));
            if (isClick1)
            {
                foreach (var item in mlist_Image)
                {
                    mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                    if (info == null)
                    {
                        info = new UIToolEditor_Attribute();
                        mDic.Add(item, info);
                    }
                    info.CreatImage = true;
                }
            }
            var isClick2 = GUILayout.Button("全不选", GUILayout.Width(100));
            if (isClick2)
            {
                foreach (var item in mlist_Image)
                {
                    mDic.TryGetValue(item, out UIToolEditor_Attribute info);
                    if (info == null)
                    {
                        info = new UIToolEditor_Attribute();
                        mDic.Add(item, info);
                    }
                    info.CreatImage = false;
                }
            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndVertical();

        
    }
    
}
