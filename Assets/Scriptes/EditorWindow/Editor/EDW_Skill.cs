using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 技能编辑器窗口Class
/// 作者 : Canyon
/// 日期 : 2017-01-05 17:10
/// 功能 : 
/// </summary>
public class EDW_Skill : EditorWindow
{
    static bool isOpenWindowView = false;

    static protected EDW_Skill vwWindow = null;
    
    [MenuItem("Tools/Windows/EDSkill")]
    static void AddWindow()
    {
        if (isOpenWindowView || vwWindow != null)
            return;

        isOpenWindowView = true;
        vwWindow = GetWindow<EDW_Skill>("SkillEditor");

        int width = 600;
        int height = 400;
        float x = 460;
        float y = 220;
        vwWindow.position = new Rect(x, y, width, height);
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
        EG_GUIHelper.FEG_BeginV();
        // 中体结构分三层(上，中，下)
        {
            // 上
            EG_GUIHelper.FEG_BeginH();
            GUIStyle style = EditorStyles.label;
            style.alignment = TextAnchor.MiddleCenter;
            string txtDecs = "类名 : 技能编辑器窗口\n"
                + "作者 : Canyon\n"
                + "日期 : 2017 - 01 - 05 17:10\n"
                + "描述 : 结构分上中下三层，中分左右两块，左边提供给美术对动作-特效；右边提供策划导出数据表。\n";
            GUILayout.Label(txtDecs, style);
            
            EG_GUIHelper.FEG_EndH();
        }

        {
            // 中 : 分(左,右)
            EG_GUIHelper.FEG_BeginH();
            {
                // 左边
                DrawLeft();
                DrawRight();
            }
            EG_GUIHelper.FEG_EndH();
        }
        {
            // 下
        }

        EG_GUIHelper.FEG_EndV();
    }

    // 在给定检视面板每秒10帧更新
    void OnInspectorUpdate()
    {
        Repaint();
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

    void DrawLeft()
    {
        EG_GUIHelper.FEG_BeginHArea();
        EG_GUIHelper.FEG_EndH();
    }

    void DrawRight()
    {
        EG_GUIHelper.FEG_BeginHArea();
        EG_GUIHelper.FEG_EndH();
    }
    #endregion
}
