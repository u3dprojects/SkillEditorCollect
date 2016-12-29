﻿using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

/// <summary>
/// 类名 : 融合项目需求的绘制
/// 作者 : Canyon
/// 日期 : 2016-12-22 17:30:00
/// 功能 : 
/// </summary>
[System.Serializable]
public class EDD_GUI_YGame : EDD_GUI{
    // 控制位移
    bool is_open_pos = false;
    AnimationCurve x_curve;
    AnimationCurve y_curve;
    AnimationCurve z_curve;

    public override void DrawAniListIndex(System.Action callFunc = null)
    {
        base.DrawAniListIndex(delegate() {
            if (callFunc != null)
            {
                callFunc();
            }

            // 获取StateMache
            is_open_pos = false;
            InitMache();
        });
    }

    void DefCurve()
    {
        x_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
        y_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
        z_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
    }

    void InitMache()
    {
        // 获取StateMache
        db_opt_ani.cur_state_mache = db_opt_ani.GetStateMache<SpriteAniCurve>();
        bool isNotNull = db_opt_ani.cur_state_mache != null;
        is_open_pos = isNotNull;
        syncMache(true);
        if (!isNotNull)
        {
            DefCurve();
        }
    }

    void SaveMache()
    {
        if(db_opt_ani.cur_state_mache == null)
        {
            db_opt_ani.cur_state_mache = db_opt_ani.AddStateMache<SpriteAniCurve>();
        }
        syncMache();
    }

    void syncMache(bool isReverse = false)
    {
        if (db_opt_ani.cur_state_mache == null)
        {
            return;
        }

        SpriteAniCurve temp = db_opt_ani.cur_state_mache as SpriteAniCurve;
        if (isReverse)
        {
            x_curve = temp.x;
            y_curve = temp.y;
            z_curve = temp.z;
        }
        else
        {
            temp.x = x_curve;
            temp.y = y_curve;
            temp.z = z_curve;
        }
    }

    void RemoveMache()
    {
        db_opt_ani.RemoveStateMache<SpriteAniCurve>();
        db_opt_ani.cur_state_mache = null;
        DefCurve();
    }

    // 动作位移
    public void DrawMovePos()
    {
        EditorGUILayout.BeginHorizontal();
        {
            is_open_pos = EditorGUILayout.Toggle("是否开启移动", is_open_pos);

            // 添加一个按钮
            if (is_open_pos)
            {
                GUI.color = Color.cyan;
                if (GUILayout.Button("SaveCurveMache", EditorStyles.miniButton, GUILayout.Width(120),GUILayout.Height(30)))
                {
                    SaveMache();
                }

                GUI.color = Color.red;
                if (GUILayout.Button("RemoveCurveMache", EditorStyles.miniButton, GUILayout.Width(120),GUILayout.Height(30)))
                {
                    RemoveMache();
                }
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);

        if (is_open_pos)
        {
            EditorGUILayout.BeginHorizontal();
            {
                x_curve = EditorGUILayout.CurveField("x", x_curve);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(space_row_interval);

            EditorGUILayout.BeginHorizontal();
            {
                y_curve = EditorGUILayout.CurveField("y", y_curve);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(space_row_interval);

            EditorGUILayout.BeginHorizontal();
            {
                z_curve = EditorGUILayout.CurveField("z", z_curve);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(space_row_interval);

            syncMache();
        }
    }

    public SpriteAniCurve curCurve
    {
        get
        {
            if (is_open_pos)
            {
                if (db_opt_ani.cur_state_mache)
                {
                    return db_opt_ani.cur_state_mache as SpriteAniCurve;
                }

                SpriteAniCurve m_Curve = new SpriteAniCurve();
                m_Curve.x = x_curve;
                m_Curve.y = y_curve;
                m_Curve.z = z_curve;
                return m_Curve;
            }
            return null;
        }
    }
}
