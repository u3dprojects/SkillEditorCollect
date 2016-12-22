using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : animator的试图Inspector 中的操作
/// 作者 : Canyon
/// 日期 : 2016-12-22 17:30:00
/// 功能 : 
/// </summary>
[System.Serializable]
public class DBOpt_GUI : System.Object{

    DBOpt_Ani db_opt_ani;
    // DBOpt_Time db_opt_time = new DBOpt_Time();
    
    // 控制行间 - 间隔距离
    float space_row_interval = 10.0f;
    
    // popup 列表选择值
    int ind_popup = 0;
    int pre_ind_popup = -1;

    // 速度控制
    float cur_speed = 1.0f;

    float min_speed = 0.0f;
    float max_speed = 3.0f;

    public void DoClear()
    {
        db_opt_ani = null;
    }

    public void Reset()
    {
        pre_ind_popup = -1;
        ind_popup = 0;

        cur_speed = 1.0f;
    }

    public void DoInit(DBOpt_Ani db_ani)
    {
        Reset();
        
        this.db_opt_ani = db_ani;
    }

    public float CurSpeed
    {
        get { return cur_speed; }
    }
    
    public void DrawRowLine()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("行间间距:");
            space_row_interval = EditorGUILayout.Slider(space_row_interval, 0, 100);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }

    public bool DrawJudged()
    {
        if (db_opt_ani == null)
        {
            EditorGUILayout.HelpBox("请将脚本绑定到一个带Animator的物体！", MessageType.Error);
            return false;
        }

        if (db_opt_ani.Keys.Count <= 0)
        {
            EditorGUILayout.HelpBox("该AnimatorController里面没有任何动画，请添加动画！", MessageType.Error);
            return false;
        }
        return true;
    }

    public void DrawRefreshAnimator()
    {
        if (GUILayout.Button("刷新Animator动画列表"))
        {
            DoInit(this.db_opt_ani);
        }
        GUILayout.Space(space_row_interval);
    }

    public void DrawAniListIndex()
    {
        ind_popup = EditorGUILayout.Popup("动画列表", ind_popup, db_opt_ani.Keys.ToArray());
        if (pre_ind_popup != ind_popup)
        {
            pre_ind_popup = ind_popup;
            db_opt_ani.ResetAniState(ind_popup);
            db_opt_ani.SetSpeed(1.0f);

            cur_speed = 1.0f;
        }

        GUILayout.Space(space_row_interval);
    }

    public void DrawAniInfo()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("总帧数: " + db_opt_ani.CurFrameCount);

            EditorGUILayout.LabelField("总时长: " + db_opt_ani.CurLens + " s");
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }

    public void DrawSpeed()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("速度:");
            // cur_speed = GUILayout.HorizontalSlider(cur_speed,min_speed, max_speed);
            cur_speed = EditorGUILayout.Slider(cur_speed, min_speed, max_speed);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }
}
