using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 技能编辑器窗口Class
/// 作者 : Canyon
/// 日期 : 2017-01-05 17:10
/// 功能 : 
/// </summary>
public class EDW_GUIHelper : EditorWindow
{
    static bool isOpenWindowView = false;

    static protected EDW_GUIHelper vwWindow = null;

    [MenuItem("Tools/Windows/GUIHelper")]
    static void AddWindow()
    {
        if (isOpenWindowView || vwWindow != null)
            return;

        isOpenWindowView = true;
        vwWindow = GetWindow<EDW_GUIHelper>("EDW_GUIHelper");
    }

    #region  == Member Attribute ===
    
    #endregion

    #region  == EditorWindow Func ===

    void OnEnable()
    {
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }

    void OnGUI()
    {
        EG_GUIHelper.FG_BeginVAsArea();
        EG_GUIHelper.FG_Label("GUILayout Vertical As TextArea");
        EG_GUIHelper.FG_Label("1");
        EG_GUIHelper.FG_Label("2");
        EG_GUIHelper.FG_Label("3");
        EG_GUIHelper.FG_Label("4");
        EG_GUIHelper.FG_EndV();

        EG_GUIHelper.FG_Space(10);

        EG_GUIHelper.FEG_BeginVAsArea();
        EG_GUIHelper.FG_Label("EditorGUILayout Vertical As TextArea");
        EG_GUIHelper.FG_Label(5);
        EG_GUIHelper.FG_Label(6);
        EG_GUIHelper.FG_Label(7);
        EG_GUIHelper.FG_Label(8);
        EG_GUIHelper.FEG_EndV();

        EG_GUIHelper.FG_Space(10);


        EG_GUIHelper.FG_BeginVArea();
        EG_GUIHelper.FG_Label("GUILayout Vertical Style TextArea");
        EG_GUIHelper.FG_Label("1");
        EG_GUIHelper.FG_Label("2");
        EG_GUIHelper.FG_Label("3");
        EG_GUIHelper.FG_Label("4");
        EG_GUIHelper.FG_EndV();

        EG_GUIHelper.FG_Space(10);

        EG_GUIHelper.FEG_BeginVArea();
        EG_GUIHelper.FG_Label("EditorGUILayout Vertical Style TextArea");
        EG_GUIHelper.FG_Label(5);
        EG_GUIHelper.FG_Label(6);
        EG_GUIHelper.FG_Label(7);
        EG_GUIHelper.FG_Label(8);
        EG_GUIHelper.FEG_EndV();

        EG_GUIHelper.FG_Space(10);
        {            
            EG_GUIHelper.FG_BeginHAsArea();
            EG_GUIHelper.FG_Label("GUILayout Horizontal As TextArea");
            EG_GUIHelper.FG_Label(5.1);
            EG_GUIHelper.FG_Label(6.1);
            EG_GUIHelper.FG_Label(7.1);
            EG_GUIHelper.FG_Label(8.1);
            EG_GUIHelper.FG_EndH();

            EG_GUIHelper.FG_Space(10);
            
            EG_GUIHelper.FEG_BeginHAsArea();
            EG_GUIHelper.FG_Label("EditorGUILayout Horizontal As TextArea");
            EG_GUIHelper.FG_Label(5.2);
            EG_GUIHelper.FG_Label(6.2);
            EG_GUIHelper.FG_Label(7.2);
            EG_GUIHelper.FG_Label(8.2);
            EG_GUIHelper.FG_EndH();
            
            EG_GUIHelper.FG_Space(10);
        }

        EG_GUIHelper.FG_BeginV();

        EG_GUIHelper.FG_BeginHArea();
        EG_GUIHelper.FG_Label("GUILayout Horizontal Style TextArea");
        EG_GUIHelper.FG_Label(5.1);
        EG_GUIHelper.FG_Label(6.1);
        EG_GUIHelper.FG_Label(7.1);
        EG_GUIHelper.FG_Label(8.1);
        EG_GUIHelper.FG_EndH();

        EG_GUIHelper.FG_EndV();

        EG_GUIHelper.FG_Space(10);

        EG_GUIHelper.FG_BeginV();

        EG_GUIHelper.FEG_BeginHArea();
        EG_GUIHelper.FG_Label("EditorGUILayout Horizontal Style TextArea");
        EG_GUIHelper.FG_Label(5.2);
        EG_GUIHelper.FG_Label(6.2);
        EG_GUIHelper.FG_Label(7.2);
        EG_GUIHelper.FG_Label(8.2);
        EG_GUIHelper.FG_EndH();

        EG_GUIHelper.FG_EndV();

        EG_GUIHelper.FG_Space(10);
    }

    void OnDestroy()
    {
        isOpenWindowView = false;
        vwWindow = null;
        EditorApplication.update -= OnUpdate;
    }

    #endregion

    #region  == Self Func ===

    void OnUpdate()
    {

    }

    #endregion
}
