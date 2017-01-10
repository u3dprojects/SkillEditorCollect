using UnityEngine;
using System.Collections.Generic;
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

    static List<GUILayoutOption> list = new List<GUILayoutOption>();

    static public GUILayoutOption[] ToOptions(float minW = 0, float minH = 10)
    {
        list.Clear();
        if (minH > 0)
        {
            list.Add(GUILayout.MinHeight(minH));
        }

        if (minW > 0)
        {
            list.Add(GUILayout.MinWidth(minW));
        }
        return list.ToArray();
    }

    #region == GUILayout Func ==

    static public void FG_BeginVAsArea(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        GUILayout.BeginVertical("As TextArea", arrs);
        GUILayout.Space(2);
    }

    static public void FG_BeginVArea(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        GUILayout.BeginVertical(EditorStyles.textArea,arrs);
        GUILayout.Space(2);
    }

    static public void FG_BeginV(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        GUILayout.BeginVertical(arrs);
        GUILayout.Space(2);
    }

    static public void FG_EndV()
    {
        GUILayout.Space(3);
        GUILayout.EndVertical();
    }

    static public void FG_BeginHAsArea(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        GUILayout.BeginHorizontal("As TextArea", arrs);
        GUILayout.Space(2);
    }

    static public void FG_BeginHArea(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        GUILayout.BeginHorizontal(EditorStyles.textArea, arrs);
        GUILayout.Space(2);
    }

    static public void FG_BeginH(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        GUILayout.BeginHorizontal(arrs);
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

    static public void FEG_BeginVAsArea(float minW = 0,float minH = 10)
    {

        GUILayoutOption[] arrs = ToOptions(minW, minH);
        EditorGUILayout.BeginVertical("As TextArea",arrs);
        GUILayout.Space(2);
    }

    static public void FEG_BeginVArea(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        EditorGUILayout.BeginVertical(EditorStyles.textArea, arrs);
        GUILayout.Space(2);
    }

    static public void FEG_BeginV(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        EditorGUILayout.BeginVertical(arrs);
        GUILayout.Space(2);
    }

    static public void FEG_EndV()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndVertical();
    }

    static public void FEG_BeginHAsArea(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        EditorGUILayout.BeginHorizontal("As TextArea", arrs);
        GUILayout.Space(2);
    }

    static public void FEG_BeginHArea(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        EditorGUILayout.BeginHorizontal(EditorStyles.textArea, arrs);
        GUILayout.Space(2);
    }

    static public void FEG_BeginH(float minW = 0, float minH = 10)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        EditorGUILayout.BeginHorizontal(arrs);
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
        GUILayout.Space(2);
    }

    static public void FEG_EndToggleGroup()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndToggleGroup();
    }

    static public void FEG_BeginScroll(ref Vector2 scrollPos,int hvScrollType = 0,float minW = 0, float minH = 70)
    {
        GUILayoutOption[] arrs = ToOptions(minW, minH);
        bool isHScroll = hvScrollType != 2 && hvScrollType != 0;
        bool isVScroll = hvScrollType != 1 && hvScrollType != 0;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, isHScroll, isVScroll,arrs);
        GUILayout.Space(2);
    }

    static public void FEG_EndScroll()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndScrollView();
    }

    static public bool FEG_BeginFadeGroup(float val,float minW = 0, float minH = 30)
    {
        bool ret = false;
        FEG_BeginV(minW, minH);
        ret = EditorGUILayout.BeginFadeGroup(val);
        GUILayout.Space(2);
        return ret;
    }

    static public void FEG_EndFadeGroup()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndFadeGroup();
        FEG_EndV();
    }

    #endregion

}
