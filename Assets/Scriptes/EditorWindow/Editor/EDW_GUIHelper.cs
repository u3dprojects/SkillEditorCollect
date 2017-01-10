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
    float fadeVal = 0.5f;
    Vector2 scorllPos;
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

        fadeVal = EditorGUILayout.Slider("FadeValue:", fadeVal,0,1);

        // scroll
        EG_GUIHelper.FEG_BeginScroll(ref scorllPos,3,0,260);

        // fade 
        //if (EG_GUIHelper.FEG_BeginFadeGroup(fadeVal))
        //{
        //    if (GUILayout.Button(" FadeGroup test"))
        //    {

        //    }
        //    EG_GUIHelper.FG_Label("FadeGroup000");
        //    EG_GUIHelper.FG_Label("FadeGroup001");
        //    EG_GUIHelper.FG_Label("FadeGroup002");
        //    EG_GUIHelper.FG_Label("FadeGroup003");
        //    EG_GUIHelper.FG_Label("FadeGroup004");
        //    EG_GUIHelper.FG_Label("FadeGroup005");
        //    EG_GUIHelper.FG_Label("FadeGroup006");
        //    EG_GUIHelper.FG_Label("FadeGroup007");
        //    EG_GUIHelper.FG_Label("FadeGroup008");
        //    EG_GUIHelper.FG_Label("FadeGroup009");
        //    EG_GUIHelper.FG_Label("FadeGroup010");
        //}
        //EG_GUIHelper.FEG_EndFadeGroup();
        EG_GUIHelper.FG_Space(10);

        EG_GUIHelper.FG_Label("Scroll View");
        EG_GUIHelper.FG_Label(1111);
        EG_GUIHelper.FG_Label(1112);
        EG_GUIHelper.FG_Label(1113);
        EG_GUIHelper.FG_Label(1115);
        EG_GUIHelper.FG_Label(1111);
        EG_GUIHelper.FG_Label(1112);
        EG_GUIHelper.FG_Label(1113);
        if (GUILayout.Button("ScrollView test"))
        {

        }
        EG_GUIHelper.FG_Label("ScrollView");
        EG_GUIHelper.FEG_EndScroll();
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
