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

    bool isCanSetMinMaxSpeed = false;

    float min_speed = 0.0f;
    float max_speed = 3.0f;

    // 动画播放进度
    bool isPreCtrlProgress = false;
    bool isCtrlProgress = false;
    float reckon_progress = 0.0f;
    float cur_progress = 0.0f;
    float pre_progress = 0.0f;

    float min_progress = 0.0f;
    float max_progress = 1.0f;

    public void DoClear()
    {
        db_opt_ani = null;
    }

    public void Reset()
    {
        pre_ind_popup = -1;
        ind_popup = 0;

        cur_speed = 1.0f;
        isCanSetMinMaxSpeed = false;
        min_speed = 0.0f;
        max_speed = 3.0f;

        reckon_progress = 0.0f;
        cur_progress = 0.0f;
        pre_progress = 0.0f;
        isPreCtrlProgress = false;
        isCtrlProgress = false;
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

    float Round(float org,int acc)
    {
        float pow = Mathf.Pow(10, acc);
        float temp = org * pow;
        return Mathf.RoundToInt(temp) / pow;
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

    public void DrawRefreshAnimator(System.Action OnClickRefresh)
    {
        if (GUILayout.Button("刷新Animator动画列表"))
        {
            if (OnClickRefresh != null)
            {
                OnClickRefresh();
            }
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

            GUILayout.Label("总时长: " + db_opt_ani.CurLens + " s");

            EditorGUILayout.LabelField("动画帧率: " + db_opt_ani.CurFrameRate +" 帧/s");
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }

    public void DrawSpeed()
    {
        isCanSetMinMaxSpeed = EditorGUILayout.Toggle("速度的限制控制??", isCanSetMinMaxSpeed);

        GUILayout.Space(space_row_interval);

        if (isCanSetMinMaxSpeed)
        {
            EditorGUILayout.BeginHorizontal();
            {
                min_speed = EditorGUILayout.FloatField("最小速度:", min_speed);
                max_speed = EditorGUILayout.FloatField("最大速度:", max_speed);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(space_row_interval);
        }
        
        EditorGUILayout.BeginHorizontal();
        {
            // cur_speed = GUILayout.HorizontalSlider(cur_speed,min_speed, max_speed);
            cur_speed = EditorGUILayout.Slider("当前速度:", cur_speed, min_speed, max_speed);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }

    void ReckonProgress(float normalizedTime)
    {
        reckon_progress = (normalizedTime % 1);
        reckon_progress = Round(reckon_progress, 2);

        cur_progress = reckon_progress * db_opt_ani.CurLens;
        cur_progress = Round(cur_progress, 2);

        pre_progress = cur_progress;
    }

    public void DrawCtrlAniProgress(bool isPause,System.Action<bool> callFunc)
    {
        
        isCtrlProgress = EditorGUILayout.Toggle("拖动进度控制？？", isCtrlProgress,GUILayout.Height(20));

        GUILayout.Space(space_row_interval);

        if(isPreCtrlProgress != isCtrlProgress) {
            isPreCtrlProgress = isCtrlProgress;

            if (callFunc != null)
            {
                callFunc(isCtrlProgress);
            }
        }
        else
        {
            isCtrlProgress = isPause;
            isPreCtrlProgress = isCtrlProgress;
        }
    }

    public void DrawAniProgress(bool isPause)
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (!isPause)
            {
                ReckonProgress(db_opt_ani.normalizedTime);
            } else if (isPause && pre_progress != cur_progress) {
                ReckonProgress(cur_progress / db_opt_ani.CurLens);
                db_opt_ani.PlayCurr(reckon_progress);
            }

            // EditorGUILayout.LabelField("当前进度:" + reckon_progress);
            GUILayout.Label("当前进度:  " + reckon_progress);
            cur_progress = EditorGUILayout.Slider(cur_progress, min_progress, max_progress * db_opt_ani.CurLens);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }
}
