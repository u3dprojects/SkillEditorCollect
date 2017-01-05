using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : E-Editor,G-Gui 绘制帮助Class
/// 作者 : Canyon
/// 日期 : 2017-01-05 17:20
/// 功能 : 
/// F - Func,G-Guilayout,V - Vertical,H-Horizontal,E - Editor
/// 外观 - EditorStyles.textArea 感觉还比较不错
/// </summary>
public static class EG_GUIHelper {

    #region == GUILayout Func ==

    static public void FG_BeginVAsArea()
    {
        GUILayout.BeginVertical("As TextArea", GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FG_BeginVArea()
    {
        GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FG_BeginV()
    {
        GUILayout.BeginVertical(GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FG_EndV()
    {
        GUILayout.Space(3);
        GUILayout.EndVertical();
    }

    static public void FG_BeginHAsArea()
    {
        GUILayout.BeginHorizontal("As TextArea", GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FG_BeginHArea()
    {
        GUILayout.BeginHorizontal(EditorStyles.textArea, GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FG_BeginH()
    {
        GUILayout.BeginHorizontal(GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FG_EndH()
    {
        GUILayout.Space(3);
        GUILayout.EndHorizontal();
    }

    static public void FG_Label(object obj)
    {
        GUILayout.Label(obj.ToString());
    }

    static public void FG_Space(float space)
    {
        GUILayout.Space(space);
    }

    #endregion

    #region == EditorGUILayout Func ==

    static public void FEG_BeginVAsArea()
    {
        EditorGUILayout.BeginVertical("As TextArea",GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FEG_BeginVArea()
    {
        EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FEG_BeginV()
    {
        EditorGUILayout.BeginVertical(GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FEG_EndV()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndVertical();
    }

    static public void FEG_BeginHAsArea()
    {
        EditorGUILayout.BeginHorizontal("As TextArea", GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FEG_BeginHArea()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.textArea, GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FEG_BeginH()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10));
        GUILayout.Space(2);
    }

    static public void FEG_EndH()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndHorizontal();
    }

    static public void FEG_BeginToggleGroup(string title,ref bool toggle)
    {
        toggle = EditorGUILayout.BeginToggleGroup(title, toggle);
    }

    static public void FEG_EndToggleGroup(string title, ref bool toggle)
    {
        EditorGUILayout.EndToggleGroup();
    }
    #endregion

}
