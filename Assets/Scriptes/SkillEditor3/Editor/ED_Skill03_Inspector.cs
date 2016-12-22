using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 当前技能控制模式
/// 作者 : Canyon
/// 日期 : 2016-12-22 09:30:00
/// 功能 : 
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Skill03), true)]
public class ED_Skill03_Inspector : Editor
{

    DBOpt_Ani db_opt_ani = new DBOpt_Ani();
    DBOpt_Time db_opt_time = new DBOpt_Time();

    ED_Skill03 m_entity;
    Animator m_ani;

    // 更新是否是通过EditorApplication的时间
    bool isUpAniByEdTime = false;

    float delta_time = 0.0f;

    int loop_times = 0;
    int cur_loop_times = 0;
    bool isFinished_OneWheel = false;

    // 控制行间 - 间隔距离
    float space_row_interval = 10.0f;

    // 暂停按钮控制
    bool isPauseing = false;

    // 播放按钮的控制值
    bool isPlaying = false;

    // popup 列表选择值
    int ind_popup = 0;
    int pre_ind_popup = -1;

    // 速度控制
    float cur_speed = 1.0f;

    float min_speed = 0.0f;
    float max_speed = 3.0f;

    void OnEnable()
    {
        DoInit();
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
        DoClear();
    }

    void DoClear()
    {
        db_opt_ani.DoClear();
        m_entity = null;
        m_ani = null;

        pre_ind_popup = -1;
        ind_popup = 0;

        OnResetMember();
    }

    void DoReInit()
    {
        DoClear();
        db_opt_ani.DoClear();

        DoInit();
    }

    void DoInit()
    {
        db_opt_time.DoReInit(isUpAniByEdTime);
        OnInitAni();
        OnResetMember();
    }

    void OnInitAni()
    {
        m_entity = target as ED_Skill03;
        if (m_entity)
            m_ani = m_entity.GetComponent<Animator>();

        if (m_ani)
            db_opt_ani.DoReInit(m_ani);
    }

    void OnResetMember()
    {
        isPauseing = false;
        isPlaying = false;
        isUpAniByEdTime = false;

        loop_times = 0;
        cur_loop_times = 0;

        OnResetMemberReckon();
    }

    void OnResetMemberReckon()
    {
        db_opt_time.OnResetMemberReckon();

        delta_time = 0.0f;
        
        isFinished_OneWheel = false;
    }

    void DoUpdateAnimator(bool isChanged)
    {
        delta_time = db_opt_time.DeltaTime;
        if (db_opt_ani.CurIsHasCondition)
        {
            db_opt_ani.SetSpeed(cur_speed);
            db_opt_ani.OnAniUpdate(delta_time);
        }
        else
        {
            // 期初的算法模式（错误的，在速度变慢的时候）
            // float progress = (db_opt_time.ProgressTime / db_opt_ani.CurLens) * cur_speed;

            float progress = 0.0f;
            if(!isChanged)
                progress = db_opt_ani.normalizedTime + delta_time * cur_speed / db_opt_ani.CurLens;
            db_opt_ani.PlayCurr(progress, delta_time);
        }
    }

    void DoPause()
    {

    }

    void DoResume()
    {
        db_opt_time.DoResume();
    }

    void OnUpdate()
    {
        if (m_ani == null || isPauseing || !isPlaying || Application.isPlaying || EditorApplication.isPlaying)
        {
            return;
        }

        db_opt_time.DoUpdateTime();


        bool isChanged = db_opt_ani.IsChanged4CheckCurStateInfo();
        if (isChanged)
        {
            OnResetMemberReckon();
        }

        DoUpdateAnimator(isChanged);

        if (m_ani.IsInTransition(0))
        {
            return;
        }
        
        cur_loop_times = Mathf.FloorToInt(db_opt_ani.normalizedTime);
        if(cur_loop_times > loop_times)
        {
            isFinished_OneWheel = true;
            loop_times = cur_loop_times;
        }else{
            isFinished_OneWheel = false;
        }

        if (isFinished_OneWheel)
        {
            OnResetMemberReckon();
            if (!db_opt_ani.isLoop)
            {
                isPlaying = false;
                isFinished_OneWheel = false;
            }
        }
    }

    void OnInitM_Ani()
    {
        db_opt_ani.DoResetAniCtrl();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("行间间距:");
            space_row_interval = EditorGUILayout.Slider(space_row_interval, 0, 100);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);

        if (m_ani == null)
        {
            EditorGUILayout.HelpBox("请将脚本绑定到一个带Animator的物体！", MessageType.Error);
            return;
        }

        if (GUILayout.Button("刷新Animator动画列表"))
        {
            DoReInit();
        }

        if (db_opt_ani.Keys.Count <= 0)
        {
            EditorGUILayout.HelpBox("该AnimatorController里面没有任何动画，请添加动画！", MessageType.Error);
            return;
        }

        GUILayout.Space(space_row_interval);

        ind_popup = EditorGUILayout.Popup("动画列表", ind_popup, db_opt_ani.Keys.ToArray());
        if (pre_ind_popup != ind_popup)
        {
            pre_ind_popup = ind_popup;
            db_opt_ani.ResetAniState(ind_popup);
            db_opt_ani.SetSpeed(1.0f);

            cur_speed = 1.0f;
        }

        GUILayout.Space(space_row_interval);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("总帧数: " + db_opt_ani.CurFrameCount);

            EditorGUILayout.LabelField("总时长: " + db_opt_ani.CurLens + " s");
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("速度:");
            // cur_speed = GUILayout.HorizontalSlider(cur_speed,min_speed, max_speed);
            cur_speed = EditorGUILayout.Slider(cur_speed, min_speed, max_speed);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Play"))
            {
                OnResetMember();
                OnInitM_Ani();
                isPlaying = true;

                // m_ani.SetBool("Skill4", true);
                db_opt_ani.SetCurCondition();
            }

            if (GUILayout.Button(isPauseing ? "ReGo" : "Pause"))
            {
                isPauseing = !isPauseing;
                if (!isPauseing)
                {
                    DoResume();
                }
            }

            if (GUILayout.Button("Stop"))
            {
                OnInitM_Ani();
                OnResetMember();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}

