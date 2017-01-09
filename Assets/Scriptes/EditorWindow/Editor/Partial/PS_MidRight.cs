using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : P-Partial,S-Skill 策划工具:技能数据表
/// 作者 : Canyon
/// 日期 : 2017-01-09 09:10
/// 功能 : 
/// </summary>
public class PS_MidRight{

    #region  == Member Attribute ===

    EDW_Skill m_wSkill;

    ED_Ani_YGame m_curAni
    {
        get
        {
            return m_wSkill.me_ani;
        }
    }

    bool isEmptySkillPath = true;
    string pathOpenSkill = "";
    
    #endregion

    public void DoInit(EDW_Skill org)
    {
        this.m_wSkill = org;
    }
    
    public void DrawShow()
    {
        EG_GUIHelper.FEG_BeginVArea();
        {
            _DrawDesc();

            _DrawSearchSkillExcel();

            if (this.m_wSkill.gobjEntity)
            {
                
                
            }
        }
        EG_GUIHelper.FEG_EndV();
    }

    void _DrawDesc()
    {
        EG_GUIHelper.FEG_BeginH();

        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleCenter;
        string txtDecs = "策划工具:技能数据表";
        GUILayout.Label(txtDecs, style);

        EG_GUIHelper.FEG_EndH();
    }

    void _DrawSearchSkillExcel()
    {
        EG_GUIHelper.FEG_BeginH();
        isEmptySkillPath = string.IsNullOrEmpty(pathOpenSkill);
        EG_GUIHelper.FEG_BeginToggleGroup("选取技能表excel文件", ref isEmptySkillPath);
        if (GUILayout.Button("选取SkillExcel"))
        {
            pathOpenSkill = UnityEditor.EditorUtility.OpenFilePanel("选取excel文件", "", "xls");
        }
        EG_GUIHelper.FG_Space(3);
        EG_GUIHelper.FEG_EndToggleGroup();
        
        EG_GUIHelper.FEG_EndH();
    }

    bool _DrawAniJudged()
    {
        bool ret = false;
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginV();
            {
                if (m_wSkill == null || m_curAni == null)
                {
                    EditorGUILayout.HelpBox("请拖动Prefab到Model Prefab中！", MessageType.Error);
                }
                else if (!m_curAni.IsHasAniCtrl)
                {
                    EditorGUILayout.HelpBox("该Animator里面没有AnimatorController \n\n请添加动画控制器-AnimatorController！", MessageType.Error);
                }
                else if (m_curAni.Keys.Count <= 0)
                {
                    EditorGUILayout.HelpBox("该AnimatorController里面没有任何动画，请添加State动画！", MessageType.Error);
                }
                else
                {
                    ret = true;
                }
            }
            EG_GUIHelper.FEG_EndV();
        }
        EG_GUIHelper.FEG_EndH();
        return ret;
    }

    void _DrawAniList()
    {
        EG_GUIHelper.FEG_BeginH();
        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.LabelField("动画列表:", style);
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniStateInfo()
    {
        EG_GUIHelper.FEG_BeginH();
        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleLeft;

        GUILayoutOption minW = GUILayout.MinWidth(90);
        GUILayout.Label("总帧数: " + m_curAni.CurFrameCount, style, minW);

        GUILayout.Label("总时长: " + m_curAni.CurLens + " s", style, minW);

        style.alignment = TextAnchor.MiddleRight;
        EditorGUILayout.LabelField("动画帧率: " + m_curAni.CurFrameRate + " 帧/s", style);
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniMinMaxSpeed()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            bool v = false;
            EG_GUIHelper.FEG_BeginToggleGroup("MinMax速度??", ref v);

            EG_GUIHelper.FEG_EndToggleGroup();
        }
        EG_GUIHelper.FEG_EndH();
    }
    
}
