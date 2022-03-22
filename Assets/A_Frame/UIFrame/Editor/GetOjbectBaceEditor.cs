using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GetOjbectBace),true)]
public class GetOjbectBaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("获取所有对象"))
        {
            UIToolEditor.UpdateScript();
        }
    }
}
