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


    // popup 列表选择值
    int ind_popup = 0;
    int pre_ind_popup = -1;

    // 速度控制
    float cur_speed = 1.0f;
    bool isCanSetMinMaxSpeed = false;
    float min_speed = 0.0f;
    float max_speed = 3.0f;

    // 动作进度
    bool isCtrlProgress = false;
    float cur_progress = 0.0f;
    float min_progress = 0.0f;
    float max_progress = 1.0f;

    // 循环次数
    bool isRound = false;
    int round_times = 1;

    #endregion

    public void DoInit(EDW_Skill org)
    {
        this.m_wSkill = org;
    }

    public void DoReset()
    {
        ind_popup = 0;
        pre_ind_popup = -1;
    }

    void OnResetProgress()
    {
        max_progress = m_curAni.CurLens;
        max_progress = max_progress > 0 ? max_progress : 1.0f;
    }

    public void DrawShow()
    {
        EG_GUIHelper.FEG_BeginVArea();
        {
            _DrawDesc();

            if (this.m_wSkill.gobjEntity)
            {
                _DrawFreshBtn();

                //if (_DrawAniJudged())
                //{
                //    _DrawAniList();

                //    _DrawAniStateInfo();

                //    _DrawAniMinMaxSpeed();

                //    _DrawAniCurSpeed();

                //    _DrawCtrlAniStateProgress();

                //    _DrawRoundTimes();
                //}
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

    void _DrawFreshBtn()
    {
        EG_GUIHelper.FEG_BeginH();
        if (GUILayout.Button("刷新Animator动作列表List"))
        {
            m_wSkill.OnInitEnAni();
        }
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

        style = EditorStyles.popup;
        style.alignment = TextAnchor.MiddleRight;
        ind_popup = EditorGUILayout.Popup(ind_popup, m_curAni.Keys.ToArray(), style);
        if (pre_ind_popup != ind_popup)
        {
            pre_ind_popup = ind_popup;
            m_curAni.SetSpeed(1.0f);
            m_curAni.ResetAniState(ind_popup);

            OnResetProgress();
        }
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
            EG_GUIHelper.FEG_BeginToggleGroup("MinMax速度??", ref isCanSetMinMaxSpeed);
            min_speed = EditorGUILayout.FloatField("Min速度:", min_speed);
            EG_GUIHelper.FG_Space(3);
            max_speed = EditorGUILayout.FloatField("Max速度:", max_speed);
            EG_GUIHelper.FEG_EndToggleGroup();

            if (max_speed < min_speed)
            {
                max_speed = min_speed;
            }
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniCurSpeed()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            GUIStyle style = EditorStyles.label;
            style.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField("当前速度:", style);

            cur_speed = EditorGUILayout.Slider(cur_speed, min_speed, max_speed);
        }
        EG_GUIHelper.FEG_EndH();
    }

    float Round(float org, int acc)
    {
        float pow = Mathf.Pow(10, acc);
        float temp = org * pow;
        return Mathf.RoundToInt(temp) / pow;
    }

    void ReckonProgress(float normalizedTime)
    {
        cur_progress = (normalizedTime % 1);
        cur_progress = cur_progress * max_progress;
        // cur_progress = Round(cur_progress, 6);
    }

    void _DrawCtrlAniStateProgress()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("控制动作进度??", ref isCtrlProgress);

            float cur_progress01 = cur_progress / max_progress;
            if (isCtrlProgress)
            {
                ReckonProgress(cur_progress01);
            }
            else
            {
                ReckonProgress(m_curAni.normalizedTime);
            }

            GUIStyle style = EditorStyles.label;
            style.alignment = TextAnchor.MiddleRight;
            EditorGUILayout.LabelField("当前动作进度: " + cur_progress01, style);

            EG_GUIHelper.FG_Space(3);

            cur_progress = EditorGUILayout.Slider(cur_progress, min_progress, max_progress);
            EG_GUIHelper.FEG_EndToggleGroup();

            //GUIStyle style = new GUIStyle();
            //style.alignment = TextAnchor.MiddleRight;
            //style.normal.textColor = Color.yellow;
            //EditorGUILayout.LabelField("(勾选时，才可控制 [当前进度]！！！)", style);
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawRoundTimes()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("控制循环次数??", ref isRound);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleRight;
            style.normal.textColor = Color.cyan;

            string desc = string.Format("已播放了{0:D2}次!", m_curAni.CurLoopCount);
            EditorGUILayout.LabelField(desc, style);

            EG_GUIHelper.FG_Space(3);

            round_times = EditorGUILayout.IntField("循环次数:", round_times);

            EG_GUIHelper.FEG_EndToggleGroup();

        }
        EG_GUIHelper.FEG_EndH();
    }
}
