using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : Demo04-进度
/// 作者 : Canyon
/// 日期 : 2016-12-22 17:20:00
/// 功能 : 目前所有的Ani操作都放到了DBU3D_Ani对象里面
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Skill04), true)]
public class ED_Skill04_Inspector : Editor
{

    DBU3D_Ani db_opt_ani = new DBU3D_Ani();
    DBOpt_Time db_opt_time = new DBOpt_Time();

    ED_Skill04 m_entity;
    Animator m_ani;

    // 更新是否是通过EditorApplication的时间
    bool isUpAniByEdTime = false;

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

        cur_speed = 1.0f;

        OnResetMember();
    }

    void DoReInit()
    {
        DoClear();

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
        m_entity = target as ED_Skill04;
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

        OnResetMemberReckon();
    }

    void OnResetMemberReckon()
    {
        db_opt_time.OnResetMemberReckon();
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

        // db_opt_ani.DoUpdateAnimator(db_opt_time.DeltaTime, cur_speed);

        db_opt_ani.DoUpdateAnimator(db_opt_time.DeltaTime, cur_speed,
            delegate () { OnResetMemberReckon(); },
            delegate (bool isloop)
            {
                OnResetMemberReckon();
                if (!isloop)
                {
                    isPlaying = false;
                }
            }
        );
    }

    void OnInitM_Ani()
    {
        db_opt_ani.DoResetAniCtrl();
        db_opt_ani.OnResetMemberReckon();
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
