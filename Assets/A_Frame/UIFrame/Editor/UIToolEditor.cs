using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

public class UIToolEditor
{
    //[MenuItem("Tools/UpdateUIScript", false, 1101)]
    public static void UpdateScript()
    {
        GameObject selectGo = Selection.activeGameObject;
        if (selectGo == null)
            return;
        Transform[] transforms = selectGo.GetComponentsInChildren<Transform>(true);
        GetOjbectBace getBace = selectGo.GetComponent<GetOjbectBace>();
        FieldInfo[] fieldInfos = getBace.GetType().GetFields();
        foreach (FieldInfo info in fieldInfos)
        {
            string[] names= info.Name.Split('_');
            string name = names[0];
            foreach (Transform uib in transforms)
            {
                string goName = uib.gameObject.name;
                goName = goName.Replace(" ", "");
                goName = goName.Replace("(", "");
                goName = goName.Replace(")", "");
                if (Equals(name, goName))
                {
                    if (!info.FieldType.IsArray)
                    {
                        UnityEngine.Object com = null;
                        if (info.FieldType == typeof(GameObject))
                        {
                            com = uib.gameObject;
                        }
                        else
                        {
                            com = uib.GetComponent(info.FieldType);
                        }
                        if (com != null)
                        {
                            info.SetValue(getBace, com);
                            break;
                        }
                    }
                    else
                    {
                        Array coms = null;
                        if (info.FieldType.GetElementType() == typeof(GameObject)|| info.FieldType.GetElementType() == typeof(Transform))
                        {
                            Transform[] ts = uib.GetComponentsInChildren<Transform>(true);
                            coms = Array.CreateInstance(info.FieldType.GetElementType(), ts.Length-1);
                            for (int i=1;i<ts.Length;i++)
                            {
                                if(info.FieldType.GetElementType() == typeof(GameObject))
                                {
                                    coms.SetValue(ts[i].gameObject, i-1);
                                }
                                else
                                {
                                    coms.SetValue(ts[i], i-1);
                                }
                            }
                        }
                        else
                        {
                            Component[] tempComs = uib.GetComponentsInChildren(info.FieldType.GetElementType(), true);
                            coms = Array.CreateInstance(info.FieldType.GetElementType(), tempComs.Length);
                            for (int i = 0; i < tempComs.Length; i++)
                            {
                                coms.SetValue(tempComs[i], i);
                            }
                        }
                        if(coms!=null)
                        {
                            info.SetValue(getBace,coms);
                            break;
                        }
                    }
                }
            }

        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(getBace);
        }
    }
    [MenuItem("GameFrame/UIFrame/CreateUIScript", false,1100)]
    public static void CreateScript()
    {
        GameObject selectGo = Selection.activeGameObject;
        UIBehaviour[] uIBehaviours = selectGo.GetComponentsInChildren<UIBehaviour>(true);
        string[] fieldNames = new string[uIBehaviours.Length];
        int[] instanceID = new int[uIBehaviours.Length];
        Type[] fieldType = new Type[uIBehaviours.Length];
        for (int i=0;i<uIBehaviours.Length;i++)//生成属性
        {
            string name = uIBehaviours[i].gameObject.name;
            name = name.Replace(" ", "");
            name = name.Replace("(", "");
            name = name.Replace(")", "");
            Debug.Log(name);
            fieldNames[i] = name +"_"+ uIBehaviours[i].GetType().Name;
            fieldType[i] = uIBehaviours[i].GetType();
            instanceID[i] = uIBehaviours[i].GetInstanceID();
            //Debug.Log(instanceID[i]);
            //Debug.Log(fieldNames[i]);
            //Debug.Log(fieldType[i]);
        }
        CreateClassFile(selectGo.name,typeof(UIBase), fieldNames, fieldType);

        EditorPrefs.SetBool("isCreateScript", true);
        EditorPrefs.SetString("className", selectGo.name);

        JArray fieldNamesJArray = new JArray(fieldNames);
        EditorPrefs.SetString("fieldNames", fieldNamesJArray.ToString());

        JArray instanceIDJArray = new JArray(instanceID);
        EditorPrefs.SetString("instanceID", instanceIDJArray.ToString());

        AssetDatabase.Refresh();
        
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="mDic"></param>
    /// <param name="isCreatBaseClass">是否生成基类</param>
    public static void CreatScript(Dictionary<Transform, UIToolEditor_Attribute> mDic)
    {       
        GameObject selectGo = Selection.activeGameObject;
        Debug.Log("CreatScript:"+selectGo);  
        UIBehaviour[] uIBehaviours = selectGo.GetComponentsInChildren<UIBehaviour>(true);
        string[] fieldNames = new string[uIBehaviours.Length];
        int[] instanceID = new int[uIBehaviours.Length];
        Type[] fieldType = new Type[uIBehaviours.Length];
        for (int i = 0; i < uIBehaviours.Length; i++)//生成属性
        {
            var mtran = uIBehaviours[i].gameObject.transform;
            mDic.TryGetValue(mtran, out UIToolEditor_Attribute mInfo);           
            string name = uIBehaviours[i].gameObject.name;
            name = name.Replace(" ", "");
            name = name.Replace("(", "");
            name = name.Replace(")", "");    
            
            bool isCreat = true;
            if (mInfo != null)
            {
                var attributeName = uIBehaviours[i].GetType().Name;
                switch (attributeName)
                {
                    case "Image":
                        if (!mInfo.CreatImage)
                        {
                            isCreat = false;
                        }
                        break;
                    case "Text":
                        if (!mInfo.CreatText)
                        {
                            isCreat = false;
                        }
                        break;
                    case "Button":
                        if (!mInfo.CreatButton)
                        {  
                            isCreat = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (isCreat)
            {
                fieldNames[i] = name + "_" + uIBehaviours[i].GetType().Name;
                fieldType[i] = uIBehaviours[i].GetType();
                instanceID[i] = uIBehaviours[i].GetInstanceID();
            }            
        }
        CreateClassFile(selectGo.name, typeof(UIBase), fieldNames, fieldType);

        EditorPrefs.SetBool("isCreateScript", true);
        EditorPrefs.SetString("className", selectGo.name);

        JArray fieldNamesJArray = new JArray(fieldNames);
        EditorPrefs.SetString("fieldNames", fieldNamesJArray.ToString());

        JArray instanceIDJArray = new JArray(instanceID);
        EditorPrefs.SetString("instanceID", instanceIDJArray.ToString());

        AssetDatabase.Refresh();
    }    


    [MenuItem("GameFrame/UIFrame/CreateUIScript",true)]
    public static bool CreateScript2()
    {
        return Selection.activeGameObject != null;
    }
    public static void AddScript()
    {
        string typeName = EditorPrefs.GetString("className");

        Assembly assembly = Assembly.Load("Assembly-CSharp");//找到对应的程序集
        Type type = assembly.GetType(typeName);

        var uiPanel = Selection.activeGameObject.AddComponent(type);

        string fieldNamesJson = EditorPrefs.GetString("fieldNames");
        JArray fieldNamesJArray = JArray.Parse(fieldNamesJson);
        string[] fieldNames = new string[fieldNamesJArray.Count];
        for (int i = 0; i < fieldNamesJArray.Count; i++)
        {
            fieldNames[i] = fieldNamesJArray[i].ToString();
        }

        string instanceIDJson = EditorPrefs.GetString("instanceID");
        JArray instanceIDJArray = JArray.Parse(instanceIDJson);
        int[] instanceID = new int[instanceIDJArray.Count];
        for (int i = 0; i < instanceIDJArray.Count; i++)
        {
            instanceID[i] = instanceIDJArray[i].ToObject<int>();
        }

        for (int i = 0; i < fieldNames.Length; i++)
        {
            try
            {
                object o = EditorUtility.InstanceIDToObject(instanceID[i]);
                uiPanel.GetType().GetField(fieldNames[i]).SetValue(uiPanel, o);
            }
            catch (Exception)
            {
               
            }           
        }
    }
    [DidReloadScripts]//在编译完成后自动调用
    public static void DidReloadScripts()
    {
        if (!EditorPrefs.GetBool("isCreateScript")) return;
        EditorPrefs.SetBool("isCreateScript", false);
        //Debug.Log("DidReloadScripts");
        AddScript();

    }
    /// <summary>
    /// 创建类文件
    /// </summary>
    /// <param name="name">类名</param>
    /// <param name="baseType">父类名</param>
    /// <param name=""></param>
    public static void CreateClassFile(string className = "ClassName", Type baseType=null,string[] fieldNames=null,Type[] fieldType=null)
    {
        //准备一个代码编译器单元
        CodeCompileUnit unit = new CodeCompileUnit();

        //设置命名空间（这个是指要生成的类的空间）
        CodeNamespace myNamespace = new CodeNamespace("");

        //导入必要的命名空间引用
        myNamespace.Imports.Add(new CodeNamespaceImport("System"));
        myNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        myNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine.UI"));

        //Code:代码体,准备要生成的类的定义
        CodeTypeDeclaration myClass = new CodeTypeDeclaration(className);

        //指定为类
        myClass.IsClass = true;

        //设置类的访问类型
        myClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;

        //设置父类
        myClass.BaseTypes.Add(baseType);

        //把这个类放在这个命名空间下
        myNamespace.Types.Add(myClass);

        //把该命名空间加入到编译器单元的命名空间集合中
        unit.Namespaces.Add(myNamespace);

        if (fieldNames != null)
            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (fieldType[i]==null)
                {
                    continue;
                }
                //添加字段
                CodeMemberField field = new CodeMemberField(fieldType[i].Name, fieldNames[i]);

                //设置访问类型
                field.Attributes = MemberAttributes.Public;

                //添加到myClass类中
                myClass.Members.Add(field);                

                if (fieldType[i].Name == "Button")
                {
                    CodeMemberMethod method = new CodeMemberMethod();
                    method.Name = fieldNames[i] + "_Logic";
                    method.Attributes = MemberAttributes.Family;
                    myClass.Members.Add(method);
                }

            }

        //增加属性
        //CodeMemberField field2 = new CodeMemberField("GameObject", "go");
        //field2.Attributes = MemberAttributes.Public;
        //myClass.Members.Add(field2);

        
        //增加虚方法
        //CodeMemberMethod method = new CodeMemberMethod();
        //method.Name = "TestMethod";
        //method.Attributes = MemberAttributes.Family;               
        //myClass.Members.Add(method);

        //生成代码
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        options.BlankLinesBetweenMembers = false;

        //输出文件路径
        string outputFile = Application.dataPath + "/Script/UI/" + className + ".cs";

        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
        {
            provider.GenerateCodeFromCompileUnit(unit, sw, options);
        }
    }
}
