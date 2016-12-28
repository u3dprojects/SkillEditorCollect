using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : animator的试图Inspector 中的操作
/// 作者 : Canyon
/// 日期 : 2016-12-22 17:30:00
/// 功能 : 
/// </summary>
[System.Serializable]
public class DBU3D_GUI : System.Object{

    DBU3D_Ani db_opt_ani;
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

    // 播放次数控制
    public bool isRound { get; set; }
    int round_times = 1;
    public int cur_round_times{get;set;}
    public bool isCompleteRound
    {
        get
        {
            if (isRound)
            {
                return round_times <= cur_round_times;
            }
            return true;
        }
    }

    // 控制位移
    bool is_open_pos = false;
    AnimationCurve x_curve;
    AnimationCurve y_curve;
    AnimationCurve z_curve;

    // 
    List<bool> m_evnt_fodeOut = new List<bool>();
    int nextIndFodeOut
    {
        get {
            return m_evnt_fodeOut.Count;
        }
    }

    public void DoClear()
    {
        db_opt_ani = null;

        Reset();
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

        isRound = false;
        round_times = 1;
        cur_round_times = 0;

        m_evnt_fodeOut.Clear();
    }

    public void DoInit(DBU3D_Ani db_ani)
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

            // 获取StateMache
            is_open_pos = false;
            InitMache();
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
        reckon_progress = Round(reckon_progress, 3);

        cur_progress = reckon_progress * db_opt_ani.CurLens;
        cur_progress = Round(cur_progress, 3);

        pre_progress = cur_progress;
    }

    public void DrawCtrlAniProgress(bool isPause,System.Action<bool> callFunc)
    {

        EditorGUILayout.BeginHorizontal();
        {
            isCtrlProgress = EditorGUILayout.Toggle("拖动进度控制？？", isCtrlProgress,GUILayout.Height(20));

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.yellow;

            EditorGUILayout.LabelField("(勾选时，才可控制 [当前进度]！！！)",style);
        }
        EditorGUILayout.EndHorizontal();

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
            GUILayout.Label("当前进度:" + reckon_progress,GUILayout.Width(110));
            cur_progress = EditorGUILayout.Slider(cur_progress, min_progress, max_progress * db_opt_ani.CurLens);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }

    public void DrawRoundTimes()
    {
        EditorGUILayout.BeginHorizontal();
        {
            isRound = EditorGUILayout.Toggle("是否控制循环次数？？", isRound);

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.cyan;

            string desc = "";
            // desc = string.Format("已播放了{0:D2}次，第{1:D2}次播放", cur_round_times, (cur_round_times + 1));
            desc = string.Format("已播放了{0:D2}次!", cur_round_times);
            EditorGUILayout.LabelField(desc, style, GUILayout.Height(20));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
        
        if (isRound)
        {
            EditorGUILayout.BeginHorizontal();
            {
                round_times = EditorGUILayout.IntField("循环播放次数:", round_times);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(space_row_interval);
        }
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

    // 添加特效
    public void DrawEffect()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("添加动作特效"))
            {
                db_opt_ani.AddCurEffect();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);

        int lens = db_opt_ani.curEffects.Count;
        
        if (lens > 0) {
            EA_Effect one;
            for (int i = 0; i < lens; i++)
            {
                lens = db_opt_ani.curEffects.Count;
                if(i > lens - 1)
                {
                    i = lens - 1;
                }

                one = db_opt_ani.curEffects[i];

                m_evnt_fodeOut.Add(false);

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (string.IsNullOrEmpty(one.name))
                        {
                            m_evnt_fodeOut[i] = EditorGUILayout.Foldout(m_evnt_fodeOut[i], "特效 - 未指定");
                        }
                        else
                        {
                            m_evnt_fodeOut[i] = EditorGUILayout.Foldout(m_evnt_fodeOut[i], "特效 - " + one.name);
                        }

                        GUI.color = Color.red;
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
                        {
                            db_opt_ani.RemoveEffect(one);
                            m_evnt_fodeOut.RemoveAt(i);
                        }
                        GUI.color = Color.white;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(space_row_interval);

                    if (m_evnt_fodeOut[i])
                    {
                        DrawOneEffect(one);
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(space_row_interval);
            }
        }else
        {
            m_evnt_fodeOut.Clear();
        }
    }

    void DrawOneEffect(EA_Effect one)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("特效文件:", GUILayout.Width(80));
            one.gobj = EditorGUILayout.ObjectField(one.gobj, typeof(GameObject), false) as GameObject;
            one.Reset(one.gobj);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("触发时间:", GUILayout.Width(80));
            one.time = EditorGUILayout.Slider(one.time, 0, 1);
        }
        GUILayout.EndHorizontal();

        if (one.isChanged)
        {
            db_opt_ani.ResetEvent(one);
        }

        GUILayout.Space(space_row_interval);
    }

}
